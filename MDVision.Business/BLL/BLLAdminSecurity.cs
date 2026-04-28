using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Message;
using MDVision.DataAccess.DAL.Settings;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Patient;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;
using MDVision.DataAccess.DCommon;
using MDVision.Datasets;
using MDVision.Model.Native;
using MDVision.Model.Security;
using MDVision.Model.User;
using UserPrivileges = MDVision.Model.Native.UserPrivileges;
using System.Text;
using MDVision.DataAccess.DAL.Patient;
using MDVision.Model;
using MDVision.DataAccess.DAL.Clinical;

namespace MDVision.Business.BLL
{
    public class BLLAdminSecurity
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminSecurity"/> class.
        /// </summary>
        public BLLAdminSecurity()
        {
            //SharedVariable
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.
        //Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new Container();
        }

        #endregion

        #region "Functions"

        #region "Login"
        //public BLObject<DSUsers> Login(ref DSSoftwareCustomersInfo ds, string UserName, string UserPassword, string CustomerRegCode = "", string EntityRegCode = "", string UserMachineIPAddress = "")

        /// <summary>
        /// Logins the specified ds.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="customerRegCode"></param>
        /// <param name="entityRegCode"></param>
        /// <param name="calleeName"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// User is inactive or not created.
        /// or
        /// Selected entity not registered.
        /// </exception>
        //public BLObject<DSUsers> Login(ref DSSoftwareCustomersInfo ds, string CustomerRegCode = "", string EntityRegCode = "", string calleeName = "")
        //{
        //    //ReturnParameters RtdParameter = new ReturnParameters();
        //    string UserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
        //    string UserPassword = MDVUtility.DecryptFrom64(MDVSession.Current.AppPassWord);
        //    DALUsersActivity objActivity = new DALUsersActivity();

        //    try
        //    {

        //        if (ds == null)
        //        {
        //            if (EntityRegCode != "")
        //            {
        //                BLLCommon objBLLCommon = new BLLCommon();
        //                BLObject<DSSoftwareCustomersInfo> ObjGetCustomer = objBLLCommon.GetCustomerSettings(UserName, UserPassword, CustomerRegCode, EntityRegCode);
        //                ds = ObjGetCustomer.Data;
        //            }
        //        }

        //        if (ds != null)
        //        {

        //            if (ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows.Count > 0)
        //            {
        //                // client connection information
        //                DataRow dr = ds.Tables[DSSoftwareCustomersInfo.TABLE_SOFTWARE_CUSTOMERS_INFO].Rows[0];
        //                ClientConfiguration.DBUserId = dr[DSSoftwareCustomersInfo.FIELD_DB_USER_ID].ToString();
        //                ClientConfiguration.DBPassword = dr[DSSoftwareCustomersInfo.FIELD_DB_PASSWORD].ToString();
        //                ClientConfiguration.DataSource = dr[DSSoftwareCustomersInfo.FIELD_DATA_SOURCE].ToString();
        //                ClientConfiguration.InitialCatalog = dr[DSSoftwareCustomersInfo.FIELD_DB_NAME].ToString();
        //                if (dr[DSSoftwareCustomersInfo.FIELD_PERSIST_SECURITY_INFO].ToString() != "")
        //                    ClientConfiguration.PersistSecurityInfo = Convert.ToBoolean(dr[DSSoftwareCustomersInfo.FIELD_PERSIST_SECURITY_INFO].ToString());
        //                else
        //                    ClientConfiguration.PersistSecurityInfo = false;

        //                if (dr[DSSoftwareCustomersInfo.FIELD_IS_PROXY].ToString() != "")
        //                    ClientConfiguration.IsProxy = Convert.ToBoolean(dr[DSSoftwareCustomersInfo.FIELD_IS_PROXY].ToString());
        //                else
        //                    ClientConfiguration.IsProxy = false;

        //                ClientConfiguration.PoolingString = dr[DSSoftwareCustomersInfo.FIELD_POOLING_STRING].ToString();
        //                ClientConfiguration.DBProviderType = dr[DSSoftwareCustomersInfo.FIELD_PROVIDER_TYPE].ToString();

        //                ClientConfiguration.Entity_ID = dr[DSSoftwareCustomersInfo.FIELD_ENTITY_ID].ToString();
        //                ClientConfiguration.Client_ID = dr[DSSoftwareCustomersInfo.FIELD_UNIQUE_CLIENT_ID].ToString();
        //                ClientConfiguration.WebEntityURL = dr[DSSoftwareCustomersInfo.FIELD_WEB_SERVICE_URL].ToString();
        //                //ClientConfiguration.IMOHostName = dr[DSSoftwareCustomersInfo.FIELD_IMOHostName].ToString();
        //                //ClientConfiguration.IMOCPTPort = dr[DSSoftwareCustomersInfo.FIELD_IMOCPTPort].ToString();
        //                //ClientConfiguration.IMOICDPort = dr[DSSoftwareCustomersInfo.FIELD_IMOICDPort].ToString();
        //                //ClientConfiguration.IMO_ID = dr[DSSoftwareCustomersInfo.FIELD_IMO_ID].ToString();
        //                //if (ConfigurationManager.AppSettings["IMOHostName"] != null || ConfigurationManager.AppSettings["IMOHostName"].ToString() != "")
        //                //    ClientConfiguration.IMOHostName = ConfigurationManager.AppSettings["IMOHostName"];
        //                //if (ConfigurationManager.AppSettings["IMOPortNumber"] != null || ConfigurationManager.AppSettings["IMOPortNumber"].ToString() != "")
        //                //    ClientConfiguration.IMOPortNumber = ConfigurationManager.AppSettings["IMOPortNumber"];
        //                //if (ConfigurationManager.AppSettings["IMO_ID"] != null || ConfigurationManager.AppSettings["IMO_ID"].ToString() != "")
        //                //    ClientConfiguration.IMO_ID = ConfigurationManager.AppSettings["IMO_ID"];
        //                //get user information
        //                DSUsers dsUser = new DSUsers();
        //                dsUser = new DALUser().LoginUser(0, UserName, ClientConfiguration.Entity_ID, "1");

        //                // InsertLoginAttempt(True, IPAddress, param.UserName)
        //                if (UserName.ToUpper() != ClientConfiguration.DefaultUser && dsUser.Users.Rows.Count <= 0)
        //                {

        //                    //     // check here user option " like login attempt
        //                    //      dsUser = DLLUser.Instance.LoadEntityUserOption(ref dsUser, UserName, ClientConfiguration.Entity_ID);
        //                    //DLLUser.Instance.LoadEntityUserOption
        //                    throw new Exception("Can’t connect, either user is inactive or entity is not assigned.");
        //                }
        //                else if (dsUser.Users.Rows.Count <= 0)
        //                    throw new Exception("Can’t connect, user is inactive");
        //                //false
        //                // set property
        //                // get the user entity option
        //                dsUser.Merge(new DALUser().LoadEntityUserOption(ref dsUser, UserName, ClientConfiguration.Entity_ID));
        //                //get the user privileges
        //                dsUser.Merge(new DALUser().LoadUserPrivileges(ref dsUser, UserName));

        //                DALMessage objMessage = new DALMessage();
        //                DSMessage dsUserMessages = objMessage.LoadMessage(0, 0, "", 2, null, null, dsUser.Users[0].UserId, "", 1, 1000);
        //                //Set Current User Messages Count
        //                dsUser.Users[0][dsUser.Users.MessagesCountColumn.ColumnName] = dsUserMessages.Tables[dsUserMessages.PatMessages.TableName].Select(dsUserMessages.PatMessages.MessageTypeColumn.ColumnName + "<>'Task'").Length;//.Rows.Count;
        //                //Set Current User Tasks Count
        //                dsUser.Users[0][dsUser.Users.UserTasksCountColumn.ColumnName] = dsUserMessages.Tables[dsUserMessages.PatMessages.TableName].Select(dsUserMessages.PatMessages.MessageTypeColumn.ColumnName + "='Task'").Length;//.Rows.Count;
        //                //if (UserName != "MDVISION" && dsUser.Tables(0).Rows(0).Item("ACTIVE_FLAG").tostring = 'N')
        //                //{
        //                //}
        //                //    //drUser["Active_FLAG"].ToString() == "Y" ? true : false;

        //                //    throw new Exception(UserName + " is Inactive.");
        //                //}
        //                //ClientID = drUser["CLIENT_ID"].ToString();

        //                // Start 26/11/2015 Muhammad Irfan 
        //                DALAppointment objAppointment = new DALAppointment();
        //                string AppDate = DateTime.Now.Date.ToString();
        //                DSAppointment dsAppointment = objAppointment.LoadAppointmentsVisits(0, 0, 0, AppDate, "", "", "", null, "0", 1, 1000, "");
        //                if (dsAppointment.Tables[dsAppointment.AppointmentsVisits.TableName].Rows.Count > 0)
        //                {
        //                    dsUser.Users[0][dsUser.Users.AppointmentsCountColumn.ColumnName] = dsAppointment.AppointmentsVisits.Rows[0][dsAppointment.AppointmentsVisits.RecordCountColumn.ColumnName];
        //                }

        //                DALAppointment objAppointmentNotes = new DALAppointment();
        //                DSAppointment dsAppointmentNotes = objAppointmentNotes.LoadAppointmentsNotes("", "", "Draft", "", "", "", 0, "");
        //                if (dsAppointmentNotes.Tables[dsAppointmentNotes.AppointmentsVisits.TableName].Rows.Count > 0)
        //                {
        //                    dsUser.Users[0][dsUser.Users.NotesCountColumn.ColumnName] = dsAppointmentNotes.Tables[dsAppointmentNotes.AppointmentsVisits.TableName].Rows[0][dsAppointmentNotes.AppointmentsVisits.RecordCountColumn.ColumnName]; ; // dsAppointmentNotes.Tables[dsAppointmentNotes.AppointmentsVisits.TableName].Rows.Count;
        //                }

        //                // End 26/11/2015 Muhammad Irfan 

        //                ClientConfiguration.UserName = UserName;
        //                ClientConfiguration.UserPassword = UserPassword;
        //                DSUsersActivity dsUsersActivity = objActivity.GetUsersActivityEventLookup();
        //                DALUsersActivity.SetEventProperty(dsUsersActivity);
        //                objActivity.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, true, "Log in user");

        //                return new BLObject<DSUsers>(dsUser);
        //            }
        //            else
        //                throw new Exception("Selected entity not registered.");

        //        }
        //        return new BLObject<DSUsers>(null);
        //    }

        //    catch (Exception ex)
        //    {
        //        objActivity.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, false, "Error during Log in user: " + ex.Message);
        //        MDVLogger.BLLErrorLog("BLLAdminSecurity::Login", ex);
        //        return new BLObject<DSUsers>(null, ex.Message);

        //    }
        //}
      

        public BLObject<List<UserModel>> Login_(List<SoftwareCustomerInfoModel_> list, string customerRegCode = "", string entityRegCode = "", string calleeName = "")
                        {
            var userName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            var userPassword = MDVUtility.DecryptFrom64(MDVSession.Current.AppPassWord);
            var objActivity = new DALUsersActivity();

            try
            {
                // MK
                //if (list == null)
                        //{
                //    if (entityRegCode != "")
                //    {
                //        BLLCommon objBllCommon = new BLLCommon();
                //        BLObject<DSSoftwareCustomersInfo> objGetCustomer = objBllCommon.GetCustomerSettings(userName, userPassword, customerRegCode, entityRegCode);
                //        list = objGetCustomer.Data;
                //    }
                        //}

                if (list == null) return new BLObject<List<UserModel>>(null);
                if (list.Count <= 0) throw new Exception("Selected entity not registered.");

                ClientConfiguration.DBUserId = list[0].DBUserId;
                ClientConfiguration.DBPassword = list[0].DBPassword;
                ClientConfiguration.DataSource = list[0].DataSource;
                ClientConfiguration.InitialCatalog = list[0].DBName;
                ClientConfiguration.PersistSecurityInfo = list[0].PersistSecurityInfo != "" &&
                                                          Convert.ToBoolean(list[0].PersistSecurityInfo != "0");
                ClientConfiguration.IsProxy = list[0].IsProxy != "" && Convert.ToBoolean(list[0].IsProxy != "0");
                ClientConfiguration.PoolingString = list[0].PoolingString;
                ClientConfiguration.DBProviderType = list[0].ProviderType;
                ClientConfiguration.Entity_ID = list[0].EntityId;
                ClientConfiguration.Client_ID = list[0].UniqueClientId;

                if (string.IsNullOrEmpty(ClientConfiguration.WebEntityURL))
                    ClientConfiguration.WebEntityURL = list[0].WebServiceURL;

                //var dsUser = new DALUser().LoginUser(0, userName, ClientConfiguration.Entity_ID, "1");
                var listUser = new DALUser().LoginUser_(0, userName, ClientConfiguration.Entity_ID, "1");
                if (userName.ToUpper() != ClientConfiguration.DefaultUser && listUser.Count <= 0)
                        {
                    throw new Exception("Can’t connect, either user is inactive or entity is not assigned.");
                        }
                else if (listUser.Count <= 0)
                    throw new Exception("Can’t connect, user is inactive");

                var listEntityUserOptions = new DALUser().LoadEntityUserOption_(listUser, userName, ClientConfiguration.Entity_ID);
                var listUserPrivileges = new DALUser().LoadUserPrivileges_(userName);
                var listUserMessages = new DALMessage().LoadMessage_(0, 0, "", 2, null, null, MDVUtility.ToLong(listUser[0].UserId), "", 1, 1000);

                if (listUserMessages.Count > 0)
                        {
                    listUser[0].MessagesCount = listUserMessages[0].OtherCount;
                        //listUserMessages.Count(n => n.MessageType != "Task").ToString();
                    listUser[0].UserTasksCount = listUserMessages[0].TaskCount;
                        //listUserMessages.Count(n => n.MessageType == "Task").ToString();
                        }
                else
                {
                    listUser[0].MessagesCount = "0";
                    listUser[0].UserTasksCount = "0";
                }

                if (listEntityUserOptions.Count > 0)
                {
                    var appDate = DateTime.Now.Date.ToString(CultureInfo.InvariantCulture);
                    var listVisitAppointment = new DALAppointment().LoadAppointmentsVisits_(0, 0, 0, appDate, "", "", "", null, listEntityUserOptions[0].Appointmentstatus, 1, 1000, "");
                    if (listVisitAppointment.Count > 0)
                        listUser[0].AppointmentsCount = listVisitAppointment[0].RecordCount;
                }
                else
                {
                    listUser[0].AppointmentsCount = "0";
                }
              
                
                // get the total pending count.
                var listdocs = new DALPatientDocument().LoadDashboardDocument(null, null, 1, 15, null, MDVUtility.ToStr(MDVSession.Current.AppUserId), null, "Pending");
                if (listdocs.ListDDocumentModel.Count > 0)
                    listUser[0].PendingDocumentsCount = listdocs.ListDDocumentModel[0].Pending;

                var listAppointmentNotes = new DALAppointment().LoadAppointmentsNotes_("", "", "Draft", "", "", "", 0, "");
                if (listAppointmentNotes.Count > 0)
                    listUser[0].NotesCount = listAppointmentNotes[0].RecordCount;

                listUser[0].AssignedResultsCount = new DALLabResult().GetAssignedLabResultsCount(MDVSession.Current.AppUserId);
                    
                ClientConfiguration.UserName = userName;
                ClientConfiguration.UserPassword = userPassword;

                var dsUsersActivity = objActivity.GetUsersActivityEventLookup();
                        DALUsersActivity.SetEventProperty(dsUsersActivity);
                        objActivity.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn, DALUsersActivity.TITLE_SOFTWARE_NAME, true, "Log in user");

                var userModel = new List<UserModel>();
                var model = new UserModel
                {
                    UserLoginModel = listUser,
                    EntityUserOptions = listEntityUserOptions,
                    UserPrivileges = listUserPrivileges
                };
                userModel.Add(model);
                return new BLObject<List<UserModel>>(userModel);

                /*
                                throw new Exception("Selected entity not registered.");
                */
                }
            catch (Exception ex)
            {
                objActivity.InsertUsersActivityLog(DALUsersActivity.AuditableEvents.LogIn,
                    DALUsersActivity.TITLE_SOFTWARE_NAME, false, "Error during Log in user: " + ex.Message);
                MDVLogger.BLLErrorLog("BLLAdminSecurity::Login", ex);
                return new BLObject<List<UserModel>>(null, ex.Message);

            }
        }

        static int _attemptsMade;

        /// <summary>
        /// GetLoginAttempts
        /// </summary>
        /// <param name="maxAttemptsAllowed"></param>
        /// <returns></returns>
        public bool GetLoginAttempts(int maxAttemptsAllowed)
        {
            if (_attemptsMade >= maxAttemptsAllowed)
            {
                _attemptsMade = 0;
                return true;
            }
            else
            {
                _attemptsMade = _attemptsMade + 1;
                return false;
            }
        }

        #endregion

        #region "Users"

        /// <summary>
        /// Loads the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="isActive">The is active.</param>
        /// <param name="isAdmin">The is admin.</param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public BLObject<DSUsers> LoadUser(long userId, string userName, string entityId, string firstName, string lastName, string isActive, string isAdmin, int pageNumber = 1, int rowsPerPage = 1000)
        {
            try
            {
                DSUsers ds = new DSUsers();
                ds = new DALUser().LoadUser(ref ds, userId, userName, entityId, firstName, lastName, isActive, isAdmin, pageNumber, rowsPerPage);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadUser", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// CheckUserHaveNoteUnSignRights
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BLObject<string> CheckUserHaveNoteUnSignRights(Int64 userId)
        {
            try
            {
                var isUserHaveNoteUnSignRights = new DALModulesForms().CheckUserHaveNoteUnSignRights(userId);
                return new BLObject<string>(isUserHaveNoteUnSignRights);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLClinical::CheckUserHaveNoteUnSignRights", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadModuleFormUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <param name="moduleFormId"></param>
        /// <returns></returns>
        public BLObject<DSUsers> LoadModuleFormUser(long userId, long moduleId, long moduleFormId)
        {
            try
            {
                var dsModuleForm = new DALModulesForms().LoadModules(0, "1");
                if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName).ToUpper() != ClientConfiguration.DefaultUser)
                {
                    if (dsModuleForm.Tables[dsModuleForm.Modules.TableName].Rows.Count > 0)
                    {
                        var rows = dsModuleForm.Tables[dsModuleForm.Modules.TableName].Select(dsModuleForm.Modules.NameColumn.ColumnName + "='Admin'");
                        foreach (var item in rows)
                        {
                            item.Delete();
                        }
                        dsModuleForm.AcceptChanges();
                    }
                }

                if (moduleId == 0)
                {
                    if (dsModuleForm.Modules.Rows.Count > 0)
                        dsModuleForm.Merge(new DALModulesForms().LoadModuleForms(MDVUtility.ToLong(dsModuleForm.Modules.Rows[0][dsModuleForm.Modules.ModuleIdColumn.ColumnName]), "1"));
                    else
                        throw new Exception("Module not exists");
                }
                else
                {
                    dsModuleForm.Merge(new DALModulesForms().LoadModuleForms(moduleId, "1"));
                }

                dsModuleForm.Merge(new DALModulesForms().LoadPrivileges(0, "1"));
                DSUsers dsTempUser = null;
                var dsUser = new DALUser().LoadUser(ref dsTempUser, userId, null, null, null, null, null, null);
                dsUser.Merge(new DALUser().LoadModuleFormUser(0, userId));
                foreach (DSModuleForm.ModulesRow dr in dsModuleForm.Modules.Rows)
                {
                    DataRow[] drModuleUser = dsUser.ModuleFormUsers.Select(dsUser.ModuleFormUsers.ModuleIdColumn.ColumnName + "=" + dr.ModuleId);
                    dr.IsSelected = drModuleUser.Length > 0;
                }
                foreach (DSModuleForm.ModuleFormsRow dr in dsModuleForm.ModuleForms.Rows)
                {
                    DataRow[] drModuleFormUser = dsUser.ModuleFormUsers.Select(dsUser.ModuleFormUsers.ModuleFormIdColumn.ColumnName + "=" + dr.ModuleFormId);
                    if (drModuleFormUser.Length > 0)
                        dr.MFUId = MDVUtility.ToLong(drModuleFormUser[0][dsUser.ModuleFormUsers.MFUIdColumn.ColumnName]);
                }
                if (moduleFormId == 0)
                {
                    if (dsModuleForm.ModuleForms.Rows.Count > 0)
                        dsUser.Merge(new DALUser().LoadModuleFormUsersPrivileges(0, userId, MDVUtility.ToLong(dsModuleForm.ModuleForms.Rows[0][dsModuleForm.ModuleForms.ModuleFormIdColumn.ColumnName])));
                    else
                        throw new Exception("Module Forms not exists");
                }
                else
                {
                    dsUser.Merge(new DALUser().LoadModuleFormUsersPrivileges(0, userId, moduleFormId));
                }
                foreach (DSModuleForm.PrivilegesRow dr in dsModuleForm.Privileges.Rows)
                {
                    DataRow[] drModuleFormPrivilege = dsUser.ModuleFormUsersPrivileges.Select(dsUser.ModuleFormUsersPrivileges.PrivilegeSelectionIdColumn.ColumnName + "=" + dr.PrivilegeSelectionid);
                    if (drModuleFormPrivilege.Length > 0)
                        dr.ModuleFormUserPriviligesId = MDVUtility.ToLong(drModuleFormPrivilege[0][dsUser.ModuleFormUsersPrivileges.ModuleFormUserPriviligesIdColumn.ColumnName]);
                }

                // Privilege Group
                // dsUser.Merge(new DALPrivilegeGroup().LoadPrivilegeGroup(0, "", "", "1"));
                var dsPrivilegeGroup = new DALPrivilegeGroup().LoadPrivilegeGroup(0, "", "", "1", 1, 1000);

                dsUser.Merge(new DALUser().LoadUserEntityGroup(0, userId));


                foreach (DSPrivilegeGroup.SecurityGroupRow dr in dsPrivilegeGroup.SecurityGroup.Rows)
                {
                    DataRow[] drUsersEntityGroup = dsUser.UsersEntityGroup.Select(dsUser.UsersEntityGroup.SecurityGroupIdColumn.ColumnName + "=" + dr.SecGroupId);
                    if (drUsersEntityGroup.Length > 0)
                        dr.UserEntityGroupId = MDVUtility.ToLong(drUsersEntityGroup[0][dsUser.UsersEntityGroup.UserEntityGroupIdColumn.ColumnName]);

                }

                dsUser.Merge(dsPrivilegeGroup);
                dsUser.Merge(dsModuleForm);
                return new BLObject<DSUsers>(dsUser);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadModuleFormUsers", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the user.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSUserLookup> LookupUser(string active, string allUsers = "")
        {
            try
            {
                var ds = new DALUser().LookupUser(active, allUsers);
                return new BLObject<DSUserLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupUser", ex);
                return new BLObject<DSUserLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// LookupCoWorkerGroupUser
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public BLObject<DSUserLookup> LookupCoWorkerGroupUser(string active = "")
        {
            try
            {
                var ds = new DALUser().LookupCoWorkerGroupUser();
                return new BLObject<DSUserLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupCoWorkerGroupUser", ex);
                return new BLObject<DSUserLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// LookupUsersForCoWorker
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public BLObject<DSUserLookup> LookupUsersForCoWorker(string active = "")
        {
            try
            {
                DSUserLookup ds = new DALUser().LookupUsersForCoWorker();
                return new BLObject<DSUserLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupUsersForCoWorker", ex);
                return new BLObject<DSUserLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// LookupFullUserName
        /// </summary>
        /// <param name="active"></param>
        /// <param name="allUsers"></param>
        /// <returns></returns>
        public BLObject<DSUserLookup> LookupFullUserName(string active, string allUsers = "")
        {
            try
            {
                DSUserLookup ds = new DALUser().LookupFullUserName(active, allUsers);
                return new BLObject<DSUserLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupFullUserName", ex);
                return new BLObject<DSUserLookup>(null, ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserentityGroupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BLObject<DSUsers> LookupSecurityEntityGroup(string IsActive)
        {
            try
            {
                DSUsers ds = new DALUser().UserSecurityGroupLookUp(IsActive);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupEntityUserGroup", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public BLObject<string> DeleteUser(string userIds)
        {
            try
            {
                userIds = new DALUser().DeleteUser(userIds);
                return new BLObject<string>(userIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteUser", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSUsers> InsertUser(ref DSUsers ds)
        {
            
            try
            {
              
                ds = new DALUser().InsertUser(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertUser", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        public BLObject<string> LoggedInUserInsert(long UserId, string SessionId)
        {

            try
            {
                return new BLObject<string>(new DALUser().LoggedInUserInsert(UserId, SessionId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoggedInUserInsert", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<LoggedInUserModel> LoggedInUserCheck(long UserId, string SessionId)
        {

            try
            {
                return new BLObject<LoggedInUserModel>(new DALUser().LoggedInUserCheck(UserId, SessionId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LoggedInUserCheck", ex);
                return new BLObject<LoggedInUserModel>(null, ex.Message);
            }
        }

        /// <summary>
        /// InsertCoWorkersGroup
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSCoWorkersGroup> InsertCoWorkersGroup(ref DSCoWorkersGroup ds)
        {
            try
            {
                ds = new DALUser().InsertCoWorkersGroup(ref ds);
                return new BLObject<DSCoWorkersGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertCoWorkersGroup", ex);
                return new BLObject<DSCoWorkersGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// UpdateCoWorkersGroup
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSCoWorkersGroup> UpdateCoWorkersGroup(ref DSCoWorkersGroup ds)
        {
            try
            {
                ds = new DALUser().UpdateCoWorkersGroup(ref ds);
                return new BLObject<DSCoWorkersGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateCoWorkersGroup", ex);
                return new BLObject<DSCoWorkersGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSUsers> UpdateUser(ref DSUsers ds)
        {
            try
            {
                ds = new DALUser().UpdateUser(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateUser", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// UpdateUserPassword
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSUsers> UpdateUserPassword(ref DSUsers ds)
        {
            try
            {
                ds = new DALUser().UpdateUserPassword(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdateUserPassword", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }
        public BLObject<string> UpdateUserPassword(int UserId, string Username, string UserPassword)
        {
            try
            {
                string returnedUserId = "";

                returnedUserId = new DALUser().UpdateUserPassword(UserId, Username, UserPassword);

                return new BLObject<string>(returnedUserId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::UpdateUserPassword", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// GetPasswordRegex
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BLObject<string> GetPasswordRegex(int userId)
        {
            try
            {
                return new BLObject<string>(new DALUser().GetPasswordRegex(userId));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::GetPasswordRegex", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// IsUserExist
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public BLObject<string> IsUserExist(string userName)
        {
            try
            {
                userName = new DALUser().IsUserExist(userName);
                return new BLObject<string>(userName);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::IsUserExist", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the user.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSUserLookup> LookupUserRolesUser(string active, string userId, string userRoleId, string emergencyRoleId)
        {
            try
            {
                var ds = new DALUser().LookupUserRolesUser(active, userId, userRoleId, emergencyRoleId);
                return new BLObject<DSUserLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupUser", ex);
                return new BLObject<DSUserLookup>(null, ex.Message);
            }
        }

        public BLObject<List<LookUpModel>> LookupUserType()
        {
            try
            {
                var obj = new DALUser().LookupUserType();
                return new BLObject<List<LookUpModel>>(obj);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupUserType", ex);
                return new BLObject<List<LookUpModel>>(null, ex.Message);
            }
        }

        #region "Users Module Forms Privileges"

        /// <summary>
        /// Inserts the module form users.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSUsers> InsertModuleFormUsers(ref DSUsers ds)
        {
            try
            {
                ds = new DALUser().InsertModuleFormUsers(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertModuleFormUsers", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the module form users.
        /// </summary>
        /// <param name="moduleFormUsersIds">The module form users ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteModuleFormUsers(string moduleFormUsersIds,long AuditUserId)
        {
            try
            {
                moduleFormUsersIds = new DALUser().DeleteModuleFormUsers(moduleFormUsersIds, AuditUserId);
                return new BLObject<string>(moduleFormUsersIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteModuleFormUsers", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the module form users privileges.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSUsers> InsertModuleFormUsersPrivileges(ref DSUsers ds)
        {
            try
            {
                ds = new DALUser().InsertModuleFormUsersPrivileges(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertModuleFormUsersPrivileges", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the module form users privileges.
        /// </summary>
        /// <param name="moduleFormUsersPrivilegesIds"></param>
        /// <param name="privilegeName"></param>
        /// <param name="formName"></param>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public BLObject<string> DeleteModuleFormUsersPrivileges(string moduleFormUsersPrivilegesIds, string privilegeName, string formName, string assignedTo, long AuditUserId)
        {
            try
            {
                moduleFormUsersPrivilegesIds = new DALUser().DeleteModuleFormUsersPrivileges(moduleFormUsersPrivilegesIds, privilegeName, formName, assignedTo, AuditUserId);
                return new BLObject<string>(moduleFormUsersPrivilegesIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteModuleFormUsersPrivileges", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the module users.
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <param name="moduleIds">The module ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteModuleUsers(long userIds, long moduleIds)
        {
            try
            {
                string str = new DALUser().DeleteModuleUsers(userIds, moduleIds);
                return new BLObject<string>(str);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteModuleUsers", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        #endregion

        #region "User Entity Group"

        /// <summary>
        /// Deletes the UserEntityGroup.
        /// </summary>
        /// <param name="userEntityGroupIds">The role ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteUserEntityGroup(string userEntityGroupIds)
        {
            try
            {
                userEntityGroupIds = new DALUser().DeleteUserEntityGroup(userEntityGroupIds);
                return new BLObject<string>(userEntityGroupIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteUserEntityGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the UserEntityGroup.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSUsers> InsertUserEntityGroup(ref DSUsers ds)
        {
            try
            {
                ds = new DALUser().InsertUserEntityGroup(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::InsertUserEntityGroup", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        #endregion

        #endregion

        #region "Roles"
        /// <summary>
        /// Loads the roles.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSRoles> LoadRoles(long roleId, string roleName, string description, string isActive, int pageNumber = 1, int rowspPage = 1000)
        {
            try
            {
                var ds = new DALRoles().LoadRoles(roleId, roleName, description, isActive, pageNumber, rowspPage);
                return new BLObject<DSRoles>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadRoles", ex);
                return new BLObject<DSRoles>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteRole(string roleIds)
        {
            try
            {
                roleIds = new DALRoles().DeleteRole(roleIds);
                return new BLObject<string>(roleIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteRole", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the role.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSRoles> InsertRole(ref DSRoles ds)
        {
            try
            {
                ds = new DALRoles().InsertRole(ref ds);
                return new BLObject<DSRoles>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::InsertRole", ex);
                return new BLObject<DSRoles>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSRoles> UpdateRole(ref DSRoles ds)
        {
            try
            {
                ds = new DALRoles().UpdateRole(ref ds);
                return new BLObject<DSRoles>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::UpdateRole", ex);
                return new BLObject<DSRoles>(null, ex.Message);
            }
        }

        #region "Roles Module Form Privileges"

        /// <summary>
        /// Loads the module form roles.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Module not exists
        /// or
        /// Module Forms not exists
        /// </exception>
        public BLObject<DSRoles> LoadModuleFormRoles(long roleId, long moduleId, long moduleFormId)
        {
            try
            {
                var dsModuleForm = new DALModulesForms().LoadModules(0, "1");
                //if ( MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) != ClientConfiguration.DefaultUser)
                //{
                //    if (dsModuleForm.Tables[dsModuleForm.Modules.TableName].Rows.Count > 0)
                //    {
                //        DataRow[] rows = dsModuleForm.Tables[dsModuleForm.Modules.TableName].Select(dsModuleForm.Modules.NameColumn.ColumnName + "='Admin'");
                //        foreach (var item in rows)
                //        {
                //            item.Delete();
                //        }
                //        dsModuleForm.AcceptChanges();
                //    }
                //}

                if (moduleId == 0)
                {
                    if (dsModuleForm.Modules.Rows.Count > 0)
                        dsModuleForm.Merge(new DALModulesForms().LoadModuleForms(MDVUtility.ToLong(dsModuleForm.Modules.Rows[0][dsModuleForm.Modules.ModuleIdColumn.ColumnName]), "1"));
                    else
                        throw new Exception("Module not exists");
                }
                else
                    dsModuleForm.Merge(new DALModulesForms().LoadModuleForms(moduleId, "1"));

                dsModuleForm.Merge(new DALModulesForms().LoadPrivileges(0, "1"));

                var dsRoles = new DALRoles().LoadRoles(roleId, null, null, null);
                dsRoles.Merge(new DALRoles().LoadModuleFormRoles(0, roleId));
                foreach (DSModuleForm.ModulesRow dr in dsModuleForm.Modules.Rows)
                {
                    DataRow[] drModuleRole = dsRoles.ModuleFormRoles.Select(dsRoles.ModuleFormRoles.ModuleIdColumn.ColumnName + "=" + dr.ModuleId);
                    dr.IsSelected = drModuleRole.Length > 0;
                }
                foreach (DSModuleForm.ModuleFormsRow dr in dsModuleForm.ModuleForms.Rows)
                {
                    DataRow[] drModuleFormRole = dsRoles.ModuleFormRoles.Select(dsRoles.ModuleFormRoles.ModuleFormIdColumn.ColumnName + "=" + dr.ModuleFormId);
                    if (drModuleFormRole.Length > 0)
                        dr.MFRId = MDVUtility.ToLong(drModuleFormRole[0][dsRoles.ModuleFormRoles.MFRIdColumn.ColumnName]);
                }

                if (moduleFormId == 0)
                {
                    if (dsModuleForm.ModuleForms.Rows.Count > 0)
                        dsRoles.Merge(new DALRoles().LoadModuleFormRolesPrivileges(0, roleId, MDVUtility.ToLong(dsModuleForm.ModuleForms.Rows[0][dsModuleForm.ModuleForms.ModuleFormIdColumn.ColumnName])));
                    else
                        throw new Exception("Module Forms not exists");
                }
                else
                {
                    dsRoles.Merge(new DALRoles().LoadModuleFormRolesPrivileges(0, roleId, moduleFormId));
                }

                foreach (DSModuleForm.PrivilegesRow dr in dsModuleForm.Privileges.Rows)
                {
                    DataRow[] drModuleFormPrivilege = dsRoles.ModuleFormRolePrivileges.Select(dsRoles.ModuleFormRolePrivileges.PrivilegeSelectionidColumn.ColumnName + "=" + dr.PrivilegeSelectionid);
                    if (drModuleFormPrivilege.Length > 0)
                        dr.ModuleFormRolePrivilegesId = MDVUtility.ToLong(drModuleFormPrivilege[0][dsRoles.ModuleFormRolePrivileges.ModuleFormRolePrivilegesIdColumn.ColumnName]);
                }

                dsRoles.Merge(dsModuleForm);
                return new BLObject<DSRoles>(dsRoles);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadModuleFormRoles", ex);
                return new BLObject<DSRoles>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the modules forms privileges.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteModulesFormsPrivileges(long roleId, long moduleId)
        {
            try
            {
                var str = new DALRoles().DeleteModulesFormsPrivileges(0, roleId, moduleId);
                return new BLObject<string>(str);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteModulesFormsPrivileges", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the module form role.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSRoles> InsertModuleFormRole(ref DSRoles ds)
        {
            try
            {
                ds = new DALRoles().InsertModuleFormRole(ref ds);
                return new BLObject<DSRoles>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertModuleFormRole", ex);
                return new BLObject<DSRoles>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the module form role privileges.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSRoles> InsertModuleFormRolePrivileges(ref DSRoles ds)
        {
            try
            {
                ds = new DALRoles().InsertModuleFormRolePrivileges(ref ds);
                return new BLObject<DSRoles>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertModuleFormRolePrivileges", ex);
                return new BLObject<DSRoles>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the module form role.
        /// </summary>
        /// <param name="moduleFormRoleId">The module form role identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteModuleFormRole(string moduleFormRoleId)
        {
            try
            {
                moduleFormRoleId = new DALRoles().DeleteModuleFormRole(moduleFormRoleId);
                return new BLObject<string>(moduleFormRoleId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteModuleFormRole", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the module form role privilege.
        /// </summary>
        /// <param name="moduleFormRolePrivilegeId">The module form role privilege identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteModuleFormRolePrivilege(string moduleFormRolePrivilegeId)
        {
            try
            {
                moduleFormRolePrivilegeId = new DALRoles().DeleteModuleFormRolePrivilege(moduleFormRolePrivilegeId);
                return new BLObject<string>(moduleFormRolePrivilegeId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteModuleFormRolePrivilege", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #endregion

        #region "Modules"
        /// <summary>
        /// Loads the modules.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns></returns>
        public BLObject<DSModuleForm> LoadModules(long moduleId, string isActive)
        {
            try
            {
                var ds = new DALModulesForms().LoadModules(moduleId, isActive);
                return new BLObject<DSModuleForm>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadModules", ex);
                return new BLObject<DSModuleForm>(null, ex.Message);
            }
        }

        #endregion

        #region "Modules Forms"
        /// <summary>
        /// Loads the forms.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="formName">Name of the form.</param>
        /// /// <param name="formDescription">Name of the form.</param>
        /// <returns></returns>
        public BLObject<DSModuleForm> LoadForms(long formId, string formName, string formDescription)
        {
            try
            {
                var ds = new DALModulesForms().LoadForms(formId, formName, formDescription);
                return new BLObject<DSModuleForm>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadForms", ex);
                return new BLObject<DSModuleForm>(null, ex.Message);
            }
        }

        /// <summary>
        /// Loads the module forms.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// /// <param name="moduleName">The is active.</param>
        /// <returns></returns>
        public BLObject<DSModuleForm> LoadModuleForms(long moduleId, string isActive, string moduleName)
        {
            try
            {
                var ds = new DALModulesForms().LoadModuleForms(moduleId, isActive, moduleName);
                return new BLObject<DSModuleForm>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadModuleForms", ex);
                return new BLObject<DSModuleForm>(null, ex.Message);
            }
        }
        public BLObject<DSModuleForm> LoadModuleFormsByUser(long moduleId, string UserId)
        {
            try
            {
                var ds = new DALModulesForms().LoadModuleFormsByUser(moduleId, UserId);
                return new BLObject<DSModuleForm>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadModuleForms", ex);
                return new BLObject<DSModuleForm>(null, ex.Message);
            }
        }
        #endregion

        #region "Forms Privileges"
        /// <summary>
        /// Loads the privileges.
        /// </summary>
        /// <param name="privilegeId">The privilege identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns></returns>
        public BLObject<DSModuleForm> LoadPrivileges(long privilegeId, string isActive)
        {
            try
            {
                var ds = new DALModulesForms().LoadPrivileges(privilegeId, isActive);
                return new BLObject<DSModuleForm>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadPrivileges", ex);
                return new BLObject<DSModuleForm>(null, ex.Message);
            }
        }
        #endregion

        #region Privilege / Security Group
        /// <summary>
        /// Inserts the privilege group.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> InsertPrivilegeGroup(ref DSPrivilegeGroup ds)
        {
            try
            {
                ds = new DALPrivilegeGroup().InsertPrivilegeGroup(ref ds);
                return new BLObject<DSPrivilegeGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::InsertPrivilegeGroup", ex);
                return new BLObject<DSPrivilegeGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Loads the privilege group.
        /// </summary>
        /// <param name="privilegeGroupId">The privilege group identifier.</param>
        /// <param name="shortName">The short name.</param>
        /// <param name="description">The description.</param>
        /// <param name="isActive">The is active.</param>
        /// <param name="pageNumber">The is active.</param>
        /// /// <param name="rowspPage">The is active.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> LoadPrivilegeGroup(long privilegeGroupId, string shortName, string description, string isActive, int pageNumber = 1, int rowspPage = 1000)
        {
            try
            {
                var ds = new DALPrivilegeGroup().LoadPrivilegeGroup(privilegeGroupId, shortName, description, isActive, pageNumber, rowspPage);
                return new BLObject<DSPrivilegeGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadPrivilegeGroup", ex);
                return new BLObject<DSPrivilegeGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the privilege group.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> UpdatePrivilegeGroup(ref DSPrivilegeGroup ds)
        {
            try
            {
                ds = new DALPrivilegeGroup().UpdatePrivilegeGroup(ref ds);
                return new BLObject<DSPrivilegeGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::UpdatePrivilegeGroup", ex);
                return new BLObject<DSPrivilegeGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the privilege group.
        /// </summary>
        /// <param name="privilegeGroupIds">The privilege group ids.</param>
        /// <returns></returns>
        public BLObject<string> DeletePrivilegeGroup(string privilegeGroupIds)
        {
            try
            {
                privilegeGroupIds = new DALPrivilegeGroup().DeletePrivilegeGroup(privilegeGroupIds);
                return new BLObject<string>(privilegeGroupIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeletePrivilegeGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Loads the privilege group by privilege group identifier.
        /// </summary>
        /// <param name="privilegeGroupId">The privilege group identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> LoadPrivilegeGroupByPrivilegeGroupId(long privilegeGroupId, long facilityId)
        {
            DSProfile dsProfile = new DSProfile();
            var dsPrivGroup = new DALPrivilegeGroup().LoadPrivilegeGroup(privilegeGroupId, null, null, null, 1, 1000);
            dsPrivGroup.Merge(new DALFacility().LoadFacility(facilityId, "", "", "", "", ""));
            dsPrivGroup.Merge(new DALPrivilegeGroup().LoadSecurityGroupFacility(0, privilegeGroupId));
            foreach (DataRow dr in dsPrivGroup.Tables[dsProfile.Facility.TableName].Rows)
            {
                DataRow[] drSecFacility = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupFacility.TableName].Select(dsPrivGroup.SecurityGroupFacility.FacilityIdColumn.ColumnName + "=" + dr[dsProfile.Facility.FacilityIdColumn.ColumnName]);
                if (drSecFacility.Length > 0)
                    dr[dsProfile.Facility.SecGroupFacilityIdColumn.ColumnName] = drSecFacility[0][dsPrivGroup.SecurityGroupFacility.SecGroupFacilityIdColumn.ColumnName];
            }

            dsPrivGroup.Merge(new DALProvider().LoadProvider(0, "", "", "", "", "", "", ""));
            dsPrivGroup.Merge(new DALPrivilegeGroup().LoadSecurityGroupProvider(0, privilegeGroupId));
            foreach (DataRow dr in dsPrivGroup.Tables[dsProfile.Provider.TableName].Rows)
            {
                DataRow[] drSecProvider = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupProvider.TableName].Select(dsPrivGroup.SecurityGroupProvider.ProviderIdColumn.ColumnName + "=" + dr[dsProfile.Provider.ProviderIdColumn.ColumnName]);
                if (drSecProvider.Length > 0)
                    dr[dsProfile.Provider.SecGroupProviderIdColumn.ColumnName] = drSecProvider[0][dsPrivGroup.SecurityGroupProvider.SecGroupProviderIdColumn.ColumnName];
            }

            dsPrivGroup.Merge(new DALResources().LoadResources(0, "", ""));
            dsPrivGroup.Merge(new DALPrivilegeGroup().LoadSecurityGroupResources(0, privilegeGroupId));
            foreach (DataRow dr in dsPrivGroup.Tables[dsProfile.Resources.TableName].Rows)
            {
                DataRow[] drSecResource = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupResource.TableName].Select(dsPrivGroup.SecurityGroupResource.ResourceIdColumn.ColumnName + "=" + dr[dsProfile.Resources.ResourceIdColumn.ColumnName]);
                if (drSecResource.Length > 0)
                    dr[dsProfile.Resources.SecGroupResourceIdColumn.ColumnName] = drSecResource[0][dsPrivGroup.SecurityGroupResource.SecGroupResourceIdColumn.ColumnName];
            }

            //dsPrivGroup.Merge(new DALEntity().LookupEntity(UserId, FacilityId));
            //dsPrivGroup.Merge(new DALPrivilegeGroup().LoadSecurityGroupEntity(0, PrivilegeGroupId));
            //foreach (DataRow dr in dsPrivGroup.Tables[dsProfile.OrganizationEntity.TableName].Rows)
            //{
            //    DataRow[] drSecEntity = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupEntity.TableName].Select(dsPrivGroup.SecurityGroupEntity.EntityIdColumn.ColumnName + "=" + dr[dsProfile.OrganizationEntity.EntityIdColumn.ColumnName]);
            //    if (drSecEntity.Length > 0)
            //        dr[dsProfile.OrganizationEntity.SecGroupEntityIdColumn.ColumnName] = drSecEntity[0][dsPrivGroup.SecurityGroupEntity.SecGroupEntityIdColumn.ColumnName];
            //}

            //dsPrivGroup.Merge(new DALPractice().LookupPractice(UserId, FacilityId));
            //dsPrivGroup.Merge(new DALPrivilegeGroup().LoadSecurityGroupPractice(0, PrivilegeGroupId));
            //foreach (DataRow dr in dsPrivGroup.Tables[dsProfile.Practice.TableName].Rows)
            //{
            //    DataRow[] drSecPractice = dsPrivGroup.Tables[dsPrivGroup.SecurityGroupPractice.TableName].Select(dsPrivGroup.SecurityGroupPractice.PracticeIdColumn.ColumnName + "=" + dr[dsProfile.Practice.PracticeIdColumn.ColumnName]);
            //    if (drSecPractice.Length > 0)
            //        dr[dsProfile.Practice.SecGroupPracticeIdColumn.ColumnName] = drSecPractice[0][dsPrivGroup.SecurityGroupPractice.SecGroupPracticeIdColumn.ColumnName];
            //}
            return new BLObject<DSPrivilegeGroup>(dsPrivGroup);
        }

        /// <summary>
        /// Saves the privilege group provider.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> SavePrivilegeGroupProvider(ref DSPrivilegeGroup ds)
        {
            try
            {
                ds = new DALPrivilegeGroup().InsertPrivilegeGroupProvider(ref ds);
                return new BLObject<DSPrivilegeGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::SavePrivilegeGroupProvider", ex);
                return new BLObject<DSPrivilegeGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the privilege group provider.
        /// </summary>
        /// <param name="privGroupProviderId">The priv group provider identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeletePrivilegeGroupProvider(string privGroupProviderId)
        {
            try
            {
                privGroupProviderId = new DALPrivilegeGroup().DeletePrivilegeGroupProvider(privGroupProviderId);
                return new BLObject<string>(privGroupProviderId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeletePrivilegeGroupProvider", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Saves the privilege group resource.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> SavePrivilegeGroupResource(ref DSPrivilegeGroup ds)
        {
            try
            {
                ds = new DALPrivilegeGroup().InsertPrivilegeGroupResource(ref ds);
                return new BLObject<DSPrivilegeGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::SavePrivilegeGroupResource", ex);
                return new BLObject<DSPrivilegeGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the privilege group resource.
        /// </summary>
        /// <param name="privGroupResourceId">The priv group resource identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeletePrivilegeGroupResource(string privGroupResourceId)
        {
            try
            {
                privGroupResourceId = new DALPrivilegeGroup().DeletePrivilegeGroupResource(privGroupResourceId);
                return new BLObject<string>(privGroupResourceId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeletePrivilegeGroupResource", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// Saves the privilege group facility.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSPrivilegeGroup> SavePrivilegeGroupFacility(ref DSPrivilegeGroup ds)
        {
            try
            {
                ds = new DALPrivilegeGroup().InsertPrivilegeGroupFacility(ref ds);
                return new BLObject<DSPrivilegeGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::SavePrivilegeGroupFacility", ex);
                return new BLObject<DSPrivilegeGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the privilege group facility.
        /// </summary>
        /// <param name="privGroupFacilityId">The priv group facility identifier.</param>
        /// <param name="privGroupId">The priv group identifier.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="practiceId">The practice identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeletePrivilegeGroupFacility(string privGroupFacilityId, string privGroupId, string entityId, string practiceId)
        {
            try
            {
                privGroupFacilityId = new DALPrivilegeGroup().DeletePrivilegeGroupFacility(privGroupFacilityId, privGroupId, entityId, practiceId);
                return new BLObject<string>(privGroupFacilityId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeletePrivilegeGroupFacility", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Users Activity log"
        public BLObject<DSUsersActivity> LoadUsersActivityLog(long userActivityId, string userName, string activityLogDate, string machineIp)
        {
            try
            {
                var ds = new DALUsersActivity().LoadUsersActivityLog(userActivityId, userName, activityLogDate, machineIp);

                return new BLObject<DSUsersActivity>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadUsersActivityLog", ex);
                return new BLObject<DSUsersActivity>(null, ex.Message);
            }

        }
        public BLObject<string> InsertUsersActivityLog(DALUsersActivity.AuditableEvents Event, string formName, bool isSuccess, string details = "", string mRNo = "", string visitAccountNo = "")
        {
            try
            {
                var obj = new DALUsersActivity();
                obj.InsertUsersActivityLog(Event, formName, isSuccess, details, mRNo, visitAccountNo);
                return new BLObject<string>("");

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::InsertUsersActivityLog", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "DB Activity"
        public BLObject<DSDBAudit> LoadDbAudit(string moduleName, string userName, string columnKeyId, string profileName)
        {
            try
            {
                var ds = new DBActivityAudit().LoadDBAudit(moduleName, userName, columnKeyId, profileName, "", "", "", null);
                return new BLObject<DSDBAudit>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadDBAudit", ex);
                return new BLObject<DSDBAudit>(null, ex.Message);
            }
        }

        /// <summary>
        /// Load User Activity Logs
        /// </summary>
        /// <param name="ModuleName"></param>
        /// <param name="UserName"></param>
        /// <param name="ColumnKeyId"></param>
        /// <param name="ProfileName"></param>
        /// <param name="DBAuditId"></param>
        /// <param name="PatientId"></param>
        /// <param name="VisitId"></param>
        /// <param name="createdDate"></param>
        /// <param name="ActivityType"></param>
        /// <param name="DBTableName"></param>
        /// <param name="ParentKeyColumnId"></param>
        /// <returns></returns>
        public BLObject<DataSet> LoadUserActivityLogs(string ModuleName, string UserName, string ColumnKeyId, string ProfileName, string DBAuditId, string PatientId, string VisitId, DateTime? createdDate, string ActivityType, string DBTableName = null, string ParentKeyColumnId = "")
        {
            try
            {
                
                DSDBAudit ds = new DSDBAudit();
                if (ProfileName.Contains("["))
                {
                    ProfileName = ProfileName.Split('[')[0].Trim();
                }
                //PMS-640
                if (ProfileName.Contains("PatientCharges"))
                {
                    //in case of patient charges profile name we are removing extra string i.e cpt code and charge number
                    ProfileName = "PatientCharges";
                }
                ds = new DBActivityAudit().LoadDBAudit(ModuleName, UserName, ColumnKeyId, ProfileName, DBAuditId, PatientId, VisitId, createdDate, DBTableName, ParentKeyColumnId,"","","",1,10000);

                //DSPatient ds = new DALActivityLog().LoadActivityLog(PatientId,PatientActivityLogId);
                DataSet dsResults = new DataSet();
                // This Column Key id is used to filter the first Record User Changes Result set of data table
                long ColumnPkId = -1;
                // ActivityType Can Be NewEntryUser for first load of activity Load view
              
                if (ActivityType.IndexOf("NewEntry") > -1)
                {
                    //New Entry DataTable
                    DataTable dtNewEntry = new DataTable("NewEntry");
                    dtNewEntry.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtNewEntry.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("User", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtNewEntry.Columns.Add(new DataColumn("EntryDate", typeof(DateTime)));
                 
                    DataRow[] insertedRows = ds.Tables[ds.DBAudit.TableName].Select("DBAuditAction='Insert' or DBTableName='CDSPatientStatus'");
                    foreach (var currentRow in insertedRows)
                    {
                        String CurrentValue = currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                        String OriginalValue = currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();
                        //Added single qoutes by Azeem Raza Tayyab on 19-May-2016 to Fix Bug#:PMS-5213
                        DataRow[] drExistingRows = dtNewEntry.Select("ProfileName='" + currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString() + "' AND ColumnKeyId=" + MDVUtility.ToLINQFormatString(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]));
                        if (drExistingRows.Length < 1)
                        {
                            string profileName = currentRow[ds.DBAudit.ProfileNameColumn.ColumnName].ToString();
                            dtNewEntry.Rows.Add(1, profileName, currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]));
                        }
                    }

                    dsResults.Tables.Add(dtNewEntry.DefaultView.ToTable(true, "ActivityLogId", "ProfileName", "User", "ColumnKeyId", "EntryDate"));
                    if (ActivityType.IndexOf("User") > -1 && dtNewEntry.Rows.Count > 0)
                    {
                        ColumnPkId = MDVUtility.ToLong(dtNewEntry.Rows[0]["ColumnKeyId"]);
                    }
                }
                // ActivityType Can Be NewEntryUser for first load of activity Load view
                if (ActivityType.IndexOf("User") > -1)
                {
                    //User Data Table
                    DataTable dtUser = new DataTable("User");
                    dtUser.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtUser.Columns.Add(new DataColumn("Actions", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("User", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    //Start 25-04-2016 Humaira Yousaf
                    dtUser.Columns.Add(new DataColumn("ColumnKeyId", typeof(string))); 
                    //pms-1179
                    DataRow[] modifiedRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='Insert' or DBAuditAction='Print HCFA Form' or DBAuditAction='update' or DBAuditAction='delete' or DBAuditAction='View' or DBAuditAction='Print') and ColumnName<>'NoteComments'");
                    foreach (var currentRow in modifiedRows)
                    {
                        //  Column Key id is used to filter the first Record User Changes Result set of data table
                        if (ActivityType.IndexOf("NewEntry") > -1 && ColumnPkId > 0)
                        {
                            if (ColumnPkId == MDVUtility.ToLong(currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]))
                            {
                                dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                            }
                        }
                        else
                        {
                            dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName]);
                        }

                    }
                    if (ActivityType.IndexOf("User") > -1 && dtUser.Rows.Count > 0)
                    {
                        ColumnPkId = MDVUtility.ToLong(dtUser.Rows[0]["ColumnKeyId"]);
                    }
                    dsResults.Tables.Add(dtUser.DefaultView.ToTable(true, "Actions", "ProfileName", "User", "Date", "ColumnKeyId"));
                    //End 25-04-2016 Humaira Yousaf
                }
                if (ActivityType.Equals("Changes"))
                {
                    //User Data Table
                    DataTable dtField = new DataTable("Field");
                    dtField.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtField.Columns.Add(new DataColumn("Field", typeof(string)));
                    dtField.Columns.Add(new DataColumn("OriginalValue", typeof(string)));
                    dtField.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    dtField.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtField.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtField.Columns.Add(new DataColumn("Date", typeof(DateTime)));

                    //pms-1179
                    DataRow[] FieldRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='Insert' or DBAuditAction='delete' or DBAuditAction='Update') AND (DBTableName<>'PatientCharges' OR ColumnName<>'StatusId')AND ( DBTableName<>'VisitId' AND ColumnName<>'SubmittedBy')AND ( DBTableName<>'VisitId' AND ColumnName<>'SubmittedDate')AND ( DBTableName<>'VisitId' AND ColumnName<>'ClaimStatusID')AND ( DBTableName<>'VisitId' AND ColumnName<>'VisitStatusID') AND ( DBTableName<>'VisitId' AND ColumnName<>'NoteComments')");
                    foreach (var currentRow in FieldRows)
                    {
                        String CurrentValue = currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                        String OriginalValue = currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();
                        if (createdDate != null)
                        {
                            DateTime CreatedDate = currentRow.IsNull("CreatedDate") ? new DateTime() : MDVUtility.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]);
                            if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSFrom") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSTo")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOD") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOB")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkto") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkFrom")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("LMPDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DischargeDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("AdmissionDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("IllnessDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("InjuryDate")
                                )
                            {
                                CurrentValue = string.IsNullOrEmpty(CurrentValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(CurrentValue).ToString("G"));
                                OriginalValue = string.IsNullOrEmpty(OriginalValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(OriginalValue).ToString("G"));
                            }
                            if (!(OriginalValue == "" && CurrentValue == "") && CreatedDate != DateTime.MinValue && createdDate.Equals(CreatedDate))
                            {
                                if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("PatientImageThumbnail"))
                                {
                                    string originalFileStream = "";
                                    string currentFileStream = "";
                                    if (OriginalValue != "")
                                    {
                                        originalFileStream = "data:image/png;base64," + currentRow[ds.DBAudit.OriginalValueColumn.ColumnName].ToString();
                                    }
                                    if (CurrentValue != "")
                                    {
                                        currentFileStream = "data:image/png;base64," + currentRow[ds.DBAudit.CurrentValueColumn.ColumnName].ToString();
                                    }
                                    dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                                    originalFileStream, currentFileStream, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName]);
                                }
                                else
                                {
                                    dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                                    OriginalValue, CurrentValue, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName]);
                                }
                            }
                        }
                        else
                        {
                            if (currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSFrom") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOSTo")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOD") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DOB")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkto") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("UAWorkFrom")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("LMPDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("DischargeDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("AdmissionDate") || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("IllnessDate")
                                || currentRow[ds.DBAudit.ColumnNameColumn.ColumnName].ToString().Equals("InjuryDate"))
                            {
                                CurrentValue = string.IsNullOrEmpty(CurrentValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(CurrentValue).ToString("G"));
                                OriginalValue = string.IsNullOrEmpty(OriginalValue) ? "" : MDVUtility.GetDateMMDDYYY(Convert.ToDateTime(OriginalValue).ToString("G"));
                            }

                            if (!(OriginalValue == "" && CurrentValue == ""))
                            {
                                dtField.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName],
                                OriginalValue, CurrentValue, currentRow[ds.DBAudit.ProfileNameColumn.ColumnName]);
                            }

                        }
                    }
                    dsResults.Tables.Add(dtField);
                }
                if (ActivityType.IndexOf("NewEntry") > -1)
                {
                    //Submit/Re-Submit Data Table
                    DataTable dtRequireSubmit = new DataTable("RequireSubmit");
                    dtRequireSubmit.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("DisplayName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("UserName", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                    dtRequireSubmit.Columns.Add(new DataColumn("CurrentValue", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("PatientInsurance", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("PatientInsuranceId", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("IsCrossedOver", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ColumnKeyId", typeof(string)));
                    dtRequireSubmit.Columns.Add(new DataColumn("ColumnDatatype", typeof(string)));

                    DataRow[] RequireSubmitRows = ds.Tables[ds.DBAudit.TableName].Select("(DBAuditAction='update' OR DBAuditAction='insert') AND ColumnName in('ClaimStatusId','StatusId') AND ColumnKeyName in ('ChargeCapId','VisitId') and currentValue in ('Submitted','ReSubmit')");
                    foreach (var currentRow in RequireSubmitRows)
                    {
                        dtRequireSubmit.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.DisplayNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], currentRow[ds.DBAudit.CreatedDateColumn.ColumnName], currentRow[ds.DBAudit.CurrentValueColumn.ColumnName], currentRow[ds.DBAudit.PatientInsuranceNameColumn.ColumnName], currentRow[ds.DBAudit.PatientInsuranceIdColumn.ColumnName], Convert.ToBoolean(currentRow[ds.DBAudit.IsCrossedOverColumn.ColumnName]) == true ? "Yes" : "No", currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], currentRow[ds.DBAudit.SubmitTypeColumn.ColumnName]);
                    }
                    dsResults.Tables.Add(dtRequireSubmit);
                }
                if (ActivityType.Equals("DeletedCharges"))
                {
                    //Deleted Charges Table
                    DataTable dtUser = new DataTable("DeletedCharges");
                    dtUser.Columns.Add(new DataColumn("ActivityLogId", typeof(Int64)));
                    dtUser.Columns.Add(new DataColumn("Actions", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ProfileName", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("User", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                    dtUser.Columns.Add(new DataColumn("ColumnKeyid", typeof(string)));
                    dtUser.Columns.Add(new DataColumn("ColumnKeyName", typeof(string)));
                    DataRow[] modifiedRows = ds.Tables[ds.DBAudit.TableName].Select("DBAuditAction='delete'");
                    foreach (var currentRow in modifiedRows)
                    {
                        dtUser.Rows.Add(currentRow[ds.DBAudit.DBAuditIdColumn.ColumnName], currentRow[ds.DBAudit.DBAuditActionColumn.ColumnName], currentRow[ds.DBAudit.ProfileNameColumn.ColumnName], currentRow[ds.DBAudit.UserNameColumn.ColumnName], Convert.ToDateTime(currentRow[ds.DBAudit.CreatedDateColumn.ColumnName]).ToString("G"), currentRow[ds.DBAudit.ColumnKeyIdColumn.ColumnName], currentRow[ds.DBAudit.ColumnKeyNameColumn.ColumnName]);
                    }
                    dsResults.Tables.Add(dtUser.DefaultView.ToTable(true, "Actions", "ProfileName", "User", "Date", "ColumnKeyid", "ColumnKeyName"));
                }
                return new BLObject<DataSet>(dsResults);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadUserActivityLogs", ex);
                return new BLObject<DataSet>(null, ex.Message);
            }
        }
        #endregion

        #region "DashBoard"

        /// <summary>
        /// LoadDashBoardSetting
        /// </summary>
        /// <param name="dbsId"></param>
        /// <param name="userId"></param>
        /// <param name="dbsType"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadDashBoardSetting(long dbsId, long userId, string dbsType)
        {
            try
            {
                var ds = new DALDashBoard().LoadDashBoardSetting(dbsId, userId, dbsType);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadDashBoardSetting", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadPatientVisitsKpi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadPatientVisitsKpi(long entity)
        {
            try
            {
                DSSettings ds = new DALDashBoard().LoadPatientVisitsKPI(entity);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadPatientVisitsKPI", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSSettings> UpdateDashboardSetting(DSSettings ds)
        {
            try
            {
                ds = new DALDashBoard().UpdateDashboardSetting(ds);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDashboard::UpdateDashBoardSetting", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// UpdateUserKpi
        /// </summary>
        /// <param name="oldKpiId"></param>
        /// <param name="newKpiId"></param>
        /// <returns></returns>
        public BLObject<string> UpdateUserKpi(int oldKpiId, int newKpiId)
        {
            try
            {
                var str = new DALDashBoard().UpdateUserKPI(oldKpiId, newKpiId);
                return new BLObject<string>(str);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDashboard::UpdateUserKPI", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadCollectedCopayKpi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadCollectedCopayKpi(long entity)
        {
            try
            {
                DSSettings ds = new DALDashBoard().LoadCollectedCopayKPI(entity);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadCollectedCopayKPI", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadCollectedRevenueKpi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadCollectedRevenueKpi(long entity)
        {
            try
            {
                DSSettings ds = new DALDashBoard().LoadCollectedRevenueKPI(entity);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadCollectedRevenueKPI", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadDashBoardKpis
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="providerId"></param>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadDashBoardKpis(long entityId, long providerId = 0, long facilityId = 0)
        {
            try
            {
                DSSettings ds = new DALDashBoard().LoadDashBoardKPIS(entityId, providerId, facilityId);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadDashBoardKPIS", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadChargesAndPaymentsKpi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadChargesAndPaymentsKpi(long entity)
        {
            try
            {
                DSSettings ds = new DALDashBoard().LoadChargesAndPaymentsKPI(entity);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadChargesAndPaymentsKPI", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadAccountReceivable
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BLObject<DSSettings> LoadAccountReceivable(long entity)
        {
            try
            {
                DSSettings ds = new DALDashBoard().LoadAccountReceivable(entity);
                return new BLObject<DSSettings>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadAccountReceivable", ex);
                return new BLObject<DSSettings>(null, ex.Message);
            }
        }

        #endregion

        #region "DefaultSetting"

        /// <summary>
        /// InsertDefaultSettings
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSUsers> InsertDefaultSettings(DSUsers ds,DataTable dtRaceIds=null)
        {
            try
            {
                ds = new DALUser().InsertDefaultSettings(ds,dtRaceIds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::InsertDefaultSettings", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// UpdatDefaultSettings
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSUsers> UpdatDefaultSettings(DSUsers ds,DataTable dtRaceIds=null)
        {
            try
            {
                ds = new DALUser().UpdatDefaultSettings(ds,dtRaceIds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::UpdatDefaultSettings", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadEntityUserOption
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="userName"></param>
        /// <param name="entityId"></param>
        /// <param name="entityRegCode"></param>
        /// <returns></returns>
        public BLObject<DSUsers> LoadEntityUserOption(ref DSUsers ds, string userName, string entityId, string entityRegCode = null)
        {
            try
            {
                using (var dss = new DALUser().LoadEntityUserOption(ref ds, userName, entityId, entityRegCode))
                {
                return new BLObject<DSUsers>(dss);
            }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadEntityUserOption", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        
        public bool LoadEntityUserOptionShowICD10(string UserName, string EntityId, string EntityRegCode = null)
        {
            try
            {
                bool Result = false;
                Result = new DALUser().LoadEntityUserOptionShowICD10(UserName, EntityId, EntityRegCode);
                return Result;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadEntityUserOptionShowICD10", ex);
                throw;
            }
            }

        /// <summary>
        /// LookupThemes
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public BLObject<DSUserLookup> LookupThemes(string active)
        {
            try
            {
                DSUserLookup ds = new DALUser().LookupThemes(active);
                return new BLObject<DSUserLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LookupThemes", ex);
                return new BLObject<DSUserLookup>(null, ex.Message);
            }
        }
        #endregion

        #region NOTIFICATIONS COUNTS
        public BLObject<DSUsers> LoadNotificationsCounts(Int64 patientId)
        {
            try
            {
                DSUsers ds = new DALUser().LoadNotificationsCounts(patientId);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadNotificationsCounts", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        #endregion

        #region Password Configuration

        /// <summary>
        /// SaveUpdatePasswordConfiguration
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public BLObject<DSUsers> SaveUpdatePasswordConfiguration(ref DSUsers ds)
        {
            try
            {
                ds = new DALUser().SaveUpdatePasswordConfiguration(ref ds);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::SaveUpdatePasswordConfiguration", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        /// <summary>
        /// LoadPasswordConfiguration
        /// </summary>
        /// <param name="passwordConfigurationId"></param>
        /// <returns></returns>
        public BLObject<DSUsers> LoadPasswordConfiguration(Int64 passwordConfigurationId)
        {
            try
            {
                DSUsers ds = new DALUser().LoadPasswordConfiguration(passwordConfigurationId);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadPasswordConfiguration", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        #endregion

        #region Co-Worker Groups
        public BLObject<DSUsers> LoadCoWorkerGroup(long CoWorkerGroupId, string Name, String IsActive, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSUsers ds = new DSUsers();
                ds = new DALUser().LoadCoWorkerGroup(CoWorkerGroupId, Name, IsActive, PageNumber, RowsPerPage);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadCoWorkerGroup", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }

        public BLObject<DSCoWorkersGroup> FillCoWorkerGroup(long CoWorkerGroupId, string Name, String IsActive, int PageNumber = 1, int RowsPerPage = 15)
        {
            try
            {
                DSCoWorkersGroup ds = new DSCoWorkersGroup();
                ds = new DALUser().FillCoWorkerGroup(CoWorkerGroupId, Name, IsActive, PageNumber, RowsPerPage);
                return new BLObject<DSCoWorkersGroup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadCoWorkerGroup", ex);
                return new BLObject<DSCoWorkersGroup>(null, ex.Message);
            }
        }

        /// <summary>
        /// DeleteCoWorkerGroup
        /// </summary>
        /// <param name="coWorkerGroupId"></param>
        /// <returns></returns>
        public BLObject<string> DeleteCoWorkerGroup(string coWorkerGroupId)
        {
            try
            {
                coWorkerGroupId = new DALUser().DeleteCoWorkerGroup(coWorkerGroupId);
                return new BLObject<string>(coWorkerGroupId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::DeleteCOWorkerGroup", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// ActiveInActiveCoWorkerGroup
        /// </summary>
        /// <param name="coWorkerGroupId"></param>
        /// <param name="isactive"></param>
        /// <returns></returns>
        public BLObject<string> ActiveInActiveCoWorkerGroup(string coWorkerGroupId, bool isactive)
        {
            try
            {
                coWorkerGroupId = new DALUser().ActiveInActiveCoWorkerGroup(coWorkerGroupId, isactive);
                return new BLObject<string>(coWorkerGroupId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::ActiveInActiveCoWorkerGroup", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #endregion



        #region Native
        public List<UserPrivileges> LoadUserPrivilegesNative(string UserName)
        {
            try
            {
                DALUser dalUser = new DALUser();
                return dalUser.LoadUserPrivilegesNative(UserName);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadAccountReceivable", ex);
                throw ex;
            }

        }
        #endregion
        public BLObject<DSUsers> LoadUserEmail(long userId)
        {
            try
            {
                DSUsers ds = new DSUsers();
                ds = new DALUser().LoadUserEmail(userId);
                return new BLObject<DSUsers>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminSecurity::LoadUser", ex);
                return new BLObject<DSUsers>(null, ex.Message);
            }
        }
    }
}