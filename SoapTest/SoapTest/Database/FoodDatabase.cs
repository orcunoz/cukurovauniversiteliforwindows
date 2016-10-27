using SoapTest.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapTest.Database
{
    public class FoodDatabase
    {
        enum Process
        {
            Select = 0,
            Insert = 1,
            Truncate = 2,
            Count = 3
        }

        public static FoodDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FoodDatabase();
                }

                return instance;
            }
        }

        private static FoodDatabase instance;

        private SqlCommand command;

        FoodDatabase()
        {
            command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = StaticModel.CuSqlConnection;
            command.CommandText = "food_process";
        }

        public List<DailyMenu> GetMenuList()
        {
            List<DailyMenu> menuList = new List<DailyMenu>();

            command.Parameters.AddWithValue("@processType", Process.Select);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            StaticModel.CuSqlConnection.Open();
            adapter.Fill(dataTable);
            StaticModel.CuSqlConnection.Close();

            command.Parameters.Clear();

            DailyMenu currentMenu = null;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                string date = ((DateTime)row[0]).ToString("yyyy-MM-dd");

                if (currentMenu == null)
                {
                    currentMenu = new DailyMenu();
                    currentMenu.date = date;
                }
                else if (currentMenu.date != date)
                {
                    menuList.Add(currentMenu);
                    currentMenu = new DailyMenu();
                    currentMenu.date = date;
                }

                if (currentMenu.foods == null)
                {
                    currentMenu.foods = new List<Food>();
                }

                currentMenu.foods.Add(new Food()
                {
                    name = (string)row[1],
                    calorie = (int)row[2],
                    contents = (string)row[3],
                    imageSrc = (string)row[4]
                });
            }

            return menuList;
        }

        public void AddMenu(DailyMenu menu)
        {
            foreach (Food food in menu.foods)
            {
                command.Parameters.AddWithValue("@processType", Process.Insert);
                command.Parameters.AddWithValue("@date", menu.date);
                command.Parameters.AddWithValue("@name", food.name);
                command.Parameters.AddWithValue("@calorie", food.calorie);
                command.Parameters.AddWithValue("@contents", food.contents);
                command.Parameters.AddWithValue("@image_src", food.imageSrc);

                StaticModel.CuSqlConnection.Open();
                command.ExecuteNonQuery();
                StaticModel.CuSqlConnection.Close();

                command.Parameters.Clear();
            }
        }

        public void ClearMenuList()
        {
            command.Parameters.AddWithValue("@processType", Process.Truncate);

            StaticModel.CuSqlConnection.Open();
            command.ExecuteNonQuery();
            StaticModel.CuSqlConnection.Close();

            command.Parameters.Clear();
        }

        public int MenuCount()
        {
            command.Parameters.AddWithValue("@processType", Process.Count);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            StaticModel.CuSqlConnection.Open();
            adapter.Fill(dataTable);
            StaticModel.CuSqlConnection.Close();

            command.Parameters.Clear();

            return (int)dataTable.Rows[0][0];
        }
    }
}
