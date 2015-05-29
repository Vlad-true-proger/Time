using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManWP
{
   public class TasksToDate
    {
      public  bool IsDone  { get; set; }
      public  DateTime Date  { get; set; }
      List<SimpleTask> tasks = new List<SimpleTask>();     public List<SimpleTask> Tasks
      {
          get { return tasks; }
          set { tasks = value; }
      }
    }
}
