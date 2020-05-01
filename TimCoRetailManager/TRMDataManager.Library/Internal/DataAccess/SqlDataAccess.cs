using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TRMDataManager.Library
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
                List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }
        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        private IDbConnection connection;
        private IDbTransaction dbTransaction;
        //Open connection/start transaction
        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            connection = new SqlConnection(connectionString);
            connection.Open();
            dbTransaction = connection.BeginTransaction();
        }
        //load using the transaction
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: dbTransaction).ToList();
            return rows;
        }
        //save using the transaction
        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: dbTransaction);
        }
        //close connection/stop transaction
        public void CommitTransaction()
        {
            dbTransaction?.Commit();
            connection?.Close();
        }
        public void RollBackTransaction()
        {
            dbTransaction?.Rollback();
            connection?.Close();
        }

        //dispose
        public void Dispose()
        {
            CommitTransaction();
        }
    }
}
