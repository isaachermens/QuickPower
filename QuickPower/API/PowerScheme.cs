using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPower.API
{
    public class PowerScheme : ICloneable, INotifyPropertyChanged
    {
        private bool _active;

        public string Name { get; set; }
        public Guid ID { get; set; }
        public bool Active { get { return _active; } set { _active = value; RaisePropertyChanged("Active"); } }

        public PowerScheme(Guid id, string name, bool active)
        {
            ID = id;
            Name = name;
            Active = active;
        }

        public object Clone()
        {
            return new PowerScheme(ID, Name, Active);
        }

        public override bool Equals(object obj)
        {
            var scheme = obj as PowerScheme;
            return scheme != null && scheme.ID.Equals(this.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
