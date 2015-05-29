using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManWP
{
   public class SimpleTask
    {
        string name;     public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string description;   public string Description
        {
            get { return description; }
            set { description = value; }
        }

        bool done;   public bool Done
        {
            get { return done; }
            set { done = value; }
        }

        int id;  public int Id
        {
            get { return id; }
            set { id = value; }
        }

       public SimpleTask(string name, int id, string description = "")
        {
            this.done = false;
            this.name = name;
            this.description = description;
            this.id = id;
        }

       public SimpleTask()
       {
           // TODO: Complete member initialization
       }
    }
}
