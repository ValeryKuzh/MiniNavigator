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

            // Завершаем редактирование
            NavigatorDataGridView.EndEdit();
            NavigatorDataGridView.CurrentCell = null;

            if (!_validationService.ValidateTypesForRow(_newRow, _table, out string error))
            {
                MessageBox.Show(error, "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int rowIndex = _table.Rows.IndexOf(_newRow);
            var gridRow = NavigatorDataGridView.Rows[rowIndex];

            // Фиксируем строку
            gridRow.ReadOnly = true;
            foreach (DataGridViewCell cell in gridRow.Cells)
                cell.ReadOnly = true;

            // Здесь можно отправить данные на сервис
            // var values = new Dictionary<string, object>();
            // foreach (DataColumn col in _table.Columns)
            //     values[col.ColumnName] = _newRow[col];
            // await _objectService.CreateObjectAsync(values);

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
