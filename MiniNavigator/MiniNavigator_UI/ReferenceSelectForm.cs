using MiniNavigator_Services.Service.Interface;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniNavigator_UI
{
    public partial class ReferenceSelectForm : Form
    {
        private readonly IObjectService _objectService;

        private Guid AttributeID;
        public object SelectedTitle { get; internal set; }
        public object SelectedID { get; internal set; }
        public ReferenceSelectForm(Guid attributeID, IObjectService objectService)
        {
            InitializeComponent();   

            this.AttributeID = attributeID;

            _objectService = objectService;

            this.Load += ReferenceSelectForm_Load;
        }

        private async void ReferenceSelectForm_Load(object sender, EventArgs e)
        {
            BindComboBox();
        }

        private async Task BindComboBox()
        {
            var list = await _objectService.GetReferenceObjectInfosByAttribute(AttributeID);

            ObjectComboBox.DataSource = list;
            ObjectComboBox.DisplayMember = "Title";
            ObjectComboBox.ValueMember = "ID";

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            SelectedTitle = ObjectComboBox.Text;
            SelectedID = ObjectComboBox.SelectedValue;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
