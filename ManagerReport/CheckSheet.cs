using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagerReport
{
    public class CheckSheet : INotifyPropertyChanged
    {
        private string nameCheckSheet;
        private bool statusDone;

        public string NameCheckSheet { get => nameCheckSheet; set => nameCheckSheet = value; }
        public bool StatusDone
        {
            set
            {
                statusDone = value;
                OnPropertyChanged();
            }
            get { return statusDone; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
