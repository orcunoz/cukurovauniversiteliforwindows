using CukurovaUniversiteli.Helper;
using CukurovaUniversiteli.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukurovaUniversiteli.Database
{
    public class FoodDatabase
    {
        enum ProcessType
        {
            Select = 0,
            Insert = 1,
            Truncate = 2,
            Count = 3
        }

        struct ProcedureFields
        {
            public const string ProcessType = "process_type";
            public const string Date = "date";
            public const string Name = "name";
            public const string Calorie = "calorie";
            public const string Contents = "contents";
            public const string ImageSrc = "image_src";
        }

        public const string procedureName = "food_procedure";

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

            DataTable dataTable = ProcedureManager.Prepare(procedureName)
                .AddValue(ProcedureFields.ProcessType, ProcessType.Select)
                .AddValue(ProcedureFields.Date, DateTime.Now.AddDays(-20))
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
                ProcedureManager.Prepare(procedureName)
                    .AddValue(ProcedureFields.ProcessType, ProcessType.Insert)
                    .AddValue(ProcedureFields.Date, menu.date)
                    .AddValue(ProcedureFields.Name, food.name)
                    .AddValue(ProcedureFields.Calorie, food.calorie)
                    .AddValue(ProcedureFields.Contents, food.contents)
                    .AddValue(ProcedureFields.ImageSrc, food.imageSrc)
                    .Execute();
            }
        }

        public void ClearMenuList()
        {
            ProcedureManager.Prepare(procedureName)
                    .AddValue(ProcedureFields.ProcessType, ProcessType.Truncate)
                    .Execute();
        }

        public int MenuCount()
        {
            return (int) ProcedureManager.Prepare(procedureName)
                    .AddValue(ProcedureFields.ProcessType, ProcessType.Count)
                    .ExecuteAndReturnValue().Rows[0][0];
        }
    }
}
