using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManWP
{
   public class Repository
    {
      

        List<EventOrTask> allEvents = new List<EventOrTask>();

        public List<EventOrTask> Events
        {
            get { return allEvents; }
            set { allEvents = value; }
        }
        List<SimpleTask> allSimpleTasks = new List<SimpleTask>();

        public List<SimpleTask> SimpleTasks
        {
            get { return allSimpleTasks; }
            set { allSimpleTasks = value; }
        }

        public void SaveInFile()
        {

        }
        public void ReadFromFile()
        {

        }
        public void Download()
        {

        }
        public void Upload()
        {

        }
       public Repository()
        {

        }
       public EventOrTask GetEventById(int id)
       {
           EventOrTask e = new EventOrTask();
           foreach(EventOrTask ev in Events)
           {
               if (ev.Id == id) e = ev;
           }
           return e;
       }

       public SimpleTask GetSimpleTaskById(int id)
       {
           SimpleTask e = new SimpleTask();
           foreach (SimpleTask ev in SimpleTasks)
           {
               if (ev.Id == id) e = ev;
           }
           return e;
       }

    }
}
