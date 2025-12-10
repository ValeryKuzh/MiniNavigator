using System;
using System.ComponentModel;

namespace MiniNavigator_UI.ViewModel
{
    public class NavObjectViewModel
    {
        public Guid ID { get; set; }
        public Guid? ObjectTypeID { get; set; }
        public string ObjectTitle { get; set; }
        public Guid? ParentID { get; set; }
        public string ParentTitle { get; set; }

        public BindingList<NavObjectViewModel> Children = new BindingList<NavObjectViewModel>();
    }
}
