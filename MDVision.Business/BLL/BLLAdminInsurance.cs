using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Admin;
using System.Data;
using System.Text.RegularExpressions;
using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using MDVision.Common.Logging;

namespace MDVision.Business.BLL
{
    public class BLLAdminInsurance
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminInsurance"/> class.
        /// </summary>
        public BLLAdminInsurance()
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
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Variable

        #endregion

        #region "Functions"
        #region "Plan Category"
        /// <summary>
        /// Loads the plan category.
        /// </summary>
        /// <param name="InsuranceId">The plan category identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> LoadPlanCategory(long PlanCategoryId, string ShortName, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSInsurance ds = new DSInsurance();
                ds = new DALPlanCategory().LoadPlanCategory(PlanCategoryId, ShortName, Description, EntityId, IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LoadPlanCategory", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the plan category.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> UpdatePlanCategory(ref DSInsurance ds)
        {
            try
            {
                ds = new DALPlanCategory().UpdatePlanCategory(ref ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::UpdatePlanCategory", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the plan category.
        /// </summary>
        /// <param name="PlanCategoryIds">The plan category ids.</param>
        /// <returns></returns>
        public BLObject<string> DeletePlanCategory(string PlanCategoryIds)
        {
            try
            {
                PlanCategoryIds = new DALPlanCategory().DeletePlanCategory(PlanCategoryIds);
                return new BLObject<string>(PlanCategoryIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::DeletePlanCategory", ex);
                return new BLObject<string>(ex.Message);
            }
        }

        /// <summary>
        /// Inserts the plan category.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> InsertPlanCategory(ref DSInsurance ds)
        {
            try
            {
                ds = new DALPlanCategory().InsertPlanCategory(ref ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::InsertPlanCategory", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the plan category.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSInsuranceLookup> LookupPlanCategory(string Active)
        {
            try
            {
                DSInsuranceLookup ds = new DSInsuranceLookup();
                ds = new DALPlanCategory().LookupPlanCategory(Active);
                return new BLObject<DSInsuranceLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupPlanCategory", ex);
                return new BLObject<DSInsuranceLookup>(null, ex.Message);
            }
        }
        #endregion

        #region "Insurance"
        /// <summary>
        /// Loads the insurance.
        /// </summary>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> LoadInsurance(long InsuranceId, string ShortName, string Description, string EntityId, string IsActive, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSInsurance ds = new DSInsurance();
                ds = new DALInsurance().LoadInsurance(InsuranceId, ShortName, Description, EntityId, IsActive, PageNumber, RowsPerPage);

                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LoadInsurance", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> UpdateInsurance(ref DSInsurance ds)
        {
            try
            {
                ds = new DALInsurance().UpdateInsurance(ref ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::UpdateInsurance", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the insurance.
        /// </summary>
        /// <param name="InsuranceIds">The insurance ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteInsurance(string InsuranceIds)
        {
            try
            {
                InsuranceIds = new DALInsurance().DeleteInsurance(InsuranceIds);
                return new BLObject<string>(InsuranceIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::DeleteInsurance", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the insurance.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> InsertInsurance(ref DSInsurance ds)
        {
            try
            {
                ds = new DALInsurance().InsertInsurance(ref ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::InsertInsurance", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }
        #endregion

        #region "Insurance Plan"
        /// <summary>
        /// Loads the insurance plan.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="InsuranceId">The insurance identifier.</param>
        /// <param name="PlanId">The plan identifier.</param>
        /// <param name="PlanType">Type of the plan.</param>
        /// <param name="ClaimFlag">The claim flag.</param>
        /// <param name="ClaimType">Type of the claim.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> LoadInsurancePlan(long InsurancePlanId, string ShortName, string Description, string InsuranceId, string PlanId, string PlanTypeId, string ClaimFlagId, string ClaimTypeId, string SubScriberId = "", string IsActive = "", string entityId = null, string Address = null, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSInsurance ds = new DSInsurance();
                ds = new DALInsurancePlan().LoadInsurancePlan(InsurancePlanId, ShortName, Description, InsuranceId, PlanId, PlanTypeId, ClaimFlagId, ClaimTypeId, IsActive, PageNumber, RowsPerPage, SubScriberId, entityId, Address);

                // // azam aftab dated 19 jan,2015 PMS-3290
                //if (SubScriberId == "")
                //{
                //    SubscriberId = "abc123abcabc"; //"ab123abcabc";//"a123abcabc";
                //}

                //if (SubScriberId != "")
                //{
                //    string NewTableName = ds.InsurancePlan.TableName;
                //    DataTable SubscriberInsurancePlans = new DataTable(ds.InsurancePlan.TableName);
                //    //if (SubscriberInsurancePlans.Rows.Count > 0)
                //    //{
                //    //Get Insurance Plans on basis of Search Pattern
                //    DataTable dtInsurancePlans = (DataTable)ds.Tables[ds.InsurancePlan.TableName];

                //    IEnumerable<DataRow> temp = dtInsurancePlans.AsEnumerable()
                //            .Where(r => !string.IsNullOrEmpty(r.Field<string>(ds.InsurancePlan.SearchPatternColumn.ColumnName)) && MDVUtility.ValidateFormat(r.Field<string>(ds.InsurancePlan.SearchPatternColumn.ColumnName).ToLower(), SubScriberId.ToLower()) == true);
                //    if (temp.Count() > 0)
                //    {
                //        SubscriberInsurancePlans = dtInsurancePlans.AsEnumerable()
                //      .Where(r => !string.IsNullOrEmpty(r.Field<string>(ds.InsurancePlan.SearchPatternColumn.ColumnName)) && MDVUtility.ValidateFormat(r.Field<string>(ds.InsurancePlan.SearchPatternColumn.ColumnName).ToLower(), SubScriberId.ToLower()) == true)
                //      .CopyToDataTable();

                //        //Clear existing Insurance Plans
                //        ds.Tables[ds.InsurancePlan.TableName].Rows.Clear();


                //        //Fill Insurance Plans on basis of SearchPattern
                //        foreach (DataRow drCurrent in SubscriberInsurancePlans.Rows)
                //        {
                //            bool result = true;
                //            string SearchPattern = drCurrent["SearchPattern"].ToString();
                //            string maskingstring_ = SearchPattern.Split('%')[0];
                //            bool HasAlphabets = MDVUtility.isAlphaNumeric(maskingstring_);//.Any(x => !char.IsLetter(x));
                //            if (HasAlphabets)
                //            {
                //                var formatChar = SubScriberId.ToLower().ToCharArray();
                //                //SubScriberId.ToLower().IndexOf(SubScriberId.Any(x => !char.IsLetter(x)););
                //                for (int i = 0; i < maskingstring_.Length; i++)
                //                {
                //                    if (MDVUtility.isAlphaNumeric(formatChar[i].ToString()))
                //                    {
                //                        if (!(formatChar[i] == drCurrent["SearchPattern"].ToString().ToLower()[i]))
                //                        {
                //                            result = false;
                //                        }
                //                    }
                //                }
                //                if (result)
                //                {
                //                    ds.Tables[ds.InsurancePlan.TableName].Rows.Clear();
                //                    ds.Tables[ds.InsurancePlan.TableName].ImportRow(drCurrent);
                //                    break;
                //                }
                //            }
                //            if (result)
                //            {
                //                ds.Tables[ds.InsurancePlan.TableName].ImportRow(drCurrent);
                //            }

                //        }
                //    }
                //    else
                //    {
                //        ds.Tables[ds.InsurancePlan.TableName].Rows.Clear();
                //    }



                //    //}
                //}
                // //END dated 19 jan,2015 PMS-3290
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LoadInsurancePlan", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        public BLObject<DSInsuranceLookup> LookupInsurancePlan(string Active, string EntityId = null, string ShortName = "")
        {
            try
            {
                DSInsuranceLookup ds = new DSInsuranceLookup();
                ds = new DALInsurancePlan().LookupInsurancePlan(Active, EntityId, ShortName);

                return new BLObject<DSInsuranceLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminProfile::LookupInsurancePlan", ex);
                return new BLObject<DSInsuranceLookup>(null, ex.Message);
            }

        }

        public BLObject<DSInsurance> SubscriberMatching(string SubScriberId, bool IsActive = true)
        {
            try
            {
                DSInsurance ds = new DSInsurance();
                List<string> insurancePlanId = new List<string>();
                var regularExpression = string.Empty;
                ds = new DALInsurancePlan().LoadInsurancePlanForPatternMatching(IsActive);
                foreach (var regex in ds.InsurancePlanRegex)
                {
                    if (regex.RegularExpression == "" && regex.SearchPattern != "")
                    {
                        regularExpression = MDVUtility.GenerateRegex(regex.SearchPattern);
                        if (this.ValidateFormatRegex(regularExpression, SubScriberId))
                        {
                            insurancePlanId.Add(regex.InsurancePlanId);
                        }
                    }
                    else
                    {
                        if (this.ValidateFormatRegex(regex.RegularExpression, SubScriberId))
                        {
                            insurancePlanId.Add(regex.InsurancePlanId);
                        }
                    }
                }
                var planIds = String.Join(",", insurancePlanId);
                ds = new DALInsurancePlan().LoadMatchedInsurancePlan(planIds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::SubscriberMatching", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }
        private bool ValidateFormatRegex(string regularExpression, string inputText)
        {
            try
            {
                Regex regex = new Regex(regularExpression);
                Match match = regex.Match(inputText);
                if (match.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::ValidateFormatRegex", ex);
                return false;
            }
        }
        public bool ValidateSubscriberIDFormat(string subscriberID, string format)
        {
            return MDVUtility.ValidateFormatRegex(format, subscriberID);
        }
        public string RegexCreator(string Pattern)
        {
            return MDVUtility.GenerateRegex(Pattern);
        }
        /// <summary>
        /// Updates the insurance plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> UpdateInsurancePlan(ref DSInsurance ds)
        {
            try
            {
                ds = new DALInsurancePlan().UpdateInsurancePlan(ref ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::UpdateInsurancePlan", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the insurance plan.
        /// </summary>
        /// <param name="InsurancePlanIds">The insurance plan ids.</param>
        /// <returns></returns>
        public BLObject<string> DeleteInsurancePlan(string InsurancePlanIds)
        {
            try
            {
                InsurancePlanIds = new DALInsurancePlan().DeleteInsurancePlan(InsurancePlanIds);
                return new BLObject<string>(InsurancePlanIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::DeleteInsurancePlan", ex);
                return new BLObject<string>("", ex.Message);
            }
        }

        /// <summary>
        /// Inserts the insurance plan.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> InsertInsurancePlan(ref DSInsurance ds)
        {
            try
            {
                ds = new DALInsurancePlan().InsertInsurancePlan(ref ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::InsertInsurancePlan", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        #region "Insurance Plan Address Info"
        /// <summary>
        /// Loads the insurance plan address information.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> LoadInsurancePlanAddressInfo(long InsurancePlanId, long IndurancePlanAddressId)
        {
            try
            {
                DSInsurance ds = new DSInsurance();
                ds = new DALInsurancePlan().LoadInsurancePlanAddressInfo(InsurancePlanId, IndurancePlanAddressId);

                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LoadInsurancePlanAddressInfo", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        public BLObject<DSInsurance> LoadInsurancePlanAddressSearch(string InsurancePlan, string Description, string Address, string City, string State, string Zip, string Telephone, int PageNumber, int RowsPerPage)
        {
            //List<InsurancePlanAddressModel> InsurancePlanAddress = new List<InsurancePlanAddressModel>();
            try
            {
                DSInsurance ds = new DSInsurance();
                ds = new DALInsurancePlan().LoadInsurancePlanAddressSearch(InsurancePlan, Description, Address, City, State, Zip, Telephone, PageNumber, RowsPerPage);

                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LoadInsurancePlanAddressInfo", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
            //return InsurancePlanAddress;
        }

        /// <summary>
        /// Updates the insurance plan address information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> UpdateInsurancePlanAddressInfo(DSInsurance ds)
        {
            try
            {
                ds = new DALInsurancePlan().UpdateInsurancePlanAddressInfo(ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::UpdateInsurancePlanAddressInfo", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Inserts the insurance plan address information.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSInsurance> InsertInsurancePlanAddressInfo(DSInsurance ds)
        {
            try
            {
                ds = new DALInsurancePlan().InsertInsurancePlanAddressInfo(ds);
                return new BLObject<DSInsurance>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::InsertInsurancePlanAddressInfo", ex);
                return new BLObject<DSInsurance>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the insurance plan address information.
        /// </summary>
        /// <param name="InsurancePlanAddressId">The insurance plan address identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteInsurancePlanAddressInfo(string InsurancePlanAddressId)
        {
            try
            {
                InsurancePlanAddressId = new DALInsurancePlan().DeleteInsurancePlanAddressInfo(InsurancePlanAddressId);
                return new BLObject<string>(InsurancePlanAddressId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::DeleteInsurancePlanAddressInfo", ex);
                return new BLObject<string>("", ex.Message);
            }
        }
        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the claim flag.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupClaimFlag()
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALInsurancePlan().LookupClaimFlag();
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupClaimFlag", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the type of the claim.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupClaimType(string IsActive)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALInsurancePlan().LookupClaimType(IsActive);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupClaimType", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the claim scrubbing profile.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupClaimScrubbingProfile()
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALInsurancePlan().LookupClaimScrubbingProfile();
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupClaimScrubbingProfile", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the plan fee link.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupPlanFeeLink()
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALInsurancePlan().LookupPlanFeeLink();
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupPlanFeeLink", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the type of the plan.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSCodeLookup> LookupPlanType(string Active)
        {
            try
            {
                DSCodeLookup ds = new DSCodeLookup();
                ds = new DALInsurancePlan().LookupPlanType(Active);
                return new BLObject<DSCodeLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupPlanType", ex);
                return new BLObject<DSCodeLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the insurance plan.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSInsuranceLookup> LookupInsurancePlan(string Active)
        {
            try
            {
                DSInsuranceLookup ds = new DSInsuranceLookup();
                ds = new DALInsurancePlan().LookupInsurancePlan(Active);
                return new BLObject<DSInsuranceLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupInsurancePlan", ex);
                return new BLObject<DSInsuranceLookup>(null, ex.Message);
            }
        }

        /// <summary>
        /// Lookups the insurance plan address.
        /// </summary>
        /// <param name="InsurancePlanId">The insurance plan identifier.</param>
        /// <returns></returns>
        public BLObject<DSInsuranceLookup> LookupInsurancePlanAddress(Int64 InsurancePlanId)
        {
            try
            {
                DSInsuranceLookup ds = new DSInsuranceLookup();
                ds = new DALInsurancePlan().LookupInsurancePlanAddress(InsurancePlanId);
                return new BLObject<DSInsuranceLookup>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLAdminInsurance::LookupInsurancePlanAddress", ex);
                return new BLObject<DSInsuranceLookup>(null, ex.Message);
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
