
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azotus;
using System.Collections.ObjectModel;

namespace Remindery
{
    public partial class Remindery
    {
        private string _comment;
        public string Comment // in new entry form & remideryviewmodel
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    NotifyPropertyChanged("Comment");
                }
            }
        }

        //public class TimeFrames
        //{
        //    public string idx;             //      Y1
        //    public string dspyText;        // e.g. Yearly
        //    public string timeFrame;       //      Year
        //    public int qty;                //      1
        //}

        public class RemEntry
        {
            private DateTime _whenDue;
            private string _comment;
            private string _recurIn;            // >> timeframe
            private bool _privateEntry = true;
            private bool _Completed = false;

            public DateTime WhenDue { get => _whenDue; set => _whenDue = value; }
            public string RecurIn { get => _recurIn; set => _recurIn = value; }
            public bool Completed { get => _Completed; set => _Completed = value; }
            public string Comment { get => _comment; set => _comment = value; }
            public bool PrivateEntry { get => _privateEntry; set => _privateEntry = value; }
            public string ShrdPriv
            {
                set { }
                get
                {
                    return (_privateEntry ? "Prv" : "Shr");
                }
            }

            public RemEntry() { }
            public RemEntry(string comment)
            {
                Comment = comment;
            }

            public bool Update()
            {
                bool rc = false;
                string pd = RecurIn.Substring(0, 1);
                int.TryParse(RecurIn.Substring(1), out int qty);

                switch (pd)
                {
                    case "D":
                        WhenDue = WhenDue.AddDays(qty);
                        break;
                    case "M":
                        WhenDue = WhenDue.AddMonths(qty);
                        break;
                    case "Y":
                        WhenDue = WhenDue.AddYears(qty);
                        break;
                    default:
                        // dunno!
                        // throw exception???
                        break;
                }
                return rc;
            }
        }

        //private RemEntry _EntNew;
        //public RemEntry EntNew
        //{
        //    get => _EntNew;
        //    set
        //    {
        //        if (_EntNew != value)
        //        {
        //            _EntNew = value;
        //            NotifyPropertyChanged("EntNew");
        //        }
        //    }
        //}

        //public class RemEntries : Profile
        //{
        //    public ObservableCollection<RemEntry> entries = new ObservableCollection<RemEntry>();
        //    public ObservableCollection<RemEntry> Entries { get => entries; set => entries = value; }
        //}
    }
}
