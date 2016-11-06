using CukurovaUniversiteli.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukurovaUniversiteli.Helper
{
    class ProcedureManager
    {
        SqlCommand command;

        public static ProcedureManager Prepare(string procedureName)
        {
            ProcedureManager process = new ProcedureManager();

            process.command = new SqlCommand();
            process.command.CommandType = CommandType.StoredProcedure;
            process.command.Connection = StaticModel.CuSqlConnection;
            process.command.CommandText = procedureName;

            return process;
        }

        public ProcedureManager AddValue(string fieldName, object value)
        {
            command.Parameters.AddWithValue("@" + fieldName, value);
            return this;
        }

        public void Execute()
        {
            StaticModel.CuSqlConnection.Open();
            command.ExecuteNonQuery();
            StaticModel.CuSqlConnection.Close();
        }

        public DataTable ExecuteAndReturnValue()
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            StaticModel.CuSqlConnection.Open();
            adapter.Fill(dataTable);
            StaticModel.CuSqlConnection.Close();

            return dataTable;
        }
    }
}