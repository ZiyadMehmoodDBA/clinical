using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDVision.WebAPI.Entities;
using MDVision.Common.Shared;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Utilities;
using System.Data;
using MDVision.Business.BLL;
using MDVision.Datasets;
using MDVision.Business.BCommon;

namespace MDVision.WebAPI.Helpers
{
    public class APILookups
    {
        public HashSet<NameValuePair> GetFacility(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            string EntityId = MDVSession.Current.EntityId;
            BLObject<DSProfileLookup> obj = new BLLAdminProfile().LookupFacility(IsActive, EntityId);
            DSProfileLookup ds = obj.Data;

            if (ds != null)
            {
                if (ds.Tables[ds.Facility.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Facility.TableName].Select("1=1", ds.Facility.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.FacilityDescriptionColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Facility.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                        else
                        {
                            //if (_EntityId == null)
                            list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.PracticeIdColumn].ToString(), dr[ds.Facility.PracticeColumn].ToString(), dr[ds.Facility.IsActiveColumn].ToString(), dr[ds.Facility.FacilityDescriptionColumn].ToString()));
                            //else
                            //    list.Add(new NameValuePair(dr[ds.Facility.ShortNameColumn.ColumnName].ToString(), dr[ds.Facility.FacilityIdColumn.ColumnName].ToString(), dr[ds.Facility.EntityIdColumn].ToString()));
                        }
                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetProvider(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            BLObject<DSProfileLookup> objProvider = new BLLAdminProfile().LookupProvider(IsActive);
            DSProfileLookup ds = objProvider.Data;
            if (ds != null)
            {

                if (ds.Tables[ds.Provider.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Provider.TableName].Select("1=1", ds.Provider.ShortNameColumn.ColumnName);

                    foreach (DataRow dr in dRows)
                    {
                        if (MDVUtility.DecryptFrom64(MDVSession.Current.AppUserName) == ClientConfiguration.DefaultUser)
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString() + " - " + dr[ds.Provider.EntityShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString()));
                        else
                            list.Add(new NameValuePair(dr[ds.Provider.ShortNameColumn.ColumnName].ToString(), dr[ds.Provider.ProviderIdColumn.ColumnName].ToString(), dr[ds.Provider.EntityIdColumn.ColumnName].ToString(), dr[ds.Provider.SpecialtyIdColumn.ColumnName].ToString(), dr[ds.Provider.IsActiveColumn.ColumnName].ToString(), dr[ds.Provider.SpecialityNameColumn.ColumnName].ToString()));

                    }
                }
            }
            return list;
        }

        public HashSet<NameValuePair> GetMaritalStatusDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLPatient().LookupMaritalStatusDemographic();

            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Status, item.Id));
                    }
                }

            }
            return list;
        }

        public HashSet<NameValuePair> GetRaceDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLPatient().LookupRaceDemographic();

            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return list;
        }

        public HashSet<NameValuePair> GetEthnicityDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            var model = new BLLPatient().LookupEthnicityDemographic();

            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.Description, item.Id));
                    }
                }

            }
            return list;
        }

        public HashSet<NameValuePair> GetGenderDemographic(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();

            var model = new BLLAdminCodes().LookupGenderDemographic();


            var ds = model.Data;
            if (ds != null)
            {
                if (ds != null)
                {
                    foreach (var item in ds)
                    {
                        list.Add(new NameValuePair(item.GenderName, item.Id));
                    }
                }

            }
            return list;
        }

        public HashSet<NameValuePair> GetLanguages(string IsActive)
        {
            HashSet<NameValuePair> list = new HashSet<NameValuePair>();
            BLObject<DSPatientLookups> objPatient = new BLLPatient().LookupLanguages();
            DSPatientLookups ds = objPatient.Data;
            if (ds != null)
            {
                if (ds.Tables[ds.Languages.TableName] != null)
                {
                    DataRow[] dRows = ds.Tables[ds.Languages.TableName].Select("1=1", ds.Languages.DescriptionColumn.ColumnName);

                    foreach (DataRow dr in dRows.OrderBy(dr => dr[ds.Languages.IdColumn.ColumnName]))
                    {
                        list.Add(new NameValuePair(dr[ds.Languages.DescriptionColumn.ColumnName].ToString(), dr[ds.Languages.IdColumn.ColumnName].ToString()));
                    }
                }
            }
            return list;
        }

    }
}
