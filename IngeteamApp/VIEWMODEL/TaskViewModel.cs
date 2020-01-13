using IngeteamApp.MODEL;
using static IngeteamApp.MODEL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IngeteamApp.Core.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using IngeteamApp.VIEW;
using System.Windows.Media.Imaging;
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
        #region TASK PART

        /*public ObservableCollection<State> StatesList = new ObservableCollection<State>()
        {
            new State (0,"New"),
            new State (1,"Resolved"),
            new State (2,"InProgress"),
            new State (3,"Feedback"),
            new State (4,"Closed")

        };*/

        private ObservableCollection<string> _statesList = new ObservableCollection<string>() { "New", "Resolved", "In Progress", "Feedback", "Closed"};
        public ObservableCollection<string> StatesList
        {
            get { return _statesList; }
            set
            {
                if (value != _statesList)
                {
                    _statesList = value;
                    RaisePropertyChanged("StatesList");
                }
            }
        }


        private static bool _disableTaskViewer = true;//It is needed one unique unique value shared by windows, si it must be static.
        private bool DisableTaskViewer
        {
            get { return _disableTaskViewer; }
            set
            {
                _disableTaskViewer = value;
                
                RaisePropertyChanged("DisableTaskViewer");
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
        private double? _currentTime;
        public double? CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                RaisePropertyChanged("CurrentTime");

            }
        }
        private DateTime? _currentStartDate = DateTime.Now;
        public DateTime? CurrentStartDate
        {
            get { return _currentStartDate; }
            set
            {
                _currentStartDate = value;
                RaisePropertyChanged("CurrentStartDate");
                //MessageBox.Show(_currentStartDate.ToString());
            }
        }

        private DateTime? _currentCloseDate = DateTime.Now;
        public DateTime? CurrentCloseDate
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
                string previous = _currentState;
                _currentState = value;
                if (!string.IsNullOrEmpty(_currentState)) _currentState.Trim();
                if (string.Equals(_currentState, "Closed") & !string.Equals(previous, _currentState)) CurrentCloseDate = DateTime.Now;
                if(string.Equals(_currentState, "In Progress") & !string.Equals(previous, _currentState) & ((string.Equals(previous, "New") || (string.Equals(previous, "Resolved"))))) CurrentStartDate = DateTime.Now;
                if(string.Equals(_currentState, "New") || string.Equals(_currentState, "Resolved"))
                {
                    CurrentCloseDate = null; CurrentStartDate = null;
                }
                if (string.Equals(previous, "Closed") && !string.Equals(_currentState, "Closed")) CurrentCloseDate = null;
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
                if (!string.IsNullOrEmpty(_currentDescription)) _currentDescription.Trim();
                RaisePropertyChanged("CurrentDescription");
            }
        }
        private int? _currentParentId;
        public int? CurrentParentId
        {
            get { return _currentParentId; }
            set
            {
                _currentParentId = value;
                RaisePropertyChanged("CurrentParentId");
            }
        }
        private int? _currentProgrammerId;
        public int? CurrentProgrammerId
        {
            get { return _currentProgrammerId; }
            set
            {
                _currentProgrammerId = value;
                RaisePropertyChanged("CurrentProgrammerId");
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
                    CurrentTime = null;
                    CurrentStartDate = DateTime.Now;
                    CurrentCloseDate = DateTime.Now;
                    CurrentState = null;
                    CurrentDescription = null;
                    CurrentProgrammerId = null;

                }

                RaisePropertyChanged("CurrentTask");
                RaisePropertyChanged("somethingSelected");

            }
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

        private bool somethingSelected
        {
            get { return CurrentTask != null; }
        }



        public TaskViewModel()
        {

        }



        private ICommand _showTasksCommand;

        public ICommand ShowTasksCommand
        {
            get
            {
                if (_showTasksCommand == null)
                    _showTasksCommand = new RelayCommand(new Action(LoadTasks), () => DisableTaskViewer);
                return _showTasksCommand;
            }
        }

        private ICommand _removeTasksCommand;

        public ICommand RemoveTasksCommand
        {
            get
            {
                if (_removeTasksCommand == null)
                    _removeTasksCommand = new RelayCommand(new Action(RemoveTask), () => (somethingSelected & DisableTaskViewer));
                return _removeTasksCommand;
            }
        }

        private ICommand _clearCommand;

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand(new Action(ClearCurrent), () => DisableTaskViewer);
                return _clearCommand;
            }
        }

        private ICommand _modifyCommand;
        public ICommand ModifyCommand
        {
            get
            {
                if (_modifyCommand == null)
                    _modifyCommand = new RelayCommand(new Action(ModifyCurrentTask), () => (somethingSelected  & DisableTaskViewer));
                return _modifyCommand;
            }
        }
        private ICommand _addTasksCommand;

        public ICommand AddTasksCommand
        {
            get
            {
                if (_addTasksCommand == null)
                    _addTasksCommand = new RelayCommand(new Action(AddTask), () => DisableTaskViewer);
                return _addTasksCommand;
            }
        }


        private void ClearCurrent()
        {
            CurrentTask = null;
            RaisePropertyChanged("CurrentTask");
        }

        private void LoadTasks()
        {
            ListTasks = App.DbConnector.loadTasks();
        }

        private void RemoveTask()
        {
            App.DbConnector.removeTasks(CurrentTask);
            LoadTasks();
            ClearCurrent();
        }
        private void ModifyCurrentTask()
        {
            //Check first if the NOT NULL fields are null, empty or only blanks
            if (!string.IsNullOrEmpty(CurrentName) & !string.IsNullOrWhiteSpace(CurrentName) & !string.IsNullOrEmpty(CurrentState) & !string.IsNullOrWhiteSpace(CurrentState))
            {
                if((CurrentCloseDate == null || CurrentStartDate == null) || (CurrentCloseDate > CurrentStartDate))
                {
                    Task t = new Task(1, CurrentName, CurrentTime, CurrentStartDate, CurrentCloseDate, CurrentState, CurrentDescription, CurrentParentId, CurrentProgrammerId);
                    App.DbConnector.modifyTask(t, CurrentTask);
                    LoadTasks();
                    ClearCurrent();

                }
                else
                {
                    MessageBox.Show("Start Date of the task must be earlier than Closing Date.");
                }
               
            }
            else
            {
                MessageBox.Show("Fields with * must be fulfilled");
            }
        }
        private void AddTask()
        {
            //Check first if the NOT NULL fields are null or only blanks
            if (!string.IsNullOrEmpty(CurrentName) & !string.IsNullOrWhiteSpace(CurrentName) & !string.IsNullOrEmpty(CurrentState) & !string.IsNullOrWhiteSpace(CurrentState))
            {
                if((CurrentCloseDate == null || CurrentStartDate == null) || (CurrentCloseDate > CurrentStartDate))
                {
                    Task t = new Task(1, CurrentName, CurrentTime, CurrentStartDate, CurrentCloseDate, CurrentState, CurrentDescription, CurrentParentId, CurrentProgrammerId); 
                    CurrentTask = null;
                    App.DbConnector.addTask(t);
                    LoadTasks();
                    ClearCurrent();

                }
                else
                {
                    MessageBox.Show("Start Date of the task must be earlier than Close Date.");
                }

            }
            else
            {
                MessageBox.Show("Fields with * must be fulfilled");
            }


        }
        #endregion


        //----------------------------------------------------------------------SUBTASK CODE------------------------------------------------------------------------------


        #region SubTask VIEWMODEL


        private static Task parent;//It is needed one unique unique value shared by windows, si it must be static.
        private bool subSomethingSelected
        {
            get { return CurrentSubTask != null; }
        }

        private static SubTasksViewer _stv = new SubTasksViewer();

        private int? _subCurrentParentId;
        public int? SubCurrentParentId
        {
            get { return _subCurrentParentId; }
            set
            {
                _subCurrentParentId = value;
                RaisePropertyChanged("SubCurrentParentId");
            }
        }
        private string _subCurrentName;
        public string SubCurrentName
        {
            get { return _subCurrentName; }
            set
            {
                _subCurrentName = value;
                if (!string.IsNullOrEmpty(_subCurrentName)) _subCurrentName.Trim();
                RaisePropertyChanged("SubCurrentName");
            }
        }
        private double? _subCurrentTime;
        public double? SubCurrentTime
        {
            get { return _subCurrentTime; }
            set
            {
                _subCurrentTime = value;
                RaisePropertyChanged("SubCurrentTime");

            }
        }
        private DateTime? _subCurrentStartDate = DateTime.Now;
        public DateTime? SubCurrentStartDate
        {
            get { return _subCurrentStartDate; }
            set
            {
                _subCurrentStartDate = value;
                RaisePropertyChanged("SubCurrentStartDate");
            }
        }

        private DateTime? _subCurrentCloseDate = DateTime.Now;
        public DateTime? SubCurrentCloseDate
        {
            get { return _subCurrentCloseDate; }
            set
            {
                _subCurrentCloseDate = value;
                RaisePropertyChanged("SubCurrentCloseDate");
            }
        }

        private string _subCurrentState;
        public string SubCurrentState
        {
            get { return _subCurrentState; }
            set
            {
                string previous = _subCurrentState;
                _subCurrentState = value;
                if (!string.IsNullOrEmpty(_subCurrentState)) _subCurrentState.Trim();
                if (string.Equals(_subCurrentState, "Closed") & !string.Equals(previous, _subCurrentState)) SubCurrentCloseDate = DateTime.Now;
                if (string.Equals(_subCurrentState, "In Progress") & !string.Equals(previous, _subCurrentState) & string.Equals(previous, "New")) SubCurrentStartDate = DateTime.Now;
                if (string.Equals(_subCurrentState, "New") || string.Equals(_subCurrentState, "Resolved"))
                {
                    SubCurrentCloseDate = null; SubCurrentStartDate = null;
                }
                if (string.Equals(previous, "Closed") && !string.Equals(_subCurrentState, "Closed")) SubCurrentCloseDate = null;
                RaisePropertyChanged("SubCurrentState");

            }
        }
        private string _subCurrentDescription;
        public string SubCurrentDescription
        {
            get { return _subCurrentDescription; }
            set
            {
                _subCurrentDescription = value;
                if (!string.IsNullOrEmpty(_subCurrentDescription)) _subCurrentDescription.Trim();
                RaisePropertyChanged("SubCurrentDescription");
            }
        }
        private int? _subCurrentProgrammerId;
        public int? SubCurrentProgrammerId
        {
            get { return _subCurrentProgrammerId; }
            set
            {
                _subCurrentProgrammerId = value;
                RaisePropertyChanged("SubCurrentProgrammerId");
            }
        }

        private TaskCollection _listSubTasks = new TaskCollection();
        public TaskCollection ListSubTasks
        {
            get { return _listSubTasks; }
            set
            {
                _listSubTasks = value;
                RaisePropertyChanged("ListSubTasks");
            }
        }

        private Task _currentSubTask;
        public Task CurrentSubTask
        {
            get { return _currentSubTask; }
            set
            {
                _currentSubTask = value;
                if (_currentSubTask != null)
                {
                    SubCurrentName = _currentSubTask.Name;
                    SubCurrentTime = _currentSubTask.Time;
                    SubCurrentStartDate = _currentSubTask.Start;
                    SubCurrentCloseDate = _currentSubTask.Close;
                    SubCurrentState = _currentSubTask.State;
                    SubCurrentDescription = _currentSubTask.Description;
                    SubCurrentProgrammerId = _currentSubTask.ProgrammerId;
                }
                else
                {
                    SubCurrentName = null;
                    SubCurrentTime = null;
                    SubCurrentStartDate = DateTime.Now;
                    SubCurrentCloseDate = DateTime.Now;
                    SubCurrentState = null;
                    SubCurrentDescription = null;
                    SubCurrentProgrammerId = null;

                }

                RaisePropertyChanged("CurrentSubTask");
                RaisePropertyChanged("subSomethingSelected");

            }
        }

        private ICommand _showSubTasksCommand;

        public ICommand ShowSubTasksCommand
        {
            get
            {
                if (_showSubTasksCommand == null)
                    _showSubTasksCommand = new RelayCommand(new Action(LoadSubTasks));
                return _showSubTasksCommand;
            }
        }

        private ICommand _removeSubTasksCommand;

        public ICommand RemoveSubTasksCommand
        {
            get
            {
                if (_removeSubTasksCommand == null)
                    _removeSubTasksCommand = new RelayCommand(new Action(RemoveSubTask), () => subSomethingSelected );
                return _removeSubTasksCommand;
            }
        }

        private ICommand _subClearCommand;

        public ICommand SubClearCommand
        {
            get
            {
                if (_subClearCommand == null)
                    _subClearCommand = new RelayCommand(new Action(SubClearCurrent));
                return _subClearCommand;
            }
        }

        private ICommand _openSubTasksCommand;
        public ICommand OpenSubTasksCommand
        {
            get
            {
                if (_openSubTasksCommand == null)
                    _openSubTasksCommand = new RelayCommand(new Action(ActivateSubtasks), () => (somethingSelected & DisableTaskViewer));
                return _openSubTasksCommand;
            }
        }
        private ICommand _closeSubTasksCommand;
        public ICommand CloseSubTasksCommand
        {
            get
            {
                if (_closeSubTasksCommand == null)
                    _closeSubTasksCommand = new RelayCommand(new Action(ReturnToTasks));
                return _closeSubTasksCommand;
            }
        }
        private ICommand _subModifyCommand;
        public ICommand SubModifyCommand
        {
            get
            {
                if (_subModifyCommand == null)
                    _subModifyCommand = new RelayCommand(new Action(ModifyCurrentSubTask), () => subSomethingSelected);
                return _subModifyCommand;
            }
        }
        private ICommand _addSubTasksCommand;

        public ICommand AddSubTasksCommand
        {
            get
            {
                if (_addSubTasksCommand == null)
                    _addSubTasksCommand = new RelayCommand(new Action(AddSubTask));
                return _addSubTasksCommand;
            }
        }


        private void SubClearCurrent()
        {
            CurrentSubTask = null;
            RaisePropertyChanged("CurrentSubTask");
        }

        private void LoadSubTasks()
        {
            ListSubTasks = App.DbConnector.loadSubTasks(parent);
        }

        private void RemoveSubTask()
        {
            App.DbConnector.removeSubTasks(CurrentSubTask);
            LoadSubTasks();
            SubClearCurrent();
        }

        private void ActivateSubtasks()
        {
            parent = CurrentTask;//because CurrentTask is dereferenced after changing the window, then in the subtasks window parent is the previous current task.
            _stv = new SubTasksViewer();
            DisableTaskViewer = false;

            _stv.Show();

        }
        private void ReturnToTasks()
        {
            _stv.Close();
            CurrentTask = parent;
            DisableTaskViewer = true;
           
        }
        private void ModifyCurrentSubTask()
        {
            //Check first if the NOT NULL fields are null, empty or only blanks
            if (!string.IsNullOrEmpty(SubCurrentName) & !string.IsNullOrWhiteSpace(SubCurrentName) & !string.IsNullOrEmpty(SubCurrentState) & !string.IsNullOrWhiteSpace(SubCurrentState))
            {
                if ((SubCurrentCloseDate == null || SubCurrentStartDate == null) || (SubCurrentCloseDate > SubCurrentStartDate))
                {
                    Task t = new Task(1, SubCurrentName, (double)SubCurrentTime, SubCurrentStartDate, SubCurrentCloseDate, SubCurrentState, SubCurrentDescription, SubCurrentParentId, SubCurrentProgrammerId);
                    App.DbConnector.modifyTask(t, CurrentSubTask);
                    LoadSubTasks();
                    SubClearCurrent();

                }
                else
                {
                    MessageBox.Show("Start Date of the task must be earlier than Closing Date.");
                }

            }
            else
            {
                MessageBox.Show("Fields with * must be fulfilled");
            }
        }
        private void AddSubTask()
        {
            
            //Check first if the NOT NULL fields are null or only blanks
            if (!string.IsNullOrEmpty(SubCurrentName) & !string.IsNullOrWhiteSpace(SubCurrentName) & !string.IsNullOrEmpty(SubCurrentState) & !string.IsNullOrWhiteSpace(SubCurrentState))
            {
                if ((SubCurrentCloseDate == null || SubCurrentStartDate == null) || (SubCurrentCloseDate > SubCurrentStartDate))
                {
                    Task t = new Task(1, SubCurrentName, SubCurrentTime, SubCurrentStartDate, SubCurrentCloseDate, SubCurrentState, SubCurrentDescription, SubCurrentParentId, SubCurrentProgrammerId);
                    CurrentSubTask = null;
                    App.DbConnector.addSubTask(t,parent);
                    LoadSubTasks();
                    SubClearCurrent();

                }
                else
                {
                    MessageBox.Show("Start Date of the task must be earlier than Close Date.");
                }

            }
            else
            {
                MessageBox.Show("Fields with * must be fulfilled");
            }


        }

        #endregion


    }
}
