using MDVision.Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDVision.DataAccess.DCommon
{
    public interface IDBManager
    {
        void AddInsertUpdateDeleteParameters(int index, string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, ParamDirection paramDeleteDirection = ParamDirection.Null, object objValue = null, int size = 0);
        void AddInsertUpdateParameters(int index, string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, object objValue = null, int size = 0);
        void AddParameters(int index, string paramName, object objValue);
        void AddParameters(int index, string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, object objValue = null, int size = 0);
        void AddParameters(string paramName, object objValue, DbType DbType = DbType.Object, ParamDirection paramDirection = ParamDirection.Null, int size = 0);
        void AddUpdateParameterValue(string parameterName, object parameterValue);
        void BeginTransaction();
        void Close();
        void CloseReader();
        void CommitTransaction();
        void CreateInsertParameters(int paramsInsertCount);
        void CreateInsertUpdateDeleteParameters(int paramsInsertUpdateCount, int paramsDeleteCount);
        void CreateParameters(int paramsCount);
        void CreateUpdateParameters(int paramsUpdateCount);
        DataSet DeleteDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName);
        void Dispose();
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName, int CommandTimeout = 180);
        DataSet ExecuteDataSet(CommandType commandType, string commandText, DataSet dataSet, List<string> tableName, int CommandTimeout = 180);
        int ExecuteNonQuery(CommandType commandType, string commandText);

        int ExecuteNonQueryWithOutputParam(CommandType commandType, string commandText);
        IDataReader ExecuteReader(CommandType commandType, string commandText);

        /// <summary>
        /// ExecuteReader fetches record from Database and return Datareader
        /// </summary>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="parameters">Sql Parameter Collection</param>
        /// <returns>DataReader</returns>
        IDataReader ExecuteReader(string procedureName, IEnumerable<IDbDataParameter> parameters);

        /// <summary>
        /// ExecuteReader fetches record from Database, Use reflection and convert single Row into Type.
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="parameters">Sql Parameters Collection</param>
        /// <returns>Type</returns>
        T ExecuteReader<T>(string procedureName) where T : class;

        /// <summary>
        /// ExecuteReaders fetches record from Database, Use reflection and convert Rows into List of Desired Type.
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="parameters">Sql Parameters Collection</param>
        /// <returns>List of Type</returns>
        List<T> ExecuteReaders<T>(string procedureName) where T : class;
        T ExecuteReadersForMultiResultSets<T>(string procedureName, params Type[] models) where T : class;

        object ExecuteScalar(CommandType commandType, string commandText);
        object ExecuteScalar(string commandText);
        DataSet InsertAndUpdateDataSet(CommandType commandType, string insertCommandText, string updateCommandText, DataSet dataSet, string tableName);
        DataSet InsertAndUpdateDataSet(CommandType commandType, string insertCommandText, string updateCommandText, string deleteCommandText, DataSet dataSet, string tableName);
        DataSet InsertDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName);
        DataSet InsertDataSet(CommandType commandType, string commandText, DataSet dataSet, List<string> tableNames);

        void ClearCommandParam();
        void Open();
        void RollBackTransaction();
        DataSet UpdateDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName);

        IDbCommand Command { get; }

        IDbConnection Connection { get; }

        string ConnectionString { get; set; }

        IDataReader DataReader { get; }

        IDbCommand DeleteCommand { get; }

        IDbDataParameter[] DeleteParameters { get; }

        IDbCommand InsertCommand { get; }

        IDbDataParameter[] InsertParameters { get; }

        ParamDirection ParamDirection { get; set; }

        IDbDataParameter[] Parameters { get; }

        DataProvider ProviderType { get; set; }

        IDbTransaction Transaction { get; }

        IDbCommand UpdateCommand { get; }

        IDbDataParameter[] UpdateParameters { get; }


        List<T> ExecuteReaderMapper<T>(string procedureName) where T : class;

    }
}
