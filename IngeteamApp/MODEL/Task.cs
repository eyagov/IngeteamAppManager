using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngeteamApp.MODEL
{
    //The Task has information about its name, description, state (New, Resolved, In Progress, Feedback, Closed), hour estimation, closing date, start date.

    public class TaskCollection : ObservableCollection<Task>
    {
        
    }

    public class Task
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        private double? _time;
        public double? Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private DateTime? _close;
        public DateTime? Close
        {
            get { return _close; }
            set { _close = value; }
        }
        private DateTime? _start;
        public DateTime? Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private int? _parentId;
        public int? ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        private int? _programmerId;
        public int? ProgrammerId
        {
            get { return _programmerId; }
            set { _programmerId = value; }
        }

        public Task(int id, string name, double? time, DateTime? start, DateTime? close, string state, string description, int? parent, int? progr)
        {
            Id = id;
            Name = name;
            Description = description;
            State = state;
            Time = time;
            Close = close;
            Start = start;
            ParentId = parent;
            ProgrammerId = progr;

        }

        public override string ToString()
        {
            string startStr = Start == null ? "Empty" : ((DateTime)Start).ToString("dd MMMM yyyy hh:mm:ss tt");
            string closeStr = Close == null ? "Empty" : ((DateTime)Close).ToString("dd MMMM yyyy hh:mm:ss tt");
            return this.Name + " - Est.Time: " + this.Time.ToString() + " Start " + startStr + " Close: " + closeStr + " State: " + this.State + " Description: " + this.Description + " Programmer Number assigned: " + this.ProgrammerId.ToString();
        }



        //DateTime.Now.ToString("M/d/yyyy");
        //DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)
    }
}
