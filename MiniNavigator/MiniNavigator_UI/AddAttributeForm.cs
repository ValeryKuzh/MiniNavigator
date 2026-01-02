using MiniNavigator_Services.DTO;
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
    public partial class AddAttributeForm : Form
    {
        public string AttributeName => NameAttributeTextBox.Text.Trim();
        public bool IsRequired => IsRequiredCheckBox.Checked;
        public bool IsVisible => IsVisibleCheckBox.Checked;
        public bool IsReference => IsreferenceCheckBox.Checked;
        public bool IsTitle => IsTitleCheckBox.Checked;

        public Type SelectedValueType =>
            DataTypeComboBox.SelectedItem as Type;

        public Guid? SelectedReferenceTypeId =>
            IsReference
                ? (ReferenceTypeComboBox.SelectedItem as ObjectTypeDTO)?.ID
                : null;

        private readonly List<ObjectTypeDTO> _objectTypes;


        public AddAttributeForm(List<ObjectTypeDTO> objectTypes)
        {
            InitializeComponent();

            _objectTypes = objectTypes;

            InitDataTypes();
            InitReferenceTypes();
            InitEvents();
        }

        private void InitDataTypes()
        {
            DataTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            DataTypeComboBox.Items.AddRange(new object[]
            {
                typeof(string),
                typeof(int),
                typeof(byte),
                typeof(long),
                typeof(decimal),
                typeof(bool),
                typeof(DateTime)
            });

            DataTypeComboBox.Format += (s, e) =>
            {
                if (e.ListItem is Type t)
                    e.Value = GetFriendlyTypeName(t);
            };

            DataTypeComboBox.SelectedIndex = 0;
        }

        private string GetFriendlyTypeName(Type type)
        {
            if (type == typeof(string)) return "Строка";
            if (type == typeof(int)) return "Целое число";
            if (type == typeof(byte)) return "Маленькое целое число";
            if (type == typeof(long)) return "Большое целое число";
            if (type == typeof(decimal)) return "Дробное число";
            if (type == typeof(bool)) return "Логическое";
            if (type == typeof(DateTime)) return "Дата";

            return type.Name;
        }

        private void InitReferenceTypes()
        {
            ReferenceTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            ReferenceTypeComboBox.DataSource = _objectTypes;
            ReferenceTypeComboBox.DisplayMember = "Title";
            ReferenceTypeComboBox.ValueMember = "ID";

            ReferenceTypeComboBox.Enabled = false;
        }

        private void InitEvents()
        {
            IsreferenceCheckBox.CheckedChanged += (s, e) =>
            {
                ReferenceTypeComboBox.SelectedItem = null;
                ReferenceTypeComboBox.Enabled = IsreferenceCheckBox.Checked;
                DataTypeComboBox.Enabled = !IsreferenceCheckBox.Checked;

                if (IsreferenceCheckBox.Checked)
                {
                    DataTypeComboBox.SelectedItem = null;
                    DataTypeComboBox.SelectedItem = typeof(Guid);
                }
                else
                {
                    DataTypeComboBox.SelectedItem = null;
                }
            };

            CancelBtn.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            OkBtn.Click += OkBtn_Click;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AttributeName))
            {
                MessageBox.Show(
                    "Введите название атрибута",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (IsReference && ReferenceTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите тип объекта для ссылки",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!IsReference && SelectedValueType == null)
            {
                MessageBox.Show(
                    "Выберите тип данных",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
