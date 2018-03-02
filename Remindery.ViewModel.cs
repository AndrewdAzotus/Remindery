
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Script.Serialization;
using System.Windows.Input;

using Azotus;
using System.Reflection;
using static Remindery.BaseMVVM;
// using static Remindery.MainWindow;

namespace Remindery
{
    public partial class Remindery : INotifyPropertyChanged
    {
        //private string _comment;
        //public string Comment { get => _comment; set => _comment = value; }

        private RemEntry _EntNew;
        public RemEntry EntNew
        {
            get => _EntNew;
            set
            {
                if (_EntNew != value)
                {
                    _EntNew = value;
                    NotifyPropertyChanged("EntNew");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #region ICommand-Defintions
        //public class CommandHandler : ICommand
        //{
        //    private Action _action;
        //    private bool _canExecute;
        //    public CommandHandler(Action action, bool canExecute)
        //    {
        //        _action = action;
        //        _canExecute = canExecute;
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return _canExecute;
        //    }

        //    public event EventHandler CanExecuteChanged;

        //    public void Execute(object parameter)
        //    {
        //        _action();
        //    }
        //}

        private ICommand _CmdExit;
        public ICommand CmdExit
        {
            get
            {
                return _CmdExit ?? (_CmdExit = new CommandHandler(() => ExecCmdExit(), _canExecCmdExit));
            }
        }

        private ICommand _CmdSave;
        public ICommand CmdSave
        {
            get
            {
                return _CmdSave ?? (_CmdSave = new CommandHandler(() => ExecCmdSave(), _canExecCmdSave));
            }
        }

        private ICommand _CmdNewEntry;
        public ICommand CmdNewEntry
        {
            get
            {
                return _CmdNewEntry ?? (_CmdNewEntry = new CommandHandler(() => NewEntry(), _canExecCmdNewEntry));
            }
        }
        #endregion

        #region Button-Commands
        private bool _canExecCmdExit = true;
        public void ExecCmdExit()
        {
            Console.WriteLine(">>> Exiting");
            Save(nameSettings, pathSettings);
            // this next line is from somewhere else and demonstrates how to update a variable bound in a form
            // AddingCategory = false;
        }

        private bool _canExecCmdSave = true;
        public void ExecCmdSave()
        {
            Console.WriteLine(">>> Saving");
            Save(nameSettings, pathSettings);
        }

        private bool _canExecCmdNewEntry = true;
        public void NewEntry()
        {
            NewEntry frm = new NewEntry(this);

            EntNew = new RemEntry();
            EntNew.Comment = "New Entry 1";

            Comment = "New Entry 3";
            Console.WriteLine(">>> Create New Entry");
                
            frm.ShowDialog();
            string comment = Comment;
        }
        #endregion

        #region alter-var-bound-in-form
        bool _AddingCategory = false;
        public bool AddingCategory
        {
            get => _AddingCategory;
            set
            {
                if (_AddingCategory != value)
                {
                    _AddingCategory = value;
                    NotifyPropertyChanged("AddingCategory");
                }
            }
        }

        private string _NamePgm;
        public string NamePgm
        {
            get => _NamePgm;
            set
            {
                if (_NamePgm != value)
                {
                    _NamePgm = value;
                    NotifyPropertyChanged("NamePgm");
                }
            }
        }
        #endregion

        #region other-variables-and-properties
        private bool _UpdatedEntries = false;
        public bool UpdatedEntries
        {
            get => _UpdatedEntries;
            set
            {
                if (_UpdatedEntries != value)
                {
                    _UpdatedEntries = value;
                    NamePgm = $"{GetType().Namespace}{(_UpdatedEntries ? " [*]" : "")}";
                }
            }
        }
        #endregion

        Profile profile = new Profile();
        readonly string pathSettings, nameSettings;

        //List<TimeFrames> timeFrames;
        private List<RemEntry> ents = new List<RemEntry>();
        public List<RemEntry> Ents { get => ents; set => ents = value; }


        public Remindery()
        {
            // sets window title
            UpdatedEntries = false;

            //timeFrames = new List<TimeFrames>();
            //TimeFrames tf = new TimeFrames
            //{
            //    idx = "Y1",
            //    dspyText = "Yearly",
            //    timeFrame = "Year",
            //    qty = 1
            //};
            //timeFrames.Add(tf);

            //string fred;
            //fred = Environment.CurrentDirectory;                                            // Execution Folder, e.g. @"A:\vs- Projects\CanopyBackup\CanopyBackup\bin\Debug"
            //fred = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);    // "C:\\Users\\andrew\\AppData\\Roaming"
            //fred = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (Debugger.IsAttached)
                pathSettings = @"D:\[Azotus-Settings]";
            else
                pathSettings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // "C:\\Users\\andrew\\AppData\\Roaming"
            nameSettings = $"{GetType().Namespace}.json";

            //remList = new RemEntries();
            //reminderList = new List<RemEntry>();
            if (File.Exists(profile.VerifyFileName(nameSettings, pathSettings)))
                Load(nameSettings, pathSettings);
            else
                InitRemidery();

            //foreach (RemEntry re in reminderList.Entries)
            //foreach (RemEntry re in remEnts)
            //{
            //    Console.WriteLine($"Remindery-Init-{re.Comment}");
            //}
        }

    // commented because it gets called during start up as well as exit
    ~Remindery()
        {

            try
            {
                //Remindery..Save($"{namePgm}-Entries.json", "");
            }
            catch (Exception exc)
            { string em = exc.Message; }
            Console.WriteLine("Exiting");
        }

        private void InitRemidery()
        {
            RemEntry entry;
            //entry = new RemEntry
            //{
            //    Comment = "New Year's Day",
            //    WhenDue = new DateTime(DateTime.Today.Year, 1, 1),
            //    RecurIn = "Y1",
            //    PrivateEntry = true
            //};
            //Ents.Add(entry);

            entry = new RemEntry
            {
                Comment = "Christmas Day",
                WhenDue = new DateTime(DateTime.Today.Year, 12, 25),
                RecurIn = "Y1",
                PrivateEntry = true
            };
            Ents.Add(entry);

            //entry = new RemEntry
            //{
            //    Comment = "New Year's Eve",
            //    WhenDue = new DateTime(DateTime.Today.Year, 12, 31),
            //    RecurIn = "Y1",
            //    PrivateEntry = true
            //};
            //Ents.Add(entry);
        }
        public void Load(string jsonFileName = null, string fldrPath = null)
        {
            string fullName = profile.VerifyFileName(jsonFileName, fldrPath);
            if (File.Exists(fullName))
            {
                string jsonFile = File.ReadAllText(fullName);
                dynamic stuff = new JavaScriptSerializer().DeserializeObject(jsonFile);

                foreach (dynamic rawEntry in stuff)
                {
                    RemEntry entry = new RemEntry();
                    foreach (KeyValuePair<string, object> kvp in rawEntry)
                    {
                        entry.GetType().GetProperty(kvp.Key.ToString(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(entry, kvp.Value);
                    }
                    ents.Add(entry);
                }
            }
        }
        public void Save(string jsonFileName = null, string fldrPath = null, bool CreatePath = true)
        {
            string fullName;
            if (fldrPath==null)
                fldrPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // "C:\\Users\\andrew\\AppData\\Roaming"
            fullName = profile.VerifyFileName(jsonFileName, fldrPath);

            if (!Directory.Exists(fldrPath))
                if (CreatePath)
                    Directory.CreateDirectory(fldrPath);

            if (!string.IsNullOrEmpty(fullName))
            {
                try
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string jsonOutput = jss.Serialize(Ents);
                    File.WriteAllText(fullName, jsonOutput);
                    UpdatedEntries = false;
                }
                catch (Exception exc)
                {
                    string bert = exc.Message;
                }
            }
        }
    }
}
