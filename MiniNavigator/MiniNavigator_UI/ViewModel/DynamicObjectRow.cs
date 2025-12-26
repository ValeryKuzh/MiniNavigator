using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.ViewModel
{
    public class DynamicObjectRow : INotifyPropertyChanged
    {
        public Guid ObjectId { get; set; }

        public Dictionary<Guid, ObjectAttributeViewModel> Attributes { get; } = new Dictionary<Guid, ObjectAttributeViewModel>();

        public string this[string columnName]
        {
            get => Attributes.Values.FirstOrDefault(a => a.Name == columnName)?.Value;
            set
            {
                var attr = Attributes.Values.FirstOrDefault(a => a.Name == columnName);
                if (attr != null)
                    attr.Value = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
