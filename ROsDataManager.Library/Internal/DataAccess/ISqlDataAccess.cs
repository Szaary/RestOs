using System.Collections.Generic;

namespace ROsDataManager.Library.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        void CommitTranaction();
        void Dispose();
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void RollbackTranaction();
        void SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void StartTranaction(string connectionStringName);
    }
}