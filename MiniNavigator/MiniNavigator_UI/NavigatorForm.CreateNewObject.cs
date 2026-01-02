using MiniNavigator_Services.DTO;
using MiniNavigator_UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI
{
    public partial class NavigatorForm
    {
        private bool _isNewRow;

        private DynamicObjectRow _editingRow;

        private bool _isEditing;
        private bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                _isEditing = value;
                SetTableReadonlyProperty();
            }
        }

        #region Initialization Dialog Buttons

        /// <summary>
        /// Кнопка подтверждения действия
        /// </summary>
        private Button ApplyBtn { get; set; }

        /// <summary>
        /// Кнопка отмены действия
        /// </summary>
        private Button CancelButton { get; set; }

        public void InitializeCreateNewObject()
        {
            ApplyBtn = CreateApplyButton();
            CancelButton = CreateCancelButton();
            
            NavigatorDataGridView.Controls.Add(ApplyBtn);
            NavigatorDataGridView.Controls.Add(CancelButton);

            PositionCreateButtons();

            NavigatorDataGridView.Resize += NavigatorDataGridView_Resize; 

            ApplyBtn.BringToFront();
            CancelButton.BringToFront();
        }

        private void NavigatorDataGridView_Resize(object sender, EventArgs e)
        {
            PositionCreateButtons();
        }

        /// <summary>
        /// Создание кнопки подтверждения на форме
        /// </summary>
        /// <returns>Кнопка подтверждения</returns>
        private Button CreateApplyButton()
        {
            var btn = new Button();
            btn.Size = new Size(120, 25);
            btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn.Text = "Подтвердить";
            btn.Click += ApplyBtn_Click;
            btn.Visible = false;
            return btn;
        }

        /// <summary>
        /// Создание кнопки отмены на форме
        /// </summary>
        /// <returns>Кнопка отмены</returns>
        private Button CreateCancelButton()
        {
            var btn = new Button();
            btn.Size = new Size(120, 25);
            btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn.Text = "Отмена";
            btn.Click += CancelBtn_Click;
            btn.Visible = false;
            return btn;
        }

        private void PositionCreateButtons()
        {
            if (ApplyBtn == null || CancelButton == null)
                return;

            int margin = 8;

            int y = NavigatorDataGridView.ClientSize.Height
                    - ApplyBtn.Height
                    - margin;

            ApplyBtn.Location = new Point(
                NavigatorDataGridView.ClientSize.Width
                - ApplyBtn.Width
                - margin,
                y
            );

            CancelButton.Location = new Point(
                ApplyBtn.Left
                - CancelButton.Width
                - margin,
                y
            );
        }

        /// <summary>
        /// Скрытие всех контролов для создания заметки
        /// </summary>
        private void HideControls()
        {
            ApplyBtn.Visible = false;
            CancelButton.Visible = false;
        }

        /// <summary>
        /// Показ контролов для создания заметки
        /// </summary>
        private void ShowControls()
        {
            ApplyBtn.Visible = true;
            CancelButton.Visible = true;
        }
        #endregion

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CancelEdit();
        }

        private void CancelEdit()
        {
            if (_editingRow == null) return;

            if (_isNewRow)
                _rows.Remove(_editingRow);

            _editingRow = null;
            _isNewRow = false;
            _isEditing = false;

            SetTableReadonlyProperty();
            NavigatorDataGridView.Invalidate();
            HideControls();
        }

        private async void ApplyBtn_Click(object sender, EventArgs e)
        {
            if (_editingRow == null)
                return;

            NavigatorDataGridView.EndEdit();

            // Валидируем все атрибуты перед созданием DTO
            foreach (var attr in _editingRow.Attributes.Values)
            {
                // Если поле обязательное
                if (attr.IsRequired)
                {
                    if (attr.IsReference && (!attr.ReferenceID.HasValue || attr.ReferenceID == Guid.Empty))
                    {
                        MessageBox.Show($"Поле '{attr.Name}' обязательно для заполнения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!attr.IsReference && string.IsNullOrWhiteSpace(attr.Value))
                    {
                        MessageBox.Show($"Поле '{attr.Name}' обязательно для заполнения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Валидируем значение на соответствие типу
                if (!attr.IsReference && !string.IsNullOrWhiteSpace(attr.Value))
                {
                    if (!_validationService.ValidateSingleValue(attr.Value, attr.ValueType, out string error))
                    {
                        MessageBox.Show(error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            ObjectDTO dto;

            try
            {
                dto = BuildObjectDto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditing = true;
                ShowControls();
                return;
            }

            try
            {
                if (_isNewRow)
                {
                    await _objectService.CreateObjectAsync(_currentTypeId, dto);
                    _editingRow.ObjectId = dto.ID;

                    var fileAttr = _editingRow.Attributes.Values
                        .FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Value) && System.IO.File.Exists(a.Value));

                    if (fileAttr != null)
                    {
                        var filePath = fileAttr.Value;
                        if(await _fileService.UploadFileAsync(dto.ID, filePath))
                        {
                            MessageBox.Show("Файл загружен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Файл не загружен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    await _objectService.UpdateObjectAsync(_editingRow.ObjectId, dto);
                    await RefreshRowAsync(_editingRow, dto);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsEditing = true;
                ShowControls();
                return;
            }

            _editingRow = null;
            IsEditing = false;

            NavigatorDataGridView.Invalidate();
            HideControls();
        }

        private ObjectDTO BuildObjectDto()
        {
            var dto = new ObjectDTO
            {
                ID = Guid.NewGuid(),
                Attributes = new Dictionary<Guid, ObjectAttributeDTO>()
            };

            foreach (var attr in _editingRow.Attributes.Values)
            {
                if (attr.IsRequired)
                {
                    if (attr.IsReference && (!attr.ReferenceID.HasValue || attr.ReferenceID == Guid.Empty))
                        throw new Exception($"Поле '{attr.Name}' обязательно");
                    if (!attr.IsReference && string.IsNullOrWhiteSpace(attr.Value))
                        throw new Exception($"Поле '{attr.Name}' обязательно");
                }

                dto.Attributes[attr.ID] = new ObjectAttributeDTO
                {
                    ID = attr.ID,
                    Name = attr.Name,
                    ValueType = attr.ValueType,
                    Value = attr.IsReference ? attr.ReferenceID.ToString() : attr.Value
                };
            }

            return dto;
        }


        private async Task RefreshRowAsync(DynamicObjectRow row, ObjectDTO dto)
        {
            foreach (var dtoAttr in dto.Attributes.Values)
            {
                if (!row.Attributes.TryGetValue(dtoAttr.ID, out var vm))
                    continue;

                if (vm.IsReference)
                {
                    if (Guid.TryParse(dtoAttr.Value, out Guid parsedId))
                    {
                        vm.ReferenceID = parsedId;
                    }
                    else
                    {
                        vm.ReferenceID = null;
                    }

                    if (vm.ReferenceID.HasValue)
                    {
                        var obj = await _objectService
                            .GetObjectInfoByIdAsync(vm.ReferenceID.Value);

                        vm.Value = obj?.Title;
                    }
                    else
                    {
                        vm.Value = null;
                    }
                }
                else
                {
                    vm.Value = dtoAttr.Value;
                }
            }

            var index = _rows.IndexOf(row);
            if (index >= 0)
                NavigatorDataGridView.InvalidateRow(index);
        }

        /// <summary>
        /// Устанавливает режим ReadOnly для всех строк, кроме редактируемой
        /// </summary>
        private void SetTableReadonlyProperty()
        {
            if (_rows == null || _editingRow == null)
                return;

            foreach (DataGridViewColumn col in NavigatorDataGridView.Columns)
            {
                foreach (DataGridViewRow row in NavigatorDataGridView.Rows)
                {
                    bool isEditingRow = row.DataBoundItem == _editingRow;
                    row.Cells[col.Index].ReadOnly = !isEditingRow;
                }
            }

            NavigatorDataGridView.AllowUserToAddRows = false;

            NavigatorDataGridView.Invalidate();
        }
    }
}
