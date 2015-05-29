using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TimeManWP.Resources;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TimeManWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<DateTime> week = new List<DateTime>();
        DateTime day = DateTime.Now;
        List<Pair> line1Busy = new List<Pair>();
        List<Pair> line2Busy = new List<Pair>();
        List<Pair> line3Busy = new List<Pair>();
     
        public MainPage()
        {
            InitializeComponent();
            Data.rep = new Repository();
            ThemeManager.OverrideOptions = ThemeManagerOverrideOptions.None;
            ThemeManager.ToLightTheme();

            Data.rep.Events.Add(new EventOrTask(true, "NewEvent", day, Convert.ToDateTime("12:00"), Convert.ToDateTime("15:35"), Data.rep.Events.Count));
            Data.rep.Events.Add(new EventOrTask(true, "NewEvent", DateTime.Now, Convert.ToDateTime("11:00"), Convert.ToDateTime("13:40"), Data.rep.Events.Count));
            Data.rep.Events.Add(new EventOrTask(false, "NewEvent", DateTime.Now, Convert.ToDateTime("14:00"), Convert.ToDateTime("14:50"), Data.rep.Events.Count));
            Data.rep.Events.Add(new EventOrTask(true, "NewEvent", DateTime.Now, Convert.ToDateTime("16:00"), Convert.ToDateTime("18:50"), Data.rep.Events.Count));
            //Data.rep.Events.Add(new EventOrTask(false, "NewEvent", DateTime.Now, Convert.ToDateTime("15:00"), Convert.ToDateTime("18:50"), Data.rep.Events.Count));
            //Data.rep.Events.Add(new EventOrTask(false, "NewEvent", DateTime.Now, Convert.ToDateTime("10:00"), Convert.ToDateTime("19:50"), Data.rep.Events.Count));
            Data.rep.Events[Data.rep.Events.Count-1].Repeat = new List<Repetition>() { new Repetition(day, Repetition.Repeat.EveryDay) };
            CreateEvents(day);
           
        }
        
        //возвращает числа, в которых проходит неделя, в которую входит заданный день
        public string TextWeek(DateTime day)
        {
            string content ="Неделя";
            DateTime start = day, end = day;
            for (int i = 0; i < 7; i++)
                if (start.DayOfWeek != DayOfWeek.Monday)
                    start = start.AddDays(-1);
                else break;
            for (int i = 0; i < 7; i++)
                if (end.DayOfWeek != DayOfWeek.Sunday)
                    end = end.AddDays(1);
                else break;
            string a = "", b = "";
            if (start.Month.ToString().Length < 2) a = "0"; if (end.Month.ToString().Length < 2) b = "0";
            content +=" "+ start.Day+"." +a+ start.Month +" - " + end.Day+ "."+b+ end.Month;
            week.Clear();
            for (DateTime st = start; st <= end; st=st.AddDays(1))
            {
                week.Add(st);
            }
                return content;
        }

        //Создает линии для событий
        public void CreateLines()
        {
            double hours24 = RectanglePaperList.Height-40;
            for (int i = 0; i < 25; i++)
            {
                PaperList.Children.Add(new Rectangle() { Margin = new Thickness(8,i*hours24/24+20,8,0), Height = 4, VerticalAlignment=VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Stretch, RadiusX = 2, RadiusY = 2, StrokeThickness = 5, Fill = new SolidColorBrush(Colors.White) });
                PaperList.Children.Last().SetValue(Grid.ColumnSpanProperty, 4);
                PaperList.Children.Add(new TextBlock() { Margin = new Thickness(0, i*hours24 / 24 -14+20, 0, 0), FontSize = 20, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Text = i + ":00" });
                PaperList.Children.Last().SetValue(Grid.ColumnProperty, 5);  
            }
        }

        //Создает события на заданный день из репозитория
        public void CreateEvents(DateTime d)
        {

            Storyboard s = AnimationWithOpacity(PaperList, PaperList.Opacity, 0, 200);
            s.Begin();
            TxblWeek.Text = TextWeek(d);
            if (day.DayOfWeek == DayOfWeek.Monday) ShedButtonDayOfWeek1.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek1.Background = new SolidColorBrush(Colors.Transparent);
            if (day.DayOfWeek == DayOfWeek.Tuesday) ShedButtonDayOfWeek2.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek2.Background = new SolidColorBrush(Colors.Transparent);
            if (day.DayOfWeek == DayOfWeek.Wednesday) ShedButtonDayOfWeek3.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek3.Background = new SolidColorBrush(Colors.Transparent);
            if (day.DayOfWeek == DayOfWeek.Thursday) ShedButtonDayOfWeek4.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek4.Background = new SolidColorBrush(Colors.Transparent);
            if (day.DayOfWeek == DayOfWeek.Friday) ShedButtonDayOfWeek5.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek5.Background = new SolidColorBrush(Colors.Transparent);
            if (day.DayOfWeek == DayOfWeek.Saturday) ShedButtonDayOfWeek6.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek6.Background = new SolidColorBrush(Colors.Transparent);
            if (day.DayOfWeek == DayOfWeek.Sunday) ShedButtonDayOfWeek7.Background = new SolidColorBrush(Colors.Red); else ShedButtonDayOfWeek7.Background = new SolidColorBrush(Colors.Transparent);
               
            s.Completed += (a, b) =>
             {
                 PaperList.Children.Clear();
                 CreateLines();
                 
                 List<EventOrTask> Events = new List<EventOrTask>();
                 foreach (EventOrTask ev in Data.rep.Events)
                 {
                     bool done = false;
                     foreach (TasksToDate t in ev.TasksForDate)
                     {
                         if (t.Date.Date == d.Date && t.IsDone == true)
                             done = true;
                     }
                     if (!done)
                     {
                         if (ev.Start.Date == d.Date) Events.Add(ev);
                         else
                         {
                             foreach (Repetition repeat in ev.Repeat)
                             {
                                 if (repeat.repetition == Repetition.Repeat.EveryDay) Events.Add(ev);
                                 if (repeat.repetition == Repetition.Repeat.EveryWeek) { if (repeat.Start.DayOfWeek == d.DayOfWeek) Events.Add(ev); }
                                 if (repeat.repetition == Repetition.Repeat.EveryMonth) { if (repeat.Start.Day == d.Day) Events.Add(ev); }
                                 if (repeat.repetition == Repetition.Repeat.EveryYear) { if (repeat.Start.Date == d.Date) Events.Add(ev); }
                             }
                         }
                     }
                 }
                   line1Busy = new List<Pair>();
                 line2Busy = new List<Pair>();
                 line3Busy = new List<Pair>();
                 List<Pair> Busy1 = new List<Pair>();
                 List<Pair> Busy2 = new List<Pair>();
                 List<Pair> Busy3 = new List<Pair>();

                 GetAllFilledTime1line(Events);

                 foreach (EventOrTask ev in Events)
                 {
                     Pair pair = new Pair(ev.Start.Hour * 60 + ev.Start.Minute, ev.End.Hour * 60 + ev.End.Minute);
                     CreateOneEventOrTask(ev);
                     Grid grid = ((Grid)PaperList.Children.Last());
                     grid.Opacity = 0.7;
                     bool con2 = ContainsListAnInterval(line2Busy, pair), con3 = ContainsListAnInterval(line3Busy, pair);
                     if (!ContainsListAnInterval(Busy1, pair))
                     {

                         Busy1.Add(pair);
                         if ((!con2) && (!con3))
                         {
                             grid.SetValue(Grid.ColumnProperty, 0);
                             grid.SetValue(Grid.ColumnSpanProperty, 4);
                         }
                         else
                         {
                             if ((con2) && (!con3))
                             {
                                 grid.SetValue(Grid.ColumnProperty, 0);
                                 grid.SetValue(Grid.ColumnSpanProperty, 2);
                             }
                             if ((con2) && (con3))
                             {
                                 grid.SetValue(Grid.ColumnProperty, 0);
                                 grid.SetValue(Grid.ColumnSpanProperty, 1);
                             }
                         }
                     }
                     else
                         if (!ContainsListAnInterval(Busy2, pair))
                         {
                             Busy2.Add(pair);
                             if (!con3)
                             {
                                 grid.SetValue(Grid.ColumnProperty, 2);
                                 grid.SetValue(Grid.ColumnSpanProperty, 2);
                             }
                             else
                             {
                                 grid.SetValue(Grid.ColumnProperty, 1);
                                 grid.SetValue(Grid.ColumnSpanProperty, 2);
                             }
                         }
                         else
                             if (!ContainsListAnInterval(Busy3, pair))
                             {
                                 grid.SetValue(Grid.ColumnProperty, 3);
                                 grid.SetValue(Grid.ColumnSpanProperty, 1);

                             }
                 }
Storyboard s2 = AnimationWithOpacity(PaperList, 0, 1, 200);
            s2.Begin();
             };
            
        }

        //Создает одно событие в списке событий
        public void CreateOneEventOrTask(EventOrTask ev)
        {
            double hour = (RectanglePaperList.Height - 40) / 24;
            double ms = ev.Start.Minute;
            double me = ev.End.Minute;
            double start = ev.Start.Hour + (double)(ms / 60);
            double end = ev.End.Hour + (double)(me / 60);
            Grid g = new Grid() { Margin = new Thickness(2, start * hour + 22, 2, 0), Height = (end - start) * hour, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Stretch };
            g.Children.Add(new Rectangle() { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch, RadiusX = 4, RadiusY = 4, StrokeThickness = 5, Fill = new SolidColorBrush(ev.Color) });

            StackPanel contforall = new StackPanel() { Margin = new Thickness(0,0,0,0), VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            StackPanel cont = new StackPanel() {Margin = new Thickness(10,0, 0, 0),  Height = double.NaN, HorizontalAlignment = HorizontalAlignment.Stretch, Orientation = System.Windows.Controls.Orientation.Horizontal };
            ScrollViewer scroll = new ScrollViewer() { Height = double.NaN, HorizontalAlignment = HorizontalAlignment.Stretch, HorizontalScrollBarVisibility=ScrollBarVisibility.Hidden, VerticalScrollBarVisibility=ScrollBarVisibility.Disabled };
            
            TextBlock name = new TextBlock() { Margin = new Thickness(0, 0, 0, 0), Height=double.NaN, Text = ev.Name, FontSize = 20};
            scroll.Content = name;
            Rectangle rect = new Rectangle() { Margin = new Thickness(2, 4, 0, 2), Height = 3, HorizontalAlignment = HorizontalAlignment.Stretch, RadiusX = 1, RadiusY = 1, StrokeThickness = 1, Fill = new SolidColorBrush(Colors.White) };
            if (ev.IsTask)
            {
                CheckBox check = new CheckBox() { Margin = new Thickness(-5, -5, -5,-5), FontSize = 16, HorizontalAlignment = HorizontalAlignment.Left, Tag = ev.Id };
                ((CheckBox)check).Checked += TaskIsDone_Checked;
                cont.Children.Add(check);
                scroll.Margin = new Thickness(-10, 15, 0, 0);
                cont.Margin = new Thickness(-2, -10, 0, 10);
                rect.Margin = new Thickness(2, -30, 0,2);
            }
            cont.Children.Add(scroll);
            contforall.Children.Add(cont);
            contforall.Children.Add(rect );
            g.Children.Add(contforall);
            PaperList.Children.Add(g);
        }
    
        private void TaskIsDone_Checked(object sender, RoutedEventArgs e)
        {
          EventOrTask ev =  Data.rep.GetEventById((int)((CheckBox)sender).Tag);
          ev.TasksForDate.Add(new TasksToDate() {IsDone=true, Date=day });
          StackPanel st = (StackPanel)((CheckBox)sender).Parent;
          StackPanel st2 = (StackPanel)st.Parent;
          Grid g = (Grid)st2.Parent;
        Storyboard s=  AnimationWithOpacity(g, g.Opacity, 0, 210);
        s.Begin();
         s.Completed+=(a,b)=> PaperList.Children.Remove(g);
        }

        public Storyboard AnimationWithOpacity(UIElement element, double from, double to, int duration)
        {
          Storyboard s = new Storyboard();
          DoubleAnimation vanish = new DoubleAnimation() { From = from, To = to, Duration = TimeSpan.FromMilliseconds(duration) };
          element.RenderTransform = (Transform)new TranslateTransform();

          Storyboard.SetTargetProperty(vanish, new PropertyPath(UIElement.OpacityProperty));
          Storyboard.SetTarget((Timeline)vanish, element);
          s.Children.Add(vanish);
          return s;
        }

        void s_Completed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //Переключает новый день и создает расписание событий на него
        private void ShedButton_Click(object sender, RoutedEventArgs e)
        {
            if (((string)((Button)sender).Tag) == "1") { day = week[0]; }
            if (((string)((Button)sender).Tag) == "2") { day = week[1]; }
            if (((string)((Button)sender).Tag) == "3") { day = week[2]; }
            if (((string)((Button)sender).Tag) == "4") { day = week[3]; }
            if (((string)((Button)sender).Tag) == "5") { day = week[4]; }
            if (((string)((Button)sender).Tag) == "6") { day = week[5]; }
            if (((string)((Button)sender).Tag) == "7") { day = week[6]; }
            CreateEvents(day);
        }

        public bool ContainsListAnInterval(List<Pair> interval, Pair pair)
        {
            bool b = false;
            foreach( Pair p in interval)
            {
                if(p.Intersect(pair.Start, pair.End))
                { b = true; break; }
            }
            return b;
        }

        public void GetAllFilledTime1line(List<EventOrTask> Events)
        {
            foreach(EventOrTask ev in Events)
            {
                Pair pair = new Pair(ev.Start.Hour * 60 + ev.Start.Minute, ev.End.Hour * 60 + ev.End.Minute);
                if (!ContainsListAnInterval(line1Busy, pair))
                {
                    line1Busy.Add(pair);
                }
                else
                {
                    if (!ContainsListAnInterval(line2Busy, pair))
                    {
                        line2Busy.Add(pair);
                    }
                    else
                    {
                        line3Busy.Add(pair);
                    }
                }
                
                
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            day = day.AddDays(7);
            CreateEvents(day);
        }

        private void ButtonPrew_Click(object sender, RoutedEventArgs e)
        {
            day = day.AddDays(-7);
            CreateEvents(day);
        }

        //Новое событие
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewItemPage.xaml", UriKind.Relative));
        }
    }
}