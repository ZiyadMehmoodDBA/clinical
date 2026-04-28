using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDVision.Datasets;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using System.ComponentModel;
using System.Data;
using MDVision.Common.Logging;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALEntity
    {


        #region Variable
        
        #endregion

        #region " Stored Procedure Names"
        private const string SchemaName = "System.";
        private const string PROC_INSERT = "";
        private const string PROC_UPDATE = "";
        private const string PROC_DELETE = "";
        private const string PROC_ENTITY_SELECT = "System.sp_OrganizationEntitySelect";
        private const string PROC_ENTITY_LOOKUP = "System.sp_OrganizationEntityLookup";



        #endregion

        #region "Query "

        #endregion

        #region "Parameters"

        private const string PARM_ENTITY_ID = "@EntityId";
        private const string PARM_SHORT_NAME = "@ShortName";
        private const string PARM_USER_ID = "@Userid";
        private const string PARM_FACILITY_ID = "@FacilityId";


        #endregion

        #region Constructors

        public DALEntity()
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


        #region "Insert, delete, update and get using dataset Functions"
        public DSProfile LoadEntity(long EntityId, string ShortName)
        {
            DSProfile ds = new DSProfile();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                if (ShortName == "")
                    ShortName = null;



                dbManager.Open();
                dbManager.CreateParameters(2);

                if (EntityId <= 0)
                    dbManager.AddParameters(0, PARM_ENTITY_ID, null);
                else
                    dbManager.AddParameters(0, PARM_ENTITY_ID, EntityId);

                dbManager.AddParameters(1, PARM_SHORT_NAME, ShortName);
                dbManager.AddParameters(2, PARM_USER_ID, MDVSession.Current.AppUserId);


                ds = (DSProfile)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ENTITY_SELECT, ds, ds.OrganizationEntity.TableName);
                return ds;

            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEntity::LoadEntity", PROC_ENTITY_SELECT, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }
        }

        public DSProfileLookup LookupEntity()
        {
            DSProfileLookup ds = new DSProfileLookup();
            IDBManager dbManager =ClientConfiguration.GetDBManager();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(1);
          
                dbManager.AddParameters(0, PARM_USER_ID, MDVSession.Current.AppUserId);
               
                ds = (DSProfileLookup)dbManager.ExecuteDataSet(CommandType.StoredProcedure, PROC_ENTITY_LOOKUP, ds, ds.OrganizationEntity.TableName);
                return ds;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALEntity::LookupEntity", PROC_ENTITY_LOOKUP, ex);
                throw ex;
                //Usual code              
            }
            finally
            {
                dbManager.Dispose();
            }



        }
        #endregion
    }
}
