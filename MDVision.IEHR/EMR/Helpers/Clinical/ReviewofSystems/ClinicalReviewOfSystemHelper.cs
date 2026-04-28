/* Author: ZeeshanAK
 * OverView: Helper for Review Of System
 * Date : January 26, 2016
 */

using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.Clinical.ReviewOfSystem;
using System.Text;
using MDVision.IEHR.EMR.Model.ReviewofSystems;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using Newtonsoft.Json;
using MDVision.Model.Clinical.ROS;
using MDVision.Model.Clinical.Notes;

namespace MDVision.IEHR.EMR.Helpers.Clinical.ReviewOfSystem
{
    public class ClinicalReviewOfSystemHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private ClinicalReviewOfSystemHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ClinicalReviewOfSystemHelper _instance = null;
        public static ClinicalReviewOfSystemHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClinicalReviewOfSystemHelper();
                return _instance;
            }
        }

        /// Author: ZeeshanAK
        /// Purpose: To load Systems for Review Of Systems section
        /// Date : January 27, 2016
        #region Load ROS Systems
        public string loadROSSystems(long rosSystemInfoId, long userId, long noteId, long ROSTemplateId, long ROSDataTemplateId)
        {
            try
            {
                DSClinicalReviewofSystem dsROS = null;
                BLObject<DSClinicalReviewofSystem> obj;
                obj = BLLClinicalObj.loadROSSystems(rosSystemInfoId, MDVSession.Current.AppUserId, noteId, ROSTemplateId, ROSDataTemplateId);

                dsROS = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Systems_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.SystemLookup.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
                        Message = obj.Message
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
        #endregion

        /// Author: ZeeshanAK
        /// Purpose: To load System Characteristics based on systemId
        /// Date : January 27, 2016
        #region Load Systems Characteristics
        public string loadSystemCharacteristics(long systemId, long ROSSystemPatientID, long ROSDataTemplateId)
        {
            try
            {
                DSClinicalReviewofSystem dsROS = null;
                BLObject<DSClinicalReviewofSystem> obj;
                obj = BLLClinicalObj.loadSystemCharacteristics(MDVUtility.ToInt64(systemId), ROSSystemPatientID, ROSDataTemplateId);

                dsROS = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        SystemCharacteristics_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.SystemLookup.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        SystemsCount = 0,
                        Message = obj.Message
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
        #endregion

        /// Author: ZeeshanAK
        /// Purpose: Update ROS System User Info 
        /// Date : February 17, 2016
        #region "Update ROS System User Info"
        public string updateROSSystemUserInfo(string systemOrder, string ROSSystemPatSortingOrder)
        {
            try
            {

                BLObject<string> obj = BLLClinicalObj.updateROSSystemUserInfo(0, MDVSession.Current.AppUserId, systemOrder, ROSSystemPatSortingOrder);

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,

                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        internal string toggleROSSystemPatientCharacteristics(long ROSSystemPatientCharacteristicsID, bool isPositive)
        {
            try
            {
                if (ROSSystemPatientCharacteristicsID > 0)
                {
                    DSClinicalReviewofSystem dsClinicalReviewofSystem = new DSClinicalReviewofSystem();
                    BLObject<DSClinicalReviewofSystem> obj = BLLClinicalObj.loadROSSystemPatientCharacteristics(0, ROSSystemPatientCharacteristicsID, 0);
                    dsClinicalReviewofSystem = obj.Data;
                    foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow dr in dsClinicalReviewofSystem.Tables[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.TableName].Rows)
                    {
                        dr.IsPositive = isPositive ? false : true;
                    }

                    #region Database Updation
                    if (dsClinicalReviewofSystem.Tables[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.TableName].Rows.Count > 0)
                    {
                        BLObject<DSClinicalReviewofSystem> objUpdate = BLLClinicalObj.updateROSSystemPatientCharacteristics(dsClinicalReviewofSystem);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "System Patients Characteristics not found."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "System Patients Characteristics not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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


        /// Author: ZeeshanAK
        /// Purpose: Insert ROS System User Info 
        /// Date : February 17, 2016
        #region "Insert ROS System User Info"
        public string insertROSSystemUserInfo(String systemOrder)
        {
            try
            {
                DSClinicalReviewofSystem dsROSSystemUserInfo = new DSClinicalReviewofSystem();

                DSClinicalReviewofSystem.ROSSystemUserInfoRow dr = dsROSSystemUserInfo.ROSSystemUserInfo.NewROSSystemUserInfoRow();

                dr.UserID = MDVSession.Current.AppUserId;

                if (!string.IsNullOrEmpty(systemOrder))
                {
                    dr.Order = MDVUtility.ToStr(systemOrder);
                }
                else
                {
                    dr[dsROSSystemUserInfo.ROSSystemUserInfo.OrderColumn] = DBNull.Value;
                }

                dsROSSystemUserInfo.ROSSystemUserInfo.AddROSSystemUserInfoRow(dr);
                BLObject<DSClinicalReviewofSystem> obj = BLLClinicalObj.insertROSSystemUserInfo(dsROSSystemUserInfo);
                dsROSSystemUserInfo = obj.Data;

                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ROSSystemUserInfo = MDVUtility.ToInt64(dsROSSystemUserInfo.Tables[dsROSSystemUserInfo.ROSSystemUserInfo.TableName].Rows[0][dsROSSystemUserInfo.ROSSystemUserInfo.OrderColumn.ColumnName])
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        /// Author: ZeeshanAK
        /// Purpose: Delete Characteristics Details Info 
        /// Date : February 19, 2016
        #region "Delete Characteristics Details Info"
        internal string deleteCharacteristicsDetails(long ROSSystemPatientCharacteristicsID, bool removeSystemCharcDetails)
        {
            try
            {
                if (ROSSystemPatientCharacteristicsID <= 0 || removeSystemCharcDetails != true)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.deleteCharacteristicsDetails(ROSSystemPatientCharacteristicsID, removeSystemCharcDetails);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        #region ROS Patient Characteristics and Details (CRUD operations)
        public string loadROSSystemPatientCharacteristics(long patientId, long rOSSystemPatientCharacteristicsId)
        {
            try
            {
                DSClinicalReviewofSystem dsROS = null;
                BLObject<DSClinicalReviewofSystem> obj;
                obj = BLLClinicalObj.loadROSSystemPatientCharacteristics(patientId, rOSSystemPatientCharacteristicsId, 0);

                dsROS = obj.Data;
                if (obj.Data != null)
                {
                    if (dsROS.Tables[dsROS.ROSSystemPatientCharacteristics.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsROS.Tables[dsROS.ROSSystemPatientCharacteristics.TableName].Rows[0];
                        ROSSystemPatientCharacteristicsModel rosSysPatChar = new ROSSystemPatientCharacteristicsModel();
                        rosSysPatChar.Description = MDVUtility.ToStr(dr[dsROS.ROSSystemPatientCharacteristics.DescriptionColumn.ColumnName]);
                        rosSysPatChar.IsPositive = Convert.ToBoolean(dr[dsROS.ROSSystemPatientCharacteristics.IsPositiveColumn.ColumnName]);
                        rosSysPatChar.ROSSystemPatientID = MDVUtility.ToLong(dr[dsROS.ROSSystemPatientCharacteristics.ROSSystemPatientIDColumn.ColumnName]);
                        rosSysPatChar.ROSSystemCharacteristicsId = MDVUtility.ToLong(dr[dsROS.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn.ColumnName]);

                        ROSCharacteristicsDetailsModel detailObj = new ROSCharacteristicsDetailsModel();

                        foreach (DataRow drDetail in dsROS.Tables[dsROS.ROSCharacteristicsDetails.TableName].Rows)
                        {
                            detailObj.ROSCharacteristicsDetailsId = MDVUtility.ToLong(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailsIdColumn.ColumnName]);
                            // detailObj.ROSSystemPatientID = MDVUtility.ToLong(drDetail[dsROS.ROSCharacteristicsDetails.ROSSystemPatientCharacteristicsIDColumn.ColumnName]);
                            detailObj.PreviousHistory = MDVUtility.ToStr(drDetail[dsROS.ROSCharacteristicsDetails.PreviousHistoryColumn.ColumnName]).Trim();
                            detailObj.ROSCharacteristicsDetailStatusId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailStatusIdColumn.ColumnName]);
                            detailObj.Onset = MDVUtility.ToStr(drDetail[dsROS.ROSCharacteristicsDetails.OnsetColumn.ColumnName]);
                            detailObj.Duration = MDVUtility.ToLong(drDetail[dsROS.ROSCharacteristicsDetails.DurationColumn.ColumnName]);

                            detailObj.ROSCharacteristicsDetailDurationId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailDurationIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailPatternId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailPatternIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailSeverityId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailSeverityIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailCourseId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailCourseIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailRadiationId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailRadiationIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailFrequencyId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailFrequencyIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailContextId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailContextIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailCharacterCSZId = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailAggravedById = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailAggravedByIdColumn.ColumnName]);
                            detailObj.ROSCharacteristicsDetailRelievedById = MDVUtility.ToInt32(drDetail[dsROS.ROSCharacteristicsDetails.ROSCharacteristicsDetailRelievedByIdColumn.ColumnName]);

                            detailObj.Location = MDVUtility.ToStr(drDetail[dsROS.ROSCharacteristicsDetails.LocationColumn.ColumnName]).Trim();
                            detailObj.PrecipitatedBY = MDVUtility.ToStr(drDetail[dsROS.ROSCharacteristicsDetails.PrecipitatedBYColumn.ColumnName]).Trim();
                            detailObj.AssociatedWith = MDVUtility.ToStr(drDetail[dsROS.ROSCharacteristicsDetails.AssociatedWithColumn.ColumnName]).Trim();
                        }
                    }
                    var response = new
                    {
                        status = true,
                        ROSSystemPatientCharacteristics_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.ROSSystemPatientCharacteristics.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ROSSystemPatientCharacteristics = 0,
                        Message = obj.Message
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

        public string saveROSSystemPatientCharacteristics(ROSSystemPatientCharacteristicsModel model, ROSCharacteristicsDetailsModel modelDetails)
        {
            try
            {
                DSClinicalReviewofSystem dsROSSystemPatientCharacteristics = new DSClinicalReviewofSystem();

                DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow dr = dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.NewROSSystemPatientCharacteristicsRow();
                if (model.ROSSystemPatientCharacteristicsID > 0)
                {
                    dr.ROSSystemPatientCharacteristicsID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                }
                else
                {
                    dr[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn] = DBNull.Value;
                }

                if (model.ROSSystemPatientID > 0)
                {
                    dr.ROSSystemPatientID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                }
                else
                {
                    dr[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.ROSSystemPatientIDColumn] = DBNull.Value;
                }

                if (model.ROSSystemCharacteristicsId > 0)
                {
                    dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                }
                else
                {
                    dr[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.Description))
                {
                    dr.Description = MDVUtility.ToStr(model.Description);
                }
                else
                {
                    dr[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.DescriptionColumn] = DBNull.Value;
                }

                dr.IsPositive = model.IsPositive;

                /*if (!string.IsNullOrEmpty(model.SoapText))
                {
                    dr.SoapText = MDVUtility.ToStr(model.SoapText);
                }
                else
                {
                    dr[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.SoapTextColumn] = DBNull.Value;
                }*/

                dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.AddROSSystemPatientCharacteristicsRow(dr);
                BLObject<DSClinicalReviewofSystem> obj = BLLClinicalObj.insertROSSystemPatientCharacteristics(dsROSSystemPatientCharacteristics);
                dsROSSystemPatientCharacteristics = obj.Data;

                if (obj.Data != null)
                {
                    Int64 systemPatientCharacteristicsId = MDVUtility.ToInt64(dsROSSystemPatientCharacteristics.Tables[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.TableName].Rows[0][dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn.ColumnName]);
                    if (systemPatientCharacteristicsId > 0)
                    {
                        if (modelDetails != null)
                        {
                            string responseCharacteristicsDetails = null;// insertUpdateCharacteristicsDetails(systemPatientCharacteristicsId, modelDetails);
                        }
                    }
                    //BLObject<string> objValue = BLLClinicalObj.updateSoapTextForBirthHX(systemPatientCharacteristicsId);
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        ROSSystemPatientCharacteristicsId = MDVUtility.ToInt64(dsROSSystemPatientCharacteristics.Tables[dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.TableName].Rows[0][dsROSSystemPatientCharacteristics.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn.ColumnName])
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.Message
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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

        public string updateROSSystemPatientCharacteristics(ROSSystemPatientCharacteristicsModel model, ROSCharacteristicsDetailsModel modelDetails)
        {
            try
            {
                if (model.ROSSystemPatientCharacteristicsID > 0)
                {
                    DSClinicalReviewofSystem dsClinicalReviewofSystem = new DSClinicalReviewofSystem();
                    BLObject<DSClinicalReviewofSystem> obj = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToInt64(modelDetails.PatientId), model.ROSSystemPatientCharacteristicsID, 0);
                    dsClinicalReviewofSystem = obj.Data;
                    foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow dr in dsClinicalReviewofSystem.Tables[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.TableName].Rows)
                    {
                        if (model.ROSSystemPatientCharacteristicsID > 0)
                        {
                            dr.ROSSystemPatientCharacteristicsID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                        }
                        else
                        {
                            dr[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn] = DBNull.Value;
                        }

                        if (model.ROSSystemPatientID > 0)
                        {
                            dr.ROSSystemPatientID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                        }
                        else
                        {
                            dr[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.ROSSystemPatientIDColumn] = DBNull.Value;
                        }

                        if (model.ROSSystemCharacteristicsId > 0)
                        {
                            dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                        }
                        else
                        {
                            dr[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.Description))
                        {
                            dr.Description = MDVUtility.ToStr(model.Description);
                        }
                        else
                        {
                            dr[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.DescriptionColumn] = DBNull.Value;
                        }

                        dr.IsPositive = model.IsPositive;
                    }
                    if (modelDetails != null)
                    {
                        string responseNewborn = null;// insertUpdateCharacteristicsDetails(model.ROSSystemPatientCharacteristicsID, modelDetails);
                    }
                    #region Database Updation
                    if (dsClinicalReviewofSystem.Tables[dsClinicalReviewofSystem.ROSSystemPatientCharacteristics.TableName].Rows.Count > 0)
                    {
                        BLObject<DSClinicalReviewofSystem> objUpdate = BLLClinicalObj.updateROSSystemPatientCharacteristics(dsClinicalReviewofSystem);
                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                status = true,
                                Message = Common.AppPrivileges.Update_Message
                            };
                            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        }
                        else
                        {
                            var response = new
                            {
                                status = false,
                                Message = objUpdate.Message
                            };
                            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        }
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = "System Patients Characteristics not found."
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    #endregion
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "System Patients Characteristics not found."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
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


        private DSClinicalReviewofSystem insertUpdateCharacteristicsDetails(long systemPatientCharacteristicsId, List<ROSCharacteristicsDetailsModel> objCharacteristicsDetailsList, DSClinicalReviewofSystem dsWrapper)
        {
            try
            {



                DSClinicalReviewofSystem resultDataset = new DSClinicalReviewofSystem();
                foreach (ROSCharacteristicsDetailsModel objCharacteristicsDetails in objCharacteristicsDetailsList)
                {
                    DSClinicalReviewofSystem dsDetails = new DSClinicalReviewofSystem();
                    if (objCharacteristicsDetails != null)
                    {
                        foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsWrapper.ROSSystemPatientCharacteristics.Rows)
                        {
                            if (objCharacteristicsDetails.ROSCharacteristicsId == drROSSystemPatientCharacteristics.ROSSystemCharacteristicsId)
                            {
                                systemPatientCharacteristicsId = drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID;
                                break;
                            }
                        }
                        if (systemPatientCharacteristicsId > 0)
                        {

                            long currentDetailsId = MDVUtility.ToInt64(objCharacteristicsDetails.ROSCharacteristicsDetailsId);
                            currentDetailsId = currentDetailsId == 0 ? -1 : currentDetailsId;
                            BLObject<DSClinicalReviewofSystem> obj = null;
                            DSClinicalReviewofSystem.ROSCharacteristicsDetailsRow RowDetails = null;
                            bool isUpdate = false;
                            //  if (currentDetailsId > 0)
                            //   {
                            //kr
                            obj = BLLClinicalObj.loadROSCharacteristicsDetails(MDVUtility.ToLong(objCharacteristicsDetails.NotesId), systemPatientCharacteristicsId, currentDetailsId, -1);
                            dsDetails = obj.Data;
                            if (dsDetails.ROSCharacteristicsDetails.Rows.Count > 0)
                            {
                                RowDetails = (DSClinicalReviewofSystem.ROSCharacteristicsDetailsRow)dsDetails.ROSCharacteristicsDetails.Rows[0];
                                isUpdate = true;
                            }
                            else
                            {
                                RowDetails = dsDetails.ROSCharacteristicsDetails.NewROSCharacteristicsDetailsRow();
                            }
                            // }
                            //  else
                            //  {
                            //   RowDetails = dsDetails.ROSCharacteristicsDetails.NewROSCharacteristicsDetailsRow();
                            // }

                            if (RowDetails != null)
                            {
                                if (dsDetails.ROSCharacteristicsDetails.Rows.Count < 1)
                                {
                                    RowDetails.ROSCharacteristicsDetailsId = currentDetailsId;
                                }
                                if (systemPatientCharacteristicsId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSSystemPatientCharacteristicsIDColumn] = systemPatientCharacteristicsId;
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSSystemPatientCharacteristicsIDColumn] = DBNull.Value;
                                }
                                //RowDetails.BirthHxId = birthHxId;
                                //------------------------
                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.PreviousHistory))
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.PreviousHistoryColumn] = MDVUtility.ToStr(objCharacteristicsDetails.PreviousHistory);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.PreviousHistoryColumn] = DBNull.Value;
                                }


                                if (objCharacteristicsDetails.ROSCharacteristicsDetailStatusId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailStatusIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailStatusId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.DetailStatusNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailStatusId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailStatusIdColumn] = DBNull.Value;
                                }


                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.Onset))
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.OnsetColumn] = MDVUtility.ToStr(objCharacteristicsDetails.Onset);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.OnsetColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.Duration > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.DurationColumn] = MDVUtility.Tofloat(objCharacteristicsDetails.Duration);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.DurationColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailDurationId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailDurationIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailDurationId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.DurationNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailDurationId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailDurationIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailPatternId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailPatternIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailPatternId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.PatternNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailPatternId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailPatternIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailSeverityId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailSeverityIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailSeverityId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.SeverityNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailSeverityId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailSeverityIdColumn] = DBNull.Value;
                                }


                                if (objCharacteristicsDetails.ROSCharacteristicsDetailCourseId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailCourseIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailCourseId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.CourseNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailCourseId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailCourseIdColumn] = DBNull.Value;
                                }
                                //-------------------------------------
                                if (objCharacteristicsDetails.ROSCharacteristicsDetailRadiationId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailRadiationIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailRadiationId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.RadiationNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailRadiationId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailRadiationIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailFrequencyId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailFrequencyIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailFrequencyId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.FrequencyNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailFrequencyId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailFrequencyIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailContextId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailContextIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailContextId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ContextNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailContextId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailContextIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZId > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZId);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.CharacterCSZNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZId_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailAggravedById > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailAggravedByIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailAggravedById);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.AggravedByNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailAggravedById_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailAggravedByIdColumn] = DBNull.Value;
                                }

                                if (objCharacteristicsDetails.ROSCharacteristicsDetailRelievedById > 0)
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailRelievedByIdColumn] = MDVUtility.ToInt32(objCharacteristicsDetails.ROSCharacteristicsDetailRelievedById);
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.RelievedByNameColumn] = MDVUtility.ToStr(objCharacteristicsDetails.ROSCharacteristicsDetailRelievedById_text);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.ROSCharacteristicsDetailRelievedByIdColumn] = DBNull.Value;
                                }
                                //---------------------------
                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.Location))
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.LocationColumn] = MDVUtility.ToStr(objCharacteristicsDetails.Location);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.LocationColumn] = DBNull.Value;
                                }

                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.PrecipitatedBY))
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.PrecipitatedBYColumn] = MDVUtility.ToStr(objCharacteristicsDetails.PrecipitatedBY);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.PrecipitatedBYColumn] = DBNull.Value;
                                }

                                if (!string.IsNullOrEmpty(objCharacteristicsDetails.AssociatedWith))
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.AssociatedWithColumn] = MDVUtility.ToStr(objCharacteristicsDetails.AssociatedWith);
                                }
                                else
                                {
                                    RowDetails[dsDetails.ROSCharacteristicsDetails.AssociatedWithColumn] = DBNull.Value;
                                }
                                //---------------------------


                                //// if no details is found against CharacteristicId, it implies for new record
                                if (!isUpdate)
                                {
                                    dsDetails.ROSCharacteristicsDetails.AddROSCharacteristicsDetailsRow(RowDetails);
                                }




                                #region Database Insertion/Updation

                                BLObject<DSClinicalReviewofSystem> objDetails = new BLObject<DSClinicalReviewofSystem>();
                                if (isUpdate)
                                {
                                    /*foreach (DataRow RowGeneralObj in dsDetails.ROSSystemPatientCharacteristics.Rows)
                                    {
                                        RowGeneralObj[dsDetails.ROSSystemPatientCharacteristics.SoapTextColumn] = createROSsoapText(dsDetails);
                                    }*/
                                    objDetails = BLLClinicalObj.updateROSCharacteristicsDetails(dsDetails);
                                    dsDetails = objDetails.Data;
                                    resultDataset.Merge(dsDetails);
                                }
                                else
                                {
                                    objDetails = BLLClinicalObj.insertROSCharacteristicsDetails(dsDetails);
                                    dsDetails = objDetails.Data;
                                    resultDataset.Merge(dsDetails);
                                    /*foreach (DataRow RowGeneralObj in dsDetails.ROSSystemPatientCharacteristics.Rows)
                                    {
                                        RowGeneralObj[dsDetails.ROSSystemPatientCharacteristics.SoapTextColumn] = createROSsoapText(dsDetails);
                                    }*/
                                    //     objDetails = BLLClinicalObj.updateROSCharacteristicsDetails(dsDetails);

                                }



                                #endregion
                            }
                        }
                    }




                }
                return resultDataset;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public string loadROSCharacteristicsDetails(long patientId, long systemPatientCharacteristicsId, long rOSCharacteristicsDetailsId, long ROSDataTemplateId)
        {
            try
            {
                DSClinicalReviewofSystem dsROS = null;
                BLObject<DSClinicalReviewofSystem> obj;
                obj = BLLClinicalObj.loadROSCharacteristicsDetails(MDVUtility.ToInt64(patientId), systemPatientCharacteristicsId, MDVUtility.ToInt64(rOSCharacteristicsDetailsId), ROSDataTemplateId);

                dsROS = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        SystemCharacteristicsDetails_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.ROSCharacteristicsDetails.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        SystemCharacteristicsDetailsCount = 0,
                        Message = obj.Message
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
        /// <summary>
        /// Author : Khaleel Ur Rehman
        /// Purpose : A function to disAssociate Systems Against Patient
        /// Date : 15-Feb-2016
        /// </summary>
        /// <param name="notesId"></param>
        /// <param name="systemPatientCharacteristicsId"></param>
        /// <param name="rOSCharacteristicsDetailsId"></param>
        /// <returns></returns>
        public string disAssociateSystemsAgainstNoteId(long notesId, long rosSystemInfoId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(notesId)) || string.IsNullOrEmpty(MDVUtility.ToStr(rosSystemInfoId)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.disAssociateSystemsAgainstNoteId(notesId, rosSystemInfoId);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        public string rOSSystemPatientReset(long rOSSystemPatientID, bool removeSystemDetails)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(rOSSystemPatientID)))
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.CheckBox_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    BLObject<string> obj = BLLClinicalObj.rOSSystemPatientReset(rOSSystemPatientID, removeSystemDetails);
                    if (obj.Data == "")
                    {
                        var response = new
                        {
                            status = true,
                            Message = Common.AppPrivileges.Delete_Message
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = false,
                            Message = obj.Data
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
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

        public string loadROSSystemInfo(long rOSSystemInfoId, long NotesId, long ROSDataTemplateId)
        {
            try
            {
                DSClinicalReviewofSystem dsROS = null;
                BLObject<DSClinicalReviewofSystem> obj;
                obj = BLLClinicalObj.loadROSSystemInfo(MDVSession.Current.AppUserId, rOSSystemInfoId, NotesId, ROSDataTemplateId);// infoModel.NotesId);

                dsROS = obj.Data;
                if (obj.Data != null)
                {
                    ROSSystemsWrapperModel mdobj = new ROSSystemsWrapperModel();

                    foreach (DSClinicalReviewofSystem.ROSSystemInfoRow drROSSystemInfo in dsROS.ROSSystemInfo.Rows)
                    {
                        mdobj.ROSComments = drROSSystemInfo.IsCommentsNull() ? string.Empty : drROSSystemInfo.Comments;
                        mdobj.ROSDataTempName = drROSSystemInfo.IsROSDataTempNameNull() ? string.Empty : drROSSystemInfo.ROSDataTempName;
                        mdobj.ROSDataTemplateId = drROSSystemInfo.IsROSDataTemplateIdNull() ? -1 : drROSSystemInfo.ROSDataTemplateId;
                        mdobj.ROSisNormal = drROSSystemInfo.IsIsNormalNull() ? false : drROSSystemInfo.IsNormal;
                        mdobj.ROSNormalDescription = drROSSystemInfo.IsDescriptionNull() ? string.Empty : drROSSystemInfo.Description;
                        mdobj.ROSSystemInfoID = drROSSystemInfo.ROSSystemInfoID;
                        mdobj.ReviewofSystemsDate = drROSSystemInfo.IsROSSystemDateNull() ? string.Empty : MDVUtility.GetDateMMDDYYY(drROSSystemInfo.ROSSystemDate.ToString());
                        mdobj.ROSTemplateId = drROSSystemInfo.IsROSTemplateIdNull() ? -1 : drROSSystemInfo.ROSTemplateId;
                    }
                    var response = new
                    {
                        status = true,
                        //  ROSPatientInfo_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.ROSSystemInfo.TableName]),
                        ROSPatientInfo_JSON = mdobj,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        PatientsCount = 0,
                        Message = obj.Message
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
        #endregion

        public DSClinicalReviewofSystem insertUpdateRosSystemPatient(ROSSystemsWrapperModel wrapperModel, DSClinicalReviewofSystem dsROSSystemPatientWrapper)
        {
            BLObject<DSClinicalReviewofSystem> objDetails = new BLObject<DSClinicalReviewofSystem>();
            DSClinicalReviewofSystem dsROSSystemPatient = new DSClinicalReviewofSystem();
            if (wrapperModel.ROSSystemInfoID > 0)
            {
                List<string> removeROSSystemPatientID = new List<string>();
                if (dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count > 0)
                {
                    dsROSSystemPatient = dsROSSystemPatientWrapper;
                    foreach (DSClinicalReviewofSystem.ROSSystemPatientRow drSystemPatient in dsROSSystemPatient.ROSSystemPatient.Rows)
                    {

                        string ROSSystemId = drSystemPatient[dsROSSystemPatient.ROSSystemPatient.ROSSystemIdColumn].ToString();
                        bool IsSystemNormal = false;
                        string NormalDescription = string.Empty;
                        if (wrapperModel.NormalSystems != null && wrapperModel.NormalSystems.Count > 0
                            && wrapperModel.NormalSystems.Any(s => ROSSystemId.Contains(s)) && wrapperModel.NormalSystems.IndexOf(ROSSystemId) > -1)
                        {
                                    IsSystemNormal = true;
                                    NormalDescription = wrapperModel.isNormalDescription[wrapperModel.NormalSystems.IndexOf(ROSSystemId)];
                                
                        }
                        if (string.IsNullOrEmpty(NormalDescription))
                        {
                            drSystemPatient[dsROSSystemPatient.ROSSystemPatient.DescriptionColumn] = DBNull.Value;
                        }
                        else
                        {
                            drSystemPatient[dsROSSystemPatient.ROSSystemPatient.DescriptionColumn] = NormalDescription;
                        }
                        if (wrapperModel.ROSisNormal)
                        {
                            IsSystemNormal = true;
                        }
                        drSystemPatient[dsROSSystemPatient.ROSSystemPatient.IsNormalColumn] = IsSystemNormal;

                        if (wrapperModel.ROSSystemInfoID > 0)
                        {
                            drSystemPatient[dsROSSystemPatient.ROSSystemPatient.ROSSystemInfoIDColumn] = wrapperModel.ROSSystemInfoID;
                        }
                        if (!(Array.IndexOf(wrapperModel.ROSSystemIds.ToArray(), ROSSystemId) >= 0))
                        {
                            removeROSSystemPatientID.Add(drSystemPatient[dsROSSystemPatient.ROSSystemPatient.ROSSystemPatientIDColumn].ToString());
                        }
                    }
                    if (dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count > 0 && removeROSSystemPatientID.Count < dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count)
                    {
                        objDetails = BLLClinicalObj.updateROSSystemPatient(dsROSSystemPatient);
                        dsROSSystemPatient = objDetails.Data;
                    }
                    else
                    {
                        BLLClinicalObj.deleteROSSystemPatients(string.Join(",", new List<string>(removeROSSystemPatientID).ToArray()));
                        return insertROSSystem(wrapperModel);
                    }
                }
                else
                {
                    return insertROSSystem(wrapperModel);
                }
            }
            return dsROSSystemPatient;
        }
        private DSClinicalReviewofSystem insertROSSystem(ROSSystemsWrapperModel wrapperModel)
        {
            BLObject<DSClinicalReviewofSystem> objDetails = new BLObject<DSClinicalReviewofSystem>();
            DSClinicalReviewofSystem dsROSSystemPatient = new DSClinicalReviewofSystem();
            foreach (string ROSSystemId in wrapperModel.ROSSystemIds)
            {
                DSClinicalReviewofSystem.ROSSystemPatientRow drSystemPatient = dsROSSystemPatient.ROSSystemPatient.NewROSSystemPatientRow();

                bool IsSystemNormal = false;
                string NormalDescription = string.Empty;

                if (wrapperModel.NormalSystems != null && wrapperModel.NormalSystems.Count > 0
                    && wrapperModel.NormalSystems.Any(s => ROSSystemId.Contains(s)) && wrapperModel.NormalSystems.IndexOf(ROSSystemId) > -1)
                {
                    IsSystemNormal = true;
                    NormalDescription = wrapperModel.isNormalDescription[wrapperModel.NormalSystems.IndexOf(ROSSystemId)];

                }

                if (string.IsNullOrEmpty(NormalDescription))
                {
                    drSystemPatient[dsROSSystemPatient.ROSSystemPatient.DescriptionColumn] = DBNull.Value;
                }
                else
                {
                    drSystemPatient[dsROSSystemPatient.ROSSystemPatient.DescriptionColumn] = NormalDescription;
                }
                if (wrapperModel.ROSisNormal)
                {
                    IsSystemNormal = true;
                }


                drSystemPatient[dsROSSystemPatient.ROSSystemPatient.IsNormalColumn] = IsSystemNormal;
                drSystemPatient[dsROSSystemPatient.ROSSystemPatient.ROSSystemIdColumn] = MDVUtility.ToInt32(ROSSystemId);
                drSystemPatient[dsROSSystemPatient.ROSSystemPatient.ROSSystemInfoIDColumn] = wrapperModel.ROSSystemInfoID;


                drSystemPatient[dsROSSystemPatient.ROSSystemPatient.SystemNameColumn] = wrapperModel.ROSSystemNames[wrapperModel.ROSSystemIds.IndexOf(ROSSystemId)];
                dsROSSystemPatient.ROSSystemPatient.AddROSSystemPatientRow(drSystemPatient);

            }
            objDetails = BLLClinicalObj.insertROSSystemPatient(dsROSSystemPatient);
            return objDetails.Data;
        }
     


        #region azhar implementations
        public string saveROSSystemPatientInfo(ROSSystemsWrapperModel wrapperModel)
        {
            try
            {
                string soapText = string.Empty;
                string AppUserName = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                var currentDateTime = DateTime.Now;

                DSClinicalReviewofSystem dsROSSystemPatientWrapper = new DSClinicalReviewofSystem();
                #region SystemPatient Info
                ROSSystemInfoModel infoModel = new ROSSystemInfoModel();
                infoModel.Comments = wrapperModel.ROSComments;
                infoModel.Description = wrapperModel.ROSNormalDescription;
                infoModel.IsNormal = wrapperModel.ROSisNormal;
                infoModel.ROSSystemDate = wrapperModel.ReviewofSystemsDate;
                infoModel.ROSSystemInfoID = wrapperModel.ROSSystemInfoID;
                infoModel.NotesId = wrapperModel.NotesId;
                infoModel.ROSTemplateId = wrapperModel.ROSTemplateId;
                infoModel.ROSDataTemplateId = wrapperModel.ROSDataTemplateId;

                BLObject<DSClinicalReviewofSystem> objROSSystemPatientWrapper = null;
                DSClinicalReviewofSystem.ROSSystemInfoRow drSysteminfo = null;

                bool isNewRos = true;
                if (infoModel.ROSSystemInfoID > 0)
                {
                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemInfo(MDVSession.Current.AppUserId, infoModel.ROSSystemInfoID, infoModel.NotesId, -1);
                    dsROSSystemPatientWrapper = objROSSystemPatientWrapper.Data;

                    if (dsROSSystemPatientWrapper.ROSSystemInfo.Rows.Count > 0)
                    {
                        isNewRos = false;
                    }

                }
            
                drSysteminfo = (isNewRos || dsROSSystemPatientWrapper.ROSSystemInfo.Rows.Count <= 0) ?
                    dsROSSystemPatientWrapper.ROSSystemInfo.NewROSSystemInfoRow() : (DSClinicalReviewofSystem.ROSSystemInfoRow)dsROSSystemPatientWrapper.ROSSystemInfo.Rows[0];


                if (infoModel.ROSSystemInfoID > 0)
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSSystemInfoIDColumn] = infoModel.ROSSystemInfoID;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSSystemInfoIDColumn] = -1;
                }
                if (!string.IsNullOrEmpty(infoModel.Comments))
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.CommentsColumn] = infoModel.Comments;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.CommentsColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(infoModel.Description))
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.DescriptionColumn] = infoModel.Description;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.DescriptionColumn] = DBNull.Value;
                }
                if (infoModel.IsNormal)
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.IsNormalColumn] = infoModel.IsNormal;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.IsNormalColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(infoModel.ROSSystemDate))
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSSystemDateColumn] = MDVUtility.ToDateTime(infoModel.ROSSystemDate);
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSSystemDateColumn] = DBNull.Value;
                }
                if (infoModel.NotesId > 0)
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.NotesIdColumn] = infoModel.NotesId;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.NotesIdColumn] = DBNull.Value;
                }

                if (infoModel.ROSTemplateId > 0)
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSTemplateIdColumn] = infoModel.ROSTemplateId;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSTemplateIdColumn] = DBNull.Value;
                }
                if (infoModel.ROSDataTemplateId > 0)
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSDataTemplateIdColumn] = infoModel.ROSDataTemplateId;
                }
                else
                {
                    drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSDataTemplateIdColumn] = DBNull.Value;
                }


            
                drSysteminfo.IsActive = true;
                drSysteminfo.CreatedBy = AppUserName;
                drSysteminfo.CreatedOn = currentDateTime;
                drSysteminfo.ModifiedBy = AppUserName;
                drSysteminfo.ModifiedOn = currentDateTime;

                //// if no details is found against CharacteristicId, it implies for new record
                if (dsROSSystemPatientWrapper.ROSSystemInfo.Rows.Count < 1)
                {
                    dsROSSystemPatientWrapper.ROSSystemInfo.AddROSSystemInfoRow(drSysteminfo);
                }

                #endregion
                BLObject<DSClinicalReviewofSystem> objDetails = new BLObject<DSClinicalReviewofSystem>();
                if (infoModel.ROSSystemInfoID > 0)
                {

                    objDetails = BLLClinicalObj.updateROSPatientInfo(dsROSSystemPatientWrapper);
                    dsROSSystemPatientWrapper = objDetails.Data;
                }
                else
                {
                    objDetails = BLLClinicalObj.insertROSPatientInfo(dsROSSystemPatientWrapper);
                    dsROSSystemPatientWrapper = objDetails.Data;


                }
                if (!string.IsNullOrEmpty(wrapperModel.systemSortingOrder))
                {
                    updateROSSystemUserInfo(wrapperModel.systemSortingOrder, wrapperModel.ROSSystemPatSortingOrder);
                }

                if (infoModel.IsNormal && (wrapperModel.isNormalDescription == null || wrapperModel.isNormalDescription.Count == 0))
                {
                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToLong(wrapperModel.NotesId), 0, 0);
                    DSClinicalReviewofSystem dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;
                    dsROSSystemPatientWrapper.Merge(dsROSSystemPatientCharac);
                }
                else
                {
                    wrapperModel.ROSSystemInfoID = MDVUtility.ToLong(drSysteminfo[dsROSSystemPatientWrapper.ROSSystemInfo.ROSSystemInfoIDColumn]);

                    #region SystemPatient

                    if (wrapperModel.ROSSystemIds != null && wrapperModel.ROSSystemIds.Count > 0)
                    {
                        if (wrapperModel.ROSSystemInfoID > 0)
                        {

                            objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatient(MDVSession.Current.AppUserId, 0, wrapperModel.NotesId);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                            DSClinicalReviewofSystem dsROSSystemPatienT = objROSSystemPatientWrapper.Data;
                            dsROSSystemPatienT = insertUpdateRosSystemPatient(wrapperModel, dsROSSystemPatienT);
                            dsROSSystemPatientWrapper.Merge(dsROSSystemPatienT);
                        }
                    }

                    #endregion

                    if (wrapperModel.ROSSystemInfoID > 0)
                    {
                        if (dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count > 0 && ((wrapperModel.charcNegCharacteristics != null && wrapperModel.charcNegCharacteristics.Count > 0) || (wrapperModel.charcPosCharacteristics != null && wrapperModel.charcPosCharacteristics.Count > 0) || (wrapperModel.SystemsWithDetails != null && wrapperModel.SystemsWithDetails.Count > 0)))
                        {
                            objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToLong(wrapperModel.NotesId), 0, 0);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                            DSClinicalReviewofSystem dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;

                            #region ROSSystemPatientCharacteristics

                            #region for loop for -ve and +ve charc
                            List<ROSSystemPatientCharacteristicsModel> sysPatientCharcModelList = new List<ROSSystemPatientCharacteristicsModel>();
                            for (int i = 0; i < wrapperModel.charcNegCharacteristics.Count; i++)
                            {
                                ROSSystemPatientCharacteristicsModel sysPatientCharcModel = new ROSSystemPatientCharacteristicsModel();
                                int charcNegCharacteristicsId = wrapperModel.charcNegCharacteristics[i] != null ? MDVUtility.ToInt32(wrapperModel.charcNegCharacteristics[i]) : 0;
                                int charcNegSystemId = wrapperModel.charcNegSystem[i] != null ? MDVUtility.ToInt32(wrapperModel.charcNegSystem[i]) : 0;
                                sysPatientCharcModel.CharacteristicsId = MDVUtility.ToInt32(charcNegCharacteristicsId);//wrapperModel.charcNegCharacteristics
                                if (wrapperModel.charcDescNeg != null && wrapperModel.charcNegSystem.Count > i && wrapperModel.charcDescNeg[i] != null)
                                {
                                    sysPatientCharcModel.Description = wrapperModel.charcDescNeg[i];
                                }
                                if (wrapperModel.AllPositiveSystems.Any(s => charcNegSystemId.ToString().Contains(s)))
                                {
                                    sysPatientCharcModel.IsPositive = true;
                                }
                                else
                                {
                                    sysPatientCharcModel.IsPositive = false;
                                }

                                sysPatientCharcModel.ROSSystemCharacteristicsId = -1;
                                sysPatientCharcModel.ROSSystemPatientCharacteristicsID = -1;

                                sysPatientCharcModel.CharcName = wrapperModel.charcNegName[i];
                                sysPatientCharcModel.SystemID = charcNegSystemId;
                                foreach (DSClinicalReviewofSystem.ROSSystemPatientRow drSystemPatient in dsROSSystemPatientWrapper.ROSSystemPatient.Rows)
                                {
                                    string SysId = drSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.ROSSystemIdColumn].ToString();
                                    if (MDVUtility.ToInt32(SysId) == sysPatientCharcModel.SystemID)
                                    {
                                        sysPatientCharcModel.ROSSystemPatientID = MDVUtility.ToLong(drSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.ROSSystemPatientIDColumn].ToString());
                                        sysPatientCharcModelList.Add(sysPatientCharcModel);
                                        break;
                                    }

                                }

                            }
                            for (int i = 0; i < wrapperModel.charcPosCharacteristics.Count; i++)
                            {
                                ROSSystemPatientCharacteristicsModel sysPatientCharcModel = new ROSSystemPatientCharacteristicsModel();
                                int charcPosCharacteristicsId = wrapperModel.charcPosCharacteristics[i] != null ? MDVUtility.ToInt32(wrapperModel.charcPosCharacteristics[i]) : 0;
                                int charcPosSystemId = wrapperModel.charcPosSystem[i] != null ? MDVUtility.ToInt32(wrapperModel.charcPosSystem[i]) : 0;
                                sysPatientCharcModel.CharacteristicsId = MDVUtility.ToInt32(charcPosCharacteristicsId);//wrapperModel.charcNegCharacteristics
                                if (wrapperModel.charcDescPos != null && wrapperModel.charcDescPos[i] != null)
                                {
                                    sysPatientCharcModel.Description = wrapperModel.charcDescPos[i];
                                }
                                sysPatientCharcModel.CharcName = wrapperModel.charcPosName[i];
                                if (wrapperModel.AllNegativeSystems.Any(s => charcPosSystemId.ToString().Contains(s)))
                                {
                                    sysPatientCharcModel.IsPositive = false;
                                }
                                else
                                {
                                    sysPatientCharcModel.IsPositive = true;
                                }

                                sysPatientCharcModel.ROSSystemCharacteristicsId = -1;
                                sysPatientCharcModel.ROSSystemPatientCharacteristicsID = -1;
                                sysPatientCharcModel.SystemID = charcPosSystemId;

                                foreach (DSClinicalReviewofSystem.ROSSystemPatientRow drSystemPatient in dsROSSystemPatientWrapper.ROSSystemPatient.Rows)
                                {
                                    string SysId = drSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.ROSSystemIdColumn].ToString();
                                    if (MDVUtility.ToInt32(SysId) == sysPatientCharcModel.SystemID)
                                    {
                                        sysPatientCharcModel.ROSSystemPatientID = MDVUtility.ToLong(drSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.ROSSystemPatientIDColumn].ToString());
                                        sysPatientCharcModelList.Add(sysPatientCharcModel);
                                        break;
                                    }

                                }


                            }

                            #endregion

                            #region update  ROSSystemPatient Characteristics
                            if (dsROSSystemPatientCharac != null && dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.Rows.Count > 0)
                            {

                                List<string> delROSSystemCharac = new List<string>();
                                foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.Rows)
                                {
                                    if (sysPatientCharcModelList.Count == 0 && infoModel.ROSDataTemplateId <= 0)
                                    {
                                        break;
                                    }

                                    string ROSSystemPatientCharacteristicsId = drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn].ToString();
                                    string ROSSystemPatientID = drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemPatientIDColumn].ToString();
                                    string ROSSystemCharacteristicsId = drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn].ToString();

                                    ROSSystemPatientCharacteristicsModel model = sysPatientCharcModelList.FirstOrDefault(n => n.ROSSystemPatientID == MDVUtility.ToLong(ROSSystemPatientID) && n.CharacteristicsId == MDVUtility.ToLong(ROSSystemCharacteristicsId));

                                    if (model == null)
                                    {
                                        if (wrapperModel.UncheckedCharacteristics.Any(s => ROSSystemCharacteristicsId.ToString().Equals(s)))
                                        {
                                            drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.DescriptionColumn] = DBNull.Value;
                                            drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.IsPositiveColumn] = DBNull.Value;
                                        }
                                    }
                                    else
                                    {
                                        sysPatientCharcModelList.Remove(model);
                                        if (model.ROSSystemPatientCharacteristicsID > 0)
                                        {
                                            drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(ROSSystemPatientCharacteristicsId))
                                            {
                                                drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID = MDVUtility.ToInt64(ROSSystemPatientCharacteristicsId);
                                            }
                                            else
                                            {
                                                drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn] = DBNull.Value;
                                            }

                                        }

                                        if (model.ROSSystemPatientID > 0)
                                        {
                                            drROSSystemPatientCharacteristics.ROSSystemPatientID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                                        }
                                        else
                                        {
                                            drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemPatientIDColumn] = DBNull.Value;
                                        }

                                        if (model.ROSSystemCharacteristicsId > 0)
                                        {
                                            drROSSystemPatientCharacteristics.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                        }
                                        else
                                        {
                                            if (model.CharacteristicsId > 0)
                                            {
                                                drROSSystemPatientCharacteristics.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.CharacteristicsId);
                                            }
                                            else
                                            {
                                                drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(model.Description))
                                        {
                                            drROSSystemPatientCharacteristics.Description = MDVUtility.ToStr(model.Description);
                                        }
                                        else
                                        {
                                            drROSSystemPatientCharacteristics[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.DescriptionColumn] = DBNull.Value;
                                        }


                                        drROSSystemPatientCharacteristics.IsPositive = model.IsPositive;


                                    }
                                }
                                //if (delROSSystemCharac != null && delROSSystemCharac.Count > 0)
                                //{
                                //    foreach (var item in delROSSystemCharac)
                                //    {
                                //        if (!string.IsNullOrEmpty(item))
                                //        {
                                //            BLLClinicalObj.deleteROSSystemPatientCharacteristics(MDVUtility.ToLong(item));
                                //        }

                                //    }

                                //}
                                if (sysPatientCharcModelList.Count == 0)
                                {
                                    BLObject<DSClinicalReviewofSystem> obj = BLLClinicalObj.updateROSSystemPatientCharacteristics(dsROSSystemPatientCharac);
                                    //delROSSystemCharac
                                    dsROSSystemPatientCharac = obj.Data;

                                }
                            }
                            #endregion
                            #region insert  ROSSystemPatient Characteristics

                            if (sysPatientCharcModelList.Count > 0)
                            {


                                foreach (ROSSystemPatientCharacteristicsModel model in sysPatientCharcModelList)
                                {
                                    DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow dr = dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.NewROSSystemPatientCharacteristicsRow();
                                    if (model.ROSSystemPatientCharacteristicsID > 0)
                                    {
                                        dr.ROSSystemPatientCharacteristicsID = MDVUtility.ToInt64(model.ROSSystemPatientCharacteristicsID);
                                    }
                                    else
                                    {
                                        dr[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsIDColumn] = DBNull.Value;
                                    }

                                    if (model.ROSSystemPatientID > 0)
                                    {
                                        dr.ROSSystemPatientID = MDVUtility.ToInt64(model.ROSSystemPatientID);
                                    }
                                    else
                                    {
                                        dr[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemPatientIDColumn] = DBNull.Value;
                                    }
                                    if (model.ROSSystemCharacteristicsId > 0)
                                    {
                                        dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.ROSSystemCharacteristicsId);
                                    }
                                    else
                                    {
                                        if (model.CharacteristicsId > 0)
                                        {
                                            dr.ROSSystemCharacteristicsId = MDVUtility.ToInt64(model.CharacteristicsId);
                                        }
                                        else
                                        {
                                            dr[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn] = DBNull.Value;
                                        }

                                    }

                                    if (!string.IsNullOrEmpty(model.Description))
                                    {
                                        dr.Description = MDVUtility.ToStr(model.Description);
                                    }
                                    else
                                    {
                                        dr[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.DescriptionColumn] = DBNull.Value;
                                    }
                                    //if (!string.IsNullOrEmpty(model.CharcName))
                                    //{
                                    //    dr.CharacteristicsName = MDVUtility.ToStr(model.CharcName);
                                    //}
                                    //else
                                    //{
                                    //    dr[dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.CharacteristicsNameColumn] = DBNull.Value;
                                    //}
                                    dr.IsPositive = model.IsPositive;
                                    dsROSSystemPatientCharac.ROSSystemPatientCharacteristics.AddROSSystemPatientCharacteristicsRow(dr);
                                }
                                BLObject<DSClinicalReviewofSystem> objIns = BLLClinicalObj.insertROSSystemPatientCharacteristics(dsROSSystemPatientCharac);
                                dsROSSystemPatientCharac = objIns.Data;
                                objIns = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToLong(wrapperModel.NotesId), 0, 0);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                                dsROSSystemPatientCharac = objIns.Data;
                            }
                            #endregion
                            if (dsROSSystemPatientCharac != null)
                            {
                                dsROSSystemPatientWrapper.Merge(dsROSSystemPatientCharac);
                            }
                            //  }
                            #endregion

                            #region charc details update
                            if (dsROSSystemPatientCharac != null)
                            {
                                foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.Rows)
                                {

                                    Int64 ROSSystemCharacteristicsId = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn.ColumnName]);
                                    //Int64 ROSSystemPatientCharacteristicsID = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn.ColumnName]);

                                    if (ROSSystemCharacteristicsId > 0)
                                    {
                                        Int64 ROSCharacteristicsDetailsId = 0;
                                        DSClinicalReviewofSystem responseCharacteristicsDetails = new DSClinicalReviewofSystem();
                                        if (drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DetailsIDColumn.ColumnName] != DBNull.Value)
                                        {
                                            ROSCharacteristicsDetailsId = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DetailsIDColumn.ColumnName]);
                                            BLObject<DSClinicalReviewofSystem> objDetail = BLLClinicalObj.loadROSCharacteristicsDetails(wrapperModel.NotesId, 0, ROSCharacteristicsDetailsId, -1);
                                            responseCharacteristicsDetails = objDetail.Data;
                                        }

                                        if (wrapperModel.rosPatientCharcObjList != null && wrapperModel.rosPatientCharcObjList.Count > 0)
                                        {

                                            if (wrapperModel.rosPatientCharcObjList.Any(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId))
                                            {
                                                List<ROSCharacteristicsDetailsModel> modelObjList = wrapperModel.rosPatientCharcObjList.Where(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId).ToList<ROSCharacteristicsDetailsModel>();

                                                responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSSystemCharacteristicsId, modelObjList, dsROSSystemPatientWrapper);
                                                wrapperModel.rosPatientCharcObjList.RemoveAll(s => s.ROSCharacteristicsId == ROSSystemCharacteristicsId);
                                            }
                                            else if (wrapperModel.rosPatientCharcObjList.Any(s => s.ROSCharacteristicsId == -1))
                                            {
                                                List<ROSCharacteristicsDetailsModel> modelObjList = wrapperModel.rosPatientCharcObjList.Where(s => s.ROSCharacteristicsId == -1).ToList<ROSCharacteristicsDetailsModel>();
                                                responseCharacteristicsDetails = insertUpdateCharacteristicsDetails(ROSSystemCharacteristicsId, modelObjList, dsROSSystemPatientWrapper);
                                                wrapperModel.rosPatientCharcObjList.RemoveAll(s => s.ROSCharacteristicsId == -1);
                                            }

                                            if (responseCharacteristicsDetails != null)
                                            {
                                                dsROSSystemPatientWrapper.Merge(responseCharacteristicsDetails);
                                            }
                                        }
                                        else
                                        {
                                            dsROSSystemPatientWrapper.Merge(responseCharacteristicsDetails);
                                        }

                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToLong(wrapperModel.NotesId), 0, 0);
                            DSClinicalReviewofSystem dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;
                            dsROSSystemPatientWrapper.Merge(dsROSSystemPatientCharac);
                        }

                    }

                }


                soapText = createROSsoapText(dsROSSystemPatientWrapper);


                var response = new
                {
                    status = true,
                    Message = Common.AppPrivileges.Save_Message,
                    soapTextROS = soapText,
                    ROSSystemInfo_JSON = MDVUtility.JSON_DataTable(dsROSSystemPatientWrapper.Tables[dsROSSystemPatientWrapper.ROSSystemInfo.TableName]),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
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

        public string saveROSSystemPatientInfoFromNotes(long NotesID, long ROSDataTemplateId)
        {
            string soapText = string.Empty;
            long ROSSystemInfoId = 0;
            DSClinicalReviewofSystem dsROSSystemPatientWrapper = new DSClinicalReviewofSystem();
            BLObject<DSClinicalReviewofSystem> objROSSystemPatientWrapper = null;
            String ROSInfoIds = BLLClinicalObj.getROSSystemInfo(NotesID, ROSDataTemplateId);

            if (!String.IsNullOrEmpty(ROSInfoIds) && !String.IsNullOrWhiteSpace(ROSInfoIds))
            {
                ROSSystemInfoId = MDVUtility.ToInt64(ROSInfoIds);

                if (ROSSystemInfoId > 0)
                {
                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemInfo(MDVSession.Current.AppUserId, MDVUtility.ToInt64(ROSSystemInfoId), NotesID, -1);
                    dsROSSystemPatientWrapper = objROSSystemPatientWrapper.Data;

                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatient(MDVSession.Current.AppUserId, 0, NotesID);
                    DSClinicalReviewofSystem dsROSSystemPatienT = objROSSystemPatientWrapper.Data;

                    dsROSSystemPatientWrapper.Merge(dsROSSystemPatienT);

                    if (dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count > 0)
                    {
                        objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToLong(NotesID), 0, 0);
                        DSClinicalReviewofSystem dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;

                        if (dsROSSystemPatientCharac != null)
                        {
                            dsROSSystemPatientWrapper.Merge(dsROSSystemPatientCharac);
                        }

                        if (dsROSSystemPatientCharac != null)
                        {

                            foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.Rows)
                            {

                                Int64 ROSSystemCharacteristicsId = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn.ColumnName]);

                                if (ROSSystemCharacteristicsId > 0)
                                {
                                    Int64 ROSCharacteristicsDetailsId = 0;
                                    DSClinicalReviewofSystem responseCharacteristicsDetails = new DSClinicalReviewofSystem();

                                    if (drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DetailsIDColumn.ColumnName] != DBNull.Value)
                                    {

                                        ROSCharacteristicsDetailsId = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DetailsIDColumn.ColumnName]);
                                        BLObject<DSClinicalReviewofSystem> objDetail = BLLClinicalObj.loadROSCharacteristicsDetails(NotesID, 0, ROSCharacteristicsDetailsId, -1);
                                        responseCharacteristicsDetails = objDetail.Data;

                                        if (responseCharacteristicsDetails != null)
                                        {
                                            dsROSSystemPatientWrapper.Merge(responseCharacteristicsDetails);
                                        }
                                    }
                                    else
                                    {
                                        dsROSSystemPatientWrapper.Merge(responseCharacteristicsDetails);
                                    }

                                }

                            }

                        }

                    }

                    soapText = createROSsoapText(dsROSSystemPatientWrapper);

                }
            }

            var response = new
            {
                status = true,
                ROSSystemInfoID = ROSSystemInfoId,
                RosSoapText_JSON = soapText,
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }

        public string GetROSSystemPatientInfoFromNotes(long NotesID, long ROSDataTemplateId)
        {
            string soapText = string.Empty;
            long ROSSystemInfoId = 0;

            DSClinicalReviewofSystem dsROSSystemPatientWrapper = new DSClinicalReviewofSystem();
            BLObject<DSClinicalReviewofSystem> objROSSystemPatientWrapper = null;

            String ROSInfoIds = BLLClinicalObj.getROSSystemInfo(NotesID, ROSDataTemplateId);

            if (!String.IsNullOrEmpty(ROSInfoIds) && !String.IsNullOrWhiteSpace(ROSInfoIds))
            {
                ROSSystemInfoId = MDVUtility.ToInt64(ROSInfoIds);

                if (ROSSystemInfoId > 0)
                {

                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemInfo(MDVSession.Current.AppUserId, MDVUtility.ToInt64(ROSSystemInfoId), NotesID, -1);
                    dsROSSystemPatientWrapper = objROSSystemPatientWrapper.Data;

                    objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatient(MDVSession.Current.AppUserId, 0, NotesID);
                    DSClinicalReviewofSystem dsROSSystemPatienT = objROSSystemPatientWrapper.Data;

                    dsROSSystemPatientWrapper.Merge(dsROSSystemPatienT);

                    if (dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count > 0)
                    {
                        objROSSystemPatientWrapper = BLLClinicalObj.loadROSSystemPatientCharacteristics(MDVUtility.ToLong(NotesID), 0, 0);//model.ROSSystemPatientID);//wrapperModel.rosSystemPatientID);//.loadROSPatientInfo(MDVUtility.ToLong(wrapperModel.PatientId), wrapperModel.ROSSystemInfoID);
                        DSClinicalReviewofSystem dsROSSystemPatientCharac = objROSSystemPatientWrapper.Data;
                        if (dsROSSystemPatientCharac != null)
                        {
                            dsROSSystemPatientWrapper.Merge(dsROSSystemPatientCharac);
                        }
                        if (dsROSSystemPatientCharac != null)
                        {
                            foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.Rows)
                            {
                                Int64 ROSSystemCharacteristicsId = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.ROSSystemCharacteristicsIdColumn.ColumnName]);

                                if (ROSSystemCharacteristicsId > 0)
                                {
                                    Int64 ROSCharacteristicsDetailsId = 0;
                                    DSClinicalReviewofSystem responseCharacteristicsDetails = new DSClinicalReviewofSystem();

                                    if (drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DetailsIDColumn.ColumnName] != DBNull.Value)
                                    {
                                        ROSCharacteristicsDetailsId = MDVUtility.ToInt64(drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DetailsIDColumn.ColumnName]);

                                        BLObject<DSClinicalReviewofSystem> objDetail = BLLClinicalObj.loadROSCharacteristicsDetails(NotesID, 0, ROSCharacteristicsDetailsId, -1);
                                        responseCharacteristicsDetails = objDetail.Data;

                                        if (responseCharacteristicsDetails != null)
                                        {
                                            dsROSSystemPatientWrapper.Merge(responseCharacteristicsDetails);
                                        }
                                    }
                                    else
                                    {
                                        dsROSSystemPatientWrapper.Merge(responseCharacteristicsDetails);
                                    }
                                }
                            }
                        }
                    }

                    soapText = createROSsoapText(dsROSSystemPatientWrapper);
                }
            }

            var response = new
            {
                status = true,
                ROSSystemInfoID = ROSSystemInfoId,
                RosSoapText_JSON = soapText,
            };

            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }


        #endregion

        #region Soap Text for ROS




        public string createROSsoapText(DSClinicalReviewofSystem dsROSSystemPatientWrapper)
        {
            StringBuilder sb = new StringBuilder();
            if (dsROSSystemPatientWrapper != null)
            {
                if (dsROSSystemPatientWrapper.ROSSystemInfo != null && dsROSSystemPatientWrapper.ROSSystemInfo.Rows.Count > 0)
                {

                    string overAllComments = string.Empty;
                    foreach (DSClinicalReviewofSystem.ROSSystemInfoRow drROSSystemInfo in dsROSSystemPatientWrapper.ROSSystemInfo.Rows)
                    {
                        string GlobalNormalDescription = (drROSSystemInfo[dsROSSystemPatientWrapper.ROSSystemInfo.DescriptionColumn] != DBNull.Value ? drROSSystemInfo.Description.ToString() : string.Empty);
                        overAllComments = (drROSSystemInfo[dsROSSystemPatientWrapper.ROSSystemInfo.CommentsColumn] != DBNull.Value ? drROSSystemInfo.Comments.ToString() : string.Empty);
                        if (!drROSSystemInfo.IsIsNormalNull())
                        {
                            if (drROSSystemInfo.IsNormal == true)
                            {
                                if (!string.IsNullOrEmpty(GlobalNormalDescription))
                                {
                                    sb.Append(GlobalNormalDescription + "<br>");
                                }

                            }
                        }
                                           
                    }
                    if (dsROSSystemPatientWrapper.ROSSystemPatient != null && dsROSSystemPatientWrapper.ROSSystemPatient.Rows.Count > 0)
                    {
                        foreach (DSClinicalReviewofSystem.ROSSystemPatientRow drROSSystemPatient in dsROSSystemPatientWrapper.ROSSystemPatient.Rows)
                        {
                            string SystemName = drROSSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.SystemNameColumn] != DBNull.Value ? drROSSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.SystemNameColumn].ToString() : "";
                            long SystemId = drROSSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.ROSSystemIdColumn] != DBNull.Value ? MDVUtility.ToLong(drROSSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.ROSSystemIdColumn]) : 0;


                            string SystemNormalDescription = drROSSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.DescriptionColumn] != DBNull.Value ? drROSSystemPatient[dsROSSystemPatientWrapper.ROSSystemPatient.DescriptionColumn].ToString() : "";
                            

                            if (!drROSSystemPatient.IsIsNormalNull())
                            {

                                if (drROSSystemPatient.IsNormal == true)
                                {

                                    sb.Append(generateSoapTextSystemName(true, SystemName, SystemId, SystemNormalDescription));

                                    foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.Rows)
                                    {
                                        if (drROSSystemPatient.ROSSystemPatientID == drROSSystemPatientCharacteristics.ROSSystemPatientID)
                                        {
                                            sb.Append("<span id='divCharc_" + drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID + "' title='Characteristics'  name='Characteristics'>" + ((drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.CharacteristicsNameColumn] != DBNull.Value) ? drROSSystemPatientCharacteristics.CharacteristicsName + " is Normal." : ""));
                                            sb.Append("</span> ");
                                        }
                                    }
                                }
                                else
                                {

                                    if (dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics != null && dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.Rows.Count > 0)
                                    {

                                        bool systemExists = true;

                                        foreach (DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics in dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.Rows)
                                        {

                                            if (drROSSystemPatient.ROSSystemPatientID == drROSSystemPatientCharacteristics.ROSSystemPatientID)
                                            {
                                                string soapTextCharcName = generateSoapTextCharcName(dsROSSystemPatientWrapper, drROSSystemPatientCharacteristics, SystemName, SystemId);
                                                if (!string.IsNullOrEmpty(soapTextCharcName))
                                                {
                                                    if (systemExists)
                                                    {

                                                        sb.Append(generateSoapTextSystemName(false, SystemName, SystemId, String.Empty));//"<div id='divSys_" + SystemId + "' title='System'  name='System'><strong>" + SystemName + " : </strong><br/>");
                                                        systemExists = false;
                                                    }
                                                    if (drROSSystemPatientCharacteristics.IsPositive == true)
                                                    {
                                                        sb.Append("<span id='divCharc_" + drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID + "' title='Characteristics'  name='Characteristics'>" + ((drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.CharacteristicsNameColumn] != DBNull.Value) ? drROSSystemPatientCharacteristics.CharacteristicsName : ""));
                                                    }
                                                    else
                                                    {
                                                        sb.Append("<span id='divCharc_" + drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID + "' title='Characteristics'  name='Characteristics'>" + ((drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.CharacteristicsNameColumn] != DBNull.Value) ? drROSSystemPatientCharacteristics.CharacteristicsName : ""));
                                                    }

                                                    sb.Append(soapTextCharcName);

                                                    if (dsROSSystemPatientWrapper.ROSCharacteristicsDetails != null && dsROSSystemPatientWrapper.ROSCharacteristicsDetails.Rows.Count > 0)
                                                    {

                                                        bool detailExists = false;
                                                        foreach (DSClinicalReviewofSystem.ROSCharacteristicsDetailsRow drROSCharacteristicsDetails in dsROSSystemPatientWrapper.ROSCharacteristicsDetails.Rows)
                                                        {
                                                            if (drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID == drROSCharacteristicsDetails.ROSSystemPatientCharacteristicsID)
                                                            {
                                                                string soapTextCharc = generateSoapTextForROSCharcDetails(dsROSSystemPatientWrapper, drROSCharacteristicsDetails);
                                                                detailExists = true;
                                                                if (!string.IsNullOrEmpty(soapTextCharc))
                                                                {

                                                                    sb.Append("; " + soapTextCharc);
                                                                    sb.Append("</span>, ");
                                                                }
                                                                else
                                                                {
                                                                    sb.Append("</span>, ");
                                                                }
                                                                break;
                                                            }
                                                        }
                                                        if (!detailExists)
                                                        {
                                                            sb.Append("</span>, ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sb.Append("</span> ");

                                                    }
                                                }
                                            }
                                        }
                                        if (!systemExists)
                                        {
                                            sb.Append("</div>");
                                            string updatedSOAP = sb.ToString();
                                            if (!string.IsNullOrEmpty(updatedSOAP) && updatedSOAP.Length > 8 && updatedSOAP.Substring(updatedSOAP.Length - 8).Equals(", </div>"))
                                            {
                                                updatedSOAP = updatedSOAP.Remove(updatedSOAP.Length - 8);
                                                updatedSOAP += "</div>";
                                            }
                                            sb = new StringBuilder();
                                            sb.Append(updatedSOAP);
                                        }
                                    }

                                }
                            }
                        }
                        sb.Append("<br>" + overAllComments);
                    }
                }


            }

            string returnResult = sb.ToString();
            if (!string.IsNullOrEmpty(returnResult) && returnResult.Length > 8 && returnResult.Substring(returnResult.Length - 8).Equals(", </div>"))
            {
                returnResult = returnResult.Remove(returnResult.Length - 8);
                returnResult += "</div>";
            }
            return returnResult;

        }//End of function.

        public string generateSoapTextSystemName(bool IsNormal, string SystemName, long SystemId, string SystemNormalDescription)
        {
            if (IsNormal)
            {

                return ("<span id='divSys_" + SystemId + "' title='" + SystemName + "'  name='ROSSystem'><strong>" + SystemName + "</strong> is Normal. " + (string.IsNullOrEmpty(SystemNormalDescription) ? "" : SystemNormalDescription + ". ") + "</span>");
              
            }
            else
            {
                return ("<div id='divSys_" + SystemId + "' title='" + SystemName + "'  name='ROSSystem'><strong>" + SystemName + "</strong>: ");
            }
        }

        public string generateSoapTextCharcName(DSClinicalReviewofSystem dsROSSystemPatientWrapper, DSClinicalReviewofSystem.ROSSystemPatientCharacteristicsRow drROSSystemPatientCharacteristics, string SystemName, long SystemId)
        {
            StringBuilder sb = new StringBuilder();


            // return charc name and positive and descriptoin
            if (drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.IsPositiveColumn] != DBNull.Value)
            {
                if (drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.CharacteristicsNameColumn] != DBNull.Value)
                {
                    bool isPositive = drROSSystemPatientCharacteristics.IsPositive;
                    string jQueryFunction = " onclick='Clinical_ReviewofSystems.toggleAttribute(this," + (isPositive ? "true" : "false") + "," + drROSSystemPatientCharacteristics.ROSSystemPatientCharacteristicsID + ",event);'";
                    string cssClass = isPositive ? "red" : "green";
                    string actionLink = "<a href='javascript:void(0);' " + jQueryFunction + " class='hoverToggle " + cssClass + "' >";
                    sb.Append("<b>" + actionLink + (isPositive ? " ( + ) " : " ( - ) ") + "</a></b>");
                }

            }
            if (drROSSystemPatientCharacteristics[dsROSSystemPatientWrapper.ROSSystemPatientCharacteristics.DescriptionColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSSystemPatientCharacteristics.Description.ToString()) ? "" : drROSSystemPatientCharacteristics.Description));
            }
            return sb.ToString();
        }


        public string generateSoapTextForROSCharcDetails(DSClinicalReviewofSystem dsROSSystemPatientWrapper, DSClinicalReviewofSystem.ROSCharacteristicsDetailsRow drROSCharacteristicsDetails)
        {
            StringBuilder sb = new StringBuilder();

            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.PreviousHistoryColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.PreviousHistory.ToString()) ? "" : " Previous history is  " + drROSCharacteristicsDetails.PreviousHistory + "."));

            }
            //Start || 12 July, 2016 || ZeeshanAK || Fix for EMR-1474
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.DetailStatusNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailStatusIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.DetailStatusName.ToString()) ? "" : " Status is  " + drROSCharacteristicsDetails.DetailStatusName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.OnsetColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.Onset.ToString()) ? "" : " Onset is  " + drROSCharacteristicsDetails.Onset + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.DurationColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.Duration.ToString()) ? "" : " Duration is  " + drROSCharacteristicsDetails.Duration));

            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.DurationNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailDurationIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.DurationName.ToString()) ? "" : "  " + drROSCharacteristicsDetails.DurationName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.PatternNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailPatternIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.PatternName.ToString()) ? "" : " Pattern is  " + drROSCharacteristicsDetails.PatternName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.SeverityNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailSeverityIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.SeverityName.ToString()) ? "" : " Severity is  " + drROSCharacteristicsDetails.SeverityName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.CourseNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailCourseIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.CourseName.ToString()) ? "" : " Course is  " + drROSCharacteristicsDetails.CourseName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.RadiationNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailRadiationIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.RadiationName.ToString()) ? "" : " Radiation to  " + drROSCharacteristicsDetails.RadiationName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.FrequencyNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailFrequencyIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.FrequencyName.ToString()) ? "" : " Frequency is  " + drROSCharacteristicsDetails.FrequencyName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ContextNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailContextIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.ContextName.ToString()) ? "" : " Context is  " + drROSCharacteristicsDetails.ContextName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.CharacterCSZNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailCharacterCSZIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.CharacterCSZName.ToString()) ? "" : " Character is  " + drROSCharacteristicsDetails.CharacterCSZName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.AggravedByNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailAggravedByIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.AggravedByName.ToString()) ? "" : " It is aggravated by  " + drROSCharacteristicsDetails.AggravedByName + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.RelievedByNameColumn] != DBNull.Value && drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.ROSCharacteristicsDetailRelievedByIdColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.RelievedByName.ToString()) ? "" : " It is relieved by  " + drROSCharacteristicsDetails.RelievedByName + "."));
            }
            //End   || 12 July, 2016 || ZeeshanAK || Fix for EMR-1474
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.LocationColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.Location.ToString()) ? "" : " Location is  " + drROSCharacteristicsDetails.Location + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.PrecipitatedBYColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.PrecipitatedBY.ToString()) ? "" : " Precipitated by  " + drROSCharacteristicsDetails.PrecipitatedBY + "."));
            }
            if (drROSCharacteristicsDetails[dsROSSystemPatientWrapper.ROSCharacteristicsDetails.AssociatedWithColumn] != DBNull.Value)
            {
                sb.Append((string.IsNullOrEmpty(drROSCharacteristicsDetails.AssociatedWith.ToString()) ? "" : " Associated feature are  " + drROSCharacteristicsDetails.AssociatedWith + "."));
            }

            return sb.ToString();


        }


        #endregion




        #region Load ROS Template Systems
        public string loadROSTemplateSystems(long rosSystemInfoId, long userId, long noteId, long ROSTemplateId)
        {
            try
            {
                DSClinicalReviewofSystemTemplate dsROS = null;
                BLObject<DSClinicalReviewofSystemTemplate> obj;
                obj = BLLClinicalObj.lookupROSTemplates(ROSTemplateId, MDVSession.Current.AppUserId, noteId, string.IsNullOrEmpty(MDVSession.Current.EntityId) ? -1 : MDVUtility.ToLong(MDVSession.Current.EntityId));

                dsROS = obj.Data;
                if (obj.Data != null)
                {
                    var response = new
                    {
                        status = true,
                        ROSTemptCount = dsROS.Tables[dsROS.ROSTemptLookup.TableName].Rows.Count,
                        ROSTempt_JSON = MDVUtility.JSON_DataTable(dsROS.Tables[dsROS.ROSTemptLookup.TableName]),
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ROSTemptCount = 0,
                        Message = obj.Message
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
        #endregion

    }
}