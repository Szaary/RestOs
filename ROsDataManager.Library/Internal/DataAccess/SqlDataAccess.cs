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
    internal class SqlDataAccess : IDisposable
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


        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure);               
            }
        }





        /// Open/close connection for transaction

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTranaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }  

        public void CommitTranaction()
        {
            _transaction?.Commit();
            _connection?.Close();

        }
        public void RollbackTranaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            CommitTranaction();        
        }


        /// Execute save/load in transaction

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
                _connection.Execute(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure, transaction: _transaction);           
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
                List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
                return rows;            
        }


    }
}
