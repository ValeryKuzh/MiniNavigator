using MiniNavigator_Services.DTO;
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

        private DataRow _newRow;
        private bool IsEditing {  get; set; }

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
            if (_newRow != null)
            {
                _table.Rows.Remove(_newRow);
                _newRow = null;
            }

            IsEditing = false;
            SetTableReadonlyProperty();
            ChangeSortMode(IsEditing);
            HideControls();
        }

        private async void ApplyBtn_Click(object sender, EventArgs e)
        {
            if (_newRow == null)
                return;

            NavigatorDataGridView.EndEdit();

            var typeId = (Guid)_table.ExtendedProperties["TypeID"];
            CreateObjectDTO newObject = CreateNewObjectDTO();

            try
            {
                // ВСЯ валидация обязательных атрибутов происходит ТУТ
                await _objectService.CreateObjectAsync(typeId, newObject);
            }
            catch (ArgumentException ex)
            {
                // Обязательные атрибуты не заполнены
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Возвращаем редактирование
                IsEditing = true;
                SetTableReadonlyProperty();
                ShowControls();

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                IsEditing = true;
                SetTableReadonlyProperty();
                ShowControls();

                return;
            }

            int rowIndex = _table.Rows.IndexOf(_newRow);
            var gridRow = NavigatorDataGridView.Rows[rowIndex];

            gridRow.ReadOnly = true;
            foreach (DataGridViewCell cell in gridRow.Cells)
                cell.ReadOnly = true;

            for (int i = 0; i < gridRow.Cells.Count; i++)
            {
                if (gridRow.Cells[i] is DataGridViewButtonCell btnCell)
                {
                    var textCell = new DataGridViewTextBoxCell
                    {
                        Value = btnCell.Value
                    };  

                    gridRow.Cells[i] = textCell;
                    textCell.ReadOnly = true;
                }
            }

            _newRow = null;
            IsEditing = false;

            SetTableReadonlyProperty();
            ChangeSortMode(IsEditing);
            HideControls();
        }

        /// <summary>
        /// Ограничивает действия пользователя на гриде, пока добавляется новая строка.
        /// </summary>
        private void RestrictGridDuringCreation()
        {
            if (_newRow != null)
            {
                ChangeSortMode(IsEditing);
            }
        }

        public CreateObjectDTO CreateNewObjectDTO()
        {
            if(_newRow != null)
            {
                return new CreateObjectDTO()
                {
                    ID = Guid.NewGuid(),
                    Attributes = AddAttributesToObjectDTO()
                };
            }
            return null;
        }

        private Dictionary<Guid, ObjectAttributeDTO> AddAttributesToObjectDTO()
        {
            var result = new Dictionary<Guid, ObjectAttributeDTO>();

            int rowIndex = _table.Rows.IndexOf(_newRow);
            if (rowIndex < 0)
                return result;

            var gridRow = NavigatorDataGridView.Rows[rowIndex];

            foreach (DataColumn column in _table.Columns)
            {
                if (!column.ExtendedProperties.ContainsKey("ID"))
                    continue;

                var attributeId = (Guid)column.ExtendedProperties["ID"];
                var isReference = column.ExtendedProperties["IsReference"] as bool? == true;

                string finalValue;

                if (isReference)
                {
                    var cell = gridRow.Cells[column.ColumnName];

                    if (cell.Tag == null)
                    {
                        MessageBox.Show(
                            $"Не выбран объект для ссылки {column.ColumnName}",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        continue;
                    }

                    finalValue = ((Guid)cell.Tag).ToString();
                }
                else
                {
                    var value = _newRow[column];
                    if (value == DBNull.Value || value == null)
                        continue;

                    finalValue = value.ToString();
                }

                result[attributeId] = new ObjectAttributeDTO
                {
                    ID = attributeId,
                    Name = column.ColumnName,
                    ValueType = column.DataType,
                    Value = finalValue
                };
            }

            return result;
        }

        private void SetTableReadonlyProperty()
        {
            if (_table == null) return;

            for (int i = 0; i < NavigatorDataGridView.Rows.Count; i++)
            {
                var gridRow = NavigatorDataGridView.Rows[i];

                if (_newRow != null && i == _table.Rows.IndexOf(_newRow))
                {
                    // только редактируемая строка
                    gridRow.ReadOnly = false;
                    foreach (DataGridViewCell cell in gridRow.Cells)
                        cell.ReadOnly = false;
                }
                else
                {
                    // все остальные строки — readonly
                    gridRow.ReadOnly = true;
                    foreach (DataGridViewCell cell in gridRow.Cells)
                        cell.ReadOnly = true;
                }
            }
        }

        private void ChangeSortMode(bool isEditing)
        {
            if (isEditing && _newRow != null)
            {
                foreach (DataGridViewColumn col in NavigatorDataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                foreach (DataGridViewColumn col in NavigatorDataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
        }
    }
}
