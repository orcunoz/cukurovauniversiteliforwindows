using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SoapTest.Controllers
{
    class CallFoodList
    {
        public static void CallFoodListFunc(Grid grd, UserControl uc)
        {
            if (grd.Children.Count > 0)
            {
                grd.Children.Clear();
            }
            grd.Children.Add(uc);

        }
    }
}
