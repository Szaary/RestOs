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



        /// <summary>
        /// Save/Load data without transactions.
        /// </summary>



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



        /// <summary>
        /// Save load data with transactions - use only when necessary.
        /// </summary>

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool isClosed = false;

        public void StartTranaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }  

        public void CommitTranaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;

        }
        public void RollbackTranaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTranaction();
                }
                catch
                {
                    // Todo - Log this issue
                }
            }
            _transaction = null;
            _connection = null;
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
