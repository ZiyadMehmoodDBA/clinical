using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.IEHR.EMR.Model.Lab;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Lab
{
    public class ClinicalLabHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public ClinicalLabHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static ClinicalLabHelper _instance = null;
        public static ClinicalLabHelper Instance()
        {
            if (_instance == null)
                _instance = new ClinicalLabHelper();
            return _instance;
        }

        /// Author: Abid Ali
        /// Purpose: To load Clinical Labs
        /// Date : April 06, 2016
        public string loadClinicalLab(ClinicalLabModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = null;
                BLObject<DSClinicalLab> obj;

                // 2 for passing null value. 1 & 0 for Active/Inactive
                byte isActive = 2;
                if (model.EntityId == 0)
                {
                    model.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                }
                if (model.IsActive == "True")
                    isActive = 1;
                else if (model.IsActive == "False")
                    isActive = 0;

                obj = BLLClinicalObj.loadClinicalLab(model.LabId, model.LabName, model.LabType, model.EntityId, isActive, model.PageNumber, model.RowsPerPage);

                dsClinicalLab = obj.Data;
                if (obj.Data != null)
                {
                    if (dsClinicalLab.Tables[dsClinicalLab.Lab.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabCount = dsClinicalLab.Tables[dsClinicalLab.Lab.TableName].Rows.Count,
                            iTotalDisplayRecords = dsClinicalLab.Lab.Rows[0][dsClinicalLab.Lab.RecordCountColumn.ColumnName],
                            ClinicalLab_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.Lab.TableName]),
                            ClinicalLabTestCount = dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows.Count,
                            ClinicalLabTest_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabCount = 0,
                            ClinicalLab_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.Lab.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
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

        /// Author: Abid Ali
        /// Purpose: To Insert/Update Clinical Labs
        /// Date : April 06, 2016
        public string insertUpdateClinicalLab(ClinicalLabModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                DSClinicalLab.LabRow dr = null;
                BLObject<DSClinicalLab> obj = BLLClinicalObj.loadClinicalLab(model.LabId, "", "", model.EntityId, 2, model.PageNumber, model.RowsPerPage);
                dsClinicalLab = obj.Data;
                bool isNewRecord = false;
                DSClinicalLab.LabRow[] arrClinicalLabRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    //A safety check 
                    arrClinicalLabRows = (DSClinicalLab.LabRow[])dsClinicalLab.Lab.Select(dsClinicalLab.Lab.LabIdColumn.ColumnName + "=" + model.LabId);
                    if (arrClinicalLabRows.Length > 0)
                    {
                        dr = arrClinicalLabRows[0];
                        message = Common.AppPrivileges.Update_Message;

                    }
                    else
                    {
                        dr = dsClinicalLab.Lab.NewLabRow();
                        dr.LabId = model.LabId;
                        dr.IsActive = true;

                        message = Common.AppPrivileges.Save_Message;
                        dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        dr.CreatedOn = DateTime.Now;
                        isNewRecord = true;
                    }
                }

                if (dr != null)
                {

                    if (!string.IsNullOrEmpty(model.LabName))
                        dr.Name = model.LabName;
                    else
                        dr[dsClinicalLab.Lab.NameColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.ClientNo))
                        dr.ClientNo = model.ClientNo;
                    else
                        dr[dsClinicalLab.Lab.TypeColumn] = DBNull.Value;

                    if (!string.IsNullOrEmpty(model.LabType))
                        dr.Type = model.LabType;
                    else
                        dr[dsClinicalLab.Lab.TypeColumn] = DBNull.Value;

                    if (model.EntityId == 0)
                        dr.EntityId = Convert.ToInt64(MDVSession.Current.EntityId);
                    else
                        dr.EntityId = model.EntityId;

                    if (!string.IsNullOrEmpty(model.Comments))
                        dr.Comments = model.Comments;
                    else
                        dr[dsClinicalLab.Lab.CommentsColumn] = DBNull.Value;

                    if (model.IsActive == "True")
                        dr.IsActive = true;
                    else if (model.IsActive == "False")
                        dr.IsActive = false;

                    dr.CategoryId = model.CategoryId;
                    dr.CodeSystemId = model.CodeSystemId;
                    dr.RequisitionTemplateId = model.RequisitionTemplateId;
                    dr.LabTypeId = model.LabTypeId;

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;

                    if (isNewRecord)
                    {
                        dsClinicalLab.Lab.AddLabRow(dr);
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSClinicalLab> objUpdate = BLLClinicalObj.insertUpdateClinicalLab(dsClinicalLab);
                if (obj.Data != null)
                {

                    Int64 labId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.Lab.TableName].Rows[0][dsClinicalLab.Lab.LabIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        message = message,
                        labId = labId,
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

                #endregion
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


        public string activeInactiveLab(ClinicalLabModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                DSClinicalLab.LabRow dr = null;
                BLObject<DSClinicalLab> obj = BLLClinicalObj.loadClinicalLab(model.LabId, "", "", model.EntityId, 2, model.PageNumber, model.RowsPerPage);
                dsClinicalLab = obj.Data;
                DSClinicalLab.LabRow[] arrClinicalLabRows = null;
                string message = string.Empty;
                if (obj.Data != null)
                {
                    arrClinicalLabRows = (DSClinicalLab.LabRow[])dsClinicalLab.Lab.Select(dsClinicalLab.Lab.LabIdColumn.ColumnName + "=" + model.LabId);
                    if (arrClinicalLabRows.Length > 0)
                    {
                        dr = arrClinicalLabRows[0];
                        message = Common.AppPrivileges.Update_Message;

                        if (model.IsActive == "True")
                            dr.IsActive = true;
                        else if (model.IsActive == "False")
                            dr.IsActive = false;
                    }
                }

                #region Database Insertion/Updation

                BLObject<DSClinicalLab> objUpdate = BLLClinicalObj.insertUpdateClinicalLab(dsClinicalLab);
                if (obj.Data != null)
                {

                    Int64 labId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.Lab.TableName].Rows[0][dsClinicalLab.Lab.LabIdColumn.ColumnName]);

                    var response = new
                    {
                        status = true,
                        Message = message,
                        labId = labId,
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

                #endregion
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
        /// Author: Abid Ali
        /// Purpose: To Delete Clinical Labs
        /// Date : April 06, 2016
        public string deleteClincialLab(string labId)
        {
            try
            {
                string result = "";
                BLObject<string> obj;
                obj = BLLClinicalObj.deleteClinicalLab(labId);
                result = obj.Data;

                if (result == "")
                {
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Delete_Message,

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
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region "Clinical Lab Test"

        public string insertClinicalLabTest(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                DSClinicalLab.LabTestRow dr = dsClinicalLab.LabTest.NewLabTestRow();

                //if (!string.IsNullOrEmpty(model.Comments))
                //    dr.Comments = model.Comments;
                //else
                //    dr[dsClinicalLab.Lab.CommentsColumn] = DBNull.Value;


                dr.LabId = MDVUtility.ToInt32(model.LabId);
                dr.LOINC = MDVUtility.ToStr(model.LOINICCODE);
                dr.LOINCDescription = MDVUtility.ToStr(model.LOINICDescription);
                if (model.Template == "True")
                    dr.IsTemplate = true;
                else if (model.Template == "False")
                    dr.IsTemplate = false;
                if (model.Active == "True")
                    dr.IsActive = true;
                else if (model.Active == "False")
                    dr.IsActive = false;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                dsClinicalLab.LabTest.AddLabTestRow(dr);

                #region Database Insertion/Updation

                BLObject<DSClinicalLab> obj = BLLClinicalObj.InsertLabTest(dsClinicalLab);
                if (obj.Data != null)
                {

                    Int64 labTestId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows[0][dsClinicalLab.LabTest.LabTestIdColumn.ColumnName]);
                    model.LabTestId = MDVUtility.ToStr(labTestId);
                    insertClinicalLabTestAttribute(model);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        labTestId = labTestId,
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

                #endregion
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

        public string insertClinicalLabTestAttribute(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                DSClinicalLab.LabTestAttributesRow dr = dsClinicalLab.LabTestAttributes.NewLabTestAttributesRow();


                dr.LabTestId = MDVUtility.ToInt64(model.LabTestId);
                dr.AttributeName = MDVUtility.ToStr(model.Attribute);
                if (!string.IsNullOrEmpty(model.UoM))
                    dr.UoM = MDVUtility.ToStr(model.UoM);
                else
                    dr[dsClinicalLab.LabTestAttributes.UoMColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.Range))
                    dr.Range = MDVUtility.ToStr(model.Range);
                else
                    dr[dsClinicalLab.LabTestAttributes.RangeColumn] = DBNull.Value;
                if (!string.IsNullOrEmpty(model.Description))
                    dr.Description = MDVUtility.ToStr(model.Description);
                else
                    dr[dsClinicalLab.LabTestAttributes.DescriptionColumn] = DBNull.Value;
                dr.IsActive = true;
                dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.CreatedOn = DateTime.Now;
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                dsClinicalLab.LabTestAttributes.AddLabTestAttributesRow(dr);

                #region Database Insertion/Updation

                BLObject<DSClinicalLab> obj = BLLClinicalObj.InsertLabTestAttribute(dsClinicalLab);
                if (obj.Data != null)
                {
                    Int64 labTestAttributeId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows[0][dsClinicalLab.LabTestAttributes.LabTestAttributeIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Save_Message,
                        labTestAttributeId = labTestAttributeId,
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

                #endregion
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

        public string loadClinicalLabTestAttribuites(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = null;
                BLObject<DSClinicalLab> obj;


                obj = BLLClinicalObj.LoadLabTestAttribute(MDVUtility.ToInt64(model.LabTestAttributeId), MDVUtility.ToInt64(model.LabTestId));

                dsClinicalLab = obj.Data;
                if (obj.Data != null)
                {
                    if (dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabTestAttributeCount = dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows.Count,
                            ClinicalLabTestAttribute_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName]),
                            ClinicalLabTestAttributeResultCount = dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName].Rows.Count,
                            ClinicalLabTestAttributeResult_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabTestAttributeCount = 0,
                            ClinicalLabTestAttribute_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName]),
                            ClinicalLabTestAttributeResultCount = 0,
                            ClinicalLabTestAttributeResult_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ClinicalLabTestAttributeCount = 0,
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

        public string loadClinicalLabTest(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = null;
                BLObject<DSClinicalLab> obj;


                obj = BLLClinicalObj.LoadLabTest(MDVUtility.ToInt64(model.LabTestId), MDVUtility.ToInt64(model.LabId),"",model.IsActive);

                dsClinicalLab = obj.Data;
                if (obj.Data != null)
                {
                    if (dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabTestAttributeCount = dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows.Count,
                            ClinicalLabTestAttribute_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName]),
                            ClinicalLabTestCount = dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows.Count,
                            ClinicalLabTest_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabTestAttributeCount = 0,
                            ClinicalLabTestAttribute_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName]),
                            ClinicalLabTestCount = 0,
                            ClinicalLabTest_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ClinicalLabTestAttributeCount = 0,
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

        public string deleteLabTest(ClinicalLabTestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.LabTestId)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteLabTest(MDVUtility.ToStr(model.LabTestId));
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

        public string deleteLabTestAttribute(ClinicalLabTestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.LabTestAttributeId)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteLabTestAttribute(MDVUtility.ToStr(model.LabTestAttributeId));
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


        public string editClinicalLabTestAttribute(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                BLObject<DSClinicalLab> objLoad = BLLClinicalObj.LoadLabTest(MDVUtility.ToInt64(model.LabTestId), MDVUtility.ToInt64(model.LabId));
                dsClinicalLab = objLoad.Data;

                foreach (DSClinicalLab.LabTestRow dr in dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows)
                {
                    if (model.Active == "True")
                        dr.IsActive = true;
                    else if (model.Active == "False")
                        dr.IsActive = false;

                    if (model.Template == "True")
                        dr.IsTemplate = true;
                    else if (model.Template == "False")
                        dr.IsTemplate = false;

                    dr.LOINC = MDVUtility.ToStr(model.LOINICCODE);
                    dr.LOINCDescription = MDVUtility.ToStr(model.LOINICDescription);
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Insertion/Updation

                BLObject<DSClinicalLab> obj = BLLClinicalObj.UpdateLabTest(dsClinicalLab);
                if (obj.Data != null)
                {

                    Int64 labTestId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows[0][dsClinicalLab.LabTest.LabTestIdColumn.ColumnName]);
                    model.LabTestId = MDVUtility.ToStr(labTestId);
                    insertClinicalLabTestAttribute(model);

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Update_Message,
                        labTestId = labTestId,
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

                #endregion
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

        public string updateUOMRange(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                BLObject<DSClinicalLab> objLoad = BLLClinicalObj.LoadLabTestAttribute(MDVUtility.ToInt64(model.LabTestAttributeId),MDVUtility.ToInt64(model.LabTestId));
                dsClinicalLab = objLoad.Data;

                foreach (DSClinicalLab.LabTestAttributesRow dr in dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows)
                {
                   
                    if (model.isUOM == "True")
                    {
                        dr.UoM = model.UoM;
                    }
                    else
                    {
                        dr.Range = model.Range;
                    }
                    

                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Insertion/Updation

                BLObject<DSClinicalLab> obj = BLLClinicalObj.UpdateLabTestAttribute(dsClinicalLab);
                if (obj.Data != null)
                {

                    Int64 labTestAttributeId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows[0][dsClinicalLab.LabTestAttributes.LabTestAttributeIdColumn.ColumnName]);
                    model.LabTestAttributeId = MDVUtility.ToStr(labTestAttributeId);
                   

                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Update_Message,
                        labTestId = labTestAttributeId,
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

                #endregion
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

        public string loadClinicalLabTestAndAttributes(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = null;
                BLObject<DSClinicalLab> obj;


                obj = BLLClinicalObj.LoadLabTestAndAttributes(MDVUtility.ToInt64(model.LabTestId), MDVUtility.ToInt64(model.LabId), model.LOINICCODE, model.LOINICDescription);

                dsClinicalLab = obj.Data;
                if (obj.Data != null)
                {
                    if (dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows.Count > 0)
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabTestAttributeCount = dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName].Rows.Count,
                            ClinicalLabTestAttribute_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName]),
                            ClinicalLabTestCount = dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows.Count,
                            ClinicalLabTest_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName]),
                            ClinicalLabTestAttributeResultCount = dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName].Rows.Count,
                            ClinicalLabTestAttributeResult_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        var response = new
                        {
                            status = true,
                            ClinicalLabTestAttributeCount = 0,
                            ClinicalLabTestAttribute_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributes.TableName]),
                            ClinicalLabTestCount = 0,
                            ClinicalLabTest_JSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));

                    }
                }

                else
                {
                    var response = new
                    {
                        status = true,
                        ClinicalLabTestAttributeCount = 0,
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

        public string saveTestActiveInActive(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                BLObject<DSClinicalLab> objLoad = BLLClinicalObj.LoadLabTest(MDVUtility.ToInt64(model.LabTestId), MDVUtility.ToInt64(model.LabId));
                dsClinicalLab = objLoad.Data;

                foreach (DSClinicalLab.LabTestRow dr in dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows)
                {
                    if (model.Active == "True")
                        dr.IsActive = true;
                    else if (model.Active == "False")
                        dr.IsActive = false;
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedOn = DateTime.Now;
                }
                #region Database Insertion/Updation

                BLObject<DSClinicalLab> obj = BLLClinicalObj.UpdateLabTest(dsClinicalLab);
                if (obj.Data != null)
                {

                    Int64 labTestId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.LabTest.TableName].Rows[0][dsClinicalLab.LabTest.LabTestIdColumn.ColumnName]);
                    model.LabTestId = MDVUtility.ToStr(labTestId);
                   
                    var response = new
                    {
                        status = true,
                        message = Common.AppPrivileges.Update_Message,
                        labTestId = labTestId,
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

                #endregion
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

        public string LoadLabTestAttributeResult(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                BLObject<DSClinicalLab> objLoad = BLLClinicalObj.GetLabTestAttributeResult(MDVUtility.ToInt64(model.LabTestAttributeId));
                dsClinicalLab = objLoad.Data;
                if (objLoad.Data != null)
                {

                    var response = new
                    {
                        status = true,
                        LabTestAttributeResultJSON = MDVUtility.JSON_DataTable(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName]),
                        LabTestAttributeResultCount = dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName].Rows.Count,
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        LabTestAttributeResultCount = 0,
                        Message = objLoad.Message
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

        public string SaveLabTestAttributeResult(ClinicalLabTestModel model)
        {
            try
            {
                DSClinicalLab dsClinicalLab = new DSClinicalLab();
                DSClinicalLab.LabTestAttributeResultRow dr = dsClinicalLab.LabTestAttributeResult.NewLabTestAttributeResultRow();
                dr.LabTestAttributeId = MDVUtility.ToInt64(model.LabTestAttributeId);
                dr.ResultName = model.ResultName;
                dsClinicalLab.LabTestAttributeResult.AddLabTestAttributeResultRow(dr);
                #region Database Insertion/Updation
                BLObject<DSClinicalLab> obj = BLLClinicalObj.InsertLabTestAttributeResult(dsClinicalLab);
                if (obj.Data != null)
                {
                    Int64 ResultId = MDVUtility.ToInt64(dsClinicalLab.Tables[dsClinicalLab.LabTestAttributeResult.TableName].Rows[0][dsClinicalLab.LabTestAttributeResult.LabTestAttributeResultIdColumn.ColumnName]);
                    var response = new
                    {
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        LabTestAttributeResultId = ResultId,
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
                #endregion
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
        public string DeleteLabTestAttributeResult(ClinicalLabTestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.LabTestAttributeResultId)))
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
                    BLObject<string> obj = BLLClinicalObj.DeleteLabTestAttributeResult(MDVUtility.ToInt64(model.LabTestAttributeResultId));
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

    }
}