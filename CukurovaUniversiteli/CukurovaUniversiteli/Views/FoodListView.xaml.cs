using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CukurovaUniversiteli.Model;
using CukurovaUniversiteli.Database;

namespace CukurovaUniversiteli.Views
{
    /// <summary>
    /// Interaction logic for FoodListView.xaml
    /// </summary>
    public partial class CafeteriaView : UserControl
    {
        public string[] monthNames = { "OCAK", "ŞUBAT", "MART", "NİSAN", "MAYIS", "HAZİRAN", "TEMMUZ", "AĞUSTOS", "EYLÜL", "EKİM", "KASIM", "ARALIK" };

        public CafeteriaView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DailyMenu menu in FoodDatabase.Instance.GetMenuList())
            {
                AddMenu(menu);
            }
        }

        public void AddMenu(DailyMenu menu)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(0, 0, 0, 1);
            border.BorderBrush = Brushes.Gray;

            DockPanel dockPanel = new DockPanel();
            
            dockPanel.Children.Add(GetDateStackPanel(menu.date));
            dockPanel.Children.Add(GetFoodsStackPanel(menu.foods));
            dockPanel.Children.Add(GetCaloriesStackPanel(menu.foods));

            border.Child = dockPanel;
            border.Width = 400;
            gridFoodList.Children.Add(border);
        }

        public StackPanel GetDateStackPanel(string date)
        {
            string[] parts = date.Split('-');
            string day = parts[2];
            string month = parts[1];
            string year = parts[0];

            StackPanel stDate = new StackPanel();

            Label labelDay = new Label();
            labelDay.Content = day;
            labelDay.FontSize = 24;
            labelDay.Height = 42;
            labelDay.Width = 100;
            labelDay.HorizontalAlignment = HorizontalAlignment.Center;
            labelDay.VerticalAlignment = VerticalAlignment.Center;
            labelDay.HorizontalContentAlignment = HorizontalAlignment.Center;
            labelDay.VerticalContentAlignment = VerticalAlignment.Center;
            labelDay.FontWeight = FontWeights.Bold;
            stDate.Children.Add(labelDay);

            Label labelMonth = new Label();
            labelMonth.Content = monthNames[int.Parse(month)-1];
            labelMonth.Height = 28;
            labelMonth.Width = 100;
            labelMonth.HorizontalAlignment = HorizontalAlignment.Center;
            labelMonth.VerticalAlignment = VerticalAlignment.Center;
            labelMonth.HorizontalContentAlignment = HorizontalAlignment.Center;
            labelMonth.VerticalContentAlignment = VerticalAlignment.Center;
            stDate.Children.Add(labelMonth);

            Label labelYear = new Label();
            labelYear.Content = year;
            labelYear.Height = 28;
            labelYear.Width = 100;
            labelYear.HorizontalAlignment = HorizontalAlignment.Center;
            labelYear.VerticalAlignment = VerticalAlignment.Center;
            labelYear.HorizontalContentAlignment = HorizontalAlignment.Center;
            labelYear.VerticalContentAlignment = VerticalAlignment.Center;
            labelYear.FontWeight = FontWeights.Bold;
            stDate.Children.Add(labelYear);

            return stDate;
        }

        public StackPanel GetFoodsStackPanel(List<Food> foods)
        {
            StackPanel st = new StackPanel();

            foreach (Food food in foods)
            {
                Label lb = new Label();
                lb.Content = food.name;
                st.Children.Add(lb);
                lb.AddHandler(MouseDownEvent, new RoutedEventHandler((object sender, RoutedEventArgs e) =>
                {
                    MessageBox.Show(food.contents.Replace('|', '\n'));
                }));
            }

            st.Width = 200;

            return st;
        }

        public StackPanel GetCaloriesStackPanel(List<Food> foods)
        {
            StackPanel st = new StackPanel();

            foreach (Food food in foods)
            {
                Label lb = new Label();
                lb.Content = "(" + food.calorie + ")";
                lb.Foreground = Brushes.Red;
                st.Children.Add(lb);
            }

            return st;
        }
    }
}
