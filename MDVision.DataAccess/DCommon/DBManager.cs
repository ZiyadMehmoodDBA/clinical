using MDVision.Common.Logging;
using MDVision.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MDVision.DataAccess.DCommon
{
    public sealed class DBManager : IDisposable, IDBManager
    {
        #region PRIVATE PROPERTIES
        private IDataReader idataReader;
        private IDbCommand idbCommand;
        private IDbConnection idbConnection;

        private IDbCommand idbDeleteCommand;
        private IDbDataParameter[] idbDeleteParameters;
        private List<IDbDataParameter> idbDeleteParametersList;
        private IDbCommand idbInsertCommand;
        private IDbDataParameter[] idbInsertParameters;

        private IDbDataParameter[] idbParameters;
        private List<IDbDataParameter> idbParametersList;

        private IDbTransaction idbTransaction;
        private IDbCommand idbUpdateCommand;
        private IDbDataParameter[] idbUpdateParameters;
        private List<IDbDataParameter> idbUpdateParametersList;
        private ParamDirection paramDirection;
        private DataProvider providerType;

        private string strConnection;
        #endregion

        #region CONSTRUCTORS

        public DBManager()
        {
            this.idbTransaction = null;

            this.idbParameters = null;
            this.idbParametersList = null;

            this.idbInsertParameters = null;


            this.idbUpdateParameters = null;
            this.idbUpdateParametersList = null;

            this.idbDeleteParameters = null;
            this.idbDeleteParametersList = null;
        }

        public DBManager(DataProvider providerType)
        {

            this.idbTransaction = null;

            this.idbParameters = null;
            this.idbParametersList = null;

            this.idbInsertParameters = null;


            this.idbUpdateParameters = null;
            this.idbUpdateParametersList = null;

            this.idbDeleteParameters = null;
            this.idbDeleteParametersList = null;

            this.providerType = providerType;
        }

        public DBManager(DataProvider providerType, string connectionString)
        {
            this.idbTransaction = null;

            this.idbParameters = null;
            this.idbParametersList = null;

            this.idbInsertParameters = null;


            this.idbUpdateParameters = null;
            this.idbUpdateParametersList = null;

            this.idbDeleteParameters = null;
            this.idbDeleteParametersList = null;

            this.providerType = providerType;
            this.strConnection = connectionString;
        }
        #endregion

        #region PARAMETERS

        public void AddInsertUpdateDeleteParameters(int index, string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, ParamDirection paramDeleteDirection = ParamDirection.Null, object objValue = null, int size = 0)
        {
            if (index < this.idbInsertParameters.Length)
            {
                if (objValue != null)
                {
                    this.idbInsertParameters[index].Value = objValue;
                }
                if (paramDirection != ParamDirection.Null)
                {
                    this.idbInsertParameters[index].Direction = (ParameterDirection)paramDirection;
                }
                this.idbInsertParameters[index].DbType = DbType;
                if (paramName != "")
                {
                    this.idbInsertParameters[index].ParameterName = paramName;
                }
                if (SourceColumn != "")
                {
                    this.idbInsertParameters[index].SourceColumn = SourceColumn;
                }
                if (size > 0)
                {
                    this.idbInsertParameters[index].Size = size;
                }
            }
            if (index < this.idbUpdateParameters.Length)
            {
                if (objValue != null)
                {
                    this.idbUpdateParameters[index].Value = objValue;
                }
                if (paramDirection != ParamDirection.Null)
                {
                    this.idbUpdateParameters[index].Direction = (ParameterDirection)paramDirection;
                }
                this.idbUpdateParameters[index].DbType = DbType;
                if (paramName != "")
                {
                    this.idbUpdateParameters[index].ParameterName = paramName;
                }
                if (SourceColumn != "")
                {
                    this.idbUpdateParameters[index].SourceColumn = SourceColumn;
                }
                if (size > 0)
                {
                    this.idbUpdateParameters[index].Size = size;
                }
            }
            if (index < this.idbDeleteParameters.Length)
            {
                if (objValue != null)
                {
                    this.idbDeleteParameters[index].Value = objValue;
                }
                if (paramDeleteDirection != ParamDirection.Null)
                {
                    this.idbDeleteParameters[index].Direction = (ParameterDirection)paramDeleteDirection;
                }
                this.idbDeleteParameters[index].DbType = DbType;
                if (paramName != "")
                {
                    this.idbDeleteParameters[index].ParameterName = paramName;
                }
                if (SourceColumn != "")
                {
                    this.idbDeleteParameters[index].SourceColumn = SourceColumn;
                }
                if (size > 0)
                {
                    this.idbDeleteParameters[index].Size = size;
                }
            }
        }

        public void AddInsertUpdateDeleteParameters(string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, ParamDirection paramDeleteDirection = ParamDirection.Null, object objValue = null, int size = 0)
        {
            this.AddParameters(paramName, objValue, DbType, paramDirection, size);

            //if (index < this.idbInsertParameters.Length)
            //{
            //    if (objValue != null)
            //    {
            //        this.idbInsertParameters[index].Value = objValue;
            //    }
            //    if (paramDirection != ParamDirection.Null)
            //    {
            //        this.idbInsertParameters[index].Direction = (ParameterDirection)paramDirection;
            //    }
            //    this.idbInsertParameters[index].DbType = DbType;
            //    if (paramName != "")
            //    {
            //        this.idbInsertParameters[index].ParameterName = paramName;
            //    }
            //    if (SourceColumn != "")
            //    {
            //        this.idbInsertParameters[index].SourceColumn = SourceColumn;
            //    }
            //    if (size > 0)
            //    {
            //        this.idbInsertParameters[index].Size = size;
            //    }
            //}

            this.AddUpdateParameters(paramName, objValue, DbType, paramDirection);
            //if (index < this.idbUpdateParameters.Length)
            //{
            //    if (objValue != null)
            //    {
            //        this.idbUpdateParameters[index].Value = objValue;
            //    }
            //    if (paramDirection != ParamDirection.Null)
            //    {
            //        this.idbUpdateParameters[index].Direction = (ParameterDirection)paramDirection;
            //    }
            //    this.idbUpdateParameters[index].DbType = DbType;
            //    if (paramName != "")
            //    {
            //        this.idbUpdateParameters[index].ParameterName = paramName;
            //    }
            //    if (SourceColumn != "")
            //    {
            //        this.idbUpdateParameters[index].SourceColumn = SourceColumn;
            //    }
            //    if (size > 0)
            //    {
            //        this.idbUpdateParameters[index].Size = size;
            //    }
            //}
            this.AddParameters(paramName, objValue, DbType, paramDeleteDirection);

            //if (index < this.idbDeleteParameters.Length)
            //{
            //    if (objValue != null)
            //    {
            //        this.idbDeleteParameters[index].Value = objValue;
            //    }
            //    if (paramDeleteDirection != ParamDirection.Null)
            //    {
            //        this.idbDeleteParameters[index].Direction = (ParameterDirection)paramDeleteDirection;
            //    }
            //    this.idbDeleteParameters[index].DbType = DbType;
            //    if (paramName != "")
            //    {
            //        this.idbDeleteParameters[index].ParameterName = paramName;
            //    }
            //    if (SourceColumn != "")
            //    {
            //        this.idbDeleteParameters[index].SourceColumn = SourceColumn;
            //    }
            //    if (size > 0)
            //    {
            //        this.idbDeleteParameters[index].Size = size;
            //    }
            //}
        }

        public void AddInsertUpdateParameters(int index, string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, object objValue = null, int size = 0)
        {
            if ((this.idbInsertParameters != null) && (index < this.idbInsertParameters.Length))
            {
                if (objValue != null)
                {
                    this.idbInsertParameters[index].Value = objValue;
                }
                if (paramDirection != ParamDirection.Null)
                {
                    this.idbInsertParameters[index].Direction = (ParameterDirection)paramDirection;
                }
                this.idbInsertParameters[index].DbType = DbType;
                if (paramName != "")
                {
                    this.idbInsertParameters[index].ParameterName = paramName;
                }
                if (SourceColumn != "")
                {
                    this.idbInsertParameters[index].SourceColumn = SourceColumn;
                }
                if (size > 0)
                {
                    this.idbInsertParameters[index].Size = size;
                }
            }
            if ((this.idbUpdateParameters != null) && (index < this.idbUpdateParameters.Length))
            {
                if (objValue != null)
                {
                    this.idbUpdateParameters[index].Value = objValue;
                }
                if (paramDirection != ParamDirection.Null)
                {
                    this.idbUpdateParameters[index].Direction = (ParameterDirection)paramDirection;
                }
                this.idbUpdateParameters[index].DbType = DbType;
                if (paramName != "")
                {
                    this.idbUpdateParameters[index].ParameterName = paramName;
                }
                if (SourceColumn != "")
                {
                    this.idbUpdateParameters[index].SourceColumn = SourceColumn;
                }
                if (size > 0)
                {
                    this.idbUpdateParameters[index].Size = size;
                }
            }
        }


        public void AddInsertUpdateParameters(string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection = ParamDirection.Null, object objValue = null, int size = 0)
        {

            this.AddParameters(paramName, objValue, DbType, paramDirection);
            this.AddUpdateParameters(paramName, objValue, DbType, paramDirection);
            /* if ((this.idbInsertParameters != null) && (index < this.idbInsertParameters.Length))
             {
                 if (objValue != null)
                 {
                     this.idbInsertParameters[index].Value = objValue;
                 }
                 if (paramDirection != ParamDirection.Null)
                 {
                     this.idbInsertParameters[index].Direction = (ParameterDirection)paramDirection;
                 }
                 this.idbInsertParameters[index].DbType = DbType;
                 if (paramName != "")
                 {
                     this.idbInsertParameters[index].ParameterName = paramName;
                 }
                 if (SourceColumn != "")
                 {
                     this.idbInsertParameters[index].SourceColumn = SourceColumn;
                 }
                 if (size > 0)
                 {
                     this.idbInsertParameters[index].Size = size;
                 }
             }*/
            /*
           if ((this.idbUpdateParameters != null) && (index < this.idbUpdateParameters.Length))
           {
               if (objValue != null)
               {
                   this.idbUpdateParameters[index].Value = objValue;
               }
               if (paramDirection != ParamDirection.Null)
               {
                   this.idbUpdateParameters[index].Direction = (ParameterDirection)paramDirection;
               }
               this.idbUpdateParameters[index].DbType = DbType;
               if (paramName != "")
               {
                   this.idbUpdateParameters[index].ParameterName = paramName;
               }
               if (SourceColumn != "")
               {
                   this.idbUpdateParameters[index].SourceColumn = SourceColumn;
               }
               if (size > 0)
               {
                   this.idbUpdateParameters[index].Size = size;
               }
           }
           */
        }


        public void AddParameters(int index, string paramName, object objValue)
        {


            if (index < this.idbParameters.Length)
            {


                this.idbParameters[index].ParameterName = paramName;
                if (objValue == null)
                {
                    this.idbParameters[index].Value = DBNull.Value;
                }
                else
                {
                    this.idbParameters[index].Value = objValue;
                }
            }
        }


        public void AddParameters(int index, string paramName, string SourceColumn, DbType DbType, ParamDirection paramDirection, object objValue = null, int size = 0)
        {

            if (index < this.idbParameters.Length)
            {
                if (objValue != null)
                {
                    this.idbParameters[index].Value = objValue;
                }

                if (paramDirection != ParamDirection.Null)
                {
                    this.idbParameters[index].Direction = (ParameterDirection)paramDirection;
                }

                this.idbParameters[index].DbType = DbType;
                if (paramName != "")
                {
                    this.idbParameters[index].ParameterName = paramName;
                }

                if (SourceColumn != "")
                {
                    this.idbParameters[index].SourceColumn = SourceColumn;
                }

                if (size > 0)
                {
                    this.idbParameters[index].Size = size;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="DbType"></param>
        /// <param name="paramDirection"></param>
        public void AddParameters(string parameterName, object parameterValue, DbType DbType = DbType.Object, ParamDirection paramDirection = ParamDirection.Null, int size = 0)
        {
            if (idbParametersList == null)
            {
                idbParametersList = new List<IDbDataParameter>();
            }

            IDbDataParameter idbParameters = new SqlParameter();


            idbParameters.Value = parameterValue == null ? DBNull.Value : parameterValue;


            if (paramDirection != ParamDirection.Null)
            {
                idbParameters.Direction = (ParameterDirection)paramDirection;
            }
            if (DbType != DbType.Object)
            {
                idbParameters.DbType = DbType;
            }
            if (parameterName != "")
            {
                idbParameters.ParameterName = parameterName;
            }
            if (size > 0)
            {
                idbParameters.Size = size;
            }

            idbParametersList.Add(idbParameters);

        }


        public void AddUpdateParameterValue(string parameterName, object parameterValue)
        {
            if (this.idbParametersList.Count > 0)
            {
                IDbDataParameter parameter = idbParametersList.Find(c => c.ParameterName == parameterName);
                if (parameter != null)
                {
                    parameter.Value = parameterValue;
                }
            }

        }

        public void AddUpdateParameters(string parameterName, object parameterValue, DbType DbType = DbType.Object, ParamDirection paramDirection = ParamDirection.Null)
        {
            if (idbUpdateParametersList == null)
            {
                idbUpdateParametersList = new List<IDbDataParameter>();
            }

            IDbDataParameter idbParameters = new SqlParameter();

            if (parameterValue != null)
            {
                idbParameters.Value = parameterValue;
            }

            if (paramDirection != ParamDirection.Null)
            {
                idbParameters.Direction = (ParameterDirection)paramDirection;
            }
            if (DbType != DbType.Object)
            {
                idbParameters.DbType = DbType;
            }
            if (parameterName != "")
            {
                idbParameters.ParameterName = parameterName;
            }
            idbUpdateParametersList.Add(idbParameters);

        }


        public void AddDeleteParameters(string parameterName, object parameterValue, DbType DbType = DbType.Object, ParamDirection paramDirection = ParamDirection.Null)
        {
            if (idbDeleteParametersList == null)
            {
                idbDeleteParametersList = new List<IDbDataParameter>();
            }

            IDbDataParameter idbParameters = new SqlParameter();

            if (parameterValue != null)
            {
                idbParameters.Value = parameterValue;
            }

            if (paramDirection != ParamDirection.Null)
            {
                idbParameters.Direction = (ParameterDirection)paramDirection;
            }
            if (DbType != DbType.Object)
            {
                idbParameters.DbType = DbType;
            }
            if (parameterName != "")
            {
                idbParameters.ParameterName = parameterName;
            }
            idbDeleteParametersList.Add(idbParameters);

        }


        private void AttachParameters(IDbCommand command, IEnumerable<IDbDataParameter> commandParameters)
        {
            foreach (IDbDataParameter parameter in commandParameters)
            {
                if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Output)) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }

                command.Parameters.Add(parameter);
            }
        }

        public void CreateInsertParameters(int paramsInsertCount)
        {
            this.idbInsertParameters = new IDbDataParameter[paramsInsertCount];

            int num;
            for (num = 0; num < paramsInsertCount; num++)
            {
                this.idbInsertParameters[num] = new SqlParameter();
            }
        }

        public void CreateInsertUpdateDeleteParameters(int paramsInsertUpdateCount, int paramsDeleteCount)
        {
            this.idbInsertParameters = new IDbDataParameter[paramsInsertUpdateCount];
            this.idbUpdateParameters = new IDbDataParameter[paramsInsertUpdateCount];

            int num;
            for (num = 0; num < paramsInsertUpdateCount; num++)
            {
                this.idbInsertParameters[num] = new SqlParameter();
                this.idbUpdateParameters[num] = new SqlParameter();
            }

            this.idbDeleteParameters = new IDbDataParameter[paramsDeleteCount];
            for (num = 0; num < paramsDeleteCount; num++)
            {
                this.idbDeleteParameters[num] = new SqlParameter();
            }
        }

        public void CreateParameters(int paramsCount)
        {
            this.idbParameters = new IDbDataParameter[paramsCount];

            int num;
            for (num = 0; num < paramsCount; num++)
            {
                this.idbParameters[num] = new SqlParameter();
            }
        }

        public void CreateUpdateParameters(int paramsUpdateCount)
        {
            this.idbUpdateParameters = new IDbDataParameter[paramsUpdateCount];

            int num;
            for (num = 0; num < paramsUpdateCount; num++)
            {
                this.idbUpdateParameters[num] = new SqlParameter();
            }
        }
        #endregion

        #region EXECUTION

        public DataSet DeleteDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);

                dataAdapter.DeleteCommand = this.idbCommand;
                dataAdapter.TableMappings.Add("Table", tableName);
                dataAdapter.Update(dataSet);

                this.idbCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::DeleteDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.DeleteCommand != null)
                {
                    dataAdapter.DeleteCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);

                dataAdapter.SelectCommand = this.idbCommand;
                dataAdapter.Fill(dataSet);

                this.idbCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::ExecuteDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.SelectCommand != null)
                {
                    dataAdapter.SelectCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName, int CommandTimeout = 180)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                    this.idbCommand.CommandTimeout = CommandTimeout;
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                dataAdapter.SelectCommand = this.idbCommand;
                dataAdapter.TableMappings.Add("Table", tableName);
                dataAdapter.Fill(dataSet);

                this.idbCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                this.GetErrorMessage(dataSet, tableName);
                dataSet.RejectChanges();

                MDVLogger.DALErrorLog("DBManager::ExecuteDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.SelectCommand != null)
                {
                    dataAdapter.SelectCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText, DataSet dataSet, List<string> tableNames, int CommandTimeout = 180)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                    this.idbCommand.CommandTimeout = CommandTimeout;
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                dataAdapter.SelectCommand = this.idbCommand;
                int counter = 0;

                foreach (var tableName in tableNames)
                {
                    string Table_name = "Table";
                    if (counter > 0)
                        Table_name += counter;

                    dataAdapter.TableMappings.Add(Table_name, tableName);
                    counter++;
                }

                dataAdapter.Fill(dataSet);
                this.idbCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::ExecuteDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.SelectCommand != null)
                {
                    dataAdapter.SelectCommand.Dispose();
                }
            }

            return dataSet;
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            int num = 0;
            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                num = this.idbCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteNonQuery", commandText, exception);
                throw exception;
            }
            finally
            {
                if (this.idbCommand != null)
                {
                    this.idbCommand.Parameters.Clear();
                }
            }

            return num;
        }

        public int ExecuteNonQueryWithOutputParam(CommandType commandType, string commandText)
        {
            int num = 0;
            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                num = this.idbCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteNonQueryWithCommand", commandText, exception);
                throw exception;
            }
            //finally
            //{
            //    if (this.idbCommand != null)
            //    {
            //        this.idbCommand.Parameters.Clear();
            //    }
            //}

            return num;
        }

        public void ClearCommandParam()
        {
            if (this.idbCommand != null)
            {
                this.idbCommand.Parameters.Clear();
            }
        }

        /// <summary>
        /// ExecuteReader fetches record from Database and return Datareader
        /// </summary>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="parameters">Sql Parameter Collection</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteReader(string procedureName, IEnumerable<IDbDataParameter> parameters)
        {
            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, CommandType.StoredProcedure, procedureName, parameters);
                this.DataReader = this.idbCommand.ExecuteReader();
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteReader", procedureName, exception);
                throw exception;
            }
            finally
            {
                if (this.idbCommand != null)
                {
                    this.idbCommand.Parameters.Clear();
                }
            }

            return this.DataReader;
        }

        /// <summary>
        /// ExecuteReader fetches record from Database, Use reflection and convert single Row into Type.
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="parameters">Sql Parameters Collection</param>
        /// <returns>Type</returns>
        public T ExecuteReader<T>(string procedureName) where T : class
        {
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            T model = Activator.CreateInstance<T>();

            try
            {
                dbManager.Open();

                using (var reader = dbManager.ExecuteReader(procedureName, this.idbParametersList))
                {
                    while (reader.Read())
                    {
                        var properties = typeof(T).GetProperties();

                        foreach (var prop in properties)
                        {

                            try
                            {
                                if (reader[prop.Name] != DBNull.Value)
                                {
                                    prop.SetValue(model, Convert.ChangeType(reader[prop.Name].ToString(), prop.PropertyType), null);
                                }
                            }
                            catch (IndexOutOfRangeException indexOutOfRangeEx)
                            {
                                // Console.WriteLine("could not find column " + prop.Name + " in resultset");
                            }
                            catch (InvalidCastException invalidCastEx)
                            {
                                //   Console.WriteLine("could not change the type of " + prop.Name);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        /// <summary>
        /// ExecuteReaders fetches record from Database, Use reflection and convert Rows into List of Desired Type.
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="parameters">Sql Parameters Collection</param>
        /// <returns>List of Type</returns>
        public List<T> ExecuteReaders<T>(string procedureName) where T : class
        {
            // IDBManager dbManager = ClientConfiguration.GetDBManager();
            List<T> modelList = new List<T>();
            try
            {
                var properties = typeof(T).GetProperties();
                this.Open();

                using (var reader = this.ExecuteReader(procedureName, this.idbParametersList))
                {
                    while (reader.Read())
                    {
                        T model = Activator.CreateInstance<T>();

                        foreach (var prop in properties)
                        {
                            try
                            {
                                if (reader[prop.Name] != DBNull.Value)
                                {
                                    prop.SetValue(model, Convert.ChangeType(reader[prop.Name].ToString(), prop.PropertyType), null);
                                }
                            }
                            catch (IndexOutOfRangeException indexOutOfRangeEx)
                            {
                            }
                            catch (InvalidCastException invalidCastEx)
                            {
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        modelList.Add(model);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                this.Dispose();
            }
        }
        public T ExecuteReadersForMultiResultSets<T>(string procedureName, params Type[] models) where T : class
        {
            T objmodelsListTobeReturned = Activator.CreateInstance<T>();
            Type mainType = typeof(T);
            try
            {
                int i = 0;
                IList models_list = null;
                using (var reader = this.ExecuteReader(procedureName, this.idbParametersList))
                {
                    do
                    {
                        if (i < models.Length)
                        {
                            var type_ = models[i];
                            var listType = typeof(List<>).MakeGenericType(type_);
                            models_list = (IList)Activator.CreateInstance(listType);
                            while (reader.Read())
                            {
                                object model = Activator.CreateInstance(type_);
                                var properties = type_.GetProperties();
                                foreach (var prop in properties)
                                {
                                    try
                                    {
                                        if (reader[prop.Name] != DBNull.Value)
                                            prop.SetValue(model, Convert.ChangeType(reader[prop.Name].ToString(), prop.PropertyType), null);
                                    }
                                    catch (Exception) { }
                                }
                                models_list.Add(model);
                            }
                            FieldInfo[] fieldInfos = mainType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                            foreach (var fieldInfo in fieldInfos)
                            {
                                if (fieldInfo.FieldType == listType)
                                {
                                    fieldInfo.SetValue(objmodelsListTobeReturned, models_list);
                                }
                            }
                        }
                        i++;
                    } while (reader.NextResult());
                }
                return objmodelsListTobeReturned;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteReadersForMultiResultSets", procedureName, ex);
                throw ex;
            }
            finally
            {
                //this.Dispose();
            }
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                this.DataReader = this.idbCommand.ExecuteReader();
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteReader", commandText, exception);
                throw exception;
            }
            finally
            {
                if (this.idbCommand != null)
                {
                    this.idbCommand.Parameters.Clear();
                }
            }

            return this.DataReader;
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            object obj2 = null;

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                obj2 = this.idbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteScalar", commandText, exception);
                throw exception;
            }
            finally
            {
                if (this.idbCommand != null)
                {
                    this.idbCommand.Parameters.Clear();
                }
            }

            return obj2;
        }
        public object ExecuteScalar(string commandText)
        {
            object obj2 = null;

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, CommandType.StoredProcedure, commandText, this.idbParametersList);
                obj2 = this.idbCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::ExecuteScalar", commandText, exception);
                throw exception;
            }
            finally
            {
                if (this.idbCommand != null)
                {
                    this.idbCommand.Parameters.Clear();
                }
            }

            return obj2;
        }

        private void GetErrorMessage(DataSet dataSet, string tableName)
        {
            if (dataSet.Tables[tableName].Columns.Contains("ErrorMessage"))
            {
                foreach (DataRow row in dataSet.Tables[tableName].Rows)
                {
                    if ((row["ErrorMessage"].ToString() != "") && (row["ErrorMessage"].ToString() != ""))
                    {
                        throw new Exception(row["ErrorMessage"].ToString());
                    }
                }
            }
        }

        public DataSet InsertAndUpdateDataSet(CommandType commandType, string insertCommandText, string updateCommandText, DataSet dataSet, string tableName)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();
            try
            {
                if (this.idbInsertCommand == null)
                {
                    this.idbInsertCommand = new SqlCommand();
                }

                if (this.idbUpdateCommand == null)
                {
                    this.idbUpdateCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbInsertCommand, this.Connection, this.Transaction, commandType, insertCommandText, this.InsertParameters);
                this.PrepareCommand(this.idbUpdateCommand, this.Connection, this.Transaction, commandType, updateCommandText, this.UpdateParameters);

                dataAdapter.InsertCommand = this.idbInsertCommand;
                dataAdapter.UpdateCommand = this.idbUpdateCommand;

                dataAdapter.TableMappings.Add("Table", tableName);
                dataAdapter.Update(dataSet);

                this.idbInsertCommand.Parameters.Clear();
                this.idbUpdateCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::InsertAndUpdateDataSet", insertCommandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.UpdateCommand != null)
                {
                    dataAdapter.UpdateCommand.Dispose();
                }

                if (dataAdapter.InsertCommand != null)
                {
                    dataAdapter.InsertCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet InsertAndUpdateDataSet(CommandType commandType, string insertCommandText, string updateCommandText, string deleteCommandText, DataSet dataSet, string tableName)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();
            try
            {
                if (this.idbInsertCommand == null)
                {
                    this.idbInsertCommand = new SqlCommand();
                }

                if (this.idbUpdateCommand == null)
                {
                    this.idbUpdateCommand = new SqlCommand();
                }

                if (this.idbDeleteCommand == null)
                {
                    this.idbDeleteCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbInsertCommand, this.Connection, this.Transaction, commandType, insertCommandText, this.InsertParameters);
                this.PrepareCommand(this.idbUpdateCommand, this.Connection, this.Transaction, commandType, updateCommandText, this.UpdateParameters);
                this.PrepareCommand(this.idbDeleteCommand, this.Connection, this.Transaction, commandType, deleteCommandText, this.DeleteParameters);

                dataAdapter.InsertCommand = this.idbInsertCommand;
                dataAdapter.UpdateCommand = this.idbUpdateCommand;
                dataAdapter.DeleteCommand = this.idbDeleteCommand;
                dataAdapter.TableMappings.Add("Table", tableName);

                dataAdapter.Update(dataSet);

                this.idbInsertCommand.Parameters.Clear();
                this.idbUpdateCommand.Parameters.Clear();
                this.idbDeleteCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::InsertAndUpdateDataSet", insertCommandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.UpdateCommand != null)
                {
                    dataAdapter.UpdateCommand.Dispose();
                }
                if (dataAdapter.InsertCommand != null)
                {
                    dataAdapter.InsertCommand.Dispose();
                }
                if (dataAdapter.DeleteCommand != null)
                {
                    dataAdapter.DeleteCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet InsertDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);

                dataAdapter.InsertCommand = this.idbCommand;
                dataAdapter.TableMappings.Add("Table", tableName);

                dataAdapter.Update(dataSet);

                this.idbCommand.Parameters.Clear();
                this.GetErrorMessage(dataSet, tableName);
            }
            catch (Exception exception)
            {
                this.GetErrorMessage(dataSet, tableName);
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::InsertDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.InsertCommand != null)
                {
                    dataAdapter.InsertCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet InsertDataSet(CommandType commandType, string commandText, DataSet dataSet, List<string> tableNames)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();

            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                dataAdapter.InsertCommand = this.idbCommand;

                int counter = 0;
                foreach (var tableName in tableNames)
                {
                    string Table_name = "Table";
                    if (counter > 0)
                        Table_name += counter;

                    dataAdapter.TableMappings.Add(Table_name, tableName);
                    counter++;

                    this.GetErrorMessage(dataSet, tableName);
                }

                dataAdapter.Update(dataSet);
                this.idbCommand.Parameters.Clear();
            }
            catch (Exception exception)
            {
                this.GetErrorMessage(dataSet, tableNames[0]);
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::InsertDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.InsertCommand != null)
                {
                    dataAdapter.InsertCommand.Dispose();
                }
            }

            return dataSet;
        }

        public DataSet UpdateDataSet(CommandType commandType, string commandText, DataSet dataSet, string tableName)
        {
            IDbDataAdapter dataAdapter = new SqlDataAdapter();
            try
            {
                if (this.idbCommand == null)
                {
                    this.idbCommand = new SqlCommand();
                }

                this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
                dataAdapter.UpdateCommand = this.idbCommand;
                dataAdapter.TableMappings.Add("Table", tableName);
                dataAdapter.Update(dataSet);

                this.idbCommand.Parameters.Clear();
                this.GetErrorMessage(dataSet, tableName);
            }
            catch (Exception exception)
            {
                this.GetErrorMessage(dataSet, tableName);
                dataSet.RejectChanges();
                MDVLogger.DALErrorLog("DBManager::UpdateDataSet", commandText, exception);
                throw exception;
            }
            finally
            {
                if (dataAdapter.UpdateCommand != null)
                {
                    dataAdapter.UpdateCommand.Dispose();
                }
            }

            return dataSet;
        }


        #endregion

        #region HELPERS
        public void Open()
        {
            try
            {
                this.idbConnection = new SqlConnection();
                this.idbConnection.ConnectionString = this.ConnectionString;

                if (this.idbConnection.State != ConnectionState.Open)
                {
                    this.idbConnection.Open();
                }
            }
            catch (Exception exception)
            {
                MDVLogger.DALErrorLog("DBManager::Open", "Trying to connect with Database.", exception);
                throw new Exception("Could not establish connection with the database.");
            }
        }

        public void CloseReader()
        {
            if (DataReader != null)
            {
                DataReader.Close();
            }
        }

        public void Close()
        {
            if ((this.idbConnection != null) && (this.idbConnection.State != ConnectionState.Closed))
            {
                this.idbConnection.Close();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Close();
            this.idbCommand = null;
            this.idbDeleteCommand = null;
            this.idbInsertCommand = null;
            this.idbUpdateCommand = null;
            this.idbTransaction = null;
            this.idbConnection = null;
        }

        private void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IEnumerable<IDbDataParameter> commandParameters)
        {
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                this.AttachParameters(command, commandParameters);
            }
        }

        #endregion

        #region TRANSACTION
        public void BeginTransaction()
        {
            this.idbConnection = new SqlConnection();
            this.idbConnection.ConnectionString = this.ConnectionString;

            if (this.idbConnection.State != ConnectionState.Open)
            {
                this.idbConnection.Open();
            }

            if (this.idbTransaction == null)
            {
                this.idbTransaction = this.idbConnection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (this.idbTransaction != null)
            {
                this.idbTransaction.Commit();
            }

            this.idbTransaction = null;
        }

        public void RollBackTransaction()
        {
            if (this.idbTransaction != null)
            {
                this.idbTransaction.Rollback();
            }

            this.idbTransaction = null;
        }




        #endregion

        #region PUBLIC PROPERTIES
        public IDbCommand Command
        {
            get
            {
                return this.idbCommand;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return this.idbConnection;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.strConnection;
            }
            set
            {
                this.strConnection = value;
            }
        }

        public IDataReader DataReader
        {
            get
            {
                return this.idataReader;
            }
            set
            {
                this.idataReader = value;
            }
        }

        public IDbCommand DeleteCommand
        {
            get
            {
                return this.idbDeleteCommand;
            }
        }

        public IDbDataParameter[] DeleteParameters
        {
            get
            {
                return this.idbDeleteParameters;
            }
        }

        public IDbCommand InsertCommand
        {
            get
            {
                return this.idbInsertCommand;
            }
        }

        public IDbDataParameter[] InsertParameters
        {
            get
            {
                return this.idbInsertParameters;
            }
        }

        public ParamDirection ParamDirection
        {
            get
            {
                return this.paramDirection;
            }
            set
            {
                this.paramDirection = value;
            }
        }

        public IDbDataParameter[] Parameters
        {
            get
            {
                return this.idbParameters;
            }
        }

        public DataProvider ProviderType
        {
            get
            {
                return this.providerType;
            }
            set
            {
                this.providerType = value;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return this.idbTransaction;
            }
        }

        public IDbCommand UpdateCommand
        {
            get
            {
                return this.idbUpdateCommand;
            }
        }

        public IDbDataParameter[] UpdateParameters
        {
            get
            {
                return this.idbUpdateParameters;
            }
        }
        #endregion




        public List<T> ExecuteReaderMapper<T>(string procedureName) where T : class
        {
            List<T> modelList = new List<T>();
            try
            {
                // this.Open();

                using (var reader = this.ExecuteReader(procedureName, this.idbParametersList))
                {
                    var columnsList = ModelUtility.GetReadersColumnList(reader);
                    while (reader.Read())
                    {
                        IBaseModel model = (IBaseModel)Activator.CreateInstance<T>();

                        model.Map(reader, columnsList);

                        modelList.Add((T)model);
                    }
                }

                return modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{

            //    this.Dispose();
            //}
        }

    }
}
