using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI
{
    public partial class NavigatorForm
    {
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

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
    }
}
