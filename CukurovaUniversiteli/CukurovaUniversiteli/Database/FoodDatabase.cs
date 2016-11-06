using CukurovaUniversiteli.Helper;
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

        public List<DailyMenu> GetMenuList()
        {
            List<DailyMenu> menuList = new List<DailyMenu>();

            DataTable dataTable = ProcedureManager.Prepare("food_process")
                .AddValue("processType", Process.Select)
                .AddValue("date", DateTime.Now.AddDays(-20))
                .ExecuteAndReturnValue();

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
                ProcedureManager.Prepare("food_process")
                    .AddValue("processType", Process.Insert)
                    .AddValue("date", menu.date)
                    .AddValue("name", food.name)
                    .AddValue("calorie", food.calorie)
                    .AddValue("contents", food.contents)
                    .AddValue("image_src", food.imageSrc)
                    .Execute();
            }
        }

        public void ClearMenuList()
        {
            ProcedureManager.Prepare("food_process")
                    .AddValue("processType", Process.Truncate)
                    .Execute();
        }

        public int MenuCount()
        {
            return (int) ProcedureManager.Prepare("food_process")
                    .AddValue("processType", Process.Count)
                    .ExecuteAndReturnValue().Rows[0][0];
        }
    }
}
