using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.Data;
using System.ComponentModel;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALCityStateZip
    {

        #region Variable
        
        #endregion
        #region "Stored Procedure Names"
        private const string PROC_City_State = "dbo.sp_citystatezip";
        private const string PROC_City_Lookup = "dbo.sp_citylookup";
        private const string PROC_State_Lookup = "dbo.sp_statelookup";
        private const string PROC_Country_Lookup = "dbo.sp_countrylookup";
        
        #endregion

        #region "Parameters"
        private const string PARM_ZIP_CODE = "@zipcode";
        private const string PARM_CITY_NAME = "@cityStateName";
        #endregion

         #region Constructors

        public DALCityStateZip()
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

        public DSCityStateZip GetCityState(string zipcode,string cityname)
        {
            DSCityStateZip ds = new DSCityStateZip();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                dbManager.CreateParameters(2);

                if (!string.IsNullOrEmpty(zipcode))
                {
                    dbManager.AddParameters(0, PARM_ZIP_CODE, zipcode);
                }
                else
                {
                    dbManager.AddParameters(0, PARM_ZIP_CODE, null);
                }

                if (!string.IsNullOrEmpty(cityname))
                {
                    dbManager.AddParameters(1, PARM_CITY_NAME, cityname);
                }
                else
                {
                    dbManager.AddParameters(1, PARM_CITY_NAME, null);
                }

                ds = (DSCityStateZip)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_City_State, ds, ds.CityState.TableName);
                if (ds.Tables[ds.CityState.TableName].Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(ds.Tables[ds.CityState.TableName].Rows[0]["ErrorMessage"])))
                    throw new Exception(Convert.ToString(ds.Tables[ds.CityState.TableName].Rows[0]["ErrorMessage"]));

                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALCityStateZip::GetCityState", PROC_City_State, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup GetCityLookup()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
               
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_City_Lookup, ds, ds.CityName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfileLookup::GetCityLookup", PROC_City_Lookup, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup GetStateLookup()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_State_Lookup, ds, ds.StateName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfileLookup::GetStateLookup", PROC_State_Lookup, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        public DSProfileLookup GetCountryLookup()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();

                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_City_Lookup, ds, ds.CountryName.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALProfileLookup::GetCountryLookup", PROC_Country_Lookup, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }


    }
}
