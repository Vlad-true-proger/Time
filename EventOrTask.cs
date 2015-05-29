using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TimeManWP
{
    public class EventOrTask :SimpleTask
    {
        
        DateTime date;     public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        
        DateTime end;       public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        DateTime start;    public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        Color color = Colors.Blue;    public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        Importance important = Importance.C;  public Importance Important
        {
            get { return important; }
            set { important = value; }
        }

        List<SimpleTask> subTasks = new List<SimpleTask>();  public List<SimpleTask> SubTasks
        {
            get { return subTasks; }
            set { subTasks = value; }
        }

        List<Repetition> repeat = new List<Repetition>();   public List<Repetition> Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        List<TasksToDate> tasksForDate = new List<TasksToDate>();   public List<TasksToDate> TasksForDate
        {
            get { return tasksForDate; }
            set { tasksForDate = value; }
        }
        String category = "";    public String Category
        {
            get { return category; }
            set { category = value; }
        }

        bool isTask=false;    public bool IsTask
        {
            get { return isTask; }
            set { isTask = value; }
        }

        public EventOrTask(bool isTask, string name, DateTime date, DateTime end, int id)
            : base(name, id)
        {
            this.Done = false;
            this.date = date;
            this.isTask = isTask;
            this.end = end;
            this.Color = Data.colors[7];
            important = Importance.C;
        }
        public EventOrTask(bool isTask, string name, DateTime date, DateTime start, DateTime end, int id)
            : base(name, id)
        {
            this.date = date;
            this.start = start;
            this.Done = false;
            this.isTask =isTask;
            this.end = end;
            this.Color = Data.colors[7];
            important = Importance.C;
        }

        public EventOrTask(bool IsTask, string name, string description, DateTime date, DateTime start, DateTime end, Importance important, Color color, List<SimpleTask> subTasks, List<Repetition> repeat, int id)
            :base(name,id, description)
        {
            this.isTask = isTask;
            this.date = date;
            this.Done = false;
            this.start = start;
            this.end = end;
            this.important = important;
            this.repeat = repeat;
            this.color = color;
            this.subTasks = subTasks;
        }

        public EventOrTask()
        {
            // TODO: Complete member initialization
        }

    }
}
