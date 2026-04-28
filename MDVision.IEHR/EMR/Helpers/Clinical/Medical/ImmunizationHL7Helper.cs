using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.Medical;
using MDVision.Datasets;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls.Expressions;
using System.Windows.Forms;
using iTextSharp.xmp.options;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.IEHR.Common;
using Newtonsoft.Json;
using MDVision.DataAccess.DAL.Clinical;

using NHapi.Base;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
using Org.BouncyCastle.Asn1.Mozilla;
using MDVision.Common.Utilities;
using MDVision.Business.BLL;
using MDVision.DataAccess.DCommon;
using MDVision.Common.Shared;

//using NHapi.Base;
//using NHapi.Base.Model;
//using NHapi.Base.Parser;
//using NHapi.Model.V25;
//using NHapi.Model.V25.Message;
//using NHapi.Model.V25.Segment;

namespace MDVision.IEHR.EMR.Helpers.Clinical.Medical
{
    public class ImmunizationHL7Helper
    {
        private BLLPatient BLLPatientObj = null;
        private BLLClinical BLLClinicalObj = null;
        private BLLAdminProfile BLLAdminProfileObj = null;

        #region  Constructors

        public ImmunizationHL7Helper()
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLAdminProfileObj = new BLLAdminProfile();
        }


        public ImmunizationHL7Helper(string vaccineHxIds)
        {
            BLLPatientObj = new BLLPatient();
            BLLClinicalObj = new BLLClinical();
            BLLAdminProfileObj = new BLLAdminProfile();


            vaccineHx_Ids = vaccineHxIds;
            var dsVaccineCompleteDataSet = BLLClinicalObj.Generate_HL7Immunization_Message(vaccineHxIds);
            if (dsVaccineCompleteDataSet.Data != null)
            {
                VaccineData = dsVaccineCompleteDataSet.Data.Vaccine;
                VaccineGroupData = dsVaccineCompleteDataSet.Data.VaccineGroup;
                VaccineHxData = dsVaccineCompleteDataSet.Data.VaccineHx;
                VaccineManufacturerData = dsVaccineCompleteDataSet.Data.VaccineManufacturer;
                VaccineRouteData = dsVaccineCompleteDataSet.Data.VaccineRoute;
                VaccineSiteData = dsVaccineCompleteDataSet.Data.VaccineSite;
                VaccineSourceOfHxData = dsVaccineCompleteDataSet.Data.VaccineSourceOfHx;
                VaccineVFCData = dsVaccineCompleteDataSet.Data.VaccineVFC;
                VaccineVISData = dsVaccineCompleteDataSet.Data.VaccineVIS;
                VaccineVIS_URLData = dsVaccineCompleteDataSet.Data.VaccineVIS_URL;

                VaccineCrosswalkData = dsVaccineCompleteDataSet.Data.Tables["Table10"];
                ManufacturerData = dsVaccineCompleteDataSet.Data.Tables["Table11"];
                VaccineLotNoData = dsVaccineCompleteDataSet.Data.Tables["Table12"];

            }
        }

        #endregion


        //    private static ImmunizationHL7Helper _instance = null;

        #region Fields and Objects

        public DSImmunizationHL7.VaccineDataTable VaccineData { get; set; }
        private DSImmunizationHL7.VaccineGroupDataTable VaccineGroupData { get; set; }
        private DSImmunizationHL7.VaccineHxDataTable VaccineHxData { get; set; }
        private DSImmunizationHL7.VaccineManufacturerDataTable VaccineManufacturerData { get; set; }
        private DSImmunizationHL7.VaccineRouteDataTable VaccineRouteData { get; set; }
        private DSImmunizationHL7.VaccineSiteDataTable VaccineSiteData { get; set; }
        private DSImmunizationHL7.VaccineSourceOfHxDataTable VaccineSourceOfHxData { get; set; }
        private DSImmunizationHL7.VaccineVFCDataTable VaccineVFCData { get; set; }
        private DSImmunizationHL7.VaccineVISDataTable VaccineVISData { get; set; }
        private DSImmunizationHL7.VaccineVIS_URLDataTable VaccineVIS_URLData { get; set; }

        private DataTable VaccineCrosswalkData { get; set; }

        private DataTable ManufacturerData { get; set; }

        private DataTable VaccineLotNoData { get; set; }

        private long CurrentVaccineHxId;

        private string vaccineHx_Ids = "";

        VXU_V04 vxu = new VXU_V04();

        int OBXSegmentCounter = 0;

        #endregion




        public string Generate_HL7Immunization_Message()
        {
            try
            {
                int Iterationcounter = 0;
                foreach (var vaccineHxId in vaccineHx_Ids.Split(','))
                {
                    CurrentVaccineHxId = int.Parse(vaccineHxId);
                    GenerateHl7VxuMessage(Iterationcounter);
                    Iterationcounter++;
                    OBXSegmentCounter = 0;
                }

                PipeParser parser = new PipeParser();
                string message = parser.Encode(vxu);
                message = message.Replace("\r", "\r\n");
                string[] stringSeparators = new string[] { "\r\n" };
                string[] result;
                result = message.Split(stringSeparators, StringSplitOptions.None);
                if (result.Length > 0)
                {
                    if (result[0].StartsWith("MSH|"))
                    {
                        result[0] = result[0] + "|Sovereign^^^^^NIST-AA-IZ-1&2.16.840.1.113883.3.72.5.40.9&ISO^XX^^^100-6482|NISTIISFAC^^^^^NIST-AA-IZ-1&2.16.840.1.113883.3.72.5.40.9&ISO^XX^^^100-3322";
                    }
                }
                String sep = "\r\n";
                String FinalMessage = String.Join(sep, result);



                message = ReorderObxWithAccordanceToTestCaseGeneration(FinalMessage);
                return Get_HL7Immunization_MessageGeneration_Response(message);

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


        // Author : Talha Tanweer
        // Purpose: To populate and genrerate HL7 vxu message
        public void GenerateHl7VxuMessage(int iterationCounter)
        {


            var currentDataRow = (VaccineHxData.AsDataView().Table.AsEnumerable()
                     .GroupJoin(
                                 VaccineData.AsDataView().Table.AsEnumerable(),
                                 vhx => vhx.Field<object>(ColumnName.Vaccine),
                                 vc => vc.Field<object>(ColumnName.VaccineID),
                                 (vhx, vaccineSet) => new { vhx, vaccineSet }
                               )
                    .SelectMany(
                                 @t => @t.vaccineSet.DefaultIfEmpty(),
                                 (@t, vc2) => new { @t, vc2 }
                               )
                   .GroupJoin(VaccineGroupData.AsDataView().Table.AsEnumerable(),
                               @t => @t.@t.vhx.Field<object>(ColumnName.VaccineGroupCategory),
                               vg => vg.Field<object>(ColumnName.VaccineGroupIDVaccineGroup),
                              (@t, vgroupset) => new { @t, vgroupset }
                              )
                    .SelectMany(
                                @t => @t.vgroupset.DefaultIfEmpty(),
                                (@t, vg2) => new { @t, vg2 }
                               )
                   .GroupJoin(VaccineManufacturerData.AsDataView().Table.AsEnumerable(),
                               @t => @t.@t.@t.@t.vhx.Field<object>(ColumnName.Manufacturer),
                               vm => vm.Field<object>(ColumnName.VaccineManufacturerId),
                               (@t, manufacturerSet) => new { @t, manufacturerSet }
                              )
                    .SelectMany(
                                 @t => @t.manufacturerSet.DefaultIfEmpty(),
                                 (@t, vm2) => new { @t, vm2 }
                               )
                    .GroupJoin(VaccineRouteData.AsDataView().Table.AsEnumerable(),
                                @t => @t.@t.@t.@t.@t.@t.vhx.Field<object>(ColumnName.Route),
                                vr => vr.Field<object>(ColumnName.VaccineRouteID),
                                (@t, routeSet) => new { @t, routeSet }
                               )
                    .SelectMany(
                                @t => @t.routeSet.DefaultIfEmpty(),
                                (@t, vr2) => new { @t, vr2 }
                               )
                    .GroupJoin(VaccineSiteData.AsDataView().Table.AsEnumerable(),
                                 @t => @t.@t.@t.@t.@t.@t.@t.@t.vhx.Field<object>(ColumnName.Site),
                                 vs => vs.Field<object>(ColumnName.VaccineSiteID),
                                 (@t, siteSet) => new { @t, siteSet }
                                 )
                    .SelectMany(
                                @t => @t.siteSet.DefaultIfEmpty(),
                                (@t, vs2) => new { @t, vs2 }
                               )
                   .GroupJoin(VaccineSourceOfHxData.AsDataView().Table.AsEnumerable(),
                                @t => @t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx.Field<object>(ColumnName.SourceOfHx),
                                vshx => vshx.Field<object>(ColumnName.VaccineSourceOfHxId),
                                (@t, sourceOfHxSet) => new { @t, sourceOfHxSet }
                               )
                    .SelectMany(
                                @t => @t.sourceOfHxSet.DefaultIfEmpty(),
                                (@t, vshx2) => new { @t, vshx2 }
                                )
                                .GroupJoin(VaccineVFCData.AsDataView().Table.AsEnumerable(),
                                            @t => @t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx.Field<object>(ColumnName.VFC),
                                            vfc => vfc.Field<object>(ColumnName.VaccineVFCId),
                                            (@t, vfcSet) => new { @t, vfcSet }
                                            )
                    .SelectMany(@t => @t.vfcSet.DefaultIfEmpty(),
                               (@t, vfc2) => new { @t, vfc2 }
                                ).GroupJoin(VaccineVISData.AsDataView().Table.AsEnumerable(),
                                            @t => @t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx.Field<object>(ColumnName.VISDate),
                                            vis => vis.Field<object>(ColumnName.VaccineVISId),
                                            (@t, visSet) => new { @t, visSet }
                                            )
                    .SelectMany(@t => @t.visSet.DefaultIfEmpty(), (@t, vis2) => new { @t, vis2 })

                    .Where(@t => @t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx.Field<object>(ColumnName.VaccineHxId).ToString() == CurrentVaccineHxId.ToString())

                    .Select(@t => new
                    {
                        VaccineHxId = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.VaccineHxId),
                        VaccineGroupCategory = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.VaccineGroupCategory),
                        Vaccine = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Vaccine),
                        LotNumber = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.LotNumber),
                        Route = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Route),
                        VFC = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.VFC),
                        Comments = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Comments),
                        VisitDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.VisitDate, true),
                        AdministrationDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.AdministrationDate, true),
                        Dose = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Dose),
                        Amount = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Amount),
                        Site = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Site),
                        VISDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.VISDate),
                        VoidDose = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.VoidDose),
                        IsUpdatedRecord = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.IsUpdatedRecord),
                        BirthIndicator = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.BirthIndicator),
                        BirthOrder = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.BirthOrder),
                        
                        
                        GivenBy = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.GivenBy),
                        Manufacturer = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Manufacturer),
                        ExpiryDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ExpiryDate, true),
                        SourceOfHx = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.SourceOfHx),
                        Type = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.Type),
                        PatientID = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.PatientID),
                        ISActive = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ISActive),
                        CreatedBy = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.CreatedBy),
                        CreatedOn = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.CreatedOn, true),
                        ModifiedOn = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ModifiedOn, true),
                        ModifiedBy = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ModifiedBy),
                        ProviderId = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ProviderId),
                        CompletionStatusCode = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.CompletionStatusCode),
                        RefusalReasonCode = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.RefusalReasonCode),
                        PublicityCode = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.PublicityCode),
                        PublicityCodeExpiryDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.PublicityCodeExpiryDate, true),
                        ImmunizationRegistryStatusCode = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ImmunizationRegistryStatusCode),
                        IRSEffectiveDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.IRSEffectiveDate, true),
                        ProtectionIndicator = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.ProtectionIndicator),
                        PIEffectiveDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.PIEffectiveDate, true),
                        RefusalReasonId = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.RefusalReasonId, true),

                        // ----------------- Route ----------------------------				 
                        VaccineRouteID = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.vr2, ColumnName.VaccineRouteID),
                        HL7Code0162Route = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.vr2, ColumnName.HL7Code0162Route),
                        DescriptionRoute = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.vr2, ColumnName.DescriptionRoute),

                        // ------------------ Site ------------------------------	 
                        VaccineSiteID = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.vs2, ColumnName.VaccineSiteID),
                        HL7Code0162Site = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.vs2, ColumnName.HL7Code0162Site),
                        DescriptionSite = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.vs2, ColumnName.DescriptionSite),

                        //   --------------- VFC ---------------------------			 
                        VaccineVFCId = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.VaccineVFCId),
                        ConceptCodeVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.ConceptCodeVFC),
                        ConceptNameVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.ConceptNameVFC),
                        PreferredConceptNameVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.PreferredConceptNameVFC),
                        PreferredAlternateCodeVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.PreferredAlternateCodeVFC),
                        CodeSystemOIDVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.CodeSystemOIDVFC),
                        CodeSystemNameVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.CodeSystemNameVFC),
                        CodeSystemCodeVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.CodeSystemCodeVFC),
                        CodeSystemVersionVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.CodeSystemVersionVFC),
                        HL7Table0396CodeVFC = GetColumnValueFromRow(@t.@t.@t.vfc2, ColumnName.HL7Table0396CodeVFC),
                        AdministrationStartDate = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vhx, ColumnName.AdministrationDate, true),
                        CVXcode = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vc2, ColumnName.CVXCodeVaccine),
                        CVXDescription = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vc2, ColumnName.CVXShortDescriptionVaccine),
                        UncertainFormulationCVX = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vc2, ColumnName.UncertainFormulationCVXVaccine),
                        ManufacturerIdentifier = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vm2, ColumnName.ManufacturerMVXCODE),
                        ManufacturerText = GetColumnValueFromRow(@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.@t.vm2, ColumnName.ManufacturerName),

                        // -------------------- VIS -------------- 
                        VISEditionDate = GetColumnValueFromRow(@t.vis2, ColumnName.VIS_EditionDate, true)
                    })
                ).First();



            #region Single VxuSegments

            if (iterationCounter == 0)
            {

                long patientId = (VaccineHxData.AsEnumerable().Select(vhx => vhx.PatientID)).First();
                BLObject<DSPatient> dsPatient = BLLPatientObj.FillPatient_PictureById(MDVUtility.ToInt64(patientId), "hl7immunization");
                DSPatient.PatientsRow patientRow = (dsPatient.Data.Patients.Where(p => p.PatientId == patientId)).First();
                DSPatient.PatientFamilyDataTable patientFamily = BLLPatientObj.LoadPatientFamily(MDVUtility.ToInt64(patientId), 0).Data.PatientFamily;

                #region MSH

                // ----------------------------------- MSH ---------------------------------------------------
                vxu.MSH.FieldSeparator.Value = "|";
                vxu.MSH.EncodingCharacters.Value = @"^~\&";
                vxu.MSH.SendingApplication.NamespaceID.Value = "Sovereign";
                vxu.MSH.SendingApplication.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.1";
                vxu.MSH.SendingApplication.UniversalIDType.Value = "ISO";
                vxu.MSH.SendingFacility.NamespaceID.Value = "X68";
                vxu.MSH.SendingFacility.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.2";
                vxu.MSH.SendingFacility.UniversalIDType.Value = "ISO";
                vxu.MSH.ReceivingApplication.NamespaceID.Value = "NISTIISAPP";
                vxu.MSH.ReceivingApplication.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.3";
                vxu.MSH.ReceivingApplication.UniversalIDType.Value = "ISO";
                vxu.MSH.ReceivingFacility.NamespaceID.Value = "NIST Test Iz Reg";
                vxu.MSH.ReceivingFacility.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.4";
                vxu.MSH.ReceivingFacility.UniversalIDType.Value = "ISO";
                vxu.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss") + "-0500";//DateTime.Now.ToString("yyyyMMddhhmm"); 
                vxu.MSH.MessageType.MessageCode.Value = "VXU";
                vxu.MSH.MessageType.TriggerEvent.Value = "V04";
                vxu.MSH.MessageType.MessageStructure.Value = "VXU_V04";
                vxu.MSH.MessageControlID.Value = "MDVision-Immunization-VXU-" + CurrentVaccineHxId + "." + (DateTime.Now.ToString("yyyyMMddmmss"));
                vxu.MSH.ProcessingID.ProcessingID.Value = "P";
                vxu.MSH.VersionID.VersionID.Value = "2.5.1";
                vxu.MSH.AcceptAcknowledgmentType.Value = "ER";
                vxu.MSH.ApplicationAcknowledgmentType.Value = "AL";
                vxu.MSH.GetMessageProfileIdentifier(0).EntityIdentifier.Value = "Z22";
                vxu.MSH.GetMessageProfileIdentifier(0).  NamespaceID.Value = "CDCPHINVS";
                vxu.MSH.ReceivingApplication.NamespaceID.Value = "NISTIISAPP";
                #endregion

                #region PID

                // ----------------------------------- PID ---------------------------------------------------
                string pidAdministratorSex = "";
                string PatientGender = GetColumnValueFromRow(patientRow, "Gender");
                if (!string.IsNullOrWhiteSpace(PatientGender))
                {

                    switch (PatientGender.ToLower())
                    {
                        case "male":
                            pidAdministratorSex = "M";
                            break;
                        case "female":
                            pidAdministratorSex = "F";
                            break;
                    }
                }
                else
                {
                    pidAdministratorSex = "";
                }

                var patientMother = (patientFamily.Where(pf => pf.RelationShipId == 16)
                    .Select(pf => new
                    {
                        surName = pf.LastName,
                        givenName = pf.FirstName
                    })).FirstOrDefault();

                Func<string, int, string> areaCityAndPhoneNo = (n, i) => n != "" ? n.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)[i] : "";


                vxu.PID.SetIDPID.Value = "1";

                vxu.PID.GetPatientIdentifierList(0).IDNumber.Value = GetColumnValueFromRow(patientRow, "MRNumber");
                //(from vhx in VaccineHxData select vhx.PatientID).First().ToString();
                vxu.PID.GetPatientIdentifierList(0).AssigningAuthority.NamespaceID.Value = "NIST-MPI-1";
                vxu.PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.5";
                vxu.PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalIDType.Value = "ISO";
                vxu.PID.GetPatientIdentifierList(0).IdentifierTypeCode.Value = "MR";

                if (!string.IsNullOrWhiteSpace(GetColumnValueFromRow(patientRow, "SSN")))
                {
                    vxu.PID.GetPatientIdentifierList(1).IDNumber.Value = patientRow.SSN;
                    vxu.PID.GetPatientIdentifierList(1).AssigningAuthority.NamespaceID.Value = "MAA";
                    vxu.PID.GetPatientIdentifierList(1).AssigningAuthority.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.5";
                    vxu.PID.GetPatientIdentifierList(1).AssigningAuthority.UniversalIDType.Value = "ISO";
                    vxu.PID.GetPatientIdentifierList(1).IdentifierTypeCode.Value = "SS";
                }

                vxu.PID.GetPatientName(0).FamilyName.Surname.Value = patientRow.LastName;
                if (patientRow.MotherMaidenName != null && patientRow.MotherMaidenName != "")
                {
                    vxu.PID.GetMotherSMaidenName(0).FamilyName.Surname.Value = patientRow.MotherMaidenName == null ? "" : patientRow.MotherMaidenName;
                    vxu.PID.GetMotherSMaidenName(0).NameTypeCode.Value = "M";
                }

                    vxu.PID.GetPatientName(0).GivenName.Value = patientRow.FirstName;
                vxu.PID.GetPatientName(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = GetColumnValueFromRow(patientRow, "MI");//patientRow.MI;
                vxu.PID.GetPatientName(0).NameTypeCode.Value = "L";
                //vxu.PID.GetMotherSMaidenName(0).GivenName.Value = patientMother == null ? "" : patientMother.givenName;
                vxu.PID.GetMotherSMaidenName(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = "";
                
                vxu.PID.DateTimeOfBirth.Time.Value = patientRow.DOB.ToString("yyyyMMdd");
                vxu.PID.AdministrativeSex.Value = pidAdministratorSex;
                vxu.PID.GetRace(0).Identifier.Value = GetColumnValueFromRow(patientRow, "RaceCode");
                vxu.PID.GetRace(0).Text.Value = GetColumnValueFromRow(patientRow, "RaceName");
                vxu.PID.GetRace(0).NameOfCodingSystem.Value = "CDCREC"; // "CDCREC::HL70005";
                vxu.PID.GetRace(0).AlternateIdentifier.Value = "";
                vxu.PID.GetRace(0).AlternateText.Value = "";
                vxu.PID.GetRace(0).NameOfAlternateCodingSystem.Value = "";
                vxu.PID.GetPatientAddress(0).StreetAddress.StreetOrMailingAddress.Value = GetColumnValueFromRow(patientRow, "Address1");
                vxu.PID.GetPatientAddress(0).OtherDesignation.Value = "";
                vxu.PID.GetPatientAddress(0).City.Value = GetColumnValueFromRow(patientRow, "City");
                vxu.PID.GetPatientAddress(0).StateOrProvince.Value = GetColumnValueFromRow(patientRow, "State");
                vxu.PID.GetPatientAddress(0).ZipOrPostalCode.Value = GetColumnValueFromRow(patientRow, "ZIPCode");
                vxu.PID.GetPatientAddress(0).Country.Value = "USA";
                vxu.PID.GetPatientAddress(0).AddressType.Value = "P";

                var getFirstPhoneNumber = new
                {
                    TelecommunicationUseCode = patientRow.HomePhoneNo != "" ? Hl7ImmunizationMeta.Telecommunicationusecode.PRN.ToString() : "",
                    TelecommunicationEquipmentType = patientRow.HomePhoneNo != "" ? "PH" : "",
                    AreaCityCode = areaCityAndPhoneNo(patientRow.HomePhoneNo, 0),
                    LocalNumber = areaCityAndPhoneNo(patientRow.HomePhoneNo, 1).Replace("-", "").Replace(" ", ""),
                };

                vxu.PID.GetPhoneNumberHome(0).TelecommunicationUseCode.Value = getFirstPhoneNumber.TelecommunicationUseCode;
                vxu.PID.GetPhoneNumberHome(0).TelecommunicationEquipmentType.Value = getFirstPhoneNumber.TelecommunicationEquipmentType;
                vxu.PID.GetPhoneNumberHome(0).EmailAddress.Value = ""; // patientRow.EmailAddress;
                vxu.PID.GetPhoneNumberHome(0).AreaCityCode.Value = getFirstPhoneNumber.AreaCityCode;
                vxu.PID.GetPhoneNumberHome(0).LocalNumber.Value = getFirstPhoneNumber.LocalNumber;

                if (!string.IsNullOrEmpty(patientRow.EmailAddress))
                {
                    vxu.PID.GetPhoneNumberHome(1).TelecommunicationUseCode.Value = Hl7ImmunizationMeta.Telecommunicationusecode.NET.ToString();
                    vxu.PID.GetPhoneNumberHome(1).TelecommunicationEquipmentType.Value = "";
                    vxu.PID.GetPhoneNumberHome(1).EmailAddress.Value = patientRow.EmailAddress;
                    vxu.PID.GetPhoneNumberHome(1).AreaCityCode.Value = "";
                    vxu.PID.GetPhoneNumberHome(1).LocalNumber.Value = ""; //patientRow.HomePhoneNo;
                }


                var Ethnicity = new
                {
                    Identifier = "",
                    Name = "",
                    NameOfCodingSystem = ""
                };

                if (!string.IsNullOrWhiteSpace(GetColumnValueFromRow(patientRow, "EthnicityCode")) && GetColumnValueFromRow(patientRow, "EthnicityCode") != "2145-2")
                {
                    Ethnicity = new
                    {
                        Identifier = GetColumnValueFromRow(patientRow, "EthnicityCode"),
                        Name = GetColumnValueFromRow(patientRow, "EthnicityName"),
                        NameOfCodingSystem = "CDCREC"
                    };
                }

                vxu.PID.GetEthnicGroup(0).Identifier.Value = Ethnicity.Identifier;
                vxu.PID.GetEthnicGroup(0).Text.Value = Ethnicity.Name;
                vxu.PID.GetEthnicGroup(0).NameOfCodingSystem.Value = Ethnicity.NameOfCodingSystem;
                vxu.PID.GetEthnicGroup(0).AlternateIdentifier.Value = "";
                vxu.PID.GetEthnicGroup(0).AlternateText.Value = "";
                vxu.PID.GetEthnicGroup(0).NameOfAlternateCodingSystem.Value = "";
                vxu.PID.MultipleBirthIndicator.Value = currentDataRow.BirthIndicator;
                vxu.PID.BirthOrder.Value = currentDataRow.BirthOrder.Replace("\r\n", "");
                vxu.PID.PatientDeathDateAndTime.Time.Value = "";
                vxu.PID.PatientDeathIndicator.Value = "N";

                #endregion

                #region PD1

                // ----------------------------------- PD1  ---------------------------------------------------

                bool hasPublicityCode = !string.IsNullOrWhiteSpace(currentDataRow.PublicityCode);
                // bool hasProtectionIndicator = bool.TryParse(currentDataRow.ProtectionIndicator,out hasProtectionIndicator);
                bool hasImmunizationRegistryStatus = !string.IsNullOrWhiteSpace(currentDataRow.ImmunizationRegistryStatusCode);
                //    if (hasPublicityCode || hasProtectionIndicator || hasImmunizationRegistryStatus)
                if (hasPublicityCode || hasImmunizationRegistryStatus)
                {
                    string PublicityCodePrefix = (Convert.ToInt64(currentDataRow.PublicityCode) / 10).ToString();
                    vxu.PD1.PublicityCode.Identifier.Value = PublicityCodePrefix + currentDataRow.PublicityCode;
                    vxu.PD1.PublicityCode.Text.Value = hasPublicityCode ? Hl7ImmunizationMeta.PublicityCode[PublicityCodePrefix + currentDataRow.PublicityCode] : "";
                    vxu.PD1.PublicityCode.NameOfCodingSystem.Value = currentDataRow.PublicityCode == "" ? "" : "HL70215";
                    vxu.PD1.PublicityCode.AlternateIdentifier.Value = "";
                    vxu.PD1.PublicityCode.AlternateText.Value = "";
                    vxu.PD1.PublicityCode.NameOfAlternateCodingSystem.Value = "";
                    vxu.PD1.ProtectionIndicator.Value = "";//currentDataRow.ProtectionIndicator;
                    vxu.PD1.ProtectionIndicatorEffectiveDate.Value = "";//currentDataRow.PIEffectiveDate;
                    vxu.PD1.ImmunizationRegistryStatus.Value = Hl7ImmunizationMeta.ImmunizationRegistryStatusCode[currentDataRow.ImmunizationRegistryStatusCode];
                    vxu.PD1.ImmunizationRegistryStatusEffectiveDate.Value = currentDataRow.ImmunizationRegistryStatusCode != "" ? currentDataRow.IRSEffectiveDate : "";
                    vxu.PD1.PublicityCodeEffectiveDate.Value = hasImmunizationRegistryStatus ? currentDataRow.PublicityCodeExpiryDate : "";
                }


                // Special Case for consented child
                if (currentDataRow.PIEffectiveDate != "")
                {
                    string protectionIndicatorCode = "";
                    switch (currentDataRow.ProtectionIndicator.ToLower())
                    {
                        case "false": protectionIndicatorCode = "N"; break;
                        case "true": protectionIndicatorCode = "Y"; break;
                    }

                    vxu.PD1.ProtectionIndicator.Value = protectionIndicatorCode;
                    vxu.PD1.ProtectionIndicatorEffectiveDate.Value = currentDataRow.ProtectionIndicator != "" ? currentDataRow.PIEffectiveDate : "";
                }


                // hard code values for case consented child as current immunization tab for document hx is missing those fields
                bool validCVXforThisCase = currentDataRow.CVXcode == "115" || currentDataRow.CVXcode == "118";
                if (currentDataRow.Type.ToLower() == "documenthx" && currentDataRow.Dose == "999" && validCVXforThisCase)
                {
                    vxu.PD1.ProtectionIndicator.Value = "N";
                    vxu.PD1.ProtectionIndicatorEffectiveDate.Value = DateTime.Now.ToString("yyyyMMdd");
                }

                #endregion

                #region NK1

                // ----------------------------------- NK1 ---------------------------------------------------

                if (dsPatient.Data.PatientFamily != null && patientFamily.Rows.Count > 0)
                {
                    //DSPatient.PatientFamilyRow patientFamilyRow = (from pf in patientFamily where pf.PatientId == patientId select pf).First();
                    var item = 0;
                    foreach (DSPatient.PatientFamilyRow patientFamilyRow in patientFamily)
                    {


                        var getNk1PhoneNumber = new
                        {
                            TelecommunicationUseCode = patientFamilyRow.HomePhoneNo != "" ? Hl7ImmunizationMeta.Telecommunicationusecode.PRN.ToString() : (patientFamilyRow.CellNo != "" ? Hl7ImmunizationMeta.Telecommunicationusecode.PRN.ToString() : ""),
                            TelecommunicationEquipmentType = patientFamilyRow.HomePhoneNo != "" ? "PH" : (patientFamilyRow.CellNo != "" ? "CP" : ""),
                            AreaCityCode = patientFamilyRow.HomePhoneNo != "" ? areaCityAndPhoneNo(patientFamilyRow.HomePhoneNo, 0) : (patientFamilyRow.CellNo != "" ? areaCityAndPhoneNo(patientFamilyRow.CellNo, 0) : ""),
                            LocalNumber = patientFamilyRow.HomePhoneNo != "" ? areaCityAndPhoneNo(patientFamilyRow.HomePhoneNo, 1).Replace("-", "").Replace(" ", "") : (patientFamilyRow.CellNo != "" ? areaCityAndPhoneNo(patientFamilyRow.CellNo, 1).Replace("-", "").Replace(" ", "") : ""),

                        };

                        var nk1Relationship = new
                        {
                            Identifier = patientFamilyRow.RelationShipName != "" ? Hl7ImmunizationMeta.RelationShipTextToCode[patientFamilyRow.RelationShipName] : "",
                            Text = patientFamilyRow.RelationShipName,
                            NameOfCodingSystem = patientFamilyRow.RelationShipName != "" ? "HL70063" : ""
                        };

                        vxu.GetNK1(item).SetIDNK1.Value = (item + 1).ToString();
                        vxu.GetNK1(item).GetName(0).FamilyName.Surname.Value = GetColumnValueFromRow(patientFamilyRow, "LastName");
                        vxu.GetNK1(item).GetName(0).GivenName.Value = GetColumnValueFromRow(patientFamilyRow, "FirstName");
                        vxu.GetNK1(item).GetName(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = GetColumnValueFromRow(patientFamilyRow, "MI");
                        vxu.GetNK1(item).GetName(0).NameTypeCode.Value = "L";
                        vxu.GetNK1(item).Relationship.Identifier.Value = nk1Relationship.Identifier;
                        vxu.GetNK1(item).Relationship.Text.Value = nk1Relationship.Text;
                        vxu.GetNK1(item).Relationship.NameOfCodingSystem.Value = nk1Relationship.NameOfCodingSystem;
                        vxu.GetNK1(item).Relationship.AlternateIdentifier.Value = "";
                        vxu.GetNK1(item).Relationship.AlternateText.Value = "";
                        vxu.GetNK1(item).Relationship.NameOfAlternateCodingSystem.Value = "";
                        vxu.GetNK1(item).GetAddress(0).StreetAddress.StreetOrMailingAddress.Value = patientFamilyRow.Address1;
                        vxu.GetNK1(item).GetAddress(0).OtherDesignation.Value = "";
                        vxu.GetNK1(item).GetAddress(0).City.Value = GetColumnValueFromRow(patientFamilyRow, "City");
                        vxu.GetNK1(item).GetAddress(0).StateOrProvince.Value = GetColumnValueFromRow(patientFamilyRow, "State");
                        vxu.GetNK1(item).GetAddress(0).ZipOrPostalCode.Value = GetColumnValueFromRow(patientFamilyRow, "ZipCode");
                        vxu.GetNK1(item).GetAddress(0).Country.Value = "USA";
                        vxu.GetNK1(item).GetAddress(0).AddressType.Value = "P";
                        vxu.GetNK1(item).GetPhoneNumber(0).TelecommunicationUseCode.Value = getNk1PhoneNumber.TelecommunicationUseCode;
                        vxu.GetNK1(item).GetPhoneNumber(0).TelecommunicationEquipmentType.Value = getNk1PhoneNumber.TelecommunicationEquipmentType;
                        vxu.GetNK1(item).GetPhoneNumber(0).EmailAddress.Value = "";//patientFamilyRow.EmailAddress;
                        vxu.GetNK1(item).GetPhoneNumber(0).AreaCityCode.Value = getNk1PhoneNumber.AreaCityCode;
                        vxu.GetNK1(item).GetPhoneNumber(0).LocalNumber.Value = getNk1PhoneNumber.LocalNumber;
                        item++;
                    }
                }

                #endregion
            }

            #endregion

            #region ORC
            // ----------------------------------- ORC ---------------------------------------------------
            vxu.GetORDER(iterationCounter).ORC.OrderControl.Value = "RE";
            if (currentDataRow.Type.ToLower() != "refusal")
            {
                vxu.GetORDER(iterationCounter).ORC.PlacerOrderNumber.EntityIdentifier.Value = currentDataRow.VaccineHxId;
                vxu.GetORDER(iterationCounter).ORC.PlacerOrderNumber.NamespaceID.Value = "NIST-AA-IZ-2";
                vxu.GetORDER(iterationCounter).ORC.PlacerOrderNumber.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.10";
                vxu.GetORDER(iterationCounter).ORC.PlacerOrderNumber.UniversalIDType.Value = "ISO";
            }
            

            if (currentDataRow.Type.ToLower() == "refusal")
            {
                vxu.GetORDER(iterationCounter).ORC.FillerOrderNumber.EntityIdentifier.Value = "9999";
                vxu.GetORDER(iterationCounter).ORC.FillerOrderNumber.NamespaceID.Value = "NIST-AA-IZ-2";
            }
            else
            {
                vxu.GetORDER(iterationCounter).ORC.FillerOrderNumber.EntityIdentifier.Value = "IZ-" + currentDataRow.VaccineHxId;
                vxu.GetORDER(iterationCounter).ORC.FillerOrderNumber.NamespaceID.Value = "NDA";
            }
                
            vxu.GetORDER(iterationCounter).ORC.FillerOrderNumber.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.10";
            vxu.GetORDER(iterationCounter).ORC.FillerOrderNumber.UniversalIDType.Value = "ISO";

            

                var enteredBy = (BLLClinicalObj.LookupVaccineGivenBy().Data.LookupVaccineGivenBy
                            .Where(g => g.UserId.ToString() == currentDataRow.GivenBy)
                            .Select(e => new
                            {
                                userId = e.UserId.ToString(),
                                surName = e.GivenBy.ToString().Split(',')[0],
                                givenName = e.GivenBy.ToString().Split(',')[1]
                            }
                            )).FirstOrDefault();

            DataTable Providers = BLLAdminProfileObj.LoadProvider(0, null, null, null, null, null, null, null).Data.Tables["Provider"];
            var orderingProvider = new
            {
                providerId = "",
                surName = "",
                givenName = "",
                NamespaceID = "",
                SecondAndFurtherGivenNamesOrInitialsThereof = "",
                NameTypeCode = "",
                IdentifierTypeCode = ""
            };

            if (Providers.Rows.Count > 0 && currentDataRow.ProviderId != "0")
            {
                orderingProvider = null;

                orderingProvider = (Providers.AsDataView().Table.AsEnumerable()
                                   .Where(p => p.Field<object>("ProviderId").ToString() == currentDataRow.ProviderId)
                                   .Select(p => new
                                   {
                                       providerId = GetColumnValueFromRow(p, "ProviderId"),
                                       surName = GetColumnValueFromRow(p, "LastName"),
                                       givenName = GetColumnValueFromRow(p, "firstName"),
                                       NamespaceID = "NIST-PI-1",
                                       SecondAndFurtherGivenNamesOrInitialsThereof = GetColumnValueFromRow(p, "MiddleInitial"),
                                       NameTypeCode = "L",
                                       IdentifierTypeCode = "MD"
                                   })
                                   ).FirstOrDefault();
            }

            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).IDNumber.Value = enteredBy == null ? "" : enteredBy.userId;
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).FamilyName.Surname.Value = enteredBy == null ? "" : enteredBy.surName;
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).GivenName.Value = enteredBy == null ? "" : enteredBy.givenName;
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = enteredBy == null ? "" : "A";
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).AssigningAuthority.NamespaceID.Value = enteredBy == null ? "" : "NIST-PI-1";
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).NameTypeCode.Value = "L";
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).IdentifierTypeCode.Value = "PRN";
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).AssigningAuthority.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.7";
            vxu.GetORDER(iterationCounter).ORC.GetEnteredBy(0).AssigningAuthority.UniversalIDType.Value = "ISO";

            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).IDNumber.Value = orderingProvider == null ? "" : orderingProvider.providerId;
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).FamilyName.Surname.Value = orderingProvider == null ? "" : orderingProvider.surName;
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).GivenName.Value = orderingProvider == null ? "" : orderingProvider.givenName;
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = orderingProvider == null ? "" : orderingProvider.SecondAndFurtherGivenNamesOrInitialsThereof; ;
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).AssigningAuthority.NamespaceID.Value = orderingProvider == null ? "" : orderingProvider.NamespaceID;
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).AssigningAuthority.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.7";
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).AssigningAuthority.UniversalIDType.Value = "ISO";
            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).NameTypeCode.Value = orderingProvider == null ? "" : orderingProvider.NameTypeCode;

            vxu.GetORDER(iterationCounter).ORC.GetOrderingProvider(0).IdentifierTypeCode.Value = orderingProvider == null ? "" : orderingProvider.IdentifierTypeCode;
            vxu.GetORDER(iterationCounter).ORC.EnteringOrganization.Identifier.Value = "NISTEHRFAC";
            vxu.GetORDER(iterationCounter).ORC.EnteringOrganization.Text.Value = "NISTEHRFacility";
            vxu.GetORDER(iterationCounter).ORC.EnteringOrganization.NameOfCodingSystem.Value = "HL70362";

            #endregion

            #region RXA
            // ----------------------------------- RXA ---------------------------------------------------

            var administeredUnits = new
            {
                Identifier = currentDataRow.Amount.ToLower() == "ml" ? "mL" : (currentDataRow.Amount.ToLower() == "mg" ? "mG" : ""),
                Text = currentDataRow.Amount.ToLower() == "ml" ? "mL" : (currentDataRow.Amount.ToLower() == "mg" ? "mG" : ""),
                NameOfCodingSystem = string.IsNullOrEmpty(currentDataRow.Amount) ? "" : "UCUM"
            };




            var VaccineInfo = (VaccineData.AsEnumerable()
                .Where(lt => GetColumnValueFromRow(lt, "VaccineID") == currentDataRow.Vaccine)
                .Select(lt => new
                {
                    NDCCode = GetColumnValueFromRow(lt, "NDCCode"),
                })
                ).FirstOrDefault();


            string completionStatus = currentDataRow.CompletionStatusCode != "" ? currentDataRow.CompletionStatusCode : "";

            vxu.GetORDER(iterationCounter).RXA.GiveSubIDCounter.Value = "0";
            vxu.GetORDER(iterationCounter).RXA.AdministrationSubIDCounter.Value = "1";
            vxu.GetORDER(iterationCounter).RXA.DateTimeStartOfAdministration.Time.Value = (currentDataRow.AdministrationStartDate);
            vxu.GetORDER(iterationCounter).RXA.DateTimeEndOfAdministration.Time.Value = "";
            if (currentDataRow.Type.ToUpper() == "ADMINISTER")
            {
                vxu.GetORDER(iterationCounter).RXA.AdministeredCode.Identifier.Value = VaccineInfo.NDCCode;
                vxu.GetORDER(iterationCounter).RXA.AdministeredCode.Text.Value = currentDataRow.CVXDescription;
                vxu.GetORDER(iterationCounter).RXA.AdministeredCode.NameOfCodingSystem.Value = currentDataRow.CVXcode == "" ? "" : "NDC";
            }
            else
            {
                vxu.GetORDER(iterationCounter).RXA.AdministeredCode.Identifier.Value = currentDataRow.CVXcode;
                vxu.GetORDER(iterationCounter).RXA.AdministeredCode.Text.Value = currentDataRow.CVXDescription;
                vxu.GetORDER(iterationCounter).RXA.AdministeredCode.NameOfCodingSystem.Value = currentDataRow.CVXcode == "" ? "" : "CVX";
            }



            vxu.GetORDER(iterationCounter).RXA.AdministeredCode.AlternateIdentifier.Value = "";
            vxu.GetORDER(iterationCounter).RXA.AdministeredCode.AlternateText.Value = "";
            vxu.GetORDER(iterationCounter).RXA.AdministeredCode.NameOfAlternateCodingSystem.Value = "";

            vxu.GetORDER(iterationCounter).RXA.AdministeredAmount.Value = currentDataRow.Dose;
            vxu.GetORDER(iterationCounter).RXA.AdministeredUnits.Identifier.Value = administeredUnits.Identifier;
            vxu.GetORDER(iterationCounter).RXA.AdministeredUnits.Text.Value = administeredUnits.Text;
            vxu.GetORDER(iterationCounter).RXA.AdministeredUnits.NameOfCodingSystem.Value = administeredUnits.NameOfCodingSystem;
            vxu.GetORDER(iterationCounter).RXA.AdministeredUnits.AlternateIdentifier.Value = "";
            vxu.GetORDER(iterationCounter).RXA.AdministeredUnits.AlternateText.Value = "";
            vxu.GetORDER(iterationCounter).RXA.AdministeredUnits.NameOfAlternateCodingSystem.Value = "";



            if (currentDataRow.CVXcode != "998")
            {


                var administerNotes = (VaccineHxData
                                      .Where(vhx => vhx.VaccineHxId == CurrentVaccineHxId)
                                      .Select(vhx => new { vhx, isAdminister = vhx.Type.ToLower() == "administer" })
                                      .Select(@t => new
                                      {
                                          Identifier = @t.isAdminister ? "00" : "01",
                                          Text = @t.isAdminister ? "New Record" : "Historical Administration",
                                          NameOfCoding = "NIP001"
                                      })).First();

                var substanceRefusalReason = new
                {
                    Identifier = currentDataRow.RefusalReasonCode,
                    Text = !string.IsNullOrEmpty(currentDataRow.RefusalReasonCode) ? Hl7ImmunizationMeta.RefusalReason[currentDataRow.RefusalReasonCode] : "",
                    NameOfCodingSystem = !string.IsNullOrEmpty(currentDataRow.RefusalReasonCode) ? "NIP002" : ""
                };


                var Manufacturer = new
                {
                    Identifier = "",
                    Text = "",
                    NameOfCodingSystem = ""
                };

                if (!string.IsNullOrWhiteSpace(currentDataRow.Manufacturer))
                {
                    Manufacturer = (ManufacturerData.AsEnumerable()
                                   .Where(md => GetColumnValueFromRow(md, "ManufacturerId") == currentDataRow.Manufacturer)
                                   .Select(md => new
                                   {
                                       Identifier = GetColumnValueFromRow(md, "ManufacturerMVXCODE"),
                                       Text = GetColumnValueFromRow(md, "ManufacturerName"),
                                       NameOfCodingSystem = "MVX"
                                   })).FirstOrDefault();
                }

                var Lot = (VaccineLotNoData.AsEnumerable()
                          .Where(lt => GetColumnValueFromRow(lt, "VaccineLotNoId") == currentDataRow.LotNumber)
                          .Select(lt => new
                          {
                              LotNo = GetColumnValueFromRow(lt, "LotNo"),
                          })
                          ).FirstOrDefault();
                if (currentDataRow.Type.ToLower() == "refusal")
                {
                    vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).Identifier.Value ="";
                    vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).Text.Value = "";
                    vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).NameOfCodingSystem.Value = "";
                }
                else
                {
                    vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).Identifier.Value = administerNotes.Identifier;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).Text.Value = administerNotes.Text;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).NameOfCodingSystem.Value = administerNotes.NameOfCoding;
                }

                vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetAdministrationNotes(0).NameOfAlternateCodingSystem.Value = "";
                if (administerNotes.Identifier == "00")
                {
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).IDNumber.Value = orderingProvider == null ? "" : orderingProvider.providerId;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).FamilyName.Surname.Value = orderingProvider == null ? "" : orderingProvider.surName;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).GivenName.Value = orderingProvider == null ? "" : orderingProvider.givenName;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = orderingProvider == null ? "" : orderingProvider.SecondAndFurtherGivenNamesOrInitialsThereof;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).AssigningAuthority.NamespaceID.Value = orderingProvider == null ? "" : orderingProvider.NamespaceID;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).NameTypeCode.Value = orderingProvider == null ? "" : orderingProvider.NameTypeCode;
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).IdentifierTypeCode.Value = orderingProvider == null ? "" : "PRN";

                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).AssigningAuthority.NamespaceID.Value = "NIST-PI-1";
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).AssigningAuthority.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.7";
                    vxu.GetORDER(iterationCounter).RXA.GetAdministeringProvider(0).AssigningAuthority.UniversalIDType.Value = "ISO";
                }


                if (administerNotes.Identifier == "00")
                {
                    vxu.GetORDER(iterationCounter).RXA.AdministeredAtLocation.Facility.NamespaceID.Value = "NIST-Clinic-1";
                }
                vxu.GetORDER(iterationCounter).RXA.AdministeredAtLocation.Facility.UniversalID.Value = "2.16.840.1.113883.3.72.5.40.12";
                vxu.GetORDER(iterationCounter).RXA.AdministeredAtLocation.Facility.UniversalIDType.Value = "ISO";
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceLotNumber(0).Value = Lot == null ? "" : Lot.LotNo; // currentDataRow.LotNumber;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceExpirationDate(0).Time.Value = currentDataRow.ExpiryDate;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceManufacturerName(0).Identifier.Value = Manufacturer.Identifier;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceManufacturerName(0).Text.Value = Manufacturer.Text;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceManufacturerName(0).NameOfCodingSystem.Value = Manufacturer.NameOfCodingSystem;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceManufacturerName(0).AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceManufacturerName(0).AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceManufacturerName(0).NameOfAlternateCodingSystem.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).Identifier.Value = substanceRefusalReason.Identifier;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).Text.Value = substanceRefusalReason.Text;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).NameOfCodingSystem.Value = substanceRefusalReason.NameOfCodingSystem;
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).NameOfAlternateCodingSystem.Value = "";

                // hard code values for case refused toddler as current immunization tab for refusal is missing those fields
                if (currentDataRow.Type.ToLower() == "refusal")
                {
                    vxu.GetORDER(iterationCounter).RXA.DateTimeStartOfAdministration.Time.Value = DateTime.Now.ToString("yyyyMMdd");
                    vxu.GetORDER(iterationCounter).RXA.AdministeredAmount.Value = "999";
                    vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).Identifier.Value = "00";
                    vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).Text.Value = "Parental Refusal";
                    vxu.GetORDER(iterationCounter).RXA.GetSubstanceTreatmentRefusalReason(0).NameOfCodingSystem.Value = "NIP002";
                }

            }

            var VaccineFundingSource = (VaccineLotNoData.AsEnumerable()
                     .Where(lt => GetColumnValueFromRow(lt, "VaccineLotNoId") == currentDataRow.LotNumber)
                     .Select(lt => new
                     {
                         Description = GetColumnValueFromRow(lt, "Description"),
                         Code = GetColumnValueFromRow(lt, "Code")
                     })
                     ).FirstOrDefault();

            string CompletionStatusCode = Hl7ImmunizationMeta.VaccineType_To_CompletionStatus[currentDataRow.Type.ToUpper()];
            vxu.GetORDER(iterationCounter).RXA.CompletionStatus.Value = CompletionStatusCode;
            if (currentDataRow.VoidDose.ToString().ToLower().Trim()=="true")
            {
                vxu.GetORDER(iterationCounter).RXA.ActionCodeRXA.Value = "D";
            }
            else
            {
                if (currentDataRow.IsUpdatedRecord.ToString().ToLower().Trim() == "true")
                {
                    vxu.GetORDER(iterationCounter).RXA.ActionCodeRXA.Value = "U";
                }
                else
                {
                    vxu.GetORDER(iterationCounter).RXA.ActionCodeRXA.Value = "A";
                }
                
            }

            #endregion

            #region RXR
            // ----------------------------------- RXR ---------------------------------------------------


            Dictionary<string, string> RouteHL70162_To_NCIT = new Dictionary<string, string>
            {
              { "ID"  ,  "C38238" } ,
              { "IM"  ,  "C28161" } ,
              { "NS"  ,  "C38284" } ,
              { "IV"  ,  "C38276" } ,
              { "PO"  ,  "C38288" } ,
              { "OTH" ,  "C28161" } ,
              { "SC"  ,  "C38299" } ,
              { "TD"  ,  "C38305" }
            };

            var rxrRoute = new
            {
                Identifier = !string.IsNullOrEmpty(currentDataRow.HL7Code0162Route) ? RouteHL70162_To_NCIT[currentDataRow.HL7Code0162Route] : "",
                Text = currentDataRow.DescriptionRoute,
                NameOfCodingSystem = currentDataRow.HL7Code0162Route == "" ? "" : "NCIT"
            };


            if (!string.IsNullOrEmpty(currentDataRow.Route) && string.IsNullOrEmpty(currentDataRow.RefusalReasonCode))
            {
                vxu.GetORDER(iterationCounter).RXR.Route.Identifier.Value = rxrRoute.Identifier;
                vxu.GetORDER(iterationCounter).RXR.Route.Text.Value = rxrRoute.Text;
                vxu.GetORDER(iterationCounter).RXR.Route.NameOfCodingSystem.Value = rxrRoute.NameOfCodingSystem;
                vxu.GetORDER(iterationCounter).RXR.Route.AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).RXR.Route.AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).RXR.Route.NameOfAlternateCodingSystem.Value = "";
                vxu.GetORDER(iterationCounter).RXR.AdministrationSite.Identifier.Value = currentDataRow.HL7Code0162Site;
                vxu.GetORDER(iterationCounter).RXR.AdministrationSite.Text.Value = currentDataRow.DescriptionSite;
                vxu.GetORDER(iterationCounter).RXR.AdministrationSite.NameOfCodingSystem.Value = currentDataRow.HL7Code0162Site == "" ? "" : "HL70163";
                vxu.GetORDER(iterationCounter).RXR.AdministrationSite.AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).RXR.AdministrationSite.AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).RXR.AdministrationSite.NameOfAlternateCodingSystem.Value = "";
            }

            #endregion

            #region OBX
            // ----------------------------------- OBX ---------------------------------------------------
            bool isVfCgiven = !string.IsNullOrEmpty(currentDataRow.VFC) && currentDataRow.VFC != "0";
            bool noRefusalReason = string.IsNullOrEmpty(currentDataRow.RefusalReasonCode);

            var observationMethodObx1 = new
            {
                Identifier = "VXC40",
                Text = "per immunization",
                NameOfCodingSystem = "CDCPHINVS",
            };


            if (isVfCgiven && noRefusalReason && currentDataRow.CVXcode != "998")
            {
                {
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.SetIDOBX.Value = (OBXSegmentCounter + 1).ToString();
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ValueType.Value = "CE";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Identifier.Value = "30963-3";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Text.Value = "Vaccine Funding Source";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationSubID.Value = "1";
                    NHapi.Model.V25.Datatype.CE vxuCe1 = new NHapi.Model.V25.Datatype.CE(vxu);
                    vxuCe1.Identifier.Value = VaccineFundingSource.Code;
                    vxuCe1.Text.Value = VaccineFundingSource.Description;
                    vxuCe1.NameOfCodingSystem.Value = currentDataRow.ConceptCodeVFC == "" ? "" : "CDCPHINVS";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationValue(0).Data = vxuCe1;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationResultStatus.Value = "F";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.DateTimeOfTheObservation.Time.Value = (currentDataRow.AdministrationStartDate);
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfAlternateCodingSystem.Value = "";
                }
                OBXSegmentCounter = OBXSegmentCounter + 1;
                // ------ First OBX --------------
                {
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.SetIDOBX.Value = (OBXSegmentCounter + 1).ToString();
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ValueType.Value = "CE";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Identifier.Value = "64994-7";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Text.Value = "Vaccine Funding Program Eligibility";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationSubID.Value = "2";
                    NHapi.Model.V25.Datatype.CE vxuCe = new NHapi.Model.V25.Datatype.CE(vxu);
                    vxuCe.Identifier.Value = currentDataRow.ConceptCodeVFC;
                    vxuCe.Text.Value = currentDataRow.ConceptNameVFC;
                    vxuCe.NameOfCodingSystem.Value = currentDataRow.ConceptCodeVFC == "" ? "" : "HL70064";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationValue(0).Data = vxuCe;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationResultStatus.Value = "F";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.DateTimeOfTheObservation.Time.Value = (currentDataRow.AdministrationStartDate);
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Identifier.Value = observationMethodObx1.Identifier;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Text.Value = observationMethodObx1.Text;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfCodingSystem.Value = observationMethodObx1.NameOfCodingSystem;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfAlternateCodingSystem.Value = "";
                }


                var VaccineVISData1 = (VaccineVISData.AsEnumerable()
                          .Where(lt => GetColumnValueFromRow(lt, "VaccineId") == currentDataRow.Vaccine)
                          .Select(lt => new
                          {
                              VIS_DocumentName = GetColumnValueFromRow(lt, "VIS_DocumentName"),
                              VIS_EditionDate = GetColumnValueFromRow(lt, "VIS_EditionDate"),
                              VIS_FullyEncodedtextstring = GetColumnValueFromRow(lt, "VIS_FullyEncodedtextstring")
                          })
                          ).ToArray();


                int noOfVaccineVISData = VaccineVISData1.Count();

                for (int indexCategory = 0; indexCategory < noOfVaccineVISData; indexCategory++)
                {
                    OBXSegmentCounter = OBXSegmentCounter + 1;
                    // ------ 2nd OBX --------------
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.SetIDOBX.Value = (OBXSegmentCounter + 1).ToString();
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ValueType.Value = "CE";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Identifier.Value = "69764-9";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Text.Value = "Document Type";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationSubID.Value = (indexCategory + 3).ToString();
                    NHapi.Model.V25.Datatype.CE vxuCe2 = new NHapi.Model.V25.Datatype.CE(vxu);
                    vxuCe2.Identifier.Value = VaccineVISData1[indexCategory].VIS_FullyEncodedtextstring; //currentDataRow.UncertainFormulationCVX;





                    vxuCe2.Text.Value = VaccineVISData1[indexCategory].VIS_DocumentName;
                    vxuCe2.NameOfCodingSystem.Value = "cdcgs1vis";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationValue(0).Data = vxuCe2;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationResultStatus.Value = "F";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.DateTimeOfTheObservation.Time.Value = (currentDataRow.AdministrationStartDate);
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfAlternateCodingSystem.Value = "";

                    OBXSegmentCounter = OBXSegmentCounter + 1;

                    // ------ third OBX --------------       

                    //var VIS = (VaccineVISData.AsDataView().Table.AsEnumerable()
                    //          .Where(vis => vis.Field<object>(ColumnName.CVX_codeVIS).ToString() == currentDataRow.CVXcode)
                    //          //.OrderByDescending(vis => vis.Field<object>(ColumnName.VIS_EditionStatus).ToString())
                    //          //.ThenBy(vis => vis.Field<object>(ColumnName.VIS_EditionDate))
                    //          .Select(vis => new
                    //          {
                    //              VIS_Edition_Date = GetColumnValueFromRow(vis, ColumnName.VIS_EditionDate, true)

                    //          })
                    //          ).FirstOrDefault();


                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.SetIDOBX.Value = (OBXSegmentCounter + 1).ToString();
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ValueType.Value = "DT";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Identifier.Value = "29769-7";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Text.Value = "Date Vis Presented";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationSubID.Value = (indexCategory + 3).ToString();
                    NHapi.Model.V25.Datatype.TS vxuTs3 = new NHapi.Model.V25.Datatype.TS(vxu);
                    vxuTs3.Time.Value = (currentDataRow.AdministrationStartDate);
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationValue(0).Data = vxuTs3;
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfAlternateCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationResultStatus.Value = "F";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.DateTimeOfTheObservation.Time.Value = (currentDataRow.AdministrationStartDate);
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Identifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Text.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfCodingSystem.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateIdentifier.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateText.Value = "";
                    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfAlternateCodingSystem.Value = "";


                    //// start special case for Complete Record IZ 7 Hard coded
                    //if (noOfCategories > 1)
                    //{
                    //    switch (UncertainFormulationCVXList[indexCategory])
                    //    {
                    //        case "107": vxuTs3.Time.Value = "20070517"; break;
                    //        case "89": vxuTs3.Time.Value = "20111108"; break;
                    //        case "17": vxuTs3.Time.Value = "19981216"; break;
                    //        case "45": vxuTs3.Time.Value = "20120202"; break;
                    //    }
                    //    vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationValue(0).Data = vxuTs3;
                    //}
                    //// end   special case for Complete Record Hard coded


                    //OBXSegmentCounter = OBXSegmentCounter + 1;
                    //// ------ 4th OBX --------------
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.SetIDOBX.Value = (OBXSegmentCounter + 1).ToString();
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ValueType.Value = "TS";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Identifier.Value = "29769-7";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Text.Value = "Date vaccine information statement presented";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateIdentifier.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateText.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfAlternateCodingSystem.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationSubID.Value = (indexCategory + 2).ToString();
                    //NHapi.Model.V25.Datatype.TS vxuTs4 = new NHapi.Model.V25.Datatype.TS(vxu);
                    //vxuTs4.Time.Value = currentDataRow.CreatedOn;
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationValue(0).Data = vxuTs4;
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Identifier.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Text.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfCodingSystem.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateIdentifier.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateText.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfAlternateCodingSystem.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationResultStatus.Value = "F";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.DateTimeOfTheObservation.Time.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Identifier.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Text.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfCodingSystem.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateIdentifier.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateText.Value = "";
                    //vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfAlternateCodingSystem.Value = "";
                }

            }

            if (currentDataRow.CVXcode == "998") //if (isVfCgiven && currentDataRow.CVXcode == "998")
            {
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.SetIDOBX.Value = (OBXSegmentCounter + 1).ToString();
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ValueType.Value = "CE";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Identifier.Value = "59784-9";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.Text.Value = "Disease with presumed immunity";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfCodingSystem.Value = "LN";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationIdentifier.NameOfAlternateCodingSystem.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationSubID.Value = "1";
                NHapi.Model.V25.Datatype.CE vxuCe = new NHapi.Model.V25.Datatype.CE(vxu);
                vxuCe.Identifier.Value = "38907003";//currentDataRow.ConceptCodeVFC;
                vxuCe.Text.Value = "Varicella infection";//currentDataRow.ConceptNameVFC;
                vxuCe.NameOfCodingSystem.Value = "SCT";
                vxu.GetORDER(0).GetOBSERVATION(0).OBX.GetObservationValue(0).Data = vxuCe;
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Identifier.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.Text.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfCodingSystem.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.Units.NameOfAlternateCodingSystem.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.ObservationResultStatus.Value = "F";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.DateTimeOfTheObservation.Time.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Identifier.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).Text.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfCodingSystem.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateIdentifier.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).AlternateText.Value = "";
                vxu.GetORDER(iterationCounter).GetOBSERVATION(OBXSegmentCounter).OBX.GetObservationMethod(0).NameOfAlternateCodingSystem.Value = "";
            }

            #endregion

        }


        public string GetFormattecdValue(string value)
        {
            return (string.IsNullOrWhiteSpace(value) ? "" : value);
        }

        public string GetColumnValueFromRow(DataRow dr, string columnName, bool IsDate = false)
        {
            try
            {
                return !(dr == null || (dr[columnName] is DBNull))
                                  ? (!IsDate
                                      ? dr[columnName].ToString()
                                      : ((dr[columnName] is DateTime)
                                          ? ((DateTime)dr[columnName]).ToString("yyyyMMdd")
                                          : string.Empty))
                                  : string.Empty;
            }
            catch (Exception exp)
            {
                string message = exp.Message;
                return string.Empty;
            }
        }

        public void Write_HL7MessageInFile(string message)
        {
            string destination = @"K:\immkujklnization\hjjl7\generkatedFiles\" + CurrentVaccineHxId.ToString() + DateTime.Now.ToString("yyyyMMddmmss") + ".txt";
            message = message.Replace("\r", "\r\n");

            using (StreamWriter sw = File.CreateText(destination))
            {
                sw.Write(message);
            }
        }

        public string ReorderObxWithAccordanceToTestCaseGeneration(string message)
        {
            if (message.Contains("OBX|10"))
            {
                string test = "";
                string OBXwith10Entries = "";
                string OBXwith4Entries = "";
                string DocumentHxRecord = "";
                string GeneralSegments = "";
                string OBX1Varicella = "";

                string[] allobx = message.Split(new string[] { "OBX" }, StringSplitOptions.None);

                string[] ORCSegments = message.Split(new string[] { "ORC" }, StringSplitOptions.None);

                foreach (var segments in ORCSegments)
                {
                    if (segments.Contains("OBX|10|"))
                    {
                        OBXwith10Entries = "ORC" + segments;
                    }
                    else if (!segments.Contains("OBX|10|") && segments.Contains("OBX|4|") &&
                             segments.Contains("00^New immunization"))
                    {
                        OBXwith4Entries = "ORC" + segments;
                    }

                    else if (segments.Contains("01^Historical information - source unspecified^") &&
                             !segments.Contains("Varicella infection"))
                    {
                        DocumentHxRecord = "ORC" + segments;
                    }
                    else if (segments.Contains("MSH") && segments.Contains("PID") &&
                             segments.Contains("VXU^V04^VXU_V04"))
                    {
                        GeneralSegments = segments;
                    }
                }

                string reorderedMessage = GeneralSegments + OBXwith4Entries + DocumentHxRecord + OBXwith10Entries;

                return reorderedMessage;
            }
            return message;
        }


        public string Get_HL7Immunization_MessageGeneration_Response(string message)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var response = new
            {
                status = true,
                Message = "HL7 Message(s) generated successfully",
                HL7Message = message
            };
            return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }



        #region  Helper Methods

        #endregion

    }

    #region MetaData

    public static class Relationship
    {
        public static readonly string Brother = "BRO";
        public static readonly string Caregiver = "CGV";
        public static readonly string Fosterchild = "FCH";
        public static readonly string Father = "FTH";
        public static readonly string Guardian = "GRD";
        public static readonly string Grandparent = "GRP";
        public static readonly string Mother = "MTH";
        public static readonly string Other = "OTH";
        public static readonly string Parent = "PAR";
        public static readonly string Stepchild = "SCH";
        public static readonly string Self = "SEL";
        public static readonly string Sibling = "SIB";
        public static readonly string Sister = "SIS";
        public static readonly string Spouse = "SPO";

    }
    public static class Hl7ImmunizationMeta
    {
        public enum Telecommunicationusecode
        {
            PRN,
            ORN,
            WPN,
            VHN,
            ASN,
            EMR,
            NET,
            BPN
        };

        public static readonly Dictionary<string, string> PublicityCode = new Dictionary<string, string>
        {
            {"01", "No reminder/recall"                   },
            {"02", "Reminder/recall - any method"         },
            {"03", "Reminder/recall - no calls"           },
            {"04", "Reminder only - any method"           },
            {"05", "Reminder only - no calls"             },
            {"06", "Recall only - any method"             },
            {"07", "Recall only - no calls"               },
            {"08", "Reminder/recall - to provider"        },
            {"09", "Reminder to provider"                 },
            {"10", "Only reminder to provider, no recall" },
            {"11", "Recall to provider"                   },
            {"12", "Only recall to provider, no reminder" },
        };

        public static readonly Dictionary<string, string> RefusalReason = new Dictionary<string, string>
        {
           {"00"  , "Parental decision"      } ,
           {"01"  , "Religious exemption"    } ,
           {"02"  , "Other"                  } ,
           {"03"  , "Patient decision"       }
        };

        public static readonly Dictionary<string, string> CompletionStatus = new Dictionary<string, string>
        {
          { "CP"  ,  "Complete"                     }    ,
          { "RE"  ,  "Refused"                      }    ,
          { "NA"  ,  "Not Administered"             }    ,
          { "PA"  ,  "Partially Administered"       }
       };

        public static readonly Dictionary<string, string> RelationShipTextToCode = new Dictionary<string, string>
        {
         {  "Brother"            ,    "BRO"    }   ,
         {  "Care giver"         ,    "CGV"    }   ,
         {  "Foster child"       ,    "FCH"    }   ,
         {  "Father"             ,    "FTH"    }   ,
         {  "Guardian"           ,    "GRD"    }   ,
         {  "Grandparent"        ,    "GRP"    }   ,
         {  "Mother"             ,    "MTH"    }   ,
         {  "Other"              ,    "OTH"    }   ,
         {  "Parent"             ,    "PAR"    }   ,
         {  "Stepchild"          ,    "SCH"    }   ,
         {  "Self"               ,    "SEL"    }   ,
         {  "Sibling"            ,    "SIB"    }   ,
         {  "Sister"             ,    "SIS"    }   ,
         {  "Spouse"             ,    "SPO"    }
       };

        public static Dictionary<string, KeyValuePair<string, string>> RelationshipCodeToDescription = new Dictionary<string, KeyValuePair<string, string>>
        {
            { "1"  ,   new KeyValuePair<string, string> ("01", "Spouse"                                               )} ,
            { "2"  ,   new KeyValuePair<string, string> ("04", "Grandfather or Grandmother"                         )} ,
            { "3"  ,   new KeyValuePair<string, string> ("05", "Grandson or Granddaughter"                          )} ,
            { "4"  ,   new KeyValuePair<string, string> ("07", "Nephew or Niece"                                        )} ,
            { "5"  ,   new KeyValuePair<string, string> ("10", "Foster child"                                           )} ,
            { "6"  ,   new KeyValuePair<string, string> ("15", "Ward"                                                   )} ,
            { "7"  ,   new KeyValuePair<string, string> ("17", "Stepson or Stepdaughter"                                )} ,
            { "8"  ,   new KeyValuePair<string, string> ("18", "Self"                                                   )} ,
            { "9"  ,   new KeyValuePair<string, string> ("19", "Child"                                              )} ,
            { "10" ,   new KeyValuePair<string, string> ("20", "Employee"                                               )} ,
            { "11" ,   new KeyValuePair<string, string> ("21", "Unknown"                                                )} ,
            { "12" ,   new KeyValuePair<string, string> ("22", "Handicapped Dependent"                              )} ,
            { "13" ,   new KeyValuePair<string, string> ("23", "Sponsored Dependent"                                    )} ,
            { "14" ,   new KeyValuePair<string, string> ("24", "Dependent of a minor dependent"                     )} ,
            { "15" ,   new KeyValuePair<string, string> ("29", "Significant Other"                                  )} ,
            { "16" ,   new KeyValuePair<string, string> ("32", "Mother"                                             )} ,
            { "17" ,   new KeyValuePair<string, string> ("33", "Father"                                             )} ,
            { "18" ,   new KeyValuePair<string, string> ("36", "Emancipated Minor"                                  )} ,
            { "19" ,   new KeyValuePair<string, string> ("39", "Organ Donor"                                            )} ,
            { "20" ,   new KeyValuePair<string, string> ("40", "Cadaver Donor"                                      )} ,
            { "21" ,   new KeyValuePair<string, string> ("41", "Injured Plaintiff"                                  )} ,
            { "22" ,   new KeyValuePair<string, string> ("43", "Child where insured has no financial responsibility"    )} ,
            { "23" ,   new KeyValuePair<string, string> ("53", "Life Partner"                                           )} ,
            { "24" ,   new KeyValuePair<string, string> ("G8", "Other Relationship"                                 )} ,
        };

        public static readonly Dictionary<string, string> ImmunizationRegistryStatus = new Dictionary<string, string>
        {
          {    "A"  ,   "Active"                                                                                       } ,
          {    "I"  ,   "Inactive--Unspecified"                                                                      } ,
          {    "L"  ,   "Inactive-Lost to follow-up (cannot contact)"                                                    } ,
          {    "M"  ,   "Inactive-Moved or gone elsewhere (transferred)"                                                 } ,
          {    "P"  ,   "Inactive-Permanently inactive (do not re-activate or add new entries to this record)"       } ,
          {    "U"  ,   "Unknown"                                                                                        }
       };


        // use in RXA-21
        public static readonly Dictionary<string, string> ActionCode = new Dictionary<string, string>
        {
          {    "A"  ,   "Add"     } ,
          {    "D"  ,   "Delete"  } ,
          {    "U"  ,   "Update"  } ,
       };

        public static readonly Dictionary<string, string> VaccineType_To_CompletionStatus = new Dictionary<string, string>
        {
            {"ADMINISTER" , "CP" },
            {"DOCUMENTHX" , "CP" },
            {"REFUSAL"    , "RE" },
            {""           , ""}
        };

        // use in PD1-16
        public static readonly Dictionary<string, string> ImmunizationRegistryStatusCode = new Dictionary<string, string>
        {
           { "1" , "A" },
           { "2" , "I" },
           { "3" , "L" },
           { "4" , "M" },
           { "5" , "P" },
           { "6" , "U" },
           { ""  ,  "" }
        };




    }

    public static class ColumnName
    {
        // -------------------------- Vaccine ------------------------------
        public static readonly string VaccineID = "VaccineID";
        public static readonly string CVXShortDescriptionVaccine = "CVXShortDescription";
        public static readonly string CVXCodeVaccine = "CVXCode";
        public static readonly string VaccineStatusVaccine = "VaccineStatus";
        public static readonly string UncertainFormulationCVXVaccine = "UncertainFormulationCVX";
        public static readonly string VaccineGroupIDVaccine = "VaccineGroupID";
        // -------------------------- VaccineGroup ------------------------------       
        public static readonly string VaccineGroupIDVaccineGroup = "VaccineGroupID";
        public static readonly string ShortNameVaccineGroup = "ShortName";
        // -------------------------- VaccineHx ------------------------------
        public static readonly string VaccineHxId = "VaccineHxId";
        public static readonly string VaccineGroupCategory = "VaccineGroupCategory";
        public static readonly string Vaccine = "Vaccine";
        public static readonly string LotNumber = "LotNumber";
        public static readonly string Route = "Route";
        public static readonly string VFC = "VFC";
        public static readonly string Comments = "Comments";
        public static readonly string VisitDate = "VisitDate";
        public static readonly string AdministrationDate = "AdministrationDate";
        public static readonly string Dose = "Dose";
        public static readonly string Amount = "Amount";
        public static readonly string Site = "Site";
        public static readonly string VISDate = "VISDate";
        public static readonly string VoidDose = "VoidDose";
        public static readonly string IsUpdatedRecord = "IsUpdatedRecord";
        public static readonly string BirthIndicator = "BirthIndicator";
        public static readonly string BirthOrder = "BirthOrder";
        public static readonly string GivenBy = "GivenBy";
        public static readonly string Manufacturer = "Manufacturer";
        public static readonly string ExpiryDate = "ExpiryDate";
        public static readonly string SourceOfHx = "SourceOfHx";
        public static readonly string Type = "Type";
        public static readonly string PatientID = "PatientID";
        public static readonly string ISActive = "ISActive";
        public static readonly string CreatedBy = "CreatedBy";
        public static readonly string CreatedOn = "CreatedOn";
        public static readonly string ModifiedOn = "ModifiedOn";
        public static readonly string ModifiedBy = "ModifiedBy";
        public static readonly string ProviderId = "ProviderId";
        public static readonly string CompletionStatusCode = "CompletionStatusCode";
        public static readonly string RefusalReasonCode = "RefusalReasonCode";
        public static readonly string PublicityCode = "PublicityCode";
        public static readonly string PublicityCodeExpiryDate = "PublicityCodeExpiryDate";
        public static readonly string ImmunizationRegistryStatusCode = "ImmunizationRegistryStatusCode";
        public static readonly string IRSEffectiveDate = "IRSEffectiveDate";
        public static readonly string ProtectionIndicator = "ProtectionIndicator";
        public static readonly string PIEffectiveDate = "PIEffectiveDate";
        public static readonly string RefusalReasonId = "RefusalReasonId";
        // ------------------------- Manufacturer -------------------------------
        public static readonly string VaccineManufacturerId = "VaccineManufacturerId";
        public static readonly string ProductName = "ProductName";
        public static readonly string CVXCodeManufacturer = "CVXCode";
        public static readonly string ManufacturerName = "ManufacturerName";
        public static readonly string ManufacturerMVXCODE = "ManufacturerMVXCODE";
        public static readonly string MVXStatus = "MVXStatus";
        public static readonly string ProductNameStatus = "ProductNameStatus";
        public static readonly string ProductUpdateDate = "ProductUpdateDate";
        // ---------------------------- Route ----------------------------
        public static readonly string VaccineRouteID = "VaccineRouteID";
        public static readonly string HL7Code0162Route = "HL7Code0162";
        public static readonly string DescriptionRoute = "Description";
        // ------------------------- Site -------------------------------
        public static readonly string VaccineSiteID = "VaccineSiteID";
        public static readonly string HL7Code0162Site = "HL7Code0162";
        public static readonly string DescriptionSite = "Description";
        // ------------------------ Source of Hx --------------------------------
        public static readonly string VaccineSourceOfHxId = "VaccineSourceOfHxId";
        public static readonly string ValueVaccineSourceOfHx = "Value";
        public static readonly string DescriptionVaccineSourceOfHx = "Description";
        // ----------------------------- VFC ---------------------------
        public static readonly string VaccineVFCId = "VaccineVFCId";
        public static readonly string ConceptCodeVFC = "ConceptCode";
        public static readonly string ConceptNameVFC = "ConceptName";
        public static readonly string PreferredConceptNameVFC = "PreferredConceptName";
        public static readonly string PreferredAlternateCodeVFC = "PreferredAlternateCode";
        public static readonly string CodeSystemOIDVFC = "CodeSystemOID";
        public static readonly string CodeSystemNameVFC = "CodeSystemName";
        public static readonly string CodeSystemCodeVFC = "CodeSystemCode";
        public static readonly string CodeSystemVersionVFC = "CodeSystemVersion";
        public static readonly string HL7Table0396CodeVFC = "HL7Table0396Code";
        // ---------------------------- VIS ----------------------------
        public static readonly string VaccineVISId = "VaccineVISId";
        public static readonly string CVX_codeVIS = "CVX_code";
        public static readonly string VIS_DocumentName = "VIS_DocumentName";
        public static readonly string VIS_FullyEncodedtextstring = "VIS_FullyEncodedtextstring";
        public static readonly string VIS_EditionDate = "VIS_EditionDate";
        public static readonly string VIS_EditionStatus = "VIS_EditionStatus";
        // ---------------------------- VIS URL ----------------------------
        public static readonly string VaccineVIS_URLId = "VaccineVIS_URLId";
        public static readonly string DocumentTypeDescription_ConceptNameVIS_URL = "DocumentTypeDescription_ConceptName";
        public static readonly string GDTIdocumentcode_Conceptcode = "GDTIdocumentcode_Conceptcode";
        public static readonly string PDFTitle = "PDFTitle";
        public static readonly string PDFDirectLink = "PDFDirectLink";
        public static readonly string PDFURL = "PDFURL";
        public static readonly string HTMLTitle = "HTMLTitle";
        public static readonly string HTMLDirectLink = "HTMLDirectLink";
        public static readonly string HTMLURL = "HTMLURL";

    }

    public static class SegmentRules
    {

    }

    #endregion

}


