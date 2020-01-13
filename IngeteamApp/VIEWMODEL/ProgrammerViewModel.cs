using IngeteamApp.Core.Commands;
using IngeteamApp.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static IngeteamApp.MODEL.Programmer;

namespace IngeteamApp.VIEWMODEL
{
    public class ProgrammerViewModel : IGeneric
    {
        private ProgrammerCollection _listProgrammers = new ProgrammerCollection();

        public ProgrammerCollection ListProgrammers
        {
            get { return _listProgrammers; }
            set 
            { 
                _listProgrammers = value;
                RaisePropertyChanged("ListProgrammers"); 
            }
        }

        private Programmer _currentProgrammer;

        public Programmer CurrentProgrammer
        {
            get { return _currentProgrammer; }
            set 
            {
                _currentProgrammer = value;
                if (_currentProgrammer != null)
                {
                    CurrentName = _currentProgrammer.Name;
                    CurrentSurname = _currentProgrammer.Surname;
                    CurrentEmail = _currentProgrammer.Email;
                    CurrentResidence = _currentProgrammer.Residence;
                    CurrentNumber = _currentProgrammer.Number;
                }
                else
                {
                    CurrentName = null;
                    CurrentSurname = null;
                    CurrentEmail = null;
                    CurrentResidence = null;
                    CurrentNumber = null;

                }

                RaisePropertyChanged("CurrentProgrammer");
                RaisePropertyChanged("somethingSelected");
            }
        }

        private string _currentName;
        public string CurrentName
        {
            get { return _currentName; }
            set
            {
                _currentName = value;
                if (!string.IsNullOrEmpty(_currentName)) _currentName.Trim();
                RaisePropertyChanged("CurrentName");
            }
        }
        private string _currentSurname;
        public string CurrentSurname
        {
            get { return _currentSurname; }
            set
            {
                _currentSurname = value;
                if (!string.IsNullOrEmpty(_currentSurname)) _currentSurname.Trim();
                RaisePropertyChanged("CurrentSurname");
            }
        }
        private string _currentEmail;
        public string CurrentEmail
        {
            get { return _currentEmail; }
            set
            {
                _currentEmail = value;
                if (!string.IsNullOrEmpty(_currentEmail)) _currentEmail.Trim();
                RaisePropertyChanged("CurrentEmail");
            }
        }

        private string _currentResidence;
        public string CurrentResidence
        {
            get { return _currentResidence; }
            set
            {
                _currentResidence = value;
                if (!string.IsNullOrEmpty(_currentResidence)) _currentResidence.Trim();
                RaisePropertyChanged("CurrentResidence");
            }
        }
        private string _currentNumber;
        public string CurrentNumber
        {
            get { return _currentNumber; }
            set
            {
                _currentNumber = value;
                if (!string.IsNullOrEmpty(_currentNumber)) _currentNumber.Trim();
                RaisePropertyChanged("CurrentNumber");
            }
        }
        private ICommand _showProgrammersCommand;

        public ICommand ShowProgrammersCommand
        {
            get
            {
                if (_showProgrammersCommand == null)
                    _showProgrammersCommand = new RelayCommand(new Action(LoadProgrammers));
                return _showProgrammersCommand;
            }
        }
        private ICommand _removeProgrammersCommand;

        public ICommand RemoveProgrammersCommand
        {
            get
            {
                if (_removeProgrammersCommand == null)
                    _removeProgrammersCommand = new RelayCommand(new Action(RemoveProgrammer), () => somethingSelected);
                return _removeProgrammersCommand;
            }
        }
        private ICommand _addProgrammersCommand;

        public ICommand AddProgrammersCommand
        {
            get
            {
                if (_addProgrammersCommand == null)
                    _addProgrammersCommand = new RelayCommand(new Action(AddProgrammer));
                return _addProgrammersCommand;
            }
        }

        private ICommand _clearCommand;

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand(new Action(ClearCurrent));
                return _clearCommand;
            }
        }

        private ICommand _modifyCommand;

        public ICommand ModifyCommand
        {
            get
            {
                if (_modifyCommand == null)
                    _modifyCommand = new RelayCommand(new Action(ModifyCurrent), () => somethingSelected);
                return _modifyCommand;
            }
        }



        public ProgrammerViewModel()
        {

        }
        private bool somethingSelected
        {
            get { return CurrentProgrammer != null; }
        }

        private void LoadProgrammers()
        {
            ListProgrammers = App.DbConnector.loadProgrammers();
        }

        private void ClearCurrent()
        {
            CurrentProgrammer = null;
            RaisePropertyChanged("CurrentProgrammer");

        }
        private void RemoveProgrammer()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to delete: " + CurrentProgrammer.ToString() + "?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                //ListProgrammers.Remove(CurrentProgrammer);
                //MessageBox.Show(CurrentProgrammer.Id.ToString());
                App.DbConnector.removeProgrammers(CurrentProgrammer);
                LoadProgrammers();
                ClearCurrent();
                                
            }
               
        }
        private void AddProgrammer()
        {
            //Check first if the NOT NULL fields are null or only blanks
            if(!string.IsNullOrEmpty(CurrentName) & !string.IsNullOrWhiteSpace(CurrentName) & !string.IsNullOrEmpty(CurrentSurname) & !string.IsNullOrWhiteSpace(CurrentSurname))
            {
                Programmer p = new Programmer(1, CurrentName, CurrentSurname, CurrentEmail, CurrentResidence, CurrentNumber);
                CurrentProgrammer = null;
                App.DbConnector.addProgrammer(p);
                LoadProgrammers();
                ClearCurrent();
            }
            else
            {
                MessageBox.Show("Fields with * must be fulfilled");
            }

            
        }

        private void ModifyCurrent()
        {
            //Check first if the NOT NULL fields are null, empty or only blanks
            if (!string.IsNullOrEmpty(CurrentName) & !string.IsNullOrWhiteSpace(CurrentName) & !string.IsNullOrEmpty(CurrentSurname) & !string.IsNullOrWhiteSpace(CurrentSurname))
            {
                Programmer p = new Programmer(1, CurrentName, CurrentSurname, CurrentEmail, CurrentResidence, CurrentNumber);
                App.DbConnector.modifyProgrammer(p, CurrentProgrammer);
                LoadProgrammers();
                ClearCurrent();
            }
            else
            {
                MessageBox.Show("Fields with * must be fulfilled");
            }

        }

    }
}
