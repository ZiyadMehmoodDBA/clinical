using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
namespace MDVision.DataAccess.DAL.Admin
{
    public class DALpersonnel
    {

        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string PROC_PERSONNEL_INSERT = "PersonnelInsert";
        private const string PROC_PERSONNEL_UPDATE = "PersonnelUpdate";
        private const string PROC_PERSONNEL_DELETE = "PersonnelDelete";
        private const string PROC_PERSONNEL_SELECT = "PersonnelGet";
             

        #endregion

        #region "Query "
           private const string QRY_PERSONNEL_SELECT = "select * from personnel";
        #endregion

        #region "Parameters"
         private const string PARM_ID = "@ID";
         private const string PARM_FNAME = "@FName";
         private const string PARM_LNAME = "@LName";
        
         
        public struct Parameters
         {
             public int ID;
             public string FNAME;
             public string LNAME;
         }
       
        #endregion
        
        //#region Singleton
        // private static DALpersonnel _instance = null;
        // /// <summary>
        // /// Singleton context
        // /// </summary>
        // public static DALpersonnel Instance
        // {
        //     get
        //     {
        //         if (_instance == null)
        //             _instance = new DALpersonnel();
        //         return _instance;
        //     }
        // }
        // #endregion
         #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DALPlanFeeLink"/> class.
        /// </summary>
        /// <param name="Obj">The object.</param>
        public DALpersonnel()
        {
            InitializeComponent();
            ClientConfiguration.SetClientObject();
           
        }
        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
        //#region Initialize
        //// private DALpersonnel() { }
        //#endregion

        #region "Support Functions"

        // public void GetData(Parameters param)
        //{
        //    IDBManager dbManager = new DBManager(DataProvider.SqlServer );
        //    dbManager.ConnectionString = CustomerConfiguration.ConnectionString(DataProvider.SqlServer ) ; //ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        //    try
        //    {
        //        //ds.Tables(ds.TABLE_BL837BATCH ).Columns.Count 
        //        dbManager.Open();
        //        dbManager.CreateParameters(1);
                          
        //        dbManager.AddParameters(0, PARM_ID, param.ID);
        //       // UPDATE, INSERT, and DELETE 
        //        IDataReader reader = dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_PERSONNEL_SELECT);
        //        if (reader!=null)
        //        {

        //            while (reader.Read())
        //            {
        //                //objList.Add(mapper(reader));
        //            }
        //        }
        //        //personnel.FName = "Homay";
        //        //personnel.LName = "Drop Table";

        //    }
        //    catch (Exception ce)
        //    {
        //        //Usual code              
        //    }
        //    finally
        //    {
        //        dbManager.Dispose();
        //    }
        //   // return new BWObject<DataSet>();
        //}
        //with list
         //public void InsertData(Parameters param)
         //{
         //    IDBManager dbManager = new DBManager(DataProvider.SqlServer);
         //    dbManager.ConnectionString = CustomerConfiguration.ConnectionString(DataProvider.SqlServer); //ConfigurationSettings.AppSettings["ConnectionString"].ToString();
         //    try
         //    {
         //        //ds.Tables(ds.TABLE_BL837BATCH ).Columns.Count 
         //        dbManager.Open();
         //        //dbManager.CreateParameters(1);

         //       // dbManager.AddParameters(0, PARM_ID, param.ID);
         //        dbManager.CreateParameters(ds.Tables[ds.tblPersonnel.TableName].Columns.Count);
         //        dbManager.AddParameters(0, PARM_ID, ds.tblPersonnel.IDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
         //        dbManager.AddParameters(1, PARM_FNAME, ds.tblPersonnel.FNameColumn.ColumnName, DbType.String, ParamDirection.Null);
         //        dbManager.AddParameters(2, PARM_LNAME, ds.tblPersonnel.LNameColumn.ColumnName, DbType.String, ParamDirection.Null);
         //        dbManager.ExecuteNonQuery(CommandType.StoredProcedure, PROC_PERSONNEL_SELECT);

         //        //personnel.FName = "Homay";
         //        //personnel.LName = "Drop Table";

         //    }
         //    catch (Exception ce)
         //    {
         //        //Usual code              
         //    }
         //    finally
         //    {
         //        dbManager.Dispose();
         //    }
         //    // return new BWObject<DataSet>();
         //}
         //with dataset
         private  void CreateParameters(IDBManager dbManager, DSPersonnel ds, Boolean IsInsert )
         {

             dbManager.CreateParameters(ds.Tables[ds.tblPersonnel.TableName].Columns.Count);
             if (IsInsert == true)
                 dbManager.AddParameters(0, PARM_ID, ds.tblPersonnel.IDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
             else
                 dbManager.AddParameters(0, PARM_ID, ds.tblPersonnel.IDColumn.ColumnName, DbType.Int32);

             dbManager.AddParameters(1, PARM_FNAME, ds.tblPersonnel.FNameColumn.ColumnName, DbType.String);
             dbManager.AddParameters(2, PARM_LNAME, ds.tblPersonnel.LNameColumn.ColumnName, DbType.String);

         }
         private  void CreateInsertUpdateParameters(IDBManager dbManager, DSPersonnel ds)
         {

             dbManager.CreateInsertUpdateDeleteParameters(ds.Tables[ds.tblPersonnel.TableName].Columns.Count, 1);
             dbManager.AddInsertUpdateDeleteParameters(0, PARM_ID, ds.tblPersonnel.IDColumn.ColumnName, DbType.Int32, ParamDirection.Output);
             dbManager.AddInsertUpdateDeleteParameters(1, PARM_FNAME, ds.tblPersonnel.FNameColumn.ColumnName, DbType.String);
             dbManager.AddInsertUpdateDeleteParameters(2, PARM_LNAME, ds.tblPersonnel.LNameColumn.ColumnName, DbType.String);

         }
        #endregion

         #region "Insert, delete, update and get using dataset Functions"

         public DSPersonnel LoadPersonnel(Parameters param)
         {
             DSPersonnel ds = new DSPersonnel();
             //IDBManager dbManager = new DBManager(DataProvider.SqlServer);
             //dbManager.ConnectionString = CustomerConfiguration.ConnectionString(DataProvider.SqlServer); //ConfigurationSettings.AppSettings["ConnectionString"].ToString();
             IDBManager dbManager =ClientConfiguration.GetDBManager();
             try
             {
                 dbManager.Open();
                 dbManager.CreateParameters(1);
                 dbManager.AddParameters(0, PARM_ID, param.ID);
                 ds = (DSPersonnel)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_SELECT, ds, ds.tblPersonnel.TableName);
                 return ds; 

             }
             catch (Exception ex)
             {
                 MDVLogger.DALErrorLog("DALpersonnel::LoadPersonnel", PROC_PERSONNEL_SELECT, ex);
                 throw ex;
                 //Usual code              
             }
             finally
             {
                 dbManager.Dispose();
             }

            
             // return new BWObject<DataSet>();
         }
         public DSPersonnel InsertPersonnel(ref DSPersonnel ds)
        {
           
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                                               
                dbManager.Open();
                this.CreateParameters(dbManager, ds, true );
                ds=(DSPersonnel)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_INSERT, ds, ds.tblPersonnel.TableName);
                ds.AcceptChanges();
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALpersonnel::InsertPersonnel", PROC_PERSONNEL_INSERT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {               
                   dbManager.Dispose();
            }
           
            
        }
         public DSPersonnel UpdatePersonnel(ref DSPersonnel ds)
         {
             IDBManager dbManager =ClientConfiguration.GetDBManager();
             try
             {
                 dbManager.Open();
                 this.CreateParameters(dbManager, ds,false  );
                 ds =  (DSPersonnel)dbManager.UpdateDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_UPDATE, ds, ds.tblPersonnel.TableName);
                 ds.AcceptChanges();
                 return ds; 
             }
             catch (Exception ex)
             {
                 MDVLogger.DALErrorLog("DALpersonnel::UpdatePersonnel", PROC_PERSONNEL_UPDATE, ex);
                 throw ex;
                 //Usual code              
             }
             finally
             {
                 dbManager.Dispose();
             }
             
             
         }
         public DSPersonnel DeletePersonnel(ref DSPersonnel ds)
         {
             IDBManager dbManager =ClientConfiguration.GetDBManager();
             try
             {
                 
                 dbManager.Open();
                 dbManager.CreateParameters(1);
                 dbManager.AddParameters(0, PARM_ID, ds.tblPersonnel.IDColumn.ColumnName, DbType.Int32);
                 ds = (DSPersonnel)dbManager.DeleteDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_DELETE, ds, ds.tblPersonnel.TableName);
                 ds.AcceptChanges();
                 return ds;
             }
             catch (Exception ex)
             {
                 MDVLogger.DALErrorLog("DALpersonnel::DeletePersonnel", PROC_PERSONNEL_DELETE, ex);
                 throw ex;
                 //Usual code              
             }
             finally
             {
                 dbManager.Dispose();
             }
            
            
         }
         public DSPersonnel InsertPersonnel(ref DSPersonnel ds, IDBManager dbManager)
         {
                        
             try
             {
                 CreateParameters(dbManager, ds,true );
                 ds = (DSPersonnel)dbManager.InsertDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_INSERT, ds, ds.tblPersonnel.TableName);
                 ds.AcceptChanges();
                 return ds;
             }
             catch (Exception ex)
             {
                 MDVLogger.DALErrorLog("DALpersonnel::InsertPersonnel", PROC_PERSONNEL_INSERT, ex);
                 throw ex;
                 //Usual code              
             }
            
           
         }
         public DSPersonnel InsertAndUpdatePersonnel(ref DSPersonnel ds, IDBManager dbManager)
         {
             try
             {
                 this.CreateInsertUpdateParameters(dbManager, ds);
                 ds = (DSPersonnel)dbManager.InsertAndUpdateDataSet(CommandType.StoredProcedure, PROC_PERSONNEL_INSERT, PROC_PERSONNEL_UPDATE ,PROC_PERSONNEL_DELETE , ds, ds.tblPersonnel.TableName);
                 ds.AcceptChanges();
                 return ds; 
             }
             catch (Exception ex)
             {
                 MDVLogger.DALErrorLog("DALpersonnel::InsertAndUpdatePersonnel", PROC_PERSONNEL_INSERT, ex);
                 throw ex;
                 //Usual code              
             }
         
         }

         #endregion

        //private Func<SqlDataReader, Payers> PayerMapper = dr => new Payers
          //   {
          //       PayerId = dr.GetString("PayerId"),
          //       PayerType = dr.GetString("PayerType"),
          //       PayerName = dr.GetString("PayerName"),
          //       PayerAddress = dr.GetString("PayerAddress"),
          //       PayerPhone = dr.GetString("PayerPhone"),
          //       CountryID = dr.GetString("CountryID"),
          //       State = dr.GetString("State"),
          //       City = dr.GetString("City"),
          //       TimeStamp = dr.GetString("TimeStamp"),
          //       ResultCode = "Success"
          //   };       
         //public List<OTAppointment> OTAppointmentGet()
         //{
         //    try
         //    {
         //        List<OTAppointment> lstOtapp = new List<OTAppointment>();
         //        SqlConnection conn = new SqlConnection(PCISMiddleLayer.MIMSConfiguration.get_ConnectionString(false));
         //        SqlCommand cmd = new SqlCommand("OTAppointmentGet", conn);
         //        cmd.CommandType = CommandType.StoredProcedure;
         //        SqlDataReader reader = null;
         //        conn.Open();

         //        reader = cmd.ExecuteReader();

         //        if (reader.HasRows)
         //        {
         //            while (reader.Read())
         //            {
         //                OTAppointment otAppointment = new OTAppointment();
         //                otAppointment.otAppointmentId = reader.GetGuid(0);
         //                otAppointment.StartDate = reader.GetDateTime(1);
         //                otAppointment.EndDate = reader.GetDateTime(2);
         //                otAppointment.StartTime = reader.GetDateTime(3);
         //                otAppointment.EndTime = reader.GetDateTime(4);
         //                otAppointment.ApprovedByFacility = reader.GetBoolean(5);
         //                otAppointment.ApprovedBy = reader.GetInt64(6);
         //                otAppointment.ApprovedByName = reader.GetString(7);
         //                otAppointment.EpisodeId = reader.GetGuid(8);
         //                otAppointment.SiteId = reader.GetInt64(9);
         //                otAppointment.LocationId = reader.GetInt64(10);
         //                otAppointment.SiteName = reader.GetString(11);
         //                otAppointment.LocationName = reader.GetString(12);
         //                otAppointment.MRNo = reader.GetString(13);
         //                otAppointment.PatientName = reader.GetString(14);
         //                otAppointment.ProviderId = reader.GetInt64(15);
         //                otAppointment.ProviderName = reader["ProviderName"].ToString();
         //                lstOtapp.Add(otAppointment);
         //            }
         //        }
         //        reader.Close();
         //        conn.Close();
         //        return lstOtapp;
         //    }
         //    catch (Exception ex)
         //    {
         //        throw ex;
         //    }
         //}
       
    //     private SqlCommand GetInsertCommand()
    //{
    //    SqlCommand cmd = new SqlCommand(DALAdultPreventiveCare.PROC_INSERT, new SqlConnection(MIMSConfiguration.ConnectionString));
    //    cmd.CommandType = CommandType.StoredProcedure;
        
    //    cmd.Parameters.Add(DALAdultPreventiveCare.PARAM_ID_PK, SqlDbType.Int).Direction = ParameterDirection.Output;
    //    cmd.Parameters..Item(DALAdultPreventiveCare.PARAM_ID_PK).SourceColumn = DSAdultPreventiveCare.FIELD_ID_PK;

    //    cmd.Parameters.Add(DALAdultPreventiveCare.PARAM_PREVENTIVECARECATID, SqlDbType.Int).SourceColumn = DSAdultPreventiveCare.FIELD_PREVENTIVE_CARE_ID_FK;
    //    cmd.Parameters.Add(DALAdultPreventiveCare.PARAM_PARENT_ID, SqlDbType.Int).SourceColumn = DSAdultPreventiveCare.FIELD_PARENT_ID;
    //    cmd.Parameters.Add(DALAdultPreventiveCare.PARAM_TITLE, SqlDbType.VarChar).SourceColumn = DSAdultPreventiveCare.FIELD_TITLE;
    //    cmd.Parameters.Add(DALAdultPreventiveCare.PARAM_RECOMMENDED, SqlDbType.VarChar).SourceColumn = DSAdultPreventiveCare.FIELD_RECOMMENDED;
    //    _with1.Add(DALAdultPreventiveCare.PARAM_IS_RESULT, SqlDbType.Bit).SourceColumn = DSAdultPreventiveCare.FIELD_IS_RESULT;
    //    _with1.Add(DALAdultPreventiveCare.PARAM_MRNO, SqlDbType.VarChar).SourceColumn = DSAdultPreventiveCare.FIELD_MRNO;
    //    return cmd;
    //}

//      IDBManager dbManager = newDBManager(DataProvider.SqlServer);
//dbManager.ConnectionString =ConfigurationSettings.AppSettings[
//  "ConnectionString"].ToString();
//try
//{
//  dbManager.Open();
//  dbManager.ExecuteReader("Select * fromemp ",CommandType.Text);
//  while(dbManager.DataReader.Read())Response.Write(dbManager.
//  DataReader["name"].ToString());
//}
 
//catch (Exception ex)
//{
////Usual Code
//}
 
//finally
//{
//  dbManager.Dispose();
//}


//        IDBManager dbManager = newDBManager(DataProvider.OleDb);
//dbManager.ConnectionString =ConfigurationSettings.AppSettings[
//  "ConnectionString"].ToString();
//try
//{
//  dbManager.Open();
//  object recordCount =dbManager.ExecuteScalar("Select count(*) from
//  emp ", CommandType.Text);
//  Response.Write(recordCount.ToString());
//}
 
//catch (Exception ce)
//{
////Usual Code
//}
 
//finally
//{
//  dbManager.Dispose();
//}
    }
}
