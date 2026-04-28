using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using MDVision.DataAccess.DAL.Clinical;
using System;
using MDVision.DataAccess.DAL.Schedule;
using MDVision.DataAccess.DAL.Appointment;
using MDVision.DataAccess.DAL.Patient;
using System.IO;
using iTextSharp.text;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using MDVision.DataAccess.DCommon; using MDVision.Common.Shared;
using iTextSharp.text.pdf.draw;
using System.Data;
using System.Web;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf.fonts;
using MDVision.DataAccess.DAL.Admin;
using MDVision.DataAccess.DAL.Clinical.Batch;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MDVision.Business.BLL
{
   public class BLLReportHeader
    {



        #region Variable
        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLAdminEDI"/> class.
        /// </summary>
        public BLLReportHeader()
        {
            //SharedVariable SharedObj
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this.SharedObj = SharedObj;
            //Add your own initialization code after the InitializeComponent() call

        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.

        private void InitializeComponent()
        {
            components = new Container();
        }
        #endregion


        //#region MeasureGroup
        //public BLObject<DSPQRS> loadMeasureGroup(string MeasureGroupName, string ProviderIds, Int32 PageNumber = 1, Int32 RowsPerPage = 15, string IsActive = "1")
        //{
        //    try
        //    {
        //        var ds = new DALMeasureGroup(SharedObj).LoadMeasureGroup(MeasureGroupName, ProviderIds, PageNumber, RowsPerPage, IsActive);
        //        return new BLObject<DSPQRS>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLReportHeader::LoadMeasureGroup", ex);
        //        return new BLObject<DSPQRS>(null, ex.Message);
        //    }
        //}

        //public BLObject<DSPQRS> fillMeasureGroup(Int64 MeasureGroupId)
        //{
        //    try
        //    {
        //        var ds = new DALMeasureGroup(SharedObj).FillMeasureGroup(MeasureGroupId);
        //        return new BLObject<DSPQRS>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLReportHeader::fillMeasureGroup", ex);
        //        return new BLObject<DSPQRS>(null, ex.Message);
        //    }
        //}

        //public BLObject<DSPQRS> UpdateMeasureGroup(DSPQRS ds)
        //{
        //    try
        //    {
        //         ds = new DALMeasureGroup(SharedObj).UpdateMeasureGroup(ds);
        //        return new BLObject<DSPQRS>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLReportHeader::UpdateMeasureGroup", ex);
        //        return new BLObject<DSPQRS>(null, ex.Message);
        //    }
        //}

        //public BLObject<DSPQRS> insertMeasureGroup(DSPQRS ds)
        //{
        //    try
        //    {
        //        ds = new DALMeasureGroup(SharedObj).InsertMeasureGroup(ds);
        //        return new BLObject<DSPQRS>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLReportHeader::InsertMeasureGroup", ex);
        //        return new BLObject<DSPQRS>(null, ex.Message);
        //    }
        //}
        //public BLObject<string> deleteMeasureGroup(Int64 measureGroupId)
        //{
        //    try
        //    {
        //        if (measureGroupId>0)
        //        {
        //            return new BLObject<string>(new DALMeasureGroup(SharedObj).DeleteMeasureGroup(measureGroupId));
        //        }
        //        else
        //        {
        //            return new BLObject<string>("Please select Record");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLReportHeader::deleteMeasureGroup", ex);
        //        return new BLObject<string>(null, ex.Message);
        //    }
        //}

        //#endregion
        //#region Measures

        //public BLObject<DSPQRS> fillMeasures(Int64? MeasuresId=null)
        //{
        //    try
        //    {
        //        var ds = new DALMeasureGroup(SharedObj).FillMeasures(MeasuresId);
        //        return new BLObject<DSPQRS>(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        MDVLogger.LogErrorMessage("BLLReportHeader::FillMeasures", ex);
        //        return new BLObject<DSPQRS>(null, ex.Message);
        //    }
        //}
        //#endregion



 

  

    }
}
