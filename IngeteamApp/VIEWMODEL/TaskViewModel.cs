using IngeteamApp.MODEL;
using static IngeteamApp.MODEL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IngeteamApp.Core.Commands;
using System.Collections.ObjectModel;
//using System.Threading.Tasks;

namespace IngeteamApp.VIEWMODEL
{
    public class State
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public State (int ind, string cont)
        {
            Index = ind;
            Content = cont;
        }
        
    }


    public class TaskViewModel : IGeneric
    {

        public static ObservableCollection<State> StatesList = new ObservableCollection<State>()
        {
            new State (0,"New"),
            new State (1,"Resolved"),
            new State (2,"InProgress"),
            new State (3,"Feedback"),
            new State (4,"Closed")

        };

        /*public ObservableCollection<string> StatesList = new ObservableCollection<string>()
        {
            "New",
            "Resolved",
            "InProgress",
            "Feedback",
            "Closed"
        };*/
        


        private string _currentName;
        public string CurrentName
        {
            get { return _currentName; }
            set
            {
                _currentName = value;
                RaisePropertyChanged("CurrentName");
            }
        }
        private double _currentTime;
        public double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                RaisePropertyChanged("CurrentTime");
            }
        }
        private DateTime _currentStartDate;
        public DateTime CurrentStartDate
        {
            get { return _currentStartDate; }
            set
            {
                _currentStartDate = value;
                RaisePropertyChanged("CurrentStartDate");
            }
        }

        private DateTime _currentCloseDate;
        public DateTime CurrentCloseDate
        {
            get { return _currentCloseDate; }
            set
            {
                _currentCloseDate = value;
                RaisePropertyChanged("CurrentCloseDate");
            }
        }

        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RaisePropertyChanged("CurrentState");
                
            }
        }
        private string _currentDescription;
        public string CurrentDescription
        {
            get { return _currentDescription; }
            set
            {
                _currentDescription = value;
                RaisePropertyChanged("CurrentDescription");
            }
        }
        private int _currentProgrammerId;
        public int CurrentProgrammerId
        {
            get { return _currentProgrammerId; }
            set
            {
                _currentProgrammerId = value;
                RaisePropertyChanged("CurrentProgrammerId");
            }
        }



        public TaskViewModel()
        {

        }


        private TaskCollection _listTasks = new TaskCollection();

        public TaskCollection ListTasks
        {
            get { return _listTasks; }
            set
            {
                _listTasks = value;
                RaisePropertyChanged("ListTasks");
            }
        }
        private Task _currentTask;

        public Task CurrentTask
        {
            get { return _currentTask; }
            set
            {
                _currentTask = value;
                if (_currentTask != null)
                {
                    CurrentName = _currentTask.Name;
                    CurrentTime = _currentTask.Time;
                    CurrentStartDate = _currentTask.Start;
                    CurrentCloseDate = _currentTask.Close;
                    CurrentState = _currentTask.State;
                    CurrentDescription = _currentTask.Description;
                    CurrentProgrammerId = _currentTask.ProgrammerId;
                }
                else
                {
                    CurrentName = null;
                    CurrentTime = 0;
                    CurrentStartDate = new DateTime();
                    CurrentCloseDate = new DateTime();
                    CurrentState = null;
                    CurrentDescription = null;
                    CurrentProgrammerId = -1;

                }

                RaisePropertyChanged("CurrentTask");
                RaisePropertyChanged("somethingSelected");
                
            }
        }


        private bool somethingSelected
        {
            get { return CurrentTask != null; }
        }

        private ICommand _showTasksCommand;

        public ICommand ShowTasksCommand
        {
            get
            {
                if (_showTasksCommand == null)
                    _showTasksCommand = new RelayCommand(new Action(LoadTasks));
                return _showTasksCommand;
            }
        }





        private void LoadTasks()
        {
            ListTasks = App.DbConnector.loadTasks();
        }



    } 
}
