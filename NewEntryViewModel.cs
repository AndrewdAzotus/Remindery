
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Remindery;

namespace Remindery
{
    public class NewEntryViewModel : INotifyPropertyChanged
    {
        #region Initialisation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion

        private Remindery.RemEntry _CreateNewEnt;
        public Remindery.RemEntry CreateNewEnt { get => _CreateNewEnt; set => _CreateNewEnt = value; }

        private string _Comment; // = "What Ever";
        public string Comment
        {
            get => _Comment;
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    NotifyPropertyChanged("Comment");
                }
            }
        }

        public NewEntryViewModel()
        {
            // Comment = "Boo!";

            
        }
    }
}
