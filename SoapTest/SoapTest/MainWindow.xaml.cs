
using Newtonsoft.Json;
using SoapTest.Database;
using SoapTest.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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

namespace SoapTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                List<DailyMenu> menuList = GetMenuList();

                FoodDatabase.Instance.ClearMenuList();
                foreach (DailyMenu menu in menuList)
                {
                    FoodDatabase.Instance.AddMenu(menu);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status);
            }

        }

        public List<DailyMenu> GetMenuList()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://cukurovauniversiteli.orcunoz.com/webservice/");
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"section\":\"Cafeteria\"," +
                              "\"method\":\"getMenuList\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();

                    return ParseJsonMenuList(data);
                }
            }

            return null;
        }

        public List<DailyMenu> ParseJsonMenuList(string jsonMenuList)
        {
            dynamic response = JsonConvert.DeserializeObject<dynamic>(jsonMenuList);

            List<DailyMenu> menuList = new List<DailyMenu>();

            Func<dynamic, List<Food>> MenuToFoodListConverter = dMenu =>
            {
                List<Food> foodList = new List<Food>();

                foreach (dynamic dFood in dMenu.foods)
                {
                    foodList.Add(new Food()
                    {
                        id = dFood.id,
                        calorie = dFood.calorie,
                        name = dFood.name,
                        contents = dFood.contents,
                        imageSrc = "http://yemekhane.cu.edu.tr/" + dFood.imageSrc
                    });
                }

                return foodList;
            };

            foreach (dynamic dMenu in response.content)
            {
                menuList.Add(new DailyMenu()
                {
                    date = dMenu.date,
                    foods = MenuToFoodListConverter(dMenu)
                });
            }

            return menuList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Views.CafeteriaView FoodListView = new Views.CafeteriaView();
            Controllers.CallFoodList.CallFoodListFunc(menux,FoodListView);
        }
    }
}
