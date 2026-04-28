using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Model.Admin.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.DataAccess.DAL.Admin
{
    public class DALTestType
    {
        #region " Stored Procedure Names"
        private const string PROC_MODIFIER_INSERT = "Provider.sp_ModifierInsert";
        private const string PROC_MODIFIER_UPDATE = "Provider.sp_ModifierUpdate";
        private const string PROC_MODIFIER_DELETE = "Provider.sp_ModifierDelete";
        private const string PROC_TEST_TYPE_SELECT = "Clinical.sp_TestTypeSelect";
        private const string PROC_TEST_TYPE_LOOKUP = "Clinical.sp_TestTypeLookup";
        #endregion

        #region "Parameters"
        private const string PARM_TEST_TYPE_ID = "@TestTypeId";
        private const string PARM_CODE = "@Code";
        private const string PARM_NAME = "@Name";
        private const string PARM_CODE_SYSTEM = "@CodeSystem";
        private const string PARM_IS_ACTIVE = "@IsActive";
        private const string PARM_CREATED_BY = "@CreatedBy";
        private const string PARM_CREATED_ON = "@CreatedOn";
        private const string PARM_MODIFIED_BY = "@ModifiedBy";
        private const string PARM_MODIFIED_ON = "@ModifiedOn";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_BIT = "@Bit";
        private const string PARM_PAGE_NUMBER = "@PageNumber";
        private const string PARM_ROWSP_PAGE = "@RowspPage";
        private const string PARM_RECORD_COUNT = "@RecordCount";

        #endregion


        #region Constructors
        public DALTestType()
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

        #region Lookups
        /// <summary>
        /// Lookups the modifier.
        /// </summary>
        /// <returns></returns>
        public List<TestTypeModel> LookupTestType(string IsActive)
        {
            TestTypeModel model = null;
            List<TestTypeModel> listTestType = new List<TestTypeModel>();
            IDBManager dbManager = ClientConfiguration.GetDBManager();
            SqlDataReader reader = null;

            try
            {
                dbManager.CreateParameters(1);
                IsActive = string.IsNullOrEmpty(IsActive) ? "1" : IsActive;
                dbManager.AddParameters(0, PARM_IS_ACTIVE, IsActive);

                dbManager.Open();
 
                reader = (SqlDataReader)dbManager.ExecuteReader(CommandType.StoredProcedure, PROC_TEST_TYPE_LOOKUP);
                while (reader.Read())
                {
                    model = new TestTypeModel();
                    model.TestTypeId = !String.IsNullOrEmpty(reader["TestTypeId"].ToString()) ? reader["TestTypeId"].ToString() : "";
                    model.Code = !String.IsNullOrEmpty(reader["Code"].ToString()) ? reader["Code"].ToString() : "";
                    model.Name = !String.IsNullOrEmpty(reader["Name"].ToString()) ? reader["Name"].ToString() : "";
                    model.CodeSystem = !String.IsNullOrEmpty(reader["CodeSystem"].ToString()) ? reader["CodeSystem"].ToString() : "";
                    //model.IsActive = !String.IsNullOrEmpty(reader["IsActive"].ToString()) ? reader["IsActive"].ToString() : "";
                    //model.CreatedBy = !String.IsNullOrEmpty(reader["CreatedBy"].ToString()) ? reader["CreatedBy"].ToString() : "";
                    //model.CreatedOn = !String.IsNullOrEmpty(reader["CreatedOn"].ToString()) ? reader["CreatedOn"].ToString() : "";
                    //model.ModifiedBy = !String.IsNullOrEmpty(reader["ModifiedBy"].ToString()) ? reader["ModifiedBy"].ToString() : "";
                    //model.ModifiedOn = !String.IsNullOrEmpty(reader["ModifiedOn"].ToString()) ? reader["ModifiedOn"].ToString() : "";
                    //model.RecordCount = !String.IsNullOrEmpty(reader["RecordCount"].ToString()) ? reader["RecordCount"].ToString() : "";

                    listTestType.Add(model);
                }
                return listTestType;
            }
            catch (Exception ex)
            {
                MDVLogger.DALErrorLog("DALTestType::LookupTestType", PROC_TEST_TYPE_LOOKUP, ex);
                throw ex;
            }
            finally
            {
                dbManager.Dispose();
            }
        }
        #endregion
    }
}
