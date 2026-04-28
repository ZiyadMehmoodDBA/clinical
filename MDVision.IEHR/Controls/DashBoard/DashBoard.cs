using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MDVision.Datasets;
using MDVision.Business.BCommon;

using MDVision.IEHR.Common;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MDVision.IEHR.EMR.HL7Folder.Summary;
using Newtonsoft.Json;
using MDVision.Model.Dashboard;
using MDVision.IEHR.EMR.Helpers.Clinical.ReportHeader;
using MDVision.IEHR.EMR.Model.ReportHeader;
using MDVision.IEHR.Controls.Scheduling;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using MDVision.Model.CCM;
using MDVision.Model.Clinical.Notes;
using System.Xml.Serialization;
using System.IO;
using MDVision.Model.User;
using MDVision.DataAccess.DAL.Settings;
using static MDVision.IEHR.Controls.DashBoard.DashBoard;

namespace MDVision.IEHR.Controls.DashBoard
{
    public class DashBoardDB
    {
        public string connetionString { get; set; }
        private BLLPatient BLLPatientObj = null;
        public DashBoardDB()
        {
            BLLPatientObj = new BLLPatient();
            this.connetionString = "Data Source=" + System.Configuration.ConfigurationManager.AppSettings["Data Source"] + ";Initial Catalog=" + System.Configuration.ConfigurationManager.AppSettings["Initial Catalog"] + ";Integrated Security=True;";
        }
        public DataSet LoadDashBoardCollentedRevenue(long EntityId, long FacilityId, long ProviderId, string tableName, long userId)
        {
            try
            {

                DSSettings dataSet = new DSSettings();
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Reports.sp_CollectedRevenue";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    if (EntityId == 0)
                    {
                        param1 = new SqlParameter("@EntityId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    if (FacilityId == 0)
                    {
                        param2 = new SqlParameter("@FacilityId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param2 = new SqlParameter("@FacilityId", FacilityId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    if (ProviderId == 0)
                    {
                        param3 = new SqlParameter("@ProviderId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param3 = new SqlParameter("@ProviderId", ProviderId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param3);

                    SqlParameter param4;
                    param4 = new SqlParameter("@UserId", userId); param4.Direction = ParameterDirection.Input; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    try
                    {

                        adapter.SelectCommand = command;
                        adapter.TableMappings.Add("Table", tableName);
                        adapter.Fill(dataSet);
                        command.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        dataSet.RejectChanges();
                        throw;
                    }
                    finally
                    {
                        if (adapter.SelectCommand != null)
                        {
                            adapter.SelectCommand.Dispose();
                        }
                    }
                }

                return dataSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet LoadDashBoardAccountReceivable(long EntityId, string tableName, long userId)
        {
            try
            {
                DSSettings dataSet = new DSSettings();
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Patient.sp_AccountReceivable";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    if (EntityId == 0)
                    {
                        param1 = new SqlParameter("@EntityId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@UserId", userId); param2.Direction = ParameterDirection.Input; param2.DbType = DbType.Int64;
                    command.Parameters.Add(param2);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    try
                    {

                        adapter.SelectCommand = command;
                        adapter.TableMappings.Add("Table", tableName);
                        adapter.Fill(dataSet);
                        command.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        dataSet.RejectChanges();
                        throw;
                    }
                    finally
                    {
                        if (adapter.SelectCommand != null)
                        {
                            adapter.SelectCommand.Dispose();
                        }
                    }
                }

                return dataSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet LoadDashBoardChargesAndPayments(long EntityId, string tableName, long userId)
        {
            try
            {

                DSSettings dataSet = new DSSettings();
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Reports.sp_ChargesAndPayments";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    if (EntityId == 0)
                    {
                        param1 = new SqlParameter("@EntityId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@UserId", userId); param2.Direction = ParameterDirection.Input; param2.DbType = DbType.Int64;
                    command.Parameters.Add(param2);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    try
                    {

                        adapter.SelectCommand = command;
                        adapter.TableMappings.Add("Table", tableName);
                        adapter.Fill(dataSet);
                        command.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        dataSet.RejectChanges();
                        throw;
                    }
                    finally
                    {
                        if (adapter.SelectCommand != null)
                        {
                            adapter.SelectCommand.Dispose();
                        }
                    }
                }

                return dataSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet LoadDashCollectedCopayment(long EntityId, string tableName, long userId)
        {
            try
            {

                DSSettings dataSet = new DSSettings();
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Reports.CollectedCopayment";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    if (EntityId == 0)
                    {
                        param1 = new SqlParameter("@EntityId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@UserId", userId); param2.Direction = ParameterDirection.Input; param2.DbType = DbType.Int64;
                    command.Parameters.Add(param2);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    try
                    {

                        adapter.SelectCommand = command;
                        adapter.TableMappings.Add("Table", tableName);
                        adapter.Fill(dataSet);
                        command.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        dataSet.RejectChanges();
                        throw;
                    }
                    finally
                    {
                        if (adapter.SelectCommand != null)
                        {
                            adapter.SelectCommand.Dispose();
                        }
                    }
                }

                return dataSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet LoadDashBoardMthPatientVisits(long EntityId, string tableName, long userId)
        {
            try
            {
                DSSettings dataSet = new DSSettings();
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Reports.MthPatientVisits";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    if (EntityId == 0)
                    {
                        param1 = new SqlParameter("@EntityId", null); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    else
                    {
                        param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input; param1.DbType = DbType.Int64;
                    }
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@UserId", userId); param2.Direction = ParameterDirection.Input; param2.DbType = DbType.Int64;
                    command.Parameters.Add(param2);


                    SqlDataAdapter adapter = new SqlDataAdapter();
                    try
                    {

                        adapter.SelectCommand = command;
                        adapter.TableMappings.Add("Table", tableName);
                        adapter.Fill(dataSet);
                        command.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        dataSet.RejectChanges();
                        throw;
                    }
                    finally
                    {
                        if (adapter.SelectCommand != null)
                        {
                            adapter.SelectCommand.Dispose();
                        }
                    }
                }

                return dataSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashboardPaymentsCount(long EntityId, string userId)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Billing.DashboardPaymentsCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@PageNumber", 1); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@RowspPage", 15); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.String;
                    command.Parameters.Add(param4);

                    SqlParameter param5;
                    param5 = new SqlParameter("@isFirstLoad", 1); param5.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param5);

                    SqlParameter param6;
                    param6 = new SqlParameter("@UserId", userId); param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param6);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public object LoadDashboardTCMCount(long EntityId, string userId)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[System].[sp_DashboardTCMCount]";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@UserId", userId); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);
                    SqlParameter param3;
                    param3 = new SqlParameter("@isTotalCount", 1); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashboardVisitCount(long EntityId, string UserId, string IsCheckedIn)
        {

            try
            {
                object res = null;

                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Clinical.sp_DashboardVisitCount";
                    command.CommandTimeout = 180;


                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@PageNumber", 1); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@RowspPage", 15); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlParameter param5;
                    param5 = new SqlParameter("@isFirstLoad", 1); param5.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param5);

                    SqlParameter param6;
                    param6 = new SqlParameter("@UserId", UserId); param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param6);

                    SqlParameter param7;
                    param7 = new SqlParameter("@IsCheckedIn", IsCheckedIn); param7.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param7);

                    SqlParameter param8;
                    param8 = new SqlParameter("@IsFaceSheet", 0); param8.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param8);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadUserMessageCount(string UserId)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Patient.sp_UserMessageCount";
                    command.CommandTimeout = 180;

                    SqlParameter param6;
                    param6 = new SqlParameter("@UserId", UserId); param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param6);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int16;
                    command.Parameters.Add(param4);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadAppointmentNotesCount(long EntityId, string NoteStatus, string userId)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Clinical.sp_AppointmentNotesCount_New";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@PageNumber", 1); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@RowspPage", 15); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlParameter param5;
                    param5 = new SqlParameter("@isFirstLoad", 1); param5.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param5);

                    SqlParameter param6;
                    param6 = new SqlParameter("@NoteStatus", NoteStatus); param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param6);



                    SqlParameter param7;
                    param7 = new SqlParameter("@UserId", userId); param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param7);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadPatMessagesCount(long EntityId, string UserId, string MsgStatusId, string AssignedToId, string MsgTypeShortName)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Clinical.sp_UsersMessagesCount";

                    SqlParameter param115;
                    param115 = new SqlParameter("@UserId", UserId); param115.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param115);

                    DataTable table = new DataTable();

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    da.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        return MDVUtility.JSON_DataTable(table);
                    }
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string LoadTasksCount(long EntityId, string UserId, string MsgStatusId, string AssignedToId, string MsgTypeShortName)
        {

            try
            {
                List<UserMessagesCount> lstMsgCount = new List<UserMessagesCount>();
                string TaskCount = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Patient.sp_PatMessagesSelectCount";

                    SqlParameter param115;
                    param115 = new SqlParameter("@MsgStatusId", MsgStatusId); param115.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param115);

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param116;
                    param116 = new SqlParameter("@AssignedToId", AssignedToId); param116.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param116);


                    SqlParameter param117;
                    param117 = new SqlParameter("@MsgTypeShortName", MsgTypeShortName); param117.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param117);

                    SqlParameter param118;
                    param118 = new SqlParameter("@PageNumber", 1); param118.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param118);

                    SqlParameter param119;
                    param119 = new SqlParameter("@RowspPage", 500); param119.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param119);

                    SqlParameter param120;
                    param120 = new SqlParameter("@RecordCount", null); param120.Size = 500; param120.Direction = ParameterDirection.Output; param120.DbType = DbType.Int64;
                    command.Parameters.Add(param120);

                    SqlParameter param121;
                    param121 = new SqlParameter("@Userid", UserId); param121.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param121);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UserMessagesCount model = new UserMessagesCount();
                        model.TaskCount = !String.IsNullOrEmpty(reader["TaskCount"].ToString()) ? reader["TaskCount"].ToString() : "0";
                        lstMsgCount.Add(model);
                    }
                    command.Connection.Close();
                }
                if (lstMsgCount.Count > 0)
                    TaskCount = lstMsgCount[0].TaskCount;
                else
                    TaskCount = "0";
                return TaskCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadPatientDocumentCount(long EntityId, DateTime FromEntryDate, DateTime ToEntryDate, string UserId)
        {

            try
            {
                object res = null;



                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Patient.sp_PatientDocumentCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlParameter param6;
                    param6 = new SqlParameter("@DOSFrom", DBNull.Value);
                    param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param6);


                    SqlParameter param8;
                    param8 = new SqlParameter("@DOSTo", DBNull.Value);
                    param8.Direction = ParameterDirection.Input;

                    command.Parameters.Add(param8);

                    SqlParameter param18;
                    param18 = new SqlParameter("@IsPending", 1); param18.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param18);


                    SqlParameter param19;
                    param19 = new SqlParameter("@UserId", UserId); param19.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param19);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashBoardCopaymentCount(long EntityId, string UserId, DateTime FromEntryDate, DateTime ToEntryDate)
        {

            try
            {
                object res = null;

                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Patient.sp_DashBoardCopaymentCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@PageNumber", 1); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@RowspPage", 15); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlParameter param5;
                    param5 = new SqlParameter("@isFirstLoad", 1); param5.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param5);

                    SqlParameter param6;
                    param6 = new SqlParameter("@UserId", UserId); param6.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param6);


                    SqlParameter param8;
                    param8 = new SqlParameter("@PaidFrom", FromEntryDate); param8.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param8);

                    SqlParameter param18;
                    param18 = new SqlParameter("@PaidTo", ToEntryDate); param18.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param18);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashBoardPatientChangesCount(long EntityId, string UserId, int Month, int Year)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.sp_DashBoardPatientChangesCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@PageNumber", 1); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@RowspPage", 15); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    SqlParameter param4;
                    param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlParameter param5;
                    param5 = new SqlParameter("@isFirstLoad", 1); param5.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param5);

                    SqlParameter param8;
                    param8 = new SqlParameter("@Month", Month); param8.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param8);

                    SqlParameter param18;
                    param18 = new SqlParameter("@Year", Year); param18.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param18);

                    SqlParameter param19;
                    param19 = new SqlParameter("@UserId", UserId); param19.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param19);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashBoardModifiedNotesCount(long EntityId, string UserId)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.sp_DashBoardModifiedNotesCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@RecordCount", null); param2.Size = 500; param2.Direction = ParameterDirection.Output; param2.DbType = DbType.Int64;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@UserId", UserId); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashBoardDataChangeRequestCount(long EntityId, string UserId)
        {

            try
            {
                object res = null;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Patient.sp_PatientsNativeCount";
                    command.CommandTimeout = 180;


                    SqlParameter param1;
                    param1 = new SqlParameter("@RecordCount", null); param1.Size = 500; param1.Direction = ParameterDirection.Output; param1.DbType = DbType.Int64;
                    command.Parameters.Add(param1);

                    SqlParameter param2;
                    param2 = new SqlParameter("@EntityId", EntityId); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@UserId", UserId); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);


                    command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }

                return MDVUtility.ToStr(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashboardOrdersResultsCount(long EntityId, string UserId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Clinical.sp_DashBoardLabOrderCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);

                    SqlParameter param4;
                    param4 = new SqlParameter("@LabOrderResultCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int64;
                    command.Parameters.Add(param4);

                    SqlParameter param5;
                    param5 = new SqlParameter("@LabOrderCount", null); param5.Size = 500; param5.Direction = ParameterDirection.Output; param5.DbType = DbType.Int64;
                    command.Parameters.Add(param5);

                    SqlParameter param6;
                    param6 = new SqlParameter("@LabOrderUnsolicitedCount", null); param6.Size = 500; param6.Direction = ParameterDirection.Output; param6.DbType = DbType.Int64;
                    command.Parameters.Add(param6);

                    SqlParameter param19;
                    param19 = new SqlParameter("@UserId", UserId); param19.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param19);
                    DataTable table = new DataTable();

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    da.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        return MDVUtility.JSON_DataTable(table);
                    }
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashBoardTCMsCount(long EntityId, string UserId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "System.sp_DashboardTCMCount";
                    command.CommandTimeout = 180;

                    SqlParameter param1;
                    param1 = new SqlParameter("@EntityId", EntityId); param1.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param1);


                    SqlParameter param2;
                    param2 = new SqlParameter("@UserId", UserId); param2.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param2);

                    SqlParameter param3;
                    param3 = new SqlParameter("@isTotalCount", 2); param3.Direction = ParameterDirection.Input;
                    command.Parameters.Add(param3);

                    DataTable table = new DataTable();

                    SqlDataAdapter da = new SqlDataAdapter(command);

                    da.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        return MDVUtility.JSON_DataTable(table);
                    }
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public object LoadDashboardActiveAccountsCount(ThreadParams threadParams)
        {
            try
            {
                var Parameters = threadParams as ThreadParams;
                List<ActiveAccountsModel> activeAccountslist = new List<ActiveAccountsModel>();
                var dashboard = new DALDashBoard(threadParams.Sharedobj);
                PatientPortalAccounts obj = dashboard.LoadPatientPortalAccounts(threadParams.EntityId, MDVUtility.ToStr(threadParams.UserId), null);

                if (obj != null)
                {
                    activeAccountslist = obj.listActiveAccounts;
                    if (activeAccountslist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientPortalLoad_JSON = JsonConvert.SerializeObject(activeAccountslist),
                            PatientPortalAccountsCount = activeAccountslist.FirstOrDefault().PatientPortalAccountsCount
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public object LoadCMMCount(long EntityId, string UserId)
        //{
        //    try
        //    {
        //        object res = null;
        //        using (SqlConnection connection = new SqlConnection(connetionString))
        //        {
        //            SqlCommand command = new SqlCommand();
        //            command.Connection = connection;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "CCM.sp_CCMCount";
        //            command.CommandTimeout = 180;

        //            SqlParameter param6;
        //            param6 = new SqlParameter("@UserId", UserId); param6.Direction = ParameterDirection.Input;
        //            command.Parameters.Add(param6);

        //            SqlParameter param2;
        //            param2 = new SqlParameter("@EntityId", EntityId); param2.Direction = ParameterDirection.Input;
        //            command.Parameters.Add(param2);

        //            SqlParameter param4;
        //            param4 = new SqlParameter("@RecordCount", null); param4.Size = 500; param4.Direction = ParameterDirection.Output; param4.DbType = DbType.Int16;
        //            command.Parameters.Add(param4);

        //            command.Connection.Open();
        //            res = command.ExecuteScalar();
        //            command.Connection.Close();
        //        }
        //        return MDVUtility.ToStr(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    public class DashBoard
    {

        private BLLMessage BLLMessageObj = null;
        private BLLPatient BLLPatientObj = null;
        private BLLBilling BLLBillingObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminSecurity BLLAdminSecurityObj = null;
        private BLLSchedule BLLScheduleObj = null;
        private BLLCCM BLLCCMObj = null;
        private BLLMobileApp BLLMobileAppObj = null;

        public DashBoard()
        {
            BLLMessageObj = new BLLMessage();
            BLLPatientObj = new BLLPatient();
            BLLBillingObj = new BLLBilling();
            BLLClinicalObj = new BLLClinical();
            BLLAdminSecurityObj = new BLLAdminSecurity();
            BLLScheduleObj = new BLLSchedule();
            BLLCCMObj = new BLLCCM();
            BLLMobileAppObj = new BLLMobileApp();


        }

        #region Singleton
        private static DashBoard _obj = null;

        public static DashBoard Instance()
        {
            if (_obj == null)
            {
                _obj = new DashBoard();
            }
            return _obj;
        }
        #endregion

        #region Dashboard Functions

        public string LoadDashBoardSettingsAsync(DashBoardModel model)
        {
            try
            {
                DSSettings dsdashboard = null;

                model.IsCheckedIn = model.IsCheckedIn == "" ? "1" : model.IsCheckedIn;
                int month = 0;
                int year = 0;
                var toDatDate = MDVUtility.ToStr(model.Month) != "" ? MDVUtility.ToDateTime(model.Month) : DateTime.Now;

                month = toDatDate.Month;
                year = toDatDate.Year;

                string msgtype = "Task";
                long msgstatusId = 2;

                long entityId = MDVUtility.ToLong(MDVSession.Current.EntityId);
                string userId = MDVUtility.ToStr(MDVSession.Current.AppUserId);

                var obj = BLLAdminSecurityObj.LoadDashBoardSetting(MDVUtility.ToLong(model.DBSId), MDVUtility.ToLong(MDVSession.Current.AppUserId), null);

                dsdashboard = obj.Data;
                DataTable dtWidget = dsdashboard.DashboardSettings.Clone();
                DataTable dtKpi = dsdashboard.DashboardSettings.Clone();

                //Load User Settings
                foreach (DataRow dr in dsdashboard.DashboardSettings.Rows)
                {
                    if (dr[dsdashboard.DashboardSettings.DBSTypeColumn.ColumnName].ToString().Contains("W"))
                    {
                        //--------------- Payments
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Payments")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Payments", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "TCM")
                        {

                            dtWidget.Rows.Add(dr.ItemArray);

                        }
                        //----------------- Appointments
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Appointments")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Appointments", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //----------------- Notes
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Encounter")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Notes", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //------------------ Tasks
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Tasks")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Tasks", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //-------------------- Documents
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Documents")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Documents", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //--------------------- Copayment
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Copayment")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Copayment", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //---------------------- Orders and Results
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Orders & Results")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Orders and Results", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //---------------------- Patient Changes
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Patient Changes")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Patient Changes", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //---------------------- Referrals
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Referrals")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Referrals", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }

                        //---------------------- Portal Requests
                        //if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Portal Requests")
                        //{
                        //    string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Portal Requests", "VIEW", "Dash Board")).ToString();
                        //    if (string.IsNullOrEmpty(privilegasMessage))
                        //    {
                        //        dtWidget.Rows.Add(dr.ItemArray);
                        //    }
                        //}
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Messages")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Messages", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }

                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Patient Portal Requests")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Portal Requests", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }

                       

                        //---------------------- Chronic Care Management
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "CCM")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Chronic Care Management", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }

                        //---------------------- Modified Notes
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Modified Notes")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Modified Notes", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //-----------------Data Change Request ---------- added by Faizan Ameen.
                        //if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Data Change Request")
                        //{
                        //    string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Data Change Request", "VIEW", "Dash Board")).ToString();
                        //    if (string.IsNullOrEmpty(privilegasMessage))
                        //    {
                        //        dtWidget.Rows.Add(dr.ItemArray);
                        //    }
                        //}
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Live Requests")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Live Requests", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }
                        //--------------- Active Accounts
                        if (dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString() == "Active Accounts")
                        {
                            string privilegasMessage = JsonConvert.DeserializeObject(AppPrivileges.GetFormSecurity("Active Accounts", "VIEW", "Dash Board")).ToString();
                            if (string.IsNullOrEmpty(privilegasMessage))
                            {
                                dtWidget.Rows.Add(dr.ItemArray);
                            }
                        }

                    }
                    if (dr[dsdashboard.DashboardSettings.DBSTypeColumn.ColumnName].ToString().Contains("K"))
                    {
                        dtKpi.Rows.Add(dr.ItemArray);
                    }
                }


                DSSettings ds = new DSSettings();
                //KPI Tasks
                Task<DSSettings> kpiRevenueTask = null;
                if (dtKpi.AsEnumerable().Any(x => x.Field<string>("KPIName").Equals("Collected Revenue")))
                    kpiRevenueTask = new Task<DSSettings>(() => (DSSettings)(new DashBoardDB().LoadDashBoardCollentedRevenue(entityId, MDVUtility.ToLong(model.ProviderId), MDVUtility.ToLong(model.FacilityId), ds.CollectedRevenueKPI.TableName, MDVUtility.ToLong(userId))));

                Task<DSSettings> kpiReceivableTask = null;
                if (dtKpi.AsEnumerable().Any(x => x.Field<string>("KPIName").Equals("Accounts Receivables")))
                    kpiReceivableTask = new Task<DSSettings>(() => (DSSettings)(new DashBoardDB().LoadDashBoardAccountReceivable(entityId, ds.AccountReceivable.TableName, MDVUtility.ToLong(userId))));

                Task<DSSettings> kpiPaymentsTask = null;
                if (dtKpi.AsEnumerable().Any(x => x.Field<string>("KPIName").Equals("Charges Vs Payments")))
                    kpiPaymentsTask = new Task<DSSettings>(() => (DSSettings)(new DashBoardDB().LoadDashBoardChargesAndPayments(entityId, ds.ChargesPaymentsKPI.TableName, MDVUtility.ToLong(userId))));

                Task<DSSettings> kpiCopaymentTask = null;
                if (dtKpi.AsEnumerable().Any(x => x.Field<string>("KPIName").Equals("Collected Copayment")))
                    kpiCopaymentTask = new Task<DSSettings>(() => (DSSettings)(new DashBoardDB().LoadDashCollectedCopayment(entityId, ds.CollectedCopaymentKPI.TableName, MDVUtility.ToLong(userId))));

                Task<DSSettings> kpiVisitsTask = null;
                if (dtKpi.AsEnumerable().Any(x => x.Field<string>("KPIName").Equals("Weekly Patient’s Visits")))
                    kpiVisitsTask = new Task<DSSettings>(() => (DSSettings)(new DashBoardDB().LoadDashBoardMthPatientVisits(entityId, ds.PatientVisitsKPI.TableName, MDVUtility.ToLong(userId))));

                ////DashBoard Widgets Count Tasks
                Task<string> widgetPaymentsTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Payments")))
                    widgetPaymentsTask = new Task<string>(() => (string)(new DashBoardDB().LoadDashboardPaymentsCount(entityId, userId)));
                Task<string> widgetTCMTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("TCM")))
                    widgetTCMTask = new Task<string>(() => (string)(new DashBoardDB().LoadDashboardTCMCount(entityId, userId)));

                Task<string> widgetEncounterTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Encounter")))
                    widgetEncounterTask = new Task<string>(() => (string)(new DashBoardDB().LoadDashboardVisitCount(entityId, userId, model.IsCheckedIn)));

                Task<string> widgetAppointmentsTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Appointments")))
                    widgetAppointmentsTask = new Task<string>(() => (string)(new DashBoardDB().LoadAppointmentNotesCount(entityId, model.NoteStatus, userId)));

                Task<string> widgetTasksTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Tasks")))
                    widgetTasksTask = new Task<string>(() => (string)(new DashBoardDB().LoadTasksCount(entityId, userId, MDVUtility.ToStr(msgstatusId), userId, msgtype)));

                Task<string> widgetMessagesTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Messages")))
                    widgetMessagesTask = new Task<string>(() => (string)(new DashBoardDB().LoadPatMessagesCount(entityId, userId, MDVUtility.ToStr(msgstatusId), userId, msgtype)));

                Task<string> widgetDocumentsTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Documents")))
                    widgetDocumentsTask = new Task<string>(() => (string)(new DashBoardDB().LoadPatientDocumentCount(entityId, MDVUtility.ToDateTime(model.FromEntry), MDVUtility.ToDateTime(model.ToEntry), userId)));

                Task<string> widgetCopaymentTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Copayment")))
                    widgetCopaymentTask = new Task<string>(() => (string)(new DashBoardDB().LoadDashBoardCopaymentCount(entityId, userId, MDVUtility.ToDateTime(model.FromEntry), MDVUtility.ToDateTime(model.ToEntry))));

                Task<string> widgetOrdersResults = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Orders & Results")))
                    widgetOrdersResults = new Task<string>(() => (string)(new DashBoardDB().LoadDashboardOrdersResultsCount(entityId, userId)));

                Task<string> widgetPatientChangesTask = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Patient Changes")))
                    widgetPatientChangesTask = new Task<string>(() => (string)(new DashBoardDB().LoadDashBoardPatientChangesCount(entityId, userId, month, year)));

                Task<string> widgetModifiedNotes = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Modified Notes")))
                    widgetModifiedNotes = new Task<string>(() => (string)(new DashBoardDB().LoadDashBoardModifiedNotesCount(entityId, userId)));

                Task<string> WidgetLiveRequest = null;
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Live Requests")))
                    WidgetLiveRequest = new Task<string>(() => (string)(new DashBoardDB().LoadDashBoardDataChangeRequestCount(entityId, userId)));

                Task<string> widgetActiveAccounts = null;
                ThreadParams threadParams = new ThreadParams();

                threadParams.UserId = MDVUtility.ToInt64(MDVSession.Current.AppUserId);
                threadParams.EntityId = MDVUtility.ToInt64(MDVSession.Current.EntityId);
                threadParams.Sharedobj = SharedVariable.GetSharedVariable();
                if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("Active Accounts")))
                    widgetActiveAccounts = new Task<string>(() => (string)(new DashBoardDB().LoadDashboardActiveAccountsCount(threadParams)));

                //Task<string> widgetPatientChangesTask = new Task<string>(() => (string)(new DashBoardDB().LoadDashBoardPatientChangesCount(entityId, userId, month, year)));

                //Task<string> widgetCCM = null;
                //if (dtWidget.AsEnumerable().Any(x => x.Field<string>("WidgetsName").Equals("CCM")))
                //    widgetCCM = new Task<string>(() => (string)(new DashBoardDB().LoadCMMCount(entityId, userId)));

                if (kpiRevenueTask != null) kpiRevenueTask.Start();
                if (kpiReceivableTask != null) kpiReceivableTask.Start();
                if (kpiPaymentsTask != null) kpiPaymentsTask.Start();
                if (kpiCopaymentTask != null) kpiCopaymentTask.Start();
                if (kpiVisitsTask != null) kpiVisitsTask.Start();

                if (widgetPaymentsTask != null) widgetPaymentsTask.Start();
                if (widgetTCMTask != null) widgetTCMTask.Start();
                if (widgetAppointmentsTask != null) widgetAppointmentsTask.Start();
                if (widgetMessagesTask != null) widgetMessagesTask.Start();
                if (widgetEncounterTask != null) widgetEncounterTask.Start();
                if (widgetTasksTask != null) widgetTasksTask.Start();
                if (widgetDocumentsTask != null) widgetDocumentsTask.Start();
                if (widgetCopaymentTask != null) widgetCopaymentTask.Start();
                if (widgetPatientChangesTask != null) widgetPatientChangesTask.Start();
                if (widgetOrdersResults != null) widgetOrdersResults.Start();
                if (widgetModifiedNotes != null) widgetModifiedNotes.Start();
                if (WidgetLiveRequest != null) WidgetLiveRequest.Start();
                if (widgetActiveAccounts != null) widgetActiveAccounts.Start();
                //if (widgetCCM != null) widgetCCM.Start();


                if (kpiRevenueTask != null) kpiRevenueTask.Wait();
                if (kpiReceivableTask != null) kpiReceivableTask.Wait();
                if (kpiPaymentsTask != null) kpiPaymentsTask.Wait();
                if (kpiCopaymentTask != null) kpiCopaymentTask.Wait();
                if (kpiVisitsTask != null) kpiVisitsTask.Wait();

                Dictionary<string, string> kpiCharts = new Dictionary<string, string>();
                //Load KPI's
                DSSettings dsSettings = new DSSettings();
                if (kpiRevenueTask != null)
                {
                    dsSettings.Merge(kpiRevenueTask.Result);
                    kpiCharts.Add("Collected Revenue", MDVUtility.JSON_DataTable(dsSettings.Tables[dsSettings.CollectedRevenueKPI.TableName]));
                }
                if (kpiReceivableTask != null)
                {
                    dsSettings.Merge(kpiReceivableTask.Result);
                    kpiCharts.Add("Accounts Receivables", MDVUtility.JSON_DataTable(dsSettings.Tables[dsSettings.AccountReceivable.TableName]));

                }
                if (kpiPaymentsTask != null)
                {
                    dsSettings.Merge(kpiPaymentsTask.Result);
                    kpiCharts.Add("Charges Vs Payments", MDVUtility.JSON_DataTable(dsSettings.Tables[dsSettings.ChargesPaymentsKPI.TableName]));

                }
                if (kpiCopaymentTask != null)
                {
                    dsSettings.Merge(kpiCopaymentTask.Result);
                    kpiCharts.Add("Collected Copayment", MDVUtility.JSON_DataTable(dsSettings.Tables[dsSettings.CollectedCopaymentKPI.TableName]));
                }
                if (kpiVisitsTask != null)
                {
                    dsSettings.Merge(kpiVisitsTask.Result);
                    kpiCharts.Add("Weekly Patient’s Visits", MDVUtility.JSON_DataTable(dsSettings.Tables[dsSettings.PatientVisitsKPI.TableName]));
                }
                //Dispose Dataset
                dsSettings.Dispose();


                if (widgetPaymentsTask != null) widgetPaymentsTask.Wait();
                if (widgetTCMTask != null) widgetTCMTask.Wait();
                if (widgetAppointmentsTask != null) widgetAppointmentsTask.Wait();
                if (widgetMessagesTask != null) widgetMessagesTask.Wait();
                if (widgetEncounterTask != null) widgetEncounterTask.Wait();
                if (widgetOrdersResults != null) widgetOrdersResults.Wait();
                if (widgetModifiedNotes != null) widgetModifiedNotes.Wait();
                if (WidgetLiveRequest != null) WidgetLiveRequest.Wait();
                if (widgetTasksTask != null) widgetTasksTask.Wait();
                if (widgetDocumentsTask != null) widgetDocumentsTask.Wait();
                if (widgetCopaymentTask != null) widgetCopaymentTask.Wait();
                if (widgetPatientChangesTask != null) widgetPatientChangesTask.Wait();
                if (widgetActiveAccounts != null) widgetActiveAccounts.Wait();
                //if (widgetCCM != null) widgetCCM.Wait();


                Dictionary<string, string> widgtsCount = new Dictionary<string, string>();
                if (widgetPaymentsTask != null)
                    widgtsCount.Add("PAYMENTS", MDVUtility.ToStr(widgetPaymentsTask.Result));
                if (widgetTCMTask != null)
                    widgtsCount.Add("TCM", MDVUtility.ToStr(widgetTCMTask.Result));
                if (widgetAppointmentsTask != null)
                    widgtsCount.Add("APPOINTMENTS", MDVUtility.ToStr(widgetAppointmentsTask.Result));
                if (widgetMessagesTask != null)
                    widgtsCount.Add("MESSAGES", widgetMessagesTask.Result);
                if (widgetEncounterTask != null)
                    widgtsCount.Add("ENCOUNTER", MDVUtility.ToStr(widgetEncounterTask.Result));
                if (widgetOrdersResults != null)
                    widgtsCount.Add("ORDERS & RESULTS", MDVUtility.ToStr(widgetOrdersResults.Result));
                if (widgetModifiedNotes != null)
                    widgtsCount.Add("MODIFIED NOTES", MDVUtility.ToStr(widgetModifiedNotes.Result));
                if (WidgetLiveRequest != null)
                    widgtsCount.Add("LIVE REQUESTS", MDVUtility.ToStr(WidgetLiveRequest.Result));
                if (widgetTasksTask != null)
                    widgtsCount.Add("TASKS", MDVUtility.ToStr(widgetTasksTask.Result));
                if (widgetDocumentsTask != null)
                    widgtsCount.Add("DOCUMENTS", MDVUtility.ToStr(widgetDocumentsTask.Result));
                if (widgetCopaymentTask != null)
                    widgtsCount.Add("COPAYMENT", MDVUtility.ToStr(widgetCopaymentTask.Result));
                if (widgetPatientChangesTask != null)
                    widgtsCount.Add("PATIENT CHANGES", MDVUtility.ToStr(widgetPatientChangesTask.Result));
                if (widgetActiveAccounts != null)
                    widgtsCount.Add("ACTIVE ACCOUNTS", MDVUtility.ToStr(widgetActiveAccounts.Result));
                //if (widgetCCM != null)
                //    widgtsCount.Add("CCM", MDVUtility.ToStr(widgetCCM.Result));


                dtWidget.Columns.Add("Count", typeof(String));
                foreach (DataRow dr in dtWidget.Rows)
                {
                    dr["Count"] = widgtsCount.FirstOrDefault(p => p.Key == dr[dsdashboard.DashboardSettings.WidgetsNameColumn.ColumnName].ToString().ToUpper().Trim()).Value;
                }

                var response = new
                {
                    status = true,
                    DASHBOARDSETTING_COUUNT = dsdashboard.DashboardSettings.Rows.Count,
                    DASHBOARDSETTING_WIDGET_JSON = MDVUtility.JSON_DataTable(dtWidget),
                    DASHBOARDSETTING_KPI_JSON = MDVUtility.JSON_DataTable(dtKpi),
                    DASHBOARDSETTING_KPI_CHARTS_JSON = JsonConvert.SerializeObject(kpiCharts),
                };
                return (JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public object LoadAppointmentNotesCount(string NoteStatus)
        {
            try
            {
                var response = new
                {
                    status = true,
                    AppointmentsCount = (string)(new DashBoardDB().LoadAppointmentNotesCount(MDVUtility.ToLong(MDVSession.Current.EntityId), NoteStatus, MDVSession.Current.AppUserId.ToString()))
                };
                return (JsonConvert.SerializeObject(response));

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Private Functions
        public string SearchAppointment(DashBoardModel model)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;

                obj = BLLScheduleObj.LoadAppointmentsVisits(MDVUtility.ToLong(model.ProviderId), MDVUtility.ToLong(model.FacilityId), 0, model.AppDate, "", "", "", null, model.IsCheckedIn, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), "", 0, "0", model.AppointmentStatusIds);

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.AppointmentsVisits.RecordCountColumn.ColumnName],
                            SchAppStatus_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchPatientAppointment(DashBoardModel model)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                obj = BLLScheduleObj.LoadPatientAppointments(MDVUtility.ToLong(model.PatientId), MDVUtility.ToLong(model.ProviderId), MDVUtility.ToLong(model.FacilityId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), model.AppointmentStatusIds);
                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.AppointmentsVisits.RecordCountColumn.ColumnName],
                            SchAppStatus_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            SchAppStatusCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string LoadNotesDraftCount()
        {
            try
            {
                DSAppointment dsAppointment = null;
                BLObject<DSAppointment> obj;

                obj = BLLScheduleObj.LoadNotesDraftCount();
                dsAppointment = obj.Data;
                if (obj.Data != null)
                {
                    if (dsAppointment.Tables[dsAppointment.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            DraftCount = dsAppointment.AppointmentsVisits.Rows[0][dsAppointment.AppointmentsVisits.Columns["DraftCount"]]
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message)
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchVisitsNotes(string VisitFrom, string VisitTo, string NoteStatus, string noteType, Int32 PageNumber = 0, Int32 RowsPerPage = 0, bool IsDraftNote = false, string providerId = null, string patientId = null)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;
                /*Pagination included by Azhar for the bug # EMR-264*/

                obj = BLLScheduleObj.LoadAppointmentsNotes(VisitFrom, VisitTo, NoteStatus, "", "", noteType, providerId != null ? long.Parse(providerId) : 0, "", patientId != null ? long.Parse(patientId) : 0, PageNumber, RowsPerPage, IsDraftNote);

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                        privilegasMessage = privilegasMessage == "" ? "Yes" : "No";

                        var response = new
                        {
                            status = true,
                            VisitsCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            //SchAppStatusCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.AppointmentsVisits.RecordCountColumn.ColumnName],
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                            HasDeleteRights = privilegasMessage,
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        // Load Visit encounters data for bulk sign
        public string SearchVisitsNotesBulkSign(string VisitFrom, string VisitTo, string NoteStatus, string noteType, Int32 PageNumber = 0, Int32 RowsPerPage = 0, int IsReadyOrMissing = 0, string providerId = null, string patientId = null, string ddlMissingInfo = null)
        {
            try
            {
                BLObject<List<NotesModel>> obj;
                obj = BLLScheduleObj.LoadAppointmentsNotesBulkSign(VisitFrom, VisitTo, NoteStatus, "", "", noteType, providerId != null ? long.Parse(providerId) : 0, "", patientId != null ? long.Parse(patientId) : 0, PageNumber, RowsPerPage, IsReadyOrMissing, ddlMissingInfo);
                if (obj.Data != null)
                {
                    if (obj.Data.Any())
                    {
                        var privilegasMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(MDVision.IEHR.Common.AppPrivileges.GetFormSecurity("Notes_Notes", "DELETE")).ToString();
                        privilegasMessage = privilegasMessage == "" ? "Yes" : "No";
                        var response = new
                        {
                            status = true,
                            VisitsCount = obj.Data.Count,
                            iTotalDisplayRecords = obj.Data.FirstOrDefault().RecordCount,
                            VisitsLoad_JSON = JsonConvert.SerializeObject(obj.Data),
                            HasDeleteRights = privilegasMessage,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SchAppStatusCount = 0,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SearchModifiedNotes(string VisitFrom, string VisitTo, int Status, Int64 ProviderId, Int32 PageNumber = 0, Int32 RowsPerPage = 0)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;

                obj = BLLClinicalObj.SearchModifiedNotes(VisitFrom, VisitTo, Status, ProviderId, PageNumber, RowsPerPage);

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName].Rows.Count,
                            iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.AppointmentsVisits.RecordCountColumn.ColumnName],
                            VisitsLoad_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.AppointmentsVisits.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            VisitsCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchPracticeMessage(DashBoardModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadPracticeMessage(0, MDVUtility.ToInt32(model.Priority), model.MessageName, model.MessageDate, MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt64(MDVSession.Current.AppUserId), model.MessageType, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsMessage = obj.Data;
                if (model.MessageType == "Patient")
                {
                    dsMessage = getMessageHash(dsMessage);
                }
                if (obj.Data != null)
                {
                    obj = BLLMessageObj.LoadUserMessagesCount(MDVUtility.ToInt64(MDVSession.Current.AppUserId), 0);

                    if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            iTotalDisplayRecords = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            MessageLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessages.TableName]),
                            MessageCount_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.MessagesCount.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private DSMessage getMessageHash(DSMessage dsMessage)
        {
            foreach (DataRow dr in dsMessage.Tables[dsMessage.UserMessages.TableName].Rows)
            {
                if (!String.IsNullOrEmpty(dr["MessageHash"].ToString()))
                {
                    string messageHash = BitConverter.ToString((byte[])dr["MessageHash"]).Replace("-", String.Empty).ToLower();
                    dr["MessageHashZ"] = messageHash;
                }

            }
            return dsMessage;
        }
        public string SearchMessage(DashBoardModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadUserMessage(0, MDVUtility.ToInt32(model.Priority), model.MessageName, model.MessageDate, 0, MDVUtility.ToInt64(MDVSession.Current.AppUserId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            iTotalDisplayRecords = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            MessageLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessages.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearcDirecthMessage(DashBoardModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadDirectMessage(model.DirectAddress, model.MessageDate, model.MessageType, MDVUtility.ToInt64(MDVSession.Current.AppUserId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            iTotalDisplayRecords = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            DirectMessageLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessages.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string LoadMessagesLog(DashBoardModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadMessageLog(MDVUtility.ToInt64(MDVSession.Current.AppUserId), MDVUtility.ToInt64(model.PatientId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            iTotalDisplayRecords = dsMessage.UserMessages.Rows[0][dsMessage.UserMessages.RecordCountColumn.ColumnName],
                            MessageLogCount = dsMessage.Tables[dsMessage.UserMessages.TableName].Rows.Count,
                            MessageLogLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.UserMessages.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            MessageLogCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchUserTask(DashBoardModel model)
        {
            try
            {

                List<DUserTaskModel> mdlist = new List<DUserTaskModel>();

                BLObject<List<DUserTaskModel>> obj = BLLMessageObj.LoadDashboardUserTasks(MDVUtility.ToInt32(model.MsgStatusId), MDVUtility.ToInt64(model.AssignedToId), model.MessageType, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));

                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                            UserTaskCount = mdlist.Count,
                            UserTaskLoad_JSON = JsonConvert.SerializeObject(mdlist)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            UserTaskCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchPayments(Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();


                List<DPaymentModel> mdlist = new List<DPaymentModel>();
                BLObject<List<DPaymentModel>> obj = BLLBillingObj.LoadDashboardPayments(MDVUtility.ToInt64(MDVSession.Current.EntityId), PageNumber, RowsPerPage);
                //obj = BLLBillingObj.LoadDashBoardPayments(MDVUtility.ToInt64(MDVSession.Current.EntityId), PageNumber, RowsPerPage);
                //dspayment = obj.Data;
                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PaymentsCount = mdlist.Count,
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                            Payments_JSON = JsonConvert.SerializeObject(mdlist)
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PaymentsCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PaymentsCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchTCMPatients(Int64 patientId, Int64 providerId, string Status, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();


                List<DTCMModel> mdlist = new List<DTCMModel>();
                BLObject<List<DTCMModel>> obj = BLLBillingObj.LoadDashboardTCMPatients(MDVUtility.ToInt64(patientId), MDVUtility.ToInt64(providerId), Status, PageNumber, RowsPerPage);
                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            TCMPatientsCount = mdlist.Count,
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                            TCMPatients_JSON = JsonConvert.SerializeObject(mdlist)
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            TCMPatientsCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        TCMPatientsCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchCopay(Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                //System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();


                DateTime? PaidFrom = DateTime.Today; //Utility.StringToDate("20150701");
                DateTime? PaidTo = DateTime.Today;

                List<DCopaymentModel> mdlist = new List<DCopaymentModel>();
                BLObject<List<DCopaymentModel>> obj = BLLBillingObj.LoadDashboardCopay(MDVUtility.ToInt64(MDVSession.Current.EntityId), PaidFrom, PaidTo, PageNumber, RowsPerPage);

                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CopayCount = mdlist.Count,
                            PaymentsCount = mdlist.Count,
                            Copay_JSON = JsonConvert.SerializeObject(mdlist),
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PaymentsCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PaymentsCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchPatientChanges(DashBoardModel model)
        {
            try
            {
                int Month = 0;
                int Year = 0;
                DateTime? TempDate = DateTime.Now;
                TempDate = MDVUtility.ToStr(model.Month) != "" ? MDVUtility.ToDateTime(model.Month) : DateTime.Now;

                if (TempDate != null)
                {
                    Month = TempDate.Value.Month;
                    Year = TempDate.Value.Year;
                }

                List<DPatientChangeModel> mdlist = new List<DPatientChangeModel>();
                BLObject<List<DPatientChangeModel>> obj = BLLPatientObj.LoadDashboardPatientChanges(Month, Year, model.ProfileName, MDVUtility.ToInt32(model.ProviderId), MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));

                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientChangesCount = mdlist.Count,
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                            PatientChanges_JSON = JsonConvert.SerializeObject(mdlist)
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientChangesCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientChangesCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string SearchPatientDocument(DashBoardModel model, Int64 PatientID, Int32 PageNumber, Int32 RowsPerPage, string IsReviewed = "", string PatDocIds = "", string bFileStream = "0", string AdvancePaymentId = null)
        {
            try
            {
                DSPatient dsPatient = null;
                BLObject<DSPatient> obj = null;

                DSPatient dsReviewedDoc = null;
                BLObject<DSPatient> objReviewedDoc = null;

                Int64 advancePaymentId = 0;

                if (AdvancePaymentId != null)
                    advancePaymentId = MDVUtility.ToInt64(AdvancePaymentId);


                if (Convert.ToBoolean(model.IsDashBoardData) == false)
                    obj = BLLPatientObj.LoadPatientDocument(PatDocIds, PatientID, "", "", "", null, null, null, null, "", 0, 0, "", 0, "", bFileStream, advancePaymentId);
                else
                {
                    DateTime? FromDOS = null;
                    DateTime? ToDOS = null;
                    DateTime? FromEntryDate = null;
                    DateTime? ToEntryDate = null;
                    string AccountNumber = null;
                    string PatientLastName = null;
                    string PatientFirstName = null;

                    if (!string.IsNullOrEmpty(model.FromEntry))
                        FromEntryDate = MDVUtility.ToDateTime(model.FromEntry);

                    if (!string.IsNullOrEmpty(model.ToEntry))
                        ToEntryDate = MDVUtility.ToDateTime(model.FromEntry);


                    if (IsReviewed == "")
                    {
                        obj = BLLPatientObj.LoadPatientDocument("", PatientID, AccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, model.EnteredBy, MDVUtility.ToInt64(model.EnteredBy), MDVUtility.ToInt64(model.AssignedtoReview), "0", MDVUtility.ToInt32(model.DocumentId), model.Active, "0", 0, PageNumber, RowsPerPage);
                        objReviewedDoc = BLLPatientObj.LoadPatientDocument("", PatientID, AccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, model.EnteredBy, MDVUtility.ToInt64(model.EnteredBy), MDVUtility.ToInt64(model.AssignedtoReview), "1", MDVUtility.ToInt32(model.DocumentId), model.Active, "0", 0, PageNumber, RowsPerPage);
                    }

                    else
                    {
                        if (IsReviewed == "0")
                            obj = BLLPatientObj.LoadPatientDocument("", PatientID, AccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, model.EnteredBy, MDVUtility.ToInt64(model.EnteredBy), MDVUtility.ToInt64(model.AssignedtoReview), "0", MDVUtility.ToInt32(model.DocumentId), model.Active, "0", 0, PageNumber, RowsPerPage);
                        else if (IsReviewed == "1")
                            objReviewedDoc = BLLPatientObj.LoadPatientDocument("", PatientID, AccountNumber, PatientLastName, PatientFirstName, FromDOS, ToDOS, FromEntryDate, ToEntryDate, model.EnteredBy, MDVUtility.ToInt64(model.EnteredBy), MDVUtility.ToInt64(model.AssignedtoReview), "1", MDVUtility.ToInt32(model.DocumentId), model.Active, "0", 0, PageNumber, RowsPerPage);
                    }

                }
                if (obj != null)
                {
                    dsPatient = obj.Data;
                }
                if (objReviewedDoc != null)
                {
                    dsReviewedDoc = objReviewedDoc.Data;
                }
                if (dsPatient != null && dsReviewedDoc != null)
                {
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0 || dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {

                                    dsPatient.Tables[dsPatient.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        foreach (DataRow dr in dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {

                                    dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count)
                        {
                            if (dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                    ReviewedCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else if (dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }

                        }
                        else if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count < dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count && dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count != 0)
                        {
                            if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count == 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count != 0)
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                            else
                            {
                                var response = new
                                {
                                    status = true,
                                    DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count,
                                    iTotalDisplayRecords = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.RecordCountColumn.ColumnName],
                                    PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                    ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                    DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                    ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                                };
                                return (JsonConvert.SerializeObject(response));
                            }
                        }
                        else
                        {
                            var response = new
                            {
                                status = true,
                                DocumentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                                iTotalDisplayRecords = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.RecordCountColumn.ColumnName],
                                PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                                ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                                DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                                ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                            };
                            return (JsonConvert.SerializeObject(response));
                        }

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else if (dsReviewedDoc == null)
                {
                    if (dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {

                                    dsPatient.Tables[dsPatient.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsPatient.Tables[dsPatient.PatientDocument.TableName].Rows.Count,
                            iTotalDisplayRecords = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.RecordCountColumn.ColumnName],
                            PendingCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.PendingColumn.ColumnName],
                            //ReviewedCount = dsPatient.PatientDocument.Rows[0][dsPatient.PatientDocument.ReviewedColumn.ColumnName],
                            DocumentLoad_JSON = MDVUtility.JSON_DataTable(dsPatient.Tables[dsPatient.PatientDocument.TableName]),
                            ReviewedDocumentLoad_JSON = "",
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            PendingCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else if (dsPatient == null)
                {
                    if (dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows)
                        {
                            byte[] byteArr = dr["FileStream"] as byte[];
                            if (byteArr != null)
                            {
                                string strBase64 = Convert.ToBase64String(byteArr);
                                // Add a New Column to Store the Base64 String
                                if (!dr.Table.Columns.Contains("Base64FileStream"))
                                {

                                    dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Columns.Add("Base64FileStream", typeof(System.String));
                                }
                                dr["Base64FileStream"] = strBase64;
                            }
                        }
                        var response = new
                        {
                            status = true,
                            DocumentCount = dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName].Rows.Count,
                            iTotalDisplayRecords = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.RecordCountColumn.ColumnName],
                            //PendingCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.PendingColumn.ColumnName],
                            ReviewedCount = dsReviewedDoc.PatientDocument.Rows[0][dsReviewedDoc.PatientDocument.ReviewedColumn.ColumnName],
                            DocumentLoad_JSON = "",
                            ReviewedDocumentLoad_JSON = MDVUtility.JSON_DataTable(dsReviewedDoc.Tables[dsReviewedDoc.PatientDocument.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            DocumentCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        DocumentCount = 0,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string updateClinical_Notes(DashBoardModel model)
        {
            try
            {
                int NotesID = MDVUtility.ToInt(model.NotesID);
                if (NotesID > 0)
                {

                    DSNotes dsNotes = new DSNotes();
                    //DSNotes.NotesRow dr = dsNotes.Notes.NewNotesRow();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 15);
                    dsNotes = objLoad.Data;
                    List<NoteComponentModel> CompModelList = new List<NoteComponentModel>();
                    foreach (DSNotes.NotesRow dr in dsNotes.Tables[dsNotes.Notes.TableName].Rows)
                    {
                        dr.NoteStatus = "Signed";
                        dr.UnSignedStatus = false;
                        dr.ModifiedOn = DateTime.Now;
                        dr.NoteText = "";
                        NoteComponentModel CompModel = new NoteComponentModel();
                        CompModel.NotesId = dr.NotesId;
                        CompModel.NoteComponentsLookupId = model.NoteComponentsLookupId;

                        CompModel.SOAPText = "<ul class='list-unstyled SignatureComponent' NoteComponentId='NCDummyId'><li> e-Signed by: " + MDVSession.Current.AppUserFirstName + " " + MDVSession.Current.AppUserLastName + " on " + String.Format("{0:F}", DateTime.Now.ToLocalTime()) + "</li></ul>";
                        CompModel.NoteSectionsLookupId = model.NoteSectionsLookupId;
                        CompModel.OrderNo = 200;
                        CompModelList.Add(CompModel);
                    }

                    #region Database Updation
                    //dsNotes.Notes.AddNotesRow(dr);
                    //dsNotes.Notes.AcceptChanges();

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        //dsNotes.Notes.Rows[0].SetModified();
                        BLObject<DSNotes> obj = BLLClinicalObj.updateClinical_Notes(dsNotes);
                        if (obj.Data != null)
                        {
                            if (CompModelList.Count > 0)
                            {
                                NoteComponentResponse response_ = insertNoteComponentsBulk(CompModelList);
                            }
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
            //return "";
        }
        public string ModifiedNoteReviewed(DashBoardModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.NotesID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr("NoteId Is Not Valid")
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {

                    BLLClinicalObj.ModifiedNoteReviewed(MDVUtility.ToInt64(model.NotesID), model.IsReviewed == "1" ? true : false);
                    var response = new
                    {
                        status = true,
                        Message = ""
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string updateClinicalNotesForCosign(DashBoardModel model)
        {
            try
            {
                int NotesID = MDVUtility.ToInt(model.NotesID);
                string CoSignedProviderId = MDVUtility.ToStr(model.CoSignedProviderId);
                if (NotesID > 0)
                {

                    DSNotes dsNotes = new DSNotes();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 15);
                    dsNotes = objLoad.Data;
                    List<NoteComponentModel> CompModelList = new List<NoteComponentModel>();
                    foreach (DSNotes.NotesRow dr in dsNotes.Tables[dsNotes.Notes.TableName].Rows)
                    {
                        if (string.IsNullOrEmpty(dr.NoteText))
                        {
                            NoteComponentModel CompModel = new NoteComponentModel();
                            CompModel.NotesId = dr.NotesId;
                            CompModel.NoteComponentsLookupId = model.NoteComponentsLookupId;
                            CompModel.SOAPText = "<ul id='coSignedByProvider' class='list-unstyled CoSignComponent' NoteComponentId='NCDummyId'><li id='coSignedBy' coSignedProviderId='" + CoSignedProviderId + "'>_____________________________________________________________________________________________________</li><li>Electronically co-Signed by: " + MDVSession.Current.AppUserLastName + ", " + MDVSession.Current.AppUserFirstName + " on " + String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Now.Date) + " at " + String.Format("{0:T}", DateTime.Now) + "</li><li>" + '"' + model.Radioval + '"' + "</li><li> Co-Sign Comments: " + model.CommentsCosign + "</li></ul>";
                            CompModel.NoteSectionsLookupId = model.NoteSectionsLookupId;
                            CompModel.OrderNo = 201;
                            CompModelList.Add(CompModel);
                            dr.NoteText = "";
                        }
                        else
                        {
                            dr.NoteText = dr.NoteText + "<ul id='coSignedByProvider' class='list-unstyled'><li id='coSignedBy' coSignedProviderId='" + CoSignedProviderId + "'>_____________________________________________________________________________________________________</li><li>Electronically co-Signed by: " + MDVSession.Current.AppUserLastName + ", " + MDVSession.Current.AppUserFirstName + " on " + String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Now.Date) + " at " + String.Format("{0:T}", DateTime.Now) + "</li><li>" + '"' + model.Radioval + '"' + "</li><li> Co-Sign Comments: " + model.CommentsCosign + "</li></ul>";
                        }
                    }

                    #region Database Updation

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        BLObject<DSNotes> obj = BLLClinicalObj.updateClinical_Notes(dsNotes);

                        if (obj.Data != null)
                        {
                            if (CompModelList.Count > 0)
                            {
                                NoteComponentResponse response_ = insertNoteComponentsBulk(CompModelList);
                            }
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
            //return "";
        }
        public string updateClinicalNotesForAmendment(DashBoardModel model)
        {
            try
            {
                Int64 NotesID = MDVUtility.ToInt64(model.NotesID);
                if (NotesID > 0)
                {

                    DSNotes dsNotes = new DSNotes();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 15);
                    dsNotes = objLoad.Data;
                    List<NoteComponentModel> CompModelList = new List<NoteComponentModel>();
                    foreach (DSNotes.NotesRow dr in dsNotes.Tables[dsNotes.Notes.TableName].Rows)
                    {
                        if (string.IsNullOrEmpty(dr.NoteText))
                        {
                            NoteComponentModel CompModel = new NoteComponentModel();
                            CompModel.NotesId = dr.NotesId;
                            CompModel.NoteComponentsLookupId = model.NoteComponentsLookupId;
                            CompModel.SOAPText = "<ul id='AmendmentSection' class='list-unstyled AmendmentComponent' NoteComponentId='NCDummyId'><li>_____________________________________________________________________________________________________</li><li>Following amendment requested by " + model.RequestedBy + " and entered by " + MDVSession.Current.AppUserLastName + ", " + MDVSession.Current.AppUserFirstName + " on " + String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Now.Date) + " at " + String.Format("{0:T}", DateTime.Now) + " is " + model.Action + ": " + model.Comments + "</li></ul>";
                            CompModel.NoteSectionsLookupId = model.NoteSectionsLookupId;
                            CompModel.OrderNo = 300;
                            CompModelList.Add(CompModel);
                            dr.NoteText = "";
                        }
                        else
                        {
                            dr.NoteText = dr.NoteText + "<ul id='AmendmentSection' class='list-unstyled'><li>_____________________________________________________________________________________________________</li><li>Following amendment requested by " + model.RequestedBy + " and entered by " + MDVSession.Current.AppUserLastName + ", " + MDVSession.Current.AppUserFirstName + " on " + String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Now.Date) + " at " + String.Format("{0:T}", DateTime.Now) + " is " + model.Action + ": " + model.Comments + "</li></ul>";
                        }
                        if (!dr.IsAmendmentForBilling)
                        {
                            dr.IsAmendmentForBilling = model.IsAmendmentForBilling;
                        }

                    }

                    #region Database Updation

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        string CompXML = string.Empty;
                        if (CompModelList.Count > 0)
                        {
                            CompXML = createNoteComponentsXml(CompModelList);
                        }
                        bool Result = BLLClinicalObj.updateClinicalNotesForAmendment(dsNotes, CompModelList, CompXML);
                        if (Result)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };

                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = ""
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = ""
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
            //return "";
        }
        private string previewClinical_Notes(Int64 NotesID, Int64 PatientId, Int64 ProviderId, string FormName, string PreviewStyle = null)
        {
            try
            {
                if (NotesID > 0)
                {
                    byte[] NotesHTML_obj = null;
                    DSNotes dsNotes = new DSNotes();
                    BLObject<DSNotes> objLoad = BLLClinicalObj.loadClinical_Notes(0, NotesID, 0, 1, 15, "1", "0");
                    dsNotes = objLoad.Data;

                    //Start//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
                    DSNotes dsHeaderNotes = new DSNotes();
                    BLObject<DSNotes> objHeaderLoad = BLLClinicalObj.loadClinicalNoteHeaderData(PatientId, ProviderId, NotesID);
                    dsHeaderNotes = objHeaderLoad.Data;
                    dsNotes.Merge(dsHeaderNotes);

                    DSPatient dsPatientInfo = new DSPatient();
                    DSProfile dsProvider = new DSProfile();
                    // Adding Header Footer to Report, If Selected Provider of patient has any Report Header | Change Implmeneted by Azhar Shahzad on Aug08/11/016
                    ReportHeader_TagsSelectModel model = ReportHeaderHelper.Instance().getReportHeaderTagsHTML(PatientId, ProviderId, NotesID, FormName, PreviewStyle);

                    List<NoteComponentModel> NoteComponentList = null;
                    BLObject<List<NoteComponentModel>> obj;
                    obj = BLLClinicalObj.loadNoteComponents(NotesID);
                    NoteComponentList = obj.Data;

                    if (dsNotes.Tables[dsNotes.Notes.TableName].Rows.Count > 0)
                    {
                        NotesHTML_obj = System.Text.Encoding.UTF8.GetBytes(dsNotes.Notes.Rows[0][dsNotes.Notes.NoteTextColumn].ToString());
                        if (PatientId > 0)
                        {
                            var response = new
                            {
                                status = true,
                                iTotalDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.RecordCountColumn],
                                NoteStatus = dsNotes.Notes.Rows[0][dsNotes.Notes.NoteStatusColumn],
                                IsReviewed = Convert.ToBoolean(dsNotes.Notes.Rows[0][dsNotes.Notes.IsReviewedColumn]),
                                //  NotesHTML = Convert.ToBase64String(NotesHTML_obj),
                                // this change is done by azhar Shahzad, to give user Print view as HTML on March 21, 2016
                                NotesHTML = HttpUtility.HtmlDecode(dsNotes.Notes.Rows[0][dsNotes.Notes.NoteTextColumn.ColumnName].ToString()),
                                NotesLoad_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]),
                                NoteHeaderPatientData = MDVUtility.JSON_DataTable(dsNotes.Tables[dsPatientInfo.Patients.TableName]),
                                NoteHeaderProviderData = MDVUtility.JSON_DataTable(dsNotes.Tables[dsProvider.Provider.TableName]),
                                NoteHeaderPracticeData = MDVUtility.JSON_DataTable(dsNotes.Tables[dsProvider.Practice.TableName]),
                                ReportHeaderInfo = HttpUtility.HtmlDecode(model.Header),
                                ReportFooterInfo = HttpUtility.HtmlDecode(model.Footer),
                                PatientName = model.PatientName,
                                PatienDOB = model.PatientDOB.ToString("MM/dd/yyyy"),
                                ProviderName = model.ProviderName.ToString(),
                                DOS = model.DOS.ToString("MM/dd/yyyy"),
                                NoteComponentListFill_JSON = NoteComponentList,
                                //End//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var response = new
                            {
                                status = true,
                                iTotalDisplayRecords = dsNotes.Notes.Rows[0][dsNotes.Notes.RecordCountColumn],
                                NoteStatus = dsNotes.Notes.Rows[0][dsNotes.Notes.NoteStatusColumn],
                                IsReviewed = Convert.ToBoolean(dsNotes.Notes.Rows[0][dsNotes.Notes.IsReviewedColumn]),
                                //  NotesHTML = Convert.ToBase64String(NotesHTML_obj),
                                // this change is done by azhar Shahzad, to give user Print view as HTML on March 21, 2016
                                NotesHTML = HttpUtility.HtmlDecode(dsNotes.Notes.Rows[0][dsNotes.Notes.NoteTextColumn.ColumnName].ToString()),
                                NotesLoad_JSON = MDVUtility.JSON_DataTable(dsNotes.Tables[dsNotes.Notes.TableName]),
                                NoteHeaderPatientData = js.Serialize("[]"),
                                NoteHeaderProviderData = js.Serialize("[]"),
                                NoteHeaderPracticeData = js.Serialize("[]"),
                                ReportHeaderInfo = HttpUtility.HtmlDecode(model.Header),
                                ReportFooterInfo = HttpUtility.HtmlDecode(model.Footer),
                                PatientName = model.PatientName.ToString(),
                                PatienDOB = model.PatientDOB.ToString(),
                                //End//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
                            };
                            return (JsonConvert.SerializeObject(response));
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            iTotalDisplayRecords = 0,
                            Message = "No Notes Found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Notes not found."
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = ex.Message,
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        private string SearchNotificationsCounts(Int64 PatientID)
        {
            try
            {
                DSUsers dsUsers = null;
                BLObject<DSUsers> obj = BLLAdminSecurityObj.LoadNotificationsCounts(PatientID);
                dsUsers = obj.Data;
                if (obj.Data != null)
                {
                    string NotificationsCountsDict = string.Empty;
                    if (dsUsers.NotificationsCounts.Rows.Count > 0)
                    {
                        DSUsers.NotificationsCountsRow dr = (DSUsers.NotificationsCountsRow)dsUsers.NotificationsCounts.Rows[0];
                        var keyValues = new Dictionary<string, string>
                        {
                          //  { "spnNotesCount", MDVUtility.ToStr(dr.NotesCount)},
                          //  { "spnAppCount", MDVUtility.ToStr(dr.AppCount)},
                            { "spnAlertCount", MDVUtility.ToStr(dr.AlertCount)}
                          //  { "spnMessageCount", MDVUtility.ToStr(dr.MessageCount)},
                          //  { "spnUserTasksCount", MDVUtility.ToStr(dr.TaskCount)}

                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        NotificationsCountsDict = js.Serialize(keyValues);

                    }


                    var response = new
                    {
                        status = true,

                        NotificationsCounts_JSON = NotificationsCountsDict
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message,
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string SearchDashBoard(DashBoardModel model)
        {
            try
            {
                DateTime? DOSFrom = null;
                DateTime? DOSTo = null;

                //DateTime EntryFrom = DateTime.Today; //Utility.StringToDate("20150701");
                //DateTime EntryTo = DateTime.Today;
                if (!String.IsNullOrEmpty(model.DOSFrom))
                {
                    DOSFrom = MDVUtility.ToDateTime(model.DOSFrom);
                }

                if (!String.IsNullOrEmpty(model.DOSTo))
                {
                    DOSTo = MDVUtility.ToDateTime(model.DOSTo);
                }
                if (String.IsNullOrEmpty(model.DocPriority))
                {
                    model.DocPriority = null;
                }
                if (String.IsNullOrEmpty(model.DocAssignToReview))
                {
                    model.DocAssignToReview = null;
                }
                if (String.IsNullOrEmpty(model.DocStatus))
                {
                    model.DocStatus = null;
                }
                if (String.IsNullOrEmpty(model.PatientId))
                {
                    model.PatientId = null;
                }

                List<DDocumentModel> mdlist = new List<DDocumentModel>();
                List<DPandingPatientDoc> patDocList = new List<DPandingPatientDoc>();
                BLObject<DPatientDoucmnetModel> obj = BLLPatientObj.LoadDashboardDocument(DOSFrom, DOSTo, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage), model.DocPriority, model.DocAssignToReview, model.PatientId, model.DocStatus);

                if (obj.Data != null)
                {
                    mdlist = obj.Data.ListDDocumentModel;
                    patDocList = obj.Data.ListDPandingPatientDoc;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PendingCount = mdlist.FirstOrDefault().Pending,
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                            DocumentLoad_JSON = JsonConvert.SerializeObject(mdlist),
                            PendingDocCount = patDocList.Count(),
                            PendingPatDocument_JSON = JsonConvert.SerializeObject(patDocList)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            PendingCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        DocumentCount = 0,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SearchPatientPortalAccounts(DashBoardModel model)
        {
            try
            {
                DateTime? DOB = null;
                long entityId = MDVUtility.ToLong(MDVSession.Current.EntityId) != 0 ? MDVUtility.ToLong(MDVSession.Current.EntityId) : 0;
                string userId = string.IsNullOrEmpty(MDVUtility.ToStr(MDVSession.Current.AppUserId)) ? null : MDVUtility.ToStr(MDVSession.Current.AppUserId);

                if (!String.IsNullOrEmpty(model.DOB))
                {
                    DOB = MDVUtility.ToDateTime(model.DOB);
                }

                if (String.IsNullOrEmpty(model.PatientId))
                {
                    model.PatientId = null;
                }

                List<ActiveAccountsModel> activeAccountslist = new List<ActiveAccountsModel>();
                BLObject<PatientPortalAccounts> obj = BLLPatientObj.LoadPatientPortalAccounts(entityId, userId, DOB, model.PatientId, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));

                if (obj.Data != null)
                {
                    activeAccountslist = obj.Data.listActiveAccounts;
                    if (activeAccountslist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientPortalLoad_JSON = JsonConvert.SerializeObject(activeAccountslist),
                            PatientPortalAccountsCount = activeAccountslist.FirstOrDefault().PatientPortalAccountsCount
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientPortalAccountsCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SearchPortalAppRequest(DashBoardModel model)
        {
            try
            {
                DSAppointment dsSchedule = null;
                BLObject<DSAppointment> obj;

                obj = BLLScheduleObj.LoadPortalAppRequest(MDVUtility.ToInt64(model.PortalAppRequestId), "", model.PortalAppDate);

                dsSchedule = obj.Data;
                if (obj.Data != null)
                {
                    if (dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PortalRequestCount = dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName].Rows.Count,
                            //iTotalDisplayRecords = dsSchedule.AppointmentsVisits.Rows[0][dsSchedule.PortalAppRequest.RecordCountColumn.ColumnName],
                            PortalRequest_JSON = MDVUtility.JSON_DataTable(dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));

                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PortalRequestCount = 0,
                            //iTotalDisplayRecords = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }


                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PortalRequestCount = 0,
                        //iTotalDisplayRecords = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string CancelAppRequest(DashBoardModel model)
        {
            try
            {

                DSAppointment dsSchedule = new DSAppointment();
                BLObject<DSAppointment> objLoad = BLLScheduleObj.LoadPortalAppRequest(MDVUtility.ToInt64(model.PortalAppRequestId), "", "");
                dsSchedule = objLoad.Data;

                foreach (DSAppointment.PortalAppRequestRow dr in dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName].Rows)
                {
                    dr.RequestStatus = "Cancel";
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation
                if (dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName].Rows.Count > 0)
                {
                    BLObject<DSAppointment> obj = BLLScheduleObj.UpdatePortalAppRequest(dsSchedule);

                    string message = new Scheduling_Appointment_Detail().SavePatientMessage(model.PracticeId, model.FacilityId, model.PortalAppDate, "Cancelled", model.ProviderName, model.PatientId, model.PatientName);

                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Update_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string ConfirmAppRequest(DashBoardModel model)
        {
            try
            {

                DSAppointment dsSchedule = new DSAppointment();
                BLObject<DSAppointment> objLoad = BLLScheduleObj.LoadPortalAppRequest(MDVUtility.ToInt64(model.PortalAppRequestId), "", "");
                dsSchedule = objLoad.Data;

                foreach (DSAppointment.PortalAppRequestRow dr in dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName].Rows)
                {
                    dr.RequestStatus = "Confirm";
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Updation
                if (dsSchedule.Tables[dsSchedule.PortalAppRequest.TableName].Rows.Count > 0)
                {
                    BLObject<DSAppointment> obj = BLLScheduleObj.UpdatePortalAppRequest(dsSchedule);
                    string message = new Scheduling_Appointment_Detail().SavePatientMessage(model.PracticeId, model.FacilityId, model.PortalAppDate, "Confirmed", model.ProviderName, model.PatientId, model.PatientName);
                    if (obj.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Update_Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = ""
                    };
                    return JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string AcceptRejectMultipleAppRequest(DashBoardModel model)
        {
            try
            {
                #region Database Updation
                BLObject<DSAppointment> obj = BLLScheduleObj.AcceptRejectMultiplePortalAppRequest(model.PortalAppRequestId, model.PortalAppStatus);

                //string message = new Scheduling_Appointment_Detail().SavePatientMessage(model.PracticeId, model.FacilityId, model.PortalAppDate, "Cancelled", model.ProviderName, model.PatientId);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Update_Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
                #endregion
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string LoadCCMEnrollmentInfo(DashBoardModel model)
        {
            try
            {
                List<CCMEnrollmentInfoModel> enrollmentInfoList = null;
                BLObject<List<CCMEnrollmentInfoModel>> obj;

                // fixMe
                if (model.RowsPerPage == "1000")
                    model.RowsPerPage = "15";

                obj = BLLCCMObj.LoadCCMEnrollmentInfo(MDVUtility.ToInt32(model.PatientId), MDVUtility.ToInt32(model.ProviderId), MDVUtility.ToInt32(model.InsurancePlanId), MDVUtility.ToInt64(MDVSession.Current.AppUserId), model.Status, MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                enrollmentInfoList = obj.Data;
                if (obj.Data != null)
                {
                    //  obj = BLLMessageObj.LoadUserMessagesCount(MDVUtility.ToInt64(MDVSession.Current.AppUserId), 0);

                    if (enrollmentInfoList.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CCMEnrollmentInfoCount = enrollmentInfoList.Count,
                            iTotalDisplayRecords = enrollmentInfoList[0].RecordCount,
                            CCMEnrollmentInfo_JSON = enrollmentInfoList,
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CCMEnrollmentInfoCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SearchCheckInPatients(string Status, Int64 ProviderId, Int64 PatientId, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<MobileAppRequestModel> mdlist = new List<MobileAppRequestModel>();
                BLObject<List<MobileAppRequestModel>> obj = BLLMobileAppObj.LoadDashboardCheckInPatients(Status, ProviderId, PatientId, PageNumber, RowsPerPage);
                // BLObject<List<DTCMModel>> obj = null;
                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CheckInPatientsCount = mdlist.Count,
                            iTotalDisplayRecords = mdlist.FirstOrDefault().RecordCount,
                            CheckInPatients_JSON = JsonConvert.SerializeObject(mdlist)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CheckInPatientsCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CheckInPatientsCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string SearchCheckInPatientsRequest(string Status, Int64 ProviderId, Int64 PatientId, Int32 PageNumber, Int32 RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<MobileAppRequestModel> mdlist = new List<MobileAppRequestModel>();
                BLObject<List<MobileAppRequestModel>> obj = BLLMobileAppObj.LoadDashboardCheckInPatientsRequest(Status, ProviderId, PatientId, PageNumber, RowsPerPage);
                // BLObject<List<DTCMModel>> obj = null;
                if (obj.Data != null)
                {
                    mdlist = obj.Data;
                    if (mdlist.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            CheckInPatientsCount = mdlist.Count,
                            iTotalDisplayRecords = mdlist.Count,
                            CheckInPatients_JSON = JsonConvert.SerializeObject(mdlist)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            CheckInPatientsCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        CheckInPatientsCount = 0,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string LoadPatientPortalSignupReq(string Status, string ProviderId, string PageNumber, string RowsPerPage)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                var obj =new BLLPatient().LoadPatientPortalSignupReq(Status, ProviderId, PageNumber, RowsPerPage);
                // BLObject<List<DTCMModel>> obj = null;
                if (obj != null)
                {
               
                    if (obj.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            PatientsrequestCount = obj.Count,
                            iTotalDisplayRecords = obj.FirstOrDefault().RecordCount,
                            Patientsrequest_JSON = JsonConvert.SerializeObject(obj)
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            PatientsrequestCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientsrequestCount = 0,
                        Message = "Record not found."
                    };
                    return (JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string patientsSignupDeleteRequest(string id)
        {
            try
            {
               
                var obj = new BLLPatient().DeletePatientPortalSignupReq(id);
                // BLObject<List<DTCMModel>> obj = null;
              

                    if (obj=="")
                    {
                        var response = new
                        {
                            status = true,
                            Message = "Successfully deleted"
       
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            PatientsrequestCount = 0,
                            Message = "error"
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        
             public string patientsSignupApproveRequest(DashBoardModel model)
        {
            try
            {

                var obj = new BLLPatient().ApprovePatientPortalSignupReq(model);
                // BLObject<List<DTCMModel>> obj = null;


                if (obj == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = "Successfully approved"

                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        PatientsrequestCount = 0,
                        Message = "error"
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string GetTPdfHelperFiles()
        {

            string lfileName = string.Empty;
            string base64String = string.Empty;
            string ServerPathForLoadFile = string.Empty;
            string lFileFullPath = string.Empty;

            try
            {

                ServerPathForLoadFile = System.Configuration.ConfigurationManager.AppSettings["MDVisionTrainingManual"];
                if (!string.IsNullOrEmpty(ServerPathForLoadFile))
                {

                    lFileFullPath = System.IO.Path.GetDirectoryName(ServerPathForLoadFile);
                    lfileName = System.IO.Path.GetFileName(ServerPathForLoadFile);

                    if (!System.IO.Directory.Exists(lFileFullPath))
                    {
                        System.IO.Directory.CreateDirectory(lFileFullPath); //Create directory if it doesn't exist
                    }

                    //if (!lfileName.Equals(""))
                    //{ 
                    if (System.IO.File.Exists(ServerPathForLoadFile))
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(ServerPathForLoadFile);
                        base64String = Convert.ToBase64String(imageBytes);

                        var response = new
                        {
                            status = true,
                            pdfHelperBase64 = base64String,
                        };

                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }

                    //}

                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "Document is not available.\nPlease contact your Administrator."

                        };

                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Invalid path key.\nPlease contact your Administrator."

                    };



                    return (JsonConvert.SerializeObject(response));

                }


            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

            }

        }

        #endregion

        #region KPI Functions

        private string LoadDashBoardKPIS()
        {
            try
            {
                DSSettings dspatientvisits = null;
                BLObject<DSSettings> obj;
                string PatientVists = string.Empty;

                obj = BLLAdminSecurityObj.LoadDashBoardKpis(MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dspatientvisits = obj.Data;
                if (obj.Data != null)
                {
                    if (dspatientvisits.Tables[dspatientvisits.PatientVisitsKPI.TableName].Rows.Count > 0)
                    {
                        PatientVists = MDVUtility.JSON_DataTable(dspatientvisits.Tables[dspatientvisits.PatientVisitsKPI.TableName]);
                    }
                }

                return PatientVists;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string LoadPatientVists()
        {
            try
            {
                DSSettings dspatientvisits = null;
                BLObject<DSSettings> obj;
                string PatientVists = string.Empty;

                obj = BLLAdminSecurityObj.LoadPatientVisitsKpi(MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dspatientvisits = obj.Data;
                if (obj.Data != null)
                {
                    if (dspatientvisits.Tables[dspatientvisits.PatientVisitsKPI.TableName].Rows.Count > 0)
                    {
                        PatientVists = MDVUtility.JSON_DataTable(dspatientvisits.Tables[dspatientvisits.PatientVisitsKPI.TableName]);
                    }
                }

                return PatientVists;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string LoadPatientCopay()
        {
            try
            {
                DSSettings dspatientcopay = null;
                BLObject<DSSettings> obj;
                string PatientCopay = string.Empty;

                obj = BLLAdminSecurityObj.LoadCollectedCopayKpi(MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dspatientcopay = obj.Data;
                if (obj.Data != null)
                {
                    if (dspatientcopay.Tables[dspatientcopay.CollectedCopaymentKPI.TableName].Rows.Count > 0)
                    {
                        PatientCopay = MDVUtility.JSON_DataTable(dspatientcopay.Tables[dspatientcopay.CollectedCopaymentKPI.TableName]);
                    }
                }

                return PatientCopay;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string LoadChargesPayments()
        {
            try
            {

                DSSettings dschargpayment = null;
                BLObject<DSSettings> obj;
                string ChargesPayments = string.Empty;

                obj = BLLAdminSecurityObj.LoadChargesAndPaymentsKpi(MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dschargpayment = obj.Data;
                if (obj.Data != null)
                {
                    if (dschargpayment.Tables[dschargpayment.ChargesPaymentsKPI.TableName].Rows.Count > 0)
                    {
                        ChargesPayments = MDVUtility.JSON_DataTable(dschargpayment.Tables[dschargpayment.ChargesPaymentsKPI.TableName]);
                    }

                }

                return ChargesPayments;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string LoadAccountRecievable()
        {
            try
            {

                DSSettings dschargpayment = null;
                BLObject<DSSettings> obj;
                string AccountRecievable = string.Empty;

                obj = BLLAdminSecurityObj.LoadAccountReceivable(MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dschargpayment = obj.Data;
                if (obj.Data != null)
                {
                    if (dschargpayment.Tables[dschargpayment.AccountReceivable.TableName].Rows.Count > 0)
                    {
                        AccountRecievable = MDVUtility.JSON_DataTable(dschargpayment.Tables[dschargpayment.AccountReceivable.TableName]);
                    }

                }

                return AccountRecievable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string LoadRevenue()
        {
            try
            {
                DSSettings dsrevenue = null;
                BLObject<DSSettings> obj;
                string Revenue = string.Empty;

                obj = BLLAdminSecurityObj.LoadCollectedRevenueKpi(MDVUtility.ToInt64(MDVSession.Current.EntityId));
                dsrevenue = obj.Data;
                if (obj.Data != null)
                {
                    if (dsrevenue.Tables[dsrevenue.CollectedRevenueKPI.TableName].Rows.Count > 0)
                    {
                        Revenue = MDVUtility.JSON_DataTable(dsrevenue.Tables[dsrevenue.CollectedRevenueKPI.TableName]);
                    }

                }

                return Revenue;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string UpdateKPIActiveInactive(DashBoardModel model)
        {
            try
            {
                DSSettings dsdashboard = null;
                BLObject<DSSettings> obj1;
                BLObject<DSSettings> obj2;
                obj1 = BLLAdminSecurityObj.LoadDashBoardSetting(MDVUtility.ToLong(model.CurrentKPIId), MDVSession.Current.AppUserId, null);
                obj2 = BLLAdminSecurityObj.LoadDashBoardSetting(MDVUtility.ToLong(model.SelectedKPIId), MDVSession.Current.AppUserId, null);

                dsdashboard = obj1.Data;
                dsdashboard.Merge(obj2.Data);

                if (dsdashboard.Tables[dsdashboard.DashboardSettings.TableName].Rows.Count > 0)
                {
                    foreach (var item in dsdashboard.Tables[dsdashboard.DashboardSettings.TableName].Rows)
                    {
                        DSSettings.DashboardSettingsRow row = (DSSettings.DashboardSettingsRow)item;
                        if (row.DBSId == MDVUtility.ToLong(model.CurrentKPIId))
                            row.IsActive = false;
                        else if (row.DBSId == MDVUtility.ToLong(model.SelectedKPIId))
                            row.IsActive = true;
                    }

                    BLObject<DSSettings> objDashboard = BLLAdminSecurityObj.UpdateDashboardSetting(dsdashboard);
                    if (objDashboard.Data != null)
                    {
                        var response = new
                        {
                            status = true,
                            Message = objDashboard.Message
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = objDashboard.Message
                        };
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj1.Message + " " + obj2.Message
                    };
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }
        public string UpdateKPI(DashBoardModel model)
        {
            try
            {

                BLObject<string> objDashboard = BLLAdminSecurityObj.UpdateUserKpi(MDVUtility.ToInt(model.CurrentKPIId), MDVUtility.ToInt(model.SelectedKPIId));
                if (string.IsNullOrEmpty(objDashboard.Data) && string.IsNullOrEmpty(objDashboard.Message))
                {
                    var response = new
                    {
                        status = true,
                        Message = objDashboard.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = string.IsNullOrEmpty(objDashboard.Data) ? objDashboard.Message : objDashboard.Data
                    };
                    return JsonConvert.SerializeObject(response);
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion

        #region Service Command Handler
        public void CommandHandler(HttpContext context)
        {
            string cammandAction = context.Request.QueryString["cammandAction"].ToUpper();
            switch (cammandAction)
            {
                case "PREVIEW_NOTES":
                    {
                        string FormName = MDVUtility.ToStr(context.Request["FormName"]);
                        string PreviewStyle = MDVUtility.ToStr(context.Request["PreviewStyle"]);
                        Int64 NotesID = MDVUtility.ToInt64(context.Request["NotesId"]);
                        Int64 PatientId = MDVUtility.ToInt64(context.Request["PatientId"]);
                        Int64 ProviderId = MDVUtility.ToInt64(context.Request["ProviderId"]);
                        string strJSONData = previewClinical_Notes(NotesID, PatientId, ProviderId, FormName, PreviewStyle);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "SEARCH_NOTIFICATIONS_COUNTS":
                    {

                        Int64 PatientID = MDVUtility.ToInt64(context.Request["PatientID"]);
                        string strJSONData = SearchNotificationsCounts(PatientID);

                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJSONData);
                    }
                    break;

                case "MDVISION_TRAINING_MANUAL":
                    {
                        string strJsonData = GetTPdfHelperFiles();
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(strJsonData);
                    }
                    break;
            }
        }

        #endregion

        #region ClinicalNotesComponent
        public NoteComponentResponse insertNoteComponentsBulk(List<NoteComponentModel> ComponentsModelList)
        {
            NoteComponentResponse response = new NoteComponentResponse();
            try
            {
                List<NoteComponentModel> NoteComponentList = null;
                BLObject<List<NoteComponentModel>> obj;

                string NoteComponentsXml = createNoteComponentsXml(ComponentsModelList);
                obj = BLLClinicalObj.insertNoteComponentsBulk(NoteComponentsXml);
                if (obj.Data != null)
                {
                    NoteComponentList = obj.Data;
                    response.Status = true;
                    response.Message = Common.AppPrivileges.Save_Message;
                    response.Data = Newtonsoft.Json.JsonConvert.SerializeObject(NoteComponentList);

                }
                else
                {
                    response.Status = false;
                    response.Message = obj.Message;
                    response.Data = "[]";
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                response.Data = "[]";
            }
            return response;
        }

        public static string createNoteComponentsXml(List<NoteComponentModel> ComponentsModel)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NoteComponentModel>));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, ComponentsModel);
                return textWriter.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion ClinicalNotesComponent

        public string DemographicLabelData(DashBoardModel model)
        {
            var PatientId = Convert.ToInt64(model.PatientId);
            var obj = BLLPatientObj.getDemographicData(PatientId);
            if (obj != null)
            {
                var response = new
                {
                    status = true,
                    DemographicData = obj,
                };
                return (JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    status = true,
                    message = "An Error occured.",
                };
                return (JsonConvert.SerializeObject(response));
            }

        }
        #region Direct Messaging
        public string SearcOutgoingDirecthMessage(DashBoardModel model)
        {
            try
            {
                DSMessage dsMessage = null;
                BLObject<DSMessage> obj;

                obj = BLLMessageObj.LoadOutgoingDirectMessage(MDVUtility.ToInt32(model.PageNumber), MDVUtility.ToInt32(model.RowsPerPage));
                dsMessage = obj.Data;
                if (obj.Data != null)
                {
                    if (dsMessage.Tables[dsMessage.DirectMessages.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = dsMessage.DirectMessages.Rows[0][dsMessage.DirectMessages.RecordCountColumn.ColumnName],
                            iTotalDisplayRecords = dsMessage.DirectMessages.Rows[0][dsMessage.DirectMessages.RecordCountColumn.ColumnName],
                            DirectMessageLoad_JSON = MDVUtility.JSON_DataTable(dsMessage.Tables[dsMessage.DirectMessages.TableName]),
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            MessageCount = 0,
                            Message = "Record not found."
                        };
                        return (JsonConvert.SerializeObject(response));
                    }
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        public string UpdateOutgoingDirectMessageStatus(DashBoardModel model)
        {
            try
            {
                BLObject<string> result;

                result = BLLMessageObj.UpdateOutgoingDirectMessageStatus(model.IDs, model.isdelivered);
                if (result != null)
                {

                    long DeliveryStatus = 1;
                    if (model.isdelivered == true)
                    {
                        DeliveryStatus = 1;
                    }
                    else if (model.isdelivered == false)
                    {
                        DeliveryStatus = 2;
                    }
                    BLObject<DSCCDA> dsCCDAReconcilation = new BLObject<DSCCDA>();
                    BLLCCDA _bllCCDAobj = new BLLCCDA();
                    dsCCDAReconcilation = _bllCCDAobj.InsertMUSetting(0, 0, 0, 0, false, false, false, false, false, DeliveryStatus, model.ID, false);


                    var response = new
                    {
                        status = true,
                    };
                    return (JsonConvert.SerializeObject(response));

                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = result
                    };
                    return (JsonConvert.SerializeObject(response));
                }

            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (JsonConvert.SerializeObject(response));
            }
        }

        #endregion
        public class ThreadParams
        {
            public Int64 UserId { get; set; }
            public Int64 EntityId { get; set; }
            public SharedVariable Sharedobj { get; internal set; }
        }
    }
}
