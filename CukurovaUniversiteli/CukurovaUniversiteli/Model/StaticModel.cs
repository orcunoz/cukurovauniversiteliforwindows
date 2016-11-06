using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukurovaUniversiteli.Model
{
    public static class StaticModel
    {
        private const string connectionString = "Data Source=localhost;Initial Catalog = CukurovaUniversiteli; Integrated Security = True";

        public static SqlConnection CuSqlConnection { get; private set; }

        public static void Init()
        {
            CuSqlConnection = new SqlConnection(connectionString);
        }
    }
}
