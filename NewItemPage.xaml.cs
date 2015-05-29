using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace TimeManWP
{
    public partial class NewItemPage : PhoneApplicationPage
    {
        DateTime start;
        DateTime end;
        DateTime date;
      
        bool isCkeckedRepetition = false;

      
        public NewItemPage()
        {
            InitializeComponent();
            ColorGrid.Visibility = Visibility.Collapsed;
            listPickerImportance.ItemsSource = ImportanceToString();
            listPickerRepeat.ItemsSource = Repetition.RepetitionToString();
            listPickerColor.Tag = 8;
            ChangeVisibilityOfRows(false);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txboxName.Text == ""){throw new Exception("Вы не ввели название");}
                if (RadioBEvent.IsChecked.Value==true&&(start==null||end==null||date==null)){throw new Exception("Вы не указали время");}
                if (RadioBTask.IsChecked.Value==true&&(start==null||end==null)&&date==null){throw new Exception("Вы не указали время");}
                
                if(RadioBTask.IsChecked==true)
                {
                    if (date == null)//SimpleTask
                    {
                        Data.rep.SimpleTasks.Add(new SimpleTask(txboxName.Text, Data.rep.SimpleTasks.Count, txboxDescription.Text));
                    }
                    else//TAsk can be with start or not
                    {
                       if(start!=null) if (start < end) { throw new Exception("Время начала меньше чем время окончания"); }
                        Data.rep.Events.Add(new EventOrTask(RadioBTask.IsChecked.Value, txboxName.Text, date, end, Data.rep.Events.Count));
                        if (start != null) Data.rep.Events[Data.rep.Events.Count-1].Start = start;
                    }
                }
                else
                {
                    if (start < end) { throw new Exception("Время начала меньше чем время окончания"); }
                    Data.rep.Events.Add(new EventOrTask(RadioBTask.IsChecked.Value, txboxName.Text, date, start, end, Data.rep.Events.Count));
                }
                int last = Data.rep.Events.Count - 1;
                Data.rep.Events[last].Description = txboxDescription.Text;//Description
                Data.rep.Events[last].Color = Data.colors[(int)listPickerColor.Tag];//Color
                if (listPickerImportance.SelectedItem != null) Data.rep.Events[last].Important = GetImportanceByString(listPickerImportance.SelectedItem.ToString());
                if (listPickerRepeat.SelectedItem != null) Data.rep.Events[last].Repeat = new List<Repetition>() { new Repetition( date, Repetition.GetRepetitionByString(listPickerRepeat.SelectedItem.ToString())) };

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        public void CreateColors()
        {
            int n = 0;
            for (int i = 1; i <= 5; i = i + 2)
                for (int j = 1; j <= 5; j = j + 2)
                {

                    Button b = new Button()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Width = double.NaN,
                        Height = double.NaN,
                        Margin = new Thickness(-9, -9, -9, -9),
                        Tag = n,
                        Background = new SolidColorBrush(Data.colors[n])
                    };
                    b.Tap += (sender, e) => ButtonColor_Tap(sender, e);
                    ColorGrid.Children.Add(b);
                    Grid.SetColumn(b, j);
                    Grid.SetRow(b, i);

                    n++;
                }
        }

    
        private void ButtonColor_Tap(object sender, RoutedEventArgs e)
        {
            listPickerColor.Background = ((Button)sender).Background;
            listPickerColor.Tag = ((Button)sender).Tag;
            ColorGrid.Visibility = Visibility.Collapsed;
            listPickerImportance.ItemsSource = ImportanceToString();
        }
        private void TimePickerStart_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if(timePickerStart!=null)
                if(timePickerStart.Value!=null)
            start = (DateTime)timePickerStart.Value;
        }
        private void TimePickerEnd_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (timePickerEnd != null)
                if (timePickerEnd.Value != null)
            end= (DateTime)timePickerEnd.Value;
        }
        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (datePicker != null)
                if (datePicker.Value != null)
                {
                    date = (DateTime)datePicker.Value;
                    ChangeVisibilityOfRows(true);

                }else
                {
                    ChangeVisibilityOfRows(false);
                }
     

        }
        public void ChangeVisibilityOfRows(bool visible)
        {
            if(visible)
            {
                    SecondMainGrid.RowDefinitions[7].Height = new GridLength(2, GridUnitType.Star);
                    GridWithDate.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                    GridWithDate.RowDefinitions[3].Height = new GridLength(6, GridUnitType.Star);
                    GridWithDate.RowDefinitions[4].Height = new GridLength(1, GridUnitType.Star);
                    GridWithDate.RowDefinitions[5].Height = new GridLength(6, GridUnitType.Star);
                    SecondMainGrid.RowDefinitions[5].Height = new GridLength(4.5, GridUnitType.Star);
                    timePickerEnd.Visibility = Visibility.Visible;
                    timePickerEnd.Visibility = Visibility.Visible;
                    listPickerRepeat.Visibility = Visibility.Visible;
            }else
            {
                SecondMainGrid.RowDefinitions[7].Height = new GridLength(0);
                GridWithDate.RowDefinitions[2].Height = new GridLength(0);
                GridWithDate.RowDefinitions[3].Height = new GridLength(0);
                GridWithDate.RowDefinitions[4].Height = new GridLength(0);
                GridWithDate.RowDefinitions[5].Height = new GridLength(0);
                SecondMainGrid.RowDefinitions[5].Height = new GridLength(2, GridUnitType.Star);
                timePickerEnd.Visibility = Visibility.Collapsed;
                timePickerStart.Visibility = Visibility.Collapsed;
                listPickerRepeat.Visibility = Visibility.Collapsed;
            }
        }

        private void checkBoxRepeat_Checked(object sender, RoutedEventArgs e)
        {
            isCkeckedRepetition = checkBoxRepeat.IsChecked.Value;
            if (isCkeckedRepetition)
            {
                listPickerRepeat.IsEnabled = true; listPickerRepeat.SelectedItem = listPickerRepeat.Items[0];

            }
            else
            {
                listPickerRepeat.IsEnabled = false; listPickerRepeat.SelectedItem = null;
            }
        }

        private void listPickerColor_Click(object sender, RoutedEventArgs e)
        {
            CreateColors();
            ColorGrid.Visibility = Visibility.Visible;
        }

        public List<string> CategoryToString()
        {
            List<string> list = new List<string>(){
                "Учеба",
       "Работа",
       "Семья",
       "Отдых",
       "Развлечение",
       "Хобби",
       "ПовседневныеДела",
       "Сон"};
            return list;
        }
        public List<string> ImportanceToString()
        {
            List<string> list = new List<string>(){
                "A","B","C"
          };
            return list;
        }
        public Category GetCategoryByString(string cat)
        {
            Category s = Category.Отдых;
            if (cat == "Работа") s = Category.Работа;
            if (cat == "Семья") s = Category.Семья;
            if (cat == "Отдых") s = Category.Отдых;
            if (cat == "Развлечение") s = Category.Развлечение;
            if (cat == "Хобби") s = Category.Хобби;
            if (cat == "ПовседневныеДела") s = Category.ПовседневныеДела;
            if (cat == "Сон") s = Category.Сон;
            if (cat == "Учеба") s = Category.Учеба;
            return s;
        }
        public Importance GetImportanceByString(string cat)
        {
            Importance im = Importance.C;
            if (cat == "A") im = Importance.A; if (cat == "B") im = Importance.B; if (cat == "C") im = Importance.C;
            return im;
        }
    }
}