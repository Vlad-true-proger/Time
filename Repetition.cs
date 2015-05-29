using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManWP
{
   public class Repetition
    {
      public enum Repeat
        {
          EveryDay,
          EveryWeek,
          EveryMonth,
          EveryYear
        }
      public Repeat repetition { get; set; }
      public DateTime Start { get; set; }
       public Repetition(DateTime start, Repeat repeat)
      {
          this.Start = start;
          this.repetition = repeat;
      }

       public Repetition()
       {
           // TODO: Complete member initialization
       }
      public static List<string> RepetitionToString()
      {
          List<string> list = new List<string>(){
                "Каждый день",
                "Каждую неделю",
                "Каждый месяц",
                "Каждый год"
          };
          return list;
      }

      public static Repetition.Repeat GetRepetitionByString(string cat)
      {

          Repetition.Repeat s = Repetition.Repeat.EveryDay;
          if (cat == "Каждый день") s = Repetition.Repeat.EveryDay;
          if (cat == "Каждую неделю") s = Repetition.Repeat.EveryWeek;
          if (cat == "Каждый месяц") s = Repetition.Repeat.EveryMonth;
          if (cat == "Каждый год") s = Repetition.Repeat.EveryYear;

          return s;
      }
    }
}
