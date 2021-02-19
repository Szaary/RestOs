﻿using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool isClosed = false;
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
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
