// Author:  Muhammad Azhar Shahzad
// Created Date: 04/01/2016
//OverView: Helper class for BirthHx
using MDVision.Datasets;
using MDVision.Business.BCommon;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MDVision.IEHR.EMR.Model.History;
using System.Text;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;

namespace MDVision.IEHR.EMR.Helpers.Clinical.History
{
    public class BirthHxHelper
    {
        private BLLClinical BLLClinicalObj = null;
        public BirthHxHelper()
        {
            BLLClinicalObj = new BLLClinical();
        }
        private static BirthHxHelper _instance = null;
        public static BirthHxHelper Instance()
        {
            if (_instance == null)
                _instance = new BirthHxHelper();
            return _instance;
        }

        #region saveBirthHx

        public string saveBirthHx(BirthHxModel model, BirthHxGeneralModel generalObj, BirthHxMaternalDeliveryModel maternalDeliveryObj, BirthHxNewbornModel newbornObj)
        {
            try
            {
                DSBirthHistory dsBirthHx = new DSBirthHistory();

                DSBirthHistory.BirthHxRow dr = dsBirthHx.BirthHx.NewBirthHxRow();

                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.BirthHxDate))
                {
                    dr.BirthHxDate = MDVUtility.ToDateTime(model.BirthHxDate);
                }
                else
                {
                    dr[dsBirthHx.BirthHx.BirthHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.BirthHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.BirthHxComments);
                }
                else
                {
                    dr[dsBirthHx.BirthHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.BirthHxUnremarkable;

                dr.IsActive = true;
                if (model.AddFromMobile != "1")
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                    dr.ModifiedOn = DateTime.Now;

                }
                else
                {
                    dr.CreatedBy = model.CreatedBy;
                    dr.ModifiedBy = model.ModifiedBy;
                    dr.CreatedOn = MDVUtility.ToDateTime(model.CreatedOn);
                    dr.ModifiedOn = MDVUtility.ToDateTime(model.ModifiedOn);

                    dr.BirthHxDate = MDVUtility.ToDateTime(model.CreatedOn);
                }
                dr.BirthHxId = -1;
                #region Database Insertion
                dsBirthHx.BirthHx.AddBirthHxRow(dr);
                BLObject<DSBirthHistory> obj = BLLClinicalObj.insertBirthHx(dsBirthHx);
                dsBirthHx = obj.Data;
                BirthHxResponseModel respModel = new BirthHxResponseModel();
                if (obj.Data != null)
                {
                    Int64 birthHxId = MDVUtility.ToInt64(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows[0][dsBirthHx.BirthHx.BirthHxIdColumn.ColumnName]);
                    if (birthHxId > 0 && !model.BirthHxUnremarkable)
                    {
                        if (!string.IsNullOrEmpty(model.IsGeneralUpdate) && model.IsGeneralUpdate.Equals("true") && generalObj != null)
                        {
                            respModel = insertUpdateGeneral(birthHxId, generalObj, respModel, model.PatientId);
                        }
                        if (!string.IsNullOrEmpty(model.IsDeliveryUpdate) && model.IsDeliveryUpdate.Equals("true") && maternalDeliveryObj != null)
                        {
                            respModel = insertUpdateMaternalDelivery(birthHxId, maternalDeliveryObj, respModel, model.PatientId);
                        }
                        if (!string.IsNullOrEmpty(model.IsNewbornUpdate) && model.IsNewbornUpdate.Equals("true") && newbornObj != null)
                        {
                            respModel = insertUpdateNewborn(birthHxId, newbornObj, respModel,model.PatientId);
                        }

                    }
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Birth Hx in Insert mode
                       Created Date: Jan 07, 2016
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForBirthHX(birthHxId);


                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;

                    var SoapInfo = getCurrentSoapText(birthHxId);

                    if (SoapInfo != null)
                    {
                        SoapText = SoapInfo["SoapText"];
                        IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                        LastUpdated = SoapInfo["LastUpdated"];
                    }

                    var response = new
                    {
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated,
                        GeneralId = respModel.GeneralId,
                        MaternalDeliveryId = respModel.MaternalDeliveryId,
                        NewbornId = respModel.NewbornId,
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        BirthHxId = MDVUtility.ToInt64(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows[0][dsBirthHx.BirthHx.BirthHxIdColumn.ColumnName])
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


        #endregion

        #region insertUpdateGeneral

        // Author:  Muhammad Azhar Shahzad
        // Created Date: Jan 07, 2016
        //OverView: This function will handle insert/update of General for current BirthHx on basis of BirthHxId
        private BirthHxResponseModel insertUpdateGeneral(long birthHxId, BirthHxGeneralModel generalObj, BirthHxResponseModel respModel, long patientId)
        {
           
            #region General
            DSBirthHistory dsGeneral = new DSBirthHistory();

            Int32 currentGeneralId = MDVUtility.ToInt32(generalObj.GeneralId);
            currentGeneralId = currentGeneralId == 0 ? -1 : currentGeneralId;
            BLObject<DSBirthHistory> objGeneral = null;
            DSBirthHistory.BirthHx_GeneralRow RowGeneral = null;
            if (currentGeneralId > 0)
            {
                objGeneral = BLLClinicalObj.loadBirthHxGeneral(birthHxId, currentGeneralId);
                dsGeneral = objGeneral.Data;
                if (dsGeneral.BirthHx_General.Rows.Count > 0)
                {
                    RowGeneral = (DSBirthHistory.BirthHx_GeneralRow)dsGeneral.BirthHx_General.Rows[0];
                }
                else
                {
                    RowGeneral = dsGeneral.BirthHx_General.NewBirthHx_GeneralRow();
                    RowGeneral.GeneralId = -1;
                }
            }
            else
            {
                RowGeneral = dsGeneral.BirthHx_General.NewBirthHx_GeneralRow();
                RowGeneral.GeneralId = -1;
            }

            if (RowGeneral != null)
            {
                if (dsGeneral.BirthHx_General.Rows.Count < 1)
                {
                    RowGeneral.GeneralId = currentGeneralId;
                }
                RowGeneral.BirthHxId = birthHxId;

                if (!string.IsNullOrEmpty(generalObj.GeneralComments))
                {
                    RowGeneral[dsGeneral.BirthHx_General.CommentsColumn] = generalObj.GeneralComments;
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.CommentsColumn] = null;
                }



                if (!string.IsNullOrEmpty(generalObj.DateAdmitted))
                {
                    RowGeneral[dsGeneral.BirthHx_General.DateAdmittedColumn] = MDVUtility.ToDateTime(generalObj.DateAdmitted);
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.DateAdmittedColumn] = DBNull.Value;
                }


                if (!string.IsNullOrEmpty(generalObj.HospitalName))
                {
                    RowGeneral[dsGeneral.BirthHx_General.HospitalNameColumn] = generalObj.HospitalName;
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.HospitalNameColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(generalObj.LengthStayatHospital))
                {
                    RowGeneral[dsGeneral.BirthHx_General.LengthStayatHospitalColumn] = generalObj.LengthStayatHospital;
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.LengthStayatHospitalColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(generalObj.ObstetricianName))
                {
                    RowGeneral[dsGeneral.BirthHx_General.ObstetricianNameColumn] = generalObj.ObstetricianName;
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.ObstetricianNameColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(generalObj.PatientDOB))
                {
                    RowGeneral[dsGeneral.BirthHx_General.PatientDOBColumn] = MDVUtility.ToDateTime(generalObj.PatientDOB);
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.PatientDOBColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(generalObj.PediatricianName))
                {
                    RowGeneral[dsGeneral.BirthHx_General.PediatricianNameColumn] = generalObj.PediatricianName;
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.PediatricianNameColumn] = DBNull.Value;
                }


                if (generalObj.ResponsiblePhysicianId > 0 && generalObj.ResponsiblePhysicianId != null)
                {
                    RowGeneral[dsGeneral.BirthHx_General.ResponsiblePhysicianIdColumn] = MDVUtility.ToInt16(generalObj.ResponsiblePhysicianId);
                }
                else
                {
                    RowGeneral[dsGeneral.BirthHx_General.ResponsiblePhysicianIdColumn] = DBNull.Value;
                }
                //else
                //{
                //    RowGeneral[dsGeneral.BirthHx_General.ResponsiblePhysicianId] = DBNull.Value;
                //}

                var date = DateTime.Now;
                RowGeneral.IsActive = true;


                if (generalObj.AddFromMobile != "1")
                {
                    if (RowGeneral.GeneralId == -1)
                    {
                        RowGeneral.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowGeneral.CreatedOn = date;
                    }


                    RowGeneral.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowGeneral.ModifiedOn = date;
					RowGeneral.PatientId = patientId;
                }
                else
                {
                    if (RowGeneral.GeneralId == -1)
                    {
                        RowGeneral.CreatedBy = generalObj.CreatedBy;
                        RowGeneral.CreatedOn = MDVUtility.ToDateTime(generalObj.CreatedOn);
                    }


                
                    RowGeneral.ModifiedBy = generalObj.ModifiedBy;
                    RowGeneral.ModifiedOn = MDVUtility.ToDateTime(generalObj.ModifiedOn);

                    RowGeneral.AddFromMobile = generalObj.AddFromMobile;

                    RowGeneral.ColumnsUpdatedFromMobileApp= string.Join(",", generalObj.lstChangedColumns.Select(i => i.columnName));

                }



                //if (RowGeneral.GeneralId == -1)
                //{
                //    RowGeneral.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //    RowGeneral.CreatedOn = date;
                //}


                //RowGeneral.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //RowGeneral.ModifiedOn = date;



                //// if no General is found against GeneralId, it implies for new record
                if (dsGeneral.BirthHx_General.Rows.Count < 1)
                {
                    dsGeneral.BirthHx_General.AddBirthHx_GeneralRow(RowGeneral);
                }

            }


            #region Database Insertion/Updation

            BLObject<DSBirthHistory> objInsertedGeneral = new BLObject<DSBirthHistory>();
            if (currentGeneralId > 0)
            {
                // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
                foreach (DataRow RowGeneralObj in dsGeneral.BirthHx_General.Rows)
                {
                    RowGeneralObj[dsGeneral.BirthHx_General.SoapTextColumn] = insertUpdateGeneralSoapText(dsGeneral, generalObj);
                }
                objInsertedGeneral = BLLClinicalObj.updateBirthHxGeneral(dsGeneral);
            }
            else
            {
                objInsertedGeneral = BLLClinicalObj.insertBirthHxGeneral(dsGeneral);
                dsGeneral = objInsertedGeneral.Data;
                foreach (DataRow RowGeneralObj in dsGeneral.BirthHx_General.Rows)
                {
                    RowGeneralObj[dsGeneral.BirthHx_General.SoapTextColumn] = insertUpdateGeneralSoapText(dsGeneral, generalObj);
                }
                objInsertedGeneral = BLLClinicalObj.updateBirthHxGeneral(dsGeneral);

            }
            if (objInsertedGeneral.Data != null)
            {
                DSBirthHistory dsMaternalDeliveryresp = objInsertedGeneral.Data;
                respModel.GeneralId = MDVUtility.ToInt64(dsMaternalDeliveryresp.BirthHx_General.Rows[0][dsMaternalDeliveryresp.BirthHx_General.GeneralIdColumn]);
                respModel.status = true;
                respModel.Message = Common.AppPrivileges.Save_Message;
            }
            else
            {
                respModel.status = false;
                respModel.Message = objInsertedGeneral.Message;
            }

            #endregion


            #endregion
            return respModel;
        }

        #endregion

        #region insertUpdateMaternalDelivery

        // Author:  Muhammad Azhar Shahzad
        // Created Date: Jan 07, 2016
        //OverView: This function will handle insert/update of MaternalDelivery for current BirthHx on basis of BirthHxId
        private BirthHxResponseModel insertUpdateMaternalDelivery(long birthHxId, BirthHxMaternalDeliveryModel maternalDeliveryObj, BirthHxResponseModel respModel, long patientId)
        {
            #region MaternalDelivery
            DSBirthHistory dsMaternalDelivery = new DSBirthHistory();

            Int32 currentMaternalDeliveryId = MDVUtility.ToInt32(maternalDeliveryObj.MaternalDeliveryId);
            currentMaternalDeliveryId = currentMaternalDeliveryId == 0 ? -1 : currentMaternalDeliveryId;
            BLObject<DSBirthHistory> objMaternalDelivery = null;
            DSBirthHistory.BirthHx_MaternalDeliveryRow RowMaternalDelivery = null;


            if (currentMaternalDeliveryId > 0)
            {
                objMaternalDelivery = BLLClinicalObj.loadBirthHxMaternalDelivery(birthHxId, currentMaternalDeliveryId);
                dsMaternalDelivery = objMaternalDelivery.Data;
                if (dsMaternalDelivery.BirthHx_MaternalDelivery.Rows.Count > 0)
                {
                    RowMaternalDelivery = (DSBirthHistory.BirthHx_MaternalDeliveryRow)dsMaternalDelivery.BirthHx_MaternalDelivery.Rows[0];
                }
                else
                {
                    RowMaternalDelivery = dsMaternalDelivery.BirthHx_MaternalDelivery.NewBirthHx_MaternalDeliveryRow();
                    RowMaternalDelivery.MaternalDeliveryId = -1;
                }
            }
            else
            {
                RowMaternalDelivery = dsMaternalDelivery.BirthHx_MaternalDelivery.NewBirthHx_MaternalDeliveryRow();
                RowMaternalDelivery.MaternalDeliveryId = -1;
            }

            if (RowMaternalDelivery != null)
            {
                if (dsMaternalDelivery.BirthHx_MaternalDelivery.Rows.Count < 1)
                {
                    RowMaternalDelivery.MaternalDeliveryId = currentMaternalDeliveryId;
                }
                RowMaternalDelivery.BirthHxId = birthHxId;

                if (!string.IsNullOrEmpty(maternalDeliveryObj.MaternalDeliveryComments))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.CommentsColumn] = maternalDeliveryObj.MaternalDeliveryComments;
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.CommentsColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(maternalDeliveryObj.Gestation))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.GestationColumn] = maternalDeliveryObj.Gestation;
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.GestationColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(maternalDeliveryObj.LaborLength))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.LaborLengthColumn] = maternalDeliveryObj.LaborLength;
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.LaborLengthColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(maternalDeliveryObj.NumberOfFetuses))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.NumberOfFetusesColumn] = maternalDeliveryObj.NumberOfFetuses;
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.NumberOfFetusesColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(maternalDeliveryObj.NumberOfLivingFetuses))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.NumberOfLivingFetusesColumn] = maternalDeliveryObj.NumberOfLivingFetuses;
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.NumberOfLivingFetusesColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(maternalDeliveryObj.NumberOfFetuses))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.NumberOfFetusesColumn] = maternalDeliveryObj.NumberOfFetuses;
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.NumberOfFetusesColumn] = DBNull.Value;
                }

                if (maternalDeliveryObj.DeliveryMethodId > 0 && maternalDeliveryObj.DeliveryMethodId != null)
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.DeliveryMethodIdColumn] = MDVUtility.ToInt16(maternalDeliveryObj.DeliveryMethodId);
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.DeliveryMethodIdColumn] = DBNull.Value;
                }



                if (maternalDeliveryObj.DeliveryPresentationId > 0 && maternalDeliveryObj.DeliveryPresentationId != null)
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.DeliveryPresentationIdColumn] = MDVUtility.ToInt16(maternalDeliveryObj.DeliveryPresentationId);
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.DeliveryPresentationIdColumn] = DBNull.Value;
                }

                if (maternalDeliveryObj.MaternalHistoryId > 0 && maternalDeliveryObj.MaternalHistoryId != null)
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.MaternalHistoryIdColumn] = MDVUtility.ToInt16(maternalDeliveryObj.MaternalHistoryId);
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.MaternalHistoryIdColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(maternalDeliveryObj.MaternalDeliveryComments))
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.CommentsColumn] = MDVUtility.ToStr(maternalDeliveryObj.MaternalDeliveryComments);
                }
                else
                {
                    RowMaternalDelivery[dsMaternalDelivery.BirthHx_MaternalDelivery.CommentsColumn] = DBNull.Value;
                }
                RowMaternalDelivery.IsActive = true;

                if (maternalDeliveryObj.AddFromMobile != "1")
                {
                    if (dsMaternalDelivery.BirthHx_MaternalDelivery.Rows.Count < 1)
                    {
                        RowMaternalDelivery.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                        RowMaternalDelivery.CreatedOn = DateTime.Now;
                    }


                    RowMaternalDelivery.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowMaternalDelivery.ModifiedOn = DateTime.Now;
					RowMaternalDelivery.PatientId = patientId;
                  


                }
                else
                {
                    if (dsMaternalDelivery.BirthHx_MaternalDelivery.Rows.Count < 1)
                    {
                        RowMaternalDelivery.CreatedBy = maternalDeliveryObj.CreatedBy;
                        RowMaternalDelivery.CreatedOn = MDVUtility.ToDateTime(maternalDeliveryObj.CreatedOn);
                    }



                    RowMaternalDelivery.ModifiedBy = maternalDeliveryObj.ModifiedBy;
                    RowMaternalDelivery.ModifiedOn = MDVUtility.ToDateTime(maternalDeliveryObj.ModifiedOn);

                    RowMaternalDelivery.AddFromMobile = maternalDeliveryObj.AddFromMobile;

                    RowMaternalDelivery.ColumnsUpdatedFromMobileApp = string.Join(",", maternalDeliveryObj.lstChangedColumns.Select(i => i.columnName));



                }




                //if (dsMaternalDelivery.BirthHx_MaternalDelivery.Rows.Count < 1)
                //{
                //    RowMaternalDelivery.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //    RowMaternalDelivery.CreatedOn = DateTime.Now;
                //}

                //RowMaternalDelivery.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                //RowMaternalDelivery.ModifiedOn = DateTime.Now;

                // if no MaternalDelivery is found against MaternalDeliveryId, it implies for new record
                if (dsMaternalDelivery.BirthHx_MaternalDelivery.Rows.Count < 1)
                {
                    dsMaternalDelivery.BirthHx_MaternalDelivery.AddBirthHx_MaternalDeliveryRow(RowMaternalDelivery);
                }
            }




            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowMaternalDeliveryObj in dsMaternalDelivery.BirthHx_MaternalDelivery.Rows)
            {
                RowMaternalDeliveryObj[dsMaternalDelivery.BirthHx_MaternalDelivery.SoapTextColumn] = insertUpdateMaternalDeliverySoapText(dsMaternalDelivery, maternalDeliveryObj);

            }
            #region Database Insertion/Updation
            BLObject<DSBirthHistory> objInsertedMaternalDelivery = null;
            if (currentMaternalDeliveryId > 0)
            {
                objInsertedMaternalDelivery = BLLClinicalObj.updateBirthHxMaternalDelivery(dsMaternalDelivery);
            }
            else
            {
                objInsertedMaternalDelivery = BLLClinicalObj.insertBirthHxMaternalDelivery(dsMaternalDelivery);
            }

            if (objInsertedMaternalDelivery.Data != null)
            {
                DSBirthHistory dsMaternalDeliveryresp = objInsertedMaternalDelivery.Data;
                respModel.MaternalDeliveryId = MDVUtility.ToInt64(dsMaternalDeliveryresp.BirthHx_MaternalDelivery.Rows[0][dsMaternalDeliveryresp.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn]);
                respModel.status = true;
                respModel.Message = Common.AppPrivileges.Save_Message;
            }
            else
            {
                respModel.status = false;
                respModel.Message = objInsertedMaternalDelivery.Message;
            }

            #endregion


            #endregion
            return respModel;
        }

        #endregion

        #region insertUpdateNewborn

        // Author:  Muhammad Azhar Shahzad
        // Created Date: Jan 07, 2016
        //OverView: This function will handle insert/update of Newborn for current BirthHx on basis of BirthHxId
        private BirthHxResponseModel insertUpdateNewborn(long birthHxId, BirthHxNewbornModel newbornObj, BirthHxResponseModel respModel, long patientId)
        {
            #region Newborn
            DSBirthHistory dsNewborn = new DSBirthHistory();


            Int32 currentNewbornId = MDVUtility.ToInt32(newbornObj.NewbornId);
            currentNewbornId = currentNewbornId == 0 ? -1 : currentNewbornId;
            BLObject<DSBirthHistory> objNewborn = null;
            DSBirthHistory.BirthHx_NewbornRow RowNewborn = null;
            if (currentNewbornId > 0)
            {
                objNewborn = BLLClinicalObj.loadBirthHistoryNewBorn(birthHxId, currentNewbornId);
                dsNewborn = objNewborn.Data;
                if (dsNewborn.BirthHx_Newborn.Rows.Count > 0)
                {
                    RowNewborn = (DSBirthHistory.BirthHx_NewbornRow)dsNewborn.BirthHx_Newborn.Rows[0];
                }
                else
                {
                    RowNewborn = dsNewborn.BirthHx_Newborn.NewBirthHx_NewbornRow();
                    RowNewborn.NewbornId = -1;
                }
            }
            else
            {
                RowNewborn = dsNewborn.BirthHx_Newborn.NewBirthHx_NewbornRow();
                RowNewborn.NewbornId = -1;
            }

            if (RowNewborn != null)
            {
                if (dsNewborn.BirthHx_Newborn.Rows.Count < 1)
                {
                    RowNewborn.NewbornId = currentNewbornId;
                }
                RowNewborn.BirthHxId = birthHxId;



                if (!string.IsNullOrEmpty(newbornObj.ApgarAt5Minutes))
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ApgarAt5MinutesColumn] = MDVUtility.ToStr(newbornObj.ApgarAt5Minutes);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ApgarAt5MinutesColumn] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(newbornObj.ApgarAtBirth))
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ApgarAtBirthColumn] = MDVUtility.ToStr(newbornObj.ApgarAtBirth);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ApgarAtBirthColumn] = DBNull.Value;
                }

                if (newbornObj.bFetalDistressYes || newbornObj.bFetalDistressNo)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.bFetalDistressColumn] = newbornObj.bFetalDistressYes ? true : false;
                }
                else
                {
                    RowNewborn.SetbFetalDistressNull();
                }

                if (newbornObj.ChestCircumference > 0 && newbornObj.ChestCircumference != null)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ChestCircumferenceColumn] = MDVUtility.Tofloat(newbornObj.ChestCircumference);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ChestCircumferenceColumn] = DBNull.Value;
                }

                if (newbornObj.HeadCircumference > 0)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.HeadCircumferenceColumn] = MDVUtility.Tofloat(newbornObj.HeadCircumference);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.HeadCircumferenceColumn] = DBNull.Value;
                }

                if (newbornObj.LengthAtBirth > 0)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.LengthAtBirthColumn] = MDVUtility.Tofloat(newbornObj.LengthAtBirth);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.LengthAtBirthColumn] = DBNull.Value;
                }

                if (newbornObj.PatientBloodTypeId > 0 && newbornObj.PatientBloodTypeId != null)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.PatientBloodTypeIdColumn] = MDVUtility.ToInt16(newbornObj.PatientBloodTypeId);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.PatientBloodTypeIdColumn] = DBNull.Value;
                }


                if (newbornObj.ProblemsAtBirthId > 0 && newbornObj.ProblemsAtBirthId != null)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ProblemsAtBirthIdColumn] = MDVUtility.ToInt16(newbornObj.ProblemsAtBirthId);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.ProblemsAtBirthIdColumn] = DBNull.Value;
                }

                if (newbornObj.WeightAtBirth > 0)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.WeightAtBirthColumn] = MDVUtility.Tofloat(newbornObj.WeightAtBirth);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.WeightAtBirthColumn] = DBNull.Value;
                }
                if (newbornObj.WeightReleased > 0)
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.WeightReleasedColumn] = MDVUtility.Tofloat(newbornObj.WeightReleased);
                }
                else
                {
                    RowNewborn[dsNewborn.BirthHx_Newborn.WeightReleasedColumn] = DBNull.Value;
                }
            }


            if (!string.IsNullOrEmpty(newbornObj.NewbornComments))
            {
                RowNewborn.Comments = MDVUtility.ToStr(newbornObj.NewbornComments);
            }
            else
            {
                RowNewborn[dsNewborn.BirthHx_Newborn.CommentsColumn] = DBNull.Value;
            }
            RowNewborn.IsActive = true;

            if (newbornObj.AddFromMobile != "1")
            {
                if (dsNewborn.BirthHx_Newborn.Rows.Count < 1)
                {
                    RowNewborn.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    RowNewborn.CreatedOn = DateTime.Now;
                }


                RowNewborn.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                RowNewborn.ModifiedOn = DateTime.Now;
				RowNewborn.PatientId = patientId;
            }
            else
            {
                if (dsNewborn.BirthHx_Newborn.Rows.Count < 1)
                {
                    RowNewborn.CreatedBy = newbornObj.CreatedBy;
                    RowNewborn.CreatedOn = MDVUtility.ToDateTime(newbornObj.CreatedOn);
                }



                RowNewborn.ModifiedBy = newbornObj.ModifiedBy;
                RowNewborn.ModifiedOn = MDVUtility.ToDateTime(newbornObj.ModifiedOn);

                RowNewborn.AddFromMobile = newbornObj.AddFromMobile;

                RowNewborn.ColumnsUpdatedFromMobileApp = string.Join(",", newbornObj.lstChangedColumns.Select(i => i.columnName));



            }





            //if (dsNewborn.BirthHx_Newborn.Rows.Count < 1)
            //{
            //    RowNewborn.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            //    RowNewborn.CreatedOn = DateTime.Now;
            //}

            //RowNewborn.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
            //RowNewborn.ModifiedOn = DateTime.Now;

            ////// if no Newborn is found against NewbornId, it implies for new record
            if (dsNewborn.BirthHx_Newborn.Rows.Count < 1)
            {
                dsNewborn.BirthHx_Newborn.AddBirthHx_NewbornRow(RowNewborn);
            }





            // Azhar Added this code on dec 14 2015 on 4pm for Soap Text
            foreach (DataRow RowNewbornObj in dsNewborn.BirthHx_Newborn.Rows)
            {
                RowNewbornObj[dsNewborn.BirthHx_Newborn.SoapTextColumn] = insertUpdateNewbornSoapText(dsNewborn, newbornObj);

            }
            #region Database Insertion/Updation

            BLObject<DSBirthHistory> objInsertedNewborn = null;
            if (currentNewbornId > 0)
            {
                objInsertedNewborn = BLLClinicalObj.updateBirthHistoryNewBorn(dsNewborn);
            }
            else
            {
                objInsertedNewborn = BLLClinicalObj.insertBirthHistoryNewBorn(dsNewborn);
            }
            if (objInsertedNewborn.Data != null)
            {
                DSBirthHistory dsNewbornresp = objInsertedNewborn.Data;
                respModel.NewbornId = MDVUtility.ToInt64(dsNewbornresp.BirthHx_Newborn.Rows[0][dsNewbornresp.BirthHx_Newborn.NewbornIdColumn]);
                respModel.status = true;
                respModel.Message = Common.AppPrivileges.Save_Message;
            }
            else
            {
                respModel.status = false;
                respModel.Message = objInsertedNewborn.Message;
            }

            #endregion


            #endregion
            return respModel;
        }

        #endregion


        #region updateBirthHx
        public string updateBirthHx(BirthHxModel model, Int64 BirthHxId, BirthHxGeneralModel generalObj, BirthHxMaternalDeliveryModel maternalDeliveryObj, BirthHxNewbornModel newbornObj)
        {
            try
            {
                if (BirthHxId > 0)
                {

                    DSBirthHistory dsBirthHx = new DSBirthHistory();
                    BLObject<DSBirthHistory> obj = BLLClinicalObj.loadBirthHx(MDVUtility.ToInt64(model.PatientId), BirthHxId, false);
                    dsBirthHx = obj.Data;
                    foreach (DSBirthHistory.BirthHxRow dr in dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows)
                    {
                        dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                        if (!string.IsNullOrEmpty(model.BirthHxDate))
                        {
                            dr.BirthHxDate = MDVUtility.ToDateTime(model.BirthHxDate);
                        }
                        else
                        {
                            dr[dsBirthHx.BirthHx.BirthHxDateColumn] = DBNull.Value;
                        }

                        if (!string.IsNullOrEmpty(model.BirthHxComments))
                        {
                            dr.Comments = MDVUtility.ToStr(model.BirthHxComments);
                        }
                        else
                        {
                            dr[dsBirthHx.BirthHx.CommentsColumn] = DBNull.Value;
                        }

                        dr.bUnremarkable = model.BirthHxUnremarkable;

                  
                        if (model.AddFromMobile != "1")
                        {
                            dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                            dr.ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            dr.ModifiedBy = model.ModifiedBy;
                            dr.ModifiedOn = MDVUtility.ToDateTime(model.ModifiedOn);
                        }

                        // getting childs Id
                        if (!string.IsNullOrEmpty(model.IsGeneralUpdate) && model.IsGeneralUpdate.Equals("true") && generalObj != null && !dr.IsNull("GeneralId"))
                        {
                            generalObj.GeneralId = dr.GeneralId;
                        }
                        if (!string.IsNullOrEmpty(model.IsDeliveryUpdate) && model.IsDeliveryUpdate.Equals("true") && maternalDeliveryObj != null && !dr.IsNull("MaternalDeliveryId"))
                        {
                            maternalDeliveryObj.MaternalDeliveryId = dr.MaternalDeliveryId;
                        }
                        if (!string.IsNullOrEmpty(model.IsNewbornUpdate) && model.IsNewbornUpdate.Equals("true") && newbornObj != null && !dr.IsNull("NewbornId"))
                        {
                            newbornObj.NewbornId = dr.NewbornId;
                        }
                    }
                    BirthHxResponseModel respModel = new BirthHxResponseModel();
                    if (!model.BirthHxUnremarkable)
                    {
                        if (!string.IsNullOrEmpty(model.IsGeneralUpdate) && model.IsGeneralUpdate.Equals("true") && generalObj != null)
                        {
                            respModel = insertUpdateGeneral(BirthHxId, generalObj, respModel, model.PatientId);
                        }
                        if (!string.IsNullOrEmpty(model.IsDeliveryUpdate) && model.IsDeliveryUpdate.Equals("true") && maternalDeliveryObj != null)
                        {
                            respModel = insertUpdateMaternalDelivery(BirthHxId, maternalDeliveryObj, respModel, model.PatientId);
                        }
                        if (!string.IsNullOrEmpty(model.IsNewbornUpdate) && model.IsNewbornUpdate.Equals("true") && newbornObj != null)
                        {
                            respModel = insertUpdateNewborn(BirthHxId, newbornObj, respModel, model.PatientId);
                        }
                    }
                    #region Database Updation
                    if (dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows.Count > 0)
                    {
                        BLObject<DSBirthHistory> objUpdate = BLLClinicalObj.updateBirthHx(dsBirthHx);
                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;

                        var SoapInfo = getCurrentSoapText(BirthHxId);

                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }

                        if (objUpdate.Data != null)
                        {
                            var response = new
                            {
                                BirthHxId = model.BirthHxId,
                                status = true,
                                SoapText = SoapText,
                                IsCreatedOrModified = IsCreatedOrModified,
                                LastUpdated = LastUpdated,
                                GeneralId=respModel.GeneralId,
                                MaternalDeliveryId=respModel.MaternalDeliveryId,
                                NewbornId = respModel.NewbornId,
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
                            Message = "Birth History not found."
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
                        Message = "Birth History not found."
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

        #region fillBirthHx

        public string fillBirthHx(BirthHxModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(model.PatientId)) && model.BirthHxId == 0)
                {
                    var response = new
                    {
                        status = false,
                        Message = MDVUtility.ToStr(Common.AppPrivileges.Select_Message)
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
                else
                {
                    DSBirthHistory dsBirthHx = null;
                    BLObject<DSBirthHistory> obj = BLLClinicalObj.loadBirthHx(MDVUtility.ToInt64(model.PatientId), model.BirthHxId, true, "1", "");
                    dsBirthHx = obj.Data;
                    if (dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows.Count > 0)
                    {
                        DataRow dr = dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows[0];
                        BirthHxModel bhxObj = new BirthHxModel();

                        bhxObj.BirthHxDate = String.IsNullOrEmpty(MDVUtility.ToStr(dr[dsBirthHx.BirthHx.BirthHxDateColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(dr[dsBirthHx.BirthHx.BirthHxDateColumn.ColumnName]).ToShortDateString();
                        bhxObj.BirthHxId = MDVUtility.ToLong(dr[dsBirthHx.BirthHx.BirthHxIdColumn.ColumnName]);
                        bhxObj.BirthHxUnremarkable = MDVUtility.ToBool(dr[dsBirthHx.BirthHx.bUnremarkableColumn.ColumnName]);
                        bhxObj.BirthHxComments = MDVUtility.ToStr(dr[dsBirthHx.BirthHx.CommentsColumn.ColumnName]);
                        bhxObj.BirthHxSoapText = MDVUtility.ToStr(dr[dsBirthHx.BirthHx.SoapTextColumn.ColumnName]);

                        BirthHxGeneralModel bhxGeneralObj = new BirthHxGeneralModel();

                        foreach (DataRow drGeneral in dsBirthHx.Tables[dsBirthHx.BirthHx_General.TableName].Rows)
                        {

                            bhxGeneralObj.GeneralId = MDVUtility.ToLong(drGeneral[dsBirthHx.BirthHx_General.GeneralIdColumn.ColumnName]);
                            bhxGeneralObj.BirthHxId = MDVUtility.ToLong(drGeneral[dsBirthHx.BirthHx_General.BirthHxIdColumn.ColumnName]);
                            bhxGeneralObj.GeneralComments = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.CommentsColumn.ColumnName]);
                            bhxGeneralObj.DateAdmitted = String.IsNullOrEmpty(MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.DateAdmittedColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(drGeneral[dsBirthHx.BirthHx_General.DateAdmittedColumn.ColumnName]).ToShortDateString();
                            bhxGeneralObj.HospitalName = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.HospitalNameColumn.ColumnName]);
                            bhxGeneralObj.LengthStayatHospital = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.LengthStayatHospitalColumn.ColumnName]).Trim();
                            bhxGeneralObj.ObstetricianName = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.ObstetricianNameColumn.ColumnName]);
                            bhxGeneralObj.PatientDOB = String.IsNullOrEmpty(MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.PatientDOBColumn.ColumnName])) ? "" : MDVUtility.ToDateTime(drGeneral[dsBirthHx.BirthHx_General.PatientDOBColumn.ColumnName]).ToShortDateString();
                            bhxGeneralObj.PediatricianName = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.PediatricianNameColumn.ColumnName]);
                            bhxGeneralObj.ResponsiblePhysicianId = MDVUtility.ToInt32(drGeneral[dsBirthHx.BirthHx_General.ResponsiblePhysicianIdColumn.ColumnName]);
                            bhxGeneralObj.ResponsiblePhysicianId = bhxGeneralObj.ResponsiblePhysicianId == 0 ? null : bhxGeneralObj.ResponsiblePhysicianId;
                            bhxGeneralObj.ResponsiblePhysicianId_text = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.ResponsiblePhysicianNameColumn.ColumnName]);
                            bhxGeneralObj.SoapText = MDVUtility.ToStr(drGeneral[dsBirthHx.BirthHx_General.SoapTextColumn.ColumnName]);


                        }

                        BirthHxMaternalDeliveryModel bhxMaternalDeliveryObj = new BirthHxMaternalDeliveryModel();
                        foreach (DataRow drMaternalDelivery in dsBirthHx.Tables[dsBirthHx.BirthHx_MaternalDelivery.TableName].Rows)
                        {

                            bhxMaternalDeliveryObj.MaternalDeliveryId = MDVUtility.ToLong(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.MaternalDeliveryIdColumn.ColumnName]);
                            bhxMaternalDeliveryObj.BirthHxId = MDVUtility.ToLong(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.BirthHxIdColumn.ColumnName]);
                            bhxMaternalDeliveryObj.MaternalDeliveryComments = MDVUtility.ToStr(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.CommentsColumn.ColumnName]);
                            bhxMaternalDeliveryObj.DeliveryMethodId = MDVUtility.ToInt32(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.DeliveryMethodIdColumn.ColumnName]);
                            bhxMaternalDeliveryObj.DeliveryMethodId = bhxMaternalDeliveryObj.DeliveryMethodId == 0 ? null : bhxMaternalDeliveryObj.DeliveryMethodId;

                            bhxMaternalDeliveryObj.DeliveryPresentationId = MDVUtility.ToInt32(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.DeliveryPresentationIdColumn.ColumnName]);
                            bhxMaternalDeliveryObj.DeliveryPresentationId = bhxMaternalDeliveryObj.DeliveryPresentationId == 0 ? null : bhxMaternalDeliveryObj.DeliveryPresentationId;

                            bhxMaternalDeliveryObj.Gestation = MDVUtility.ToStr(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.GestationColumn.ColumnName]).Trim();
                            bhxMaternalDeliveryObj.LaborLength = MDVUtility.ToStr(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.LaborLengthColumn.ColumnName]).Trim();

                            bhxMaternalDeliveryObj.MaternalHistoryId = MDVUtility.ToInt32(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.MaternalHistoryIdColumn.ColumnName]);
                            bhxMaternalDeliveryObj.MaternalHistoryId = bhxMaternalDeliveryObj.MaternalHistoryId == 0 ? null : bhxMaternalDeliveryObj.MaternalHistoryId;

                            bhxMaternalDeliveryObj.NumberOfFetuses = MDVUtility.ToStr(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.NumberOfFetusesColumn.ColumnName]).Trim();
                            bhxMaternalDeliveryObj.NumberOfLivingFetuses = MDVUtility.ToStr(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.NumberOfLivingFetusesColumn.ColumnName]).Trim();
                            bhxMaternalDeliveryObj.SoapText = MDVUtility.ToStr(drMaternalDelivery[dsBirthHx.BirthHx_MaternalDelivery.SoapTextColumn.ColumnName]);

                        }
                        BirthHxNewbornModel bhxNewBornObj = new BirthHxNewbornModel();
                        foreach (DataRow drNewBorn in dsBirthHx.Tables[dsBirthHx.BirthHx_Newborn.TableName].Rows)
                        {

                            bhxNewBornObj.NewbornId = MDVUtility.ToLong(drNewBorn[dsBirthHx.BirthHx_Newborn.NewbornIdColumn.ColumnName]);
                            bhxNewBornObj.ApgarAt5Minutes = MDVUtility.ToStr(drNewBorn[dsBirthHx.BirthHx_Newborn.ApgarAt5MinutesColumn.ColumnName]);
                            bhxNewBornObj.ApgarAtBirth = MDVUtility.ToStr(drNewBorn[dsBirthHx.BirthHx_Newborn.ApgarAtBirthColumn.ColumnName]);
                            //bhxNewBornObj.bFetalDistress = Convert.ToBoolean(drNewBorn[dsBirthHx.BirthHx_Newborn.bFetalDistressColumn.ColumnName]);
                            if (!drNewBorn.IsNull("bFetalDistress"))
                            {
                                bhxNewBornObj.bFetalDistress = Convert.ToBoolean(drNewBorn[dsBirthHx.BirthHx_Newborn.bFetalDistressColumn.ColumnName]);
                                if (bhxNewBornObj.bFetalDistress == true)
                                {
                                    bhxNewBornObj.bFetalDistressYes = true;
                                    bhxNewBornObj.bFetalDistressNo = false;
                                }
                                else
                                {
                                    bhxNewBornObj.bFetalDistressYes = false;
                                    bhxNewBornObj.bFetalDistressNo = true;
                                }
                            }
                            else
                            {
                                bhxNewBornObj.bFetalDistress = null;
                            }


                            bhxNewBornObj.BirthHxId = MDVUtility.ToLong(drNewBorn[dsBirthHx.BirthHx_Newborn.BirthHxIdColumn.ColumnName]);
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.ChestCircumferenceColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.ChestCircumference = MDVUtility.Tofloat(drNewBorn[dsBirthHx.BirthHx_Newborn.ChestCircumferenceColumn.ColumnName]);
                                bhxNewBornObj.ChestCircumference = bhxNewBornObj.ChestCircumference == 0.0 ? null : bhxNewBornObj.ChestCircumference;
                            }
                            bhxNewBornObj.NewbornComments = MDVUtility.ToStr(drNewBorn[dsBirthHx.BirthHx_Newborn.CommentsColumn.ColumnName]);
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.HeadCircumferenceColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.HeadCircumference = MDVUtility.Tofloat(drNewBorn[dsBirthHx.BirthHx_Newborn.HeadCircumferenceColumn.ColumnName]);
                                bhxNewBornObj.HeadCircumference = bhxNewBornObj.HeadCircumference == 0.0 ? null : bhxNewBornObj.HeadCircumference;
                            }
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.LengthAtBirthColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.LengthAtBirth = MDVUtility.Tofloat(drNewBorn[dsBirthHx.BirthHx_Newborn.LengthAtBirthColumn.ColumnName]);
                                bhxNewBornObj.LengthAtBirth = bhxNewBornObj.LengthAtBirth == 0.0 ? null : bhxNewBornObj.LengthAtBirth;
                            }
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.PatientBloodTypeIdColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.PatientBloodTypeId = MDVUtility.ToInt32(drNewBorn[dsBirthHx.BirthHx_Newborn.PatientBloodTypeIdColumn.ColumnName]);
                                bhxNewBornObj.PatientBloodTypeId = bhxNewBornObj.PatientBloodTypeId == 0 ? null : bhxNewBornObj.PatientBloodTypeId;
                            }
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.ProblemsAtBirthIdColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.ProblemsAtBirthId = MDVUtility.ToInt32(drNewBorn[dsBirthHx.BirthHx_Newborn.ProblemsAtBirthIdColumn.ColumnName]);
                                bhxNewBornObj.ProblemsAtBirthId = bhxNewBornObj.ProblemsAtBirthId == 0 ? null : bhxNewBornObj.ProblemsAtBirthId;
                            }
                            bhxNewBornObj.SoapText = MDVUtility.ToStr(drNewBorn[dsBirthHx.BirthHx_Newborn.SoapTextColumn.ColumnName]);
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.WeightAtBirthColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.WeightAtBirth = MDVUtility.Tofloat(drNewBorn[dsBirthHx.BirthHx_Newborn.WeightAtBirthColumn.ColumnName]);
                                bhxNewBornObj.WeightAtBirth = bhxNewBornObj.WeightAtBirth == 0.0 ? null : bhxNewBornObj.WeightAtBirth;
                            }
                            if (drNewBorn[dsBirthHx.BirthHx_Newborn.WeightReleasedColumn.ColumnName] != DBNull.Value)
                            {
                                bhxNewBornObj.WeightReleased = MDVUtility.Tofloat(drNewBorn[dsBirthHx.BirthHx_Newborn.WeightReleasedColumn.ColumnName]);
                                bhxNewBornObj.WeightReleased = bhxNewBornObj.WeightReleased == 0.0 ? null : bhxNewBornObj.WeightReleased;
                            }


                        }



                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var SoapText = string.Empty;
                        var IsCreatedOrModified = string.Empty;
                        var LastUpdated = string.Empty;

                        var SoapInfo = getCurrentSoapText(bhxObj.BirthHxId);

                        if (SoapInfo != null)
                        {
                            SoapText = SoapInfo["SoapText"];
                            IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                            LastUpdated = SoapInfo["LastUpdated"];
                        }

                        var response = new
                        {
                            BirthHxId = bhxObj.BirthHxId,
                            status = true,
                            BirthHxFill_JSON = js.Serialize(bhxObj),
                            SoapText = SoapText,
                            IsCreatedOrModified = IsCreatedOrModified,
                            LastUpdated = LastUpdated,
                            GeneralHxFill_JSON = js.Serialize(bhxGeneralObj),
                            MaternalDeliveryHxFill_JSON = js.Serialize(bhxMaternalDeliveryObj),
                            NewBornFill_JSON = js.Serialize(bhxNewBornObj),
                            BirthHxLoad_JSON = MDVUtility.JSON_DataTable(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName]),
                        };
                        return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var response = new
                        {
                            status = true,
                            BirthHxFill_JSON = js.Serialize(new BirthHxModel()),
                            GeneralHxFill_JSON = js.Serialize(new BirthHxGeneralModel()),
                            MaternalDeliveryHxFill_JSON = js.Serialize(new BirthHxMaternalDeliveryModel()),
                            NewBornFill_JSON = js.Serialize(new BirthHxNewbornModel()),
                            BirthHxLoad_JSON = MDVUtility.JSON_DataTable(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName]),
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


        public Dictionary<string, string> getCurrentSoapText(Int64 BirthHxId)
        {
            try
            {
                DSHistorySummary dsHistorySummarySoap = null;
                BLObject<DSHistorySummary> objSummary;
                objSummary = BLLClinicalObj.loadHxLog(BirthHxId, "BirthHx", "Current", 1, 10);
                dsHistorySummarySoap = objSummary.Data;

                var SoapText = string.Empty;
                var IsCreatedOrModified = string.Empty;
                var LastUpdated = string.Empty;

                if (dsHistorySummarySoap != null && dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows.Count > 0)
                {
                    var Hxdr = dsHistorySummarySoap.Tables[dsHistorySummarySoap.HxLog.TableName].Rows[0];
                    SoapText = MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.SoapTextColumn.ColumnName]);
                    IsCreatedOrModified = MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.ActionColumn.ColumnName]);
                    LastUpdated = string.Concat(MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.ModifiedOnColumn.ColumnName]), " ", MDVUtility.ToStr(Hxdr[dsHistorySummarySoap.HxLog.ModifiedByColumn.ColumnName]));
                }

                var response = new Dictionary<string, string>
                {
                   {"SoapText", SoapText},
                   {"IsCreatedOrModified" , IsCreatedOrModified},
                   {"LastUpdated" , LastUpdated},
                   {"BirthHxId" ,MDVUtility.ToStr( BirthHxId)}
                };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Soap Text for Birth History
        internal string insertUpdateGeneralSoapText(DSBirthHistory dsBirthHistory, BirthHxGeneralModel modelObj)
        {
            //dsGeneral.BirthHx_General
            string GeneralSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            //Start || 3 July, 2016 || Talha Tanweer || To change responsible physician name
            var responsiblePhysician = new
            {
                FirstName = modelObj.ResponsiblePhysicianId_text.Split(',').Length > 1 ? modelObj.ResponsiblePhysicianId_text.Split(',')[1].Trim() : "",
                LastName = modelObj.ResponsiblePhysicianId_text.Split(',').Length > 0 ? modelObj.ResponsiblePhysicianId_text.Split(',')[0].Trim() : "",
            };
            //End   || 3 July, 2016 || Talha Tanweer || To change responsible physician name


            if (modelObj != null)
            {

                //sb.Append("<div id='BirthGeneral_" + modelObj.GeneralId + "' title='General'  name='Birth Hx'><strong>General: </strong>");
                sb.Append((string.IsNullOrEmpty(modelObj.HospitalName) ? "" : "The patient was born at " + modelObj.HospitalName + "."));//kr
                sb.Append((string.IsNullOrEmpty(modelObj.PatientDOB.ToString()) ? "" : " Patient's Date of Birth is " + MDVUtility.GetDateMMDDYYY(modelObj.PatientDOB) + "."));//kr
                sb.Append((string.IsNullOrEmpty(modelObj.LengthStayatHospital) ? "" : " Length of Stay at the Hospital was " + modelObj.LengthStayatHospital + " days."));//kr
                sb.Append((string.IsNullOrEmpty(modelObj.DateAdmitted.ToString()) ? "" : " Date of Admission was " + MDVUtility.GetDateMMDDYYY(modelObj.DateAdmitted) + "."));//kr
                sb.Append((string.IsNullOrEmpty(modelObj.ObstetricianName) ? "" : " The Obstetrician was  " + modelObj.ObstetricianName + "."));//kr
                sb.Append((string.IsNullOrEmpty(modelObj.PediatricianName) ? "" : " The Pediatrician was  " + modelObj.PediatricianName + "."));//kr
                sb.Append(((string.IsNullOrEmpty(modelObj.ResponsiblePhysicianId_text) || modelObj.ResponsiblePhysicianId <= 0) ? "" : " The Responsible Physician was  " + responsiblePhysician.LastName + " " + responsiblePhysician.FirstName + ". "));//kr
                sb.Append((string.IsNullOrEmpty(modelObj.GeneralComments) ? "" : "Comments: " + modelObj.GeneralComments));
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Insert(0, "<div id='BirthGeneral_" + modelObj.GeneralId + "' title='General'  name='Birth Hx'><strong>General: </strong>");
                    sb.Append("</div>");
                }
            }
            else
                if (dsBirthHistory.BirthHx_General != null && dsBirthHistory.BirthHx_General.Rows.Count > 0)
                {
                    foreach (var item in dsBirthHistory.BirthHx_General)
                    {
                        //sb.Append("<div id='BirthGeneral_" + item.GeneralId + "' title='General'  name='Birth Hx'><strong>General: </strong>");
                        sb.Append((string.IsNullOrEmpty(item.HospitalName) ? "" : "The patient was born at " + item.HospitalName + "."));//kr

                        sb.Append((string.IsNullOrEmpty(item.PatientDOB.ToString()) ? "" : " Patient's Date of Birth is " + item.PatientDOB + "."));//kr
                        sb.Append((string.IsNullOrEmpty(item.LengthStayatHospital) ? "" : " Length of Stay at the Hospital was " + item.LengthStayatHospital + " days."));//kr
                        sb.Append((string.IsNullOrEmpty(item.DateAdmitted.ToString()) ? "" : " Date of Admission was " + item.DateAdmitted + "."));//kr
                        sb.Append((string.IsNullOrEmpty(item.ObstetricianName) ? "" : " The Obstetrician was  " + item.ObstetricianName + "."));//kr
                        sb.Append((string.IsNullOrEmpty(item.PediatricianName) ? "" : " The Pediatrician was  " + item.PediatricianName + "."));//kr
                        sb.Append((string.IsNullOrEmpty(item.ResponsiblePhysicianName) ? "" : " The Responsible Physician was  " + item.ResponsiblePhysicianName + ". "));
                        sb.Append((string.IsNullOrEmpty(item.Comments) ? "" : "Comments: " + item.Comments));
                        if (!string.IsNullOrEmpty(sb.ToString()))
                        {
                            sb.Insert(0, "<div id='BirthGeneral_" + item.GeneralId + "' title='General'  name='Birth Hx'><strong>General: </strong>");
                            sb.Append("</div>");
                        }
                    }
                }

                else
                {
                    return string.Empty;
                }
            string returnResult = sb.ToString();
            if (!string.IsNullOrEmpty(returnResult) && returnResult.Substring(returnResult.Length - 7).Equals(",</div>"))
            {
                returnResult = returnResult.Remove(returnResult.Length - 7);
                returnResult += "</div>";
            }
            return returnResult;
        }

        internal string insertUpdateNewbornSoapText(DSBirthHistory dsBirthHistory, BirthHxNewbornModel modelObj)
        {
            string NewbornSoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {

                //   sb.Append("<div id='BirthNewborn_" + modelObj.NewbornId + "' title='Newborn'  name='Birth Hx'><strong>Newborn Information: </strong>");
                sb.Append(((modelObj.HeadCircumference <= 0 || modelObj.HeadCircumference == null) ? "" : "Head Circumference:  " + modelObj.HeadCircumference + " inches,"));
                sb.Append(((modelObj.ChestCircumference <= 0 || modelObj.ChestCircumference == null) ? "" : " Chest Circumference: " + modelObj.ChestCircumference + " inches,"));
                sb.Append(((modelObj.WeightAtBirth <= 0 || modelObj.WeightAtBirth == null) ? "" : " Weight at Birth: " + modelObj.WeightAtBirth + " lbs.,"));
                sb.Append(((modelObj.LengthAtBirth <= 0 || modelObj.LengthAtBirth == null) ? "" : " Length at Birth: " + modelObj.LengthAtBirth + " inches,"));
                sb.Append((string.IsNullOrEmpty(modelObj.ApgarAtBirth) ? "" : " Apgar at Birth: " + modelObj.ApgarAtBirth + ","));
                sb.Append((string.IsNullOrEmpty(modelObj.ApgarAt5Minutes) ? "" : " Apgar at 5 Minutes:  " + modelObj.ApgarAt5Minutes + ","));
                sb.Append(((modelObj.WeightReleased <= 0 || modelObj.WeightReleased == null) ? "" : " Weight Released: " + modelObj.WeightReleased + " lbs.,"));
                //sb.Append((string.IsNullOrEmpty(modelObj.ApgarAtBirth) ? "" : " Apgar at Birth: " + modelObj.ApgarAtBirth + ","));
                sb.Append(((string.IsNullOrEmpty(modelObj.PatientBloodTypeId_text) || modelObj.PatientBloodTypeId <= 0) ? "" : " Patient's Blood Type: " + modelObj.PatientBloodTypeId_text + ","));
                sb.Append(((string.IsNullOrEmpty(modelObj.ProblemsAtBirthId_text) || modelObj.ProblemsAtBirthId <= 0) ? "" : " Problems at Birth: " + modelObj.ProblemsAtBirthId_text + ","));
                sb.Append(((modelObj.bFetalDistressYes || modelObj.bFetalDistressNo) ? " Fetal Distress: " + ((modelObj.bFetalDistressYes) ? "Present," : "Absent,") : ""));
                sb.Append((string.IsNullOrEmpty(modelObj.NewbornComments) ? "" : "Comments: " + modelObj.NewbornComments));
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Insert(0, "<div id='BirthNewborn_" + modelObj.NewbornId + "' title='Newborn Information'  name='Birth Hx'><strong>Newborn Information: </strong>");
                    sb.Append("</div>");
                }
            }
            else
                if (dsBirthHistory.BirthHx_Newborn != null && dsBirthHistory.BirthHx_Newborn.Rows.Count > 0)
                {
                    foreach (var item in dsBirthHistory.BirthHx_Newborn)
                    {
                        // sb.Append("<div id='BirthNewborn_" + item.NewbornId + "' title='Newborn'  name='Birth Hx'><strong>Newborn Information: </strong>");
                        sb.Append(((item.HeadCircumference <= 0 || item.IsHeadCircumferenceNull()) ? "" : "Head Circumference:  " + item.HeadCircumference + " inches,"));
                        sb.Append(((item.ChestCircumference <= 0 || item.IsChestCircumferenceNull()) ? "" : " Chest Circumference: " + item.ChestCircumference + " inches,"));
                        sb.Append(((item.WeightAtBirth <= 0 || item.IsWeightAtBirthNull()) ? "" : " Weight at Birth: " + item.WeightAtBirth + " lbs.,"));
                        sb.Append(((item.LengthAtBirth <= 0 || item.IsLengthAtBirthNull()) ? "" : " Length at Birth:" + item.LengthAtBirth + " inches,"));
                        sb.Append((string.IsNullOrEmpty(item.ApgarAtBirth) ? "" : " Apgar at Birth: " + item.ApgarAtBirth + ","));
                        sb.Append((string.IsNullOrEmpty(item.ApgarAt5Minutes) ? "" : " Apgar at 5 Minutes:  " + item.ApgarAt5Minutes + ","));
                        sb.Append(((item.WeightReleased <= 0 || item.IsWeightReleasedNull()) ? "" : " Weight Released:  " + item.WeightReleased + " lbs.,"));
                        //sb.Append((string.IsNullOrEmpty(item.ApgarAtBirth) ? "" : " Apgar at Birth: " + item.ApgarAtBirth + ","));
                        sb.Append((string.IsNullOrEmpty(item.PatientBloodType) ? "" : " Patient's Blood Type: " + item.PatientBloodType + ","));
                        sb.Append((string.IsNullOrEmpty(item.ProblemsAtBirth) ? "" : " Problems at Birth: " + item.ProblemsAtBirth + ","));
                        //sb.Append((item.IsNewbornIdNull() ? "" : " Fetal Distress: " + ((item.bFetalDistress) ? "Present," : "Absent,")));

                        sb.Append(("Fetal Distress: " + ((item.bFetalDistress) ? "Present," : "Absent,")));
                        sb.Append((string.IsNullOrEmpty(item.Comments) ? "" : "Comments: " + item.Comments));
                        if (!string.IsNullOrEmpty(sb.ToString()))
                        {
                            sb.Insert(0, "<div id='BirthNewborn_" + item.NewbornId + "' title='Newborn Information'  name='Birth Hx'><strong>Newborn Information: </strong>");
                            sb.Append("</div>");
                        }
                    }
                }

                else
                {
                    return string.Empty;
                }
            string returnResult = sb.ToString();
            if (!string.IsNullOrEmpty(returnResult) && returnResult.Substring(returnResult.Length - 7).Equals(",</div>"))
            {
                returnResult = returnResult.Remove(returnResult.Length - 7);
                returnResult += "</div>";
            }
            return returnResult;
        }

        internal string insertUpdateMaternalDeliverySoapText(DSBirthHistory dsBirthHistory, BirthHxMaternalDeliveryModel modelObj)
        {
            //dsMaternalDelivery.BirthHx_MaternalDelivery
            string MaternalDeliverySoapText = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (modelObj != null)
            {
                //sb.Append("<div id='BirthMaternalDelivery_" + modelObj.MaternalDeliveryId + "' title='MaternalDelivery'  name='Birth Hx'><strong>Maternal & Delivery:  </strong>");
                sb.Append((string.IsNullOrEmpty(modelObj.Gestation) ? "" : " Gestation: " + modelObj.Gestation + "  weeks,"));
                sb.Append((string.IsNullOrEmpty(modelObj.NumberOfFetuses) ? "" : " Number of Fetuses: " + modelObj.NumberOfFetuses + ","));
                sb.Append((string.IsNullOrEmpty(modelObj.NumberOfLivingFetuses) ? "" : " Number of Living Fetuses: " + modelObj.NumberOfLivingFetuses + ","));
                sb.Append((string.IsNullOrEmpty(modelObj.LaborLength) ? "" : " Labor Length: " + modelObj.LaborLength + " Hours,"));
                sb.Append(((string.IsNullOrEmpty(modelObj.DeliveryMethodId_text) || modelObj.DeliveryMethodId <= 0) ? "" : " Delivery Method:  " + modelObj.DeliveryMethodId_text + ","));
                sb.Append(((string.IsNullOrEmpty(modelObj.DeliveryPresentationId_text) || modelObj.DeliveryPresentationId <= 0) ? "" : " Delivery Presentation:  " + modelObj.DeliveryPresentationId_text + ","));
                sb.Append(((string.IsNullOrEmpty(modelObj.MaternalHistoryId_text) || modelObj.MaternalHistoryId <= 0) ? "" : " Maternal History:  " + modelObj.MaternalHistoryId_text + ", "));
                sb.Append((string.IsNullOrEmpty(modelObj.MaternalDeliveryComments) ? "" : "Comments: " + modelObj.MaternalDeliveryComments));
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Insert(0, "<div id='BirthMaternalDelivery_" + modelObj.MaternalDeliveryId + "' title='Maternal & Delivery'  name='Birth Hx'><strong>Maternal & Delivery:  </strong>");
                    sb.Append("</div>");
                }
            }
            else
                if (dsBirthHistory.BirthHx_MaternalDelivery != null && dsBirthHistory.BirthHx_MaternalDelivery.Rows.Count > 0)
                {
                    foreach (var item in dsBirthHistory.BirthHx_MaternalDelivery)
                    {
                        //   sb.Append("<div id='BirthMaternalDelivery_" + item.MaternalDeliveryId + "' title='MaternalDelivery'  name='Birth Hx'><strong>Maternal & Delivery:  </strong>");
                        sb.Append((string.IsNullOrEmpty(item.Gestation) ? "" : " Gestation: " + item.Gestation + "  weeks,"));
                        sb.Append((string.IsNullOrEmpty(item.NumberOfFetuses) ? "" : " Number of Fetuses: " + item.NumberOfFetuses + ","));
                        sb.Append((string.IsNullOrEmpty(item.NumberOfLivingFetuses) ? "" : " Number of Living Fetuses: " + item.NumberOfLivingFetuses + ","));
                        sb.Append((string.IsNullOrEmpty(item.LaborLength) ? "" : " Labor Length: " + item.LaborLength + ","));
                        sb.Append((string.IsNullOrEmpty(item.DeliveryMethod) ? "" : " Delivery Method:  " + item.DeliveryMethod + ","));
                        sb.Append((string.IsNullOrEmpty(item.DeliveryPresentation) ? "" : " Delivery Presentation:  " + item.DeliveryPresentation + ","));
                        sb.Append((string.IsNullOrEmpty(item.MaternalHistory) ? "" : " Maternal History:  " + item.MaternalHistory + ", "));
                        sb.Append((string.IsNullOrEmpty(item.Comments) ? "" : "Comments: " + item.Comments));

                        if (!string.IsNullOrEmpty(sb.ToString()))
                        {
                            sb.Insert(0, "<div id='BirthMaternalDelivery_" + item.MaternalDeliveryId + "' title='Maternal & Delivery'  name='Birth Hx'><strong>Maternal & Delivery:  </strong>");
                            sb.Append("</div>");
                        }
                    }
                }

                else
                {
                    return string.Empty;
                }
            string returnResult = sb.ToString();
            if (!string.IsNullOrEmpty(returnResult) && returnResult.Substring(returnResult.Length - 7).Equals(",</div>"))
            {
                returnResult = returnResult.Remove(returnResult.Length - 7);
                returnResult += "</div>";
            }
            return returnResult;
        }

        #endregion

        #region 'Attachment/Detachment of Birth History with Progress note'
        /// <summary>
        /// This Function will detach BirthHx from notes
        /// </summary>
        /// <param name="birthHxId"></param>
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string detach_BirthHx_From_Notes(long birthHxId, long notesId)
        {
            try
            {
                if (string.IsNullOrEmpty(MDVUtility.ToStr(birthHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<string> obj = BLLClinicalObj.detachBirthHxFromNotes(birthHxId, notesId);
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

        /// <summary>
        /// This Function will attach BirthHx to notes
        /// </summary>
        /// <param name="birthHxId"></param>    
        /// <param name="notesId"></param>
        /// <returns></returns>
        internal string attach_BirthHx_With_Notes(long birthHxId, long notesId)
        {
            try
            {
                DSBirthHistory dsBirthHx = null;
                if (string.IsNullOrEmpty(MDVUtility.ToStr(birthHxId)) || string.IsNullOrEmpty(MDVUtility.ToStr(notesId)))
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
                    BLObject<DSBirthHistory> obj = BLLClinicalObj.attachBirthHxWithNotes(birthHxId, notesId);
                    if (obj.Data != null)
                    {
                        dsBirthHx = obj.Data;
                        var response = new
                        {
                            status = true,
                            BirthHxTotalCount = dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows.Count,
                            BirthHxCount = dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows.Count,
                            BirthHxLoad_JSON = MDVUtility.JSON_DataTable(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName]),
                            //    BirthHxHistoryLoad_JSON = MDVUtility.JSON_DataTable(dsBirthHx.Tables[dsBirthHx.BirthHxHistory.TableName]),
                            Message = Common.AppPrivileges.Update_Message
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
        //end azhar changed jan 13,2016


        public string saveBirthHx(BirthHxModel model)
        {
            try
            {
                DSBirthHistory dsBirthHx = new DSBirthHistory();
                DSBirthHistory.BirthHxRow dr;
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.BirthHxId)) && model.BirthHxId > 0)
                {
                    BLObject<DSBirthHistory> obj = BLLClinicalObj.loadBirthHx(model.PatientId, model.BirthHxId, false);
                    dsBirthHx = obj.Data;
                    dr = dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows[0] as DSBirthHistory.BirthHxRow;
                }
                else
                {
                    dr = dsBirthHx.BirthHx.NewBirthHxRow();
                    dr.BirthHxId = -1;
                }
                dr.PatientId = MDVUtility.ToInt64(model.PatientId);

                if (!string.IsNullOrEmpty(model.BirthHxDate))
                {
                    dr.BirthHxDate = MDVUtility.ToDateTime(model.BirthHxDate);
                }
                else
                {
                    dr[dsBirthHx.BirthHx.BirthHxDateColumn] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(model.BirthHxComments))
                {
                    dr.Comments = MDVUtility.ToStr(model.BirthHxComments);
                }
                else
                {
                    dr[dsBirthHx.BirthHx.CommentsColumn] = DBNull.Value;
                }

                dr.bUnremarkable = model.BirthHxUnremarkable;

                dr.IsActive = true;
                if (model.NotesId > 0)
                {
                    dr.NotesId = model.NotesId;
                }
                else
                {
                    dr[dsBirthHx.BirthHx.NotesIdColumn] = DBNull.Value;
                }
                if (model.BirthHxId <= 0)
                {
                    dr.CreatedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                    dr.CreatedOn = DateTime.Now;
                }
                dr.ModifiedBy = MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName);
                dr.ModifiedOn = DateTime.Now;

                #region Database Insertion
                BLObject<DSBirthHistory> objdata = new BLObject<DSBirthHistory>();
                if (!string.IsNullOrEmpty(MDVUtility.ToStr(model.BirthHxId)) && MDVUtility.ToInt64(model.BirthHxId) > 0)
                {
                    objdata = BLLClinicalObj.updateBirthHx(dsBirthHx);
                }
                else
                {
                    dsBirthHx.BirthHx.AddBirthHxRow(dr);
                    objdata = BLLClinicalObj.insertBirthHx(dsBirthHx);
                }




                dsBirthHx = objdata.Data;
                BirthHxResponseModel respModel = new BirthHxResponseModel();
                if (objdata.Data != null)
                {
                    Int64 birthHxId = MDVUtility.ToInt64(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows[0][dsBirthHx.BirthHx.BirthHxIdColumn.ColumnName]);
                    model.BirthHxId = birthHxId;
                    if (birthHxId > 0 && !model.BirthHxUnremarkable)
                    {
                        if (!string.IsNullOrEmpty(model.IsGeneralUpdate) && model.IsGeneralUpdate.ToLower().Equals("true") && model.BirthHxGeneral != null)
                        {
                            respModel = insertUpdateGeneral(birthHxId, model.BirthHxGeneral, respModel, model.PatientId);
                        }
                        if (!string.IsNullOrEmpty(model.IsDeliveryUpdate) && model.IsDeliveryUpdate.ToLower().Equals("true") && model.BirthHxMaternalDelivery != null)
                        {
                            respModel = insertUpdateMaternalDelivery(birthHxId, model.BirthHxMaternalDelivery, respModel, model.PatientId);
                        }
                        if (!string.IsNullOrEmpty(model.IsNewbornUpdate) && model.IsNewbornUpdate.ToLower().Equals("true") && model.BirthHxNewborn != null)
                        {
                            respModel = insertUpdateNewborn(birthHxId, model.BirthHxNewborn, respModel,model.PatientId);
                        }
                    }
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To update Soap Text of Birth Hx in Insert mode
                       Created Date: Jan 07, 2016
                   */
                    BLObject<string> objValue = BLLClinicalObj.updateSoapTextForBirthHX(birthHxId);


                    var SoapText = string.Empty;
                    var IsCreatedOrModified = string.Empty;
                    var LastUpdated = string.Empty;

                    var SoapInfo = getCurrentSoapText(birthHxId);

                    if (SoapInfo != null)
                    {
                        SoapText = SoapInfo["SoapText"];
                        IsCreatedOrModified = SoapInfo["IsCreatedOrModified"];
                        LastUpdated = SoapInfo["LastUpdated"];
                    }

                    var response = new
                    {
                        SoapText = SoapText,
                        IsCreatedOrModified = IsCreatedOrModified,
                        LastUpdated = LastUpdated,
                        GeneralId = respModel.GeneralId,
                        MaternalDeliveryId = respModel.MaternalDeliveryId,
                        NewbornId = respModel.NewbornId,
                        status = true,
                        Message = Common.AppPrivileges.Save_Message,
                        BirthHxId = MDVUtility.ToInt64(dsBirthHx.Tables[dsBirthHx.BirthHx.TableName].Rows[0][dsBirthHx.BirthHx.BirthHxIdColumn.ColumnName])
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = objdata.Message
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
    }
}