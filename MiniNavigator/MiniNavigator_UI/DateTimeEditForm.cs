using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI
{
    /// <summary>
    /// Форма для работы с DateTime атрибутами
    /// </summary>
    public partial class DateTimeEditForm : Form
    {
        public DateTime SelectedDate { get; private set; }

        public DateTimeEditForm(DateTime? currentDate = null)
        {
            InitializeComponent();

            EditDateTimePicker.Format = DateTimePickerFormat.Custom;
            EditDateTimePicker.CustomFormat = "dd.MM.yyyy";

            if (currentDate.HasValue)
            {
                EditDateTimePicker.Value = currentDate.Value;
            }
            else
            {
                EditDateTimePicker.Value = DateTime.Now;
            }

            CancelBtn.Click += CancelBtn_Click;
            OkBtn.Click += OkBtn_Click;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            SelectedDate = EditDateTimePicker.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
