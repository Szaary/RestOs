using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Daper with use of Generics : T, U - brillant
/// Internal - this cant be seen outside of data library
/// </summary>
namespace ROsDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List <T> rows = connection.Query<T>(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }
        
        //public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        //{
        //    string connectionString = GetConnectionString(connectionStringName);

        //    using (IDbConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Execute(storedProcedure, parameters,
        //             commandType: CommandType.StoredProcedure);                
        //    }
        //}



    }
}
