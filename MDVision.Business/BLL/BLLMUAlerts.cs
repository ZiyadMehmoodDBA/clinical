using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.MU;
using MDVision.Model.MU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDVision.Business.BLL
{
    public class BLLMUAlerts
    {
        #region " Constructors "
        /// <summary>
        /// Initializes a new instance of the <see cref="BLLMUAlerts"/> class.
        /// </summary>
        public BLLMUAlerts()
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

        public BLObject<string> InsertMUAlerts(List<MUAlertsModel> AlertList)
        {
            try
            {
                string xml_ = MDVUtility.GetXmlOfObject(typeof(List<MUAlertsModel>), AlertList);
                return new BLObject<string>(new DALMUAlerts().InsertMUAlerts(xml_));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMUAlerts::InsertMUAlerts", ex);
                return new BLObject<string>(ex.Message);
            }
        }

        public BLObject<List<MUAlertsModel>> UpdateMUAlerts(List<MUAlertsModel> AlertList,bool IsFromNote=false)
        {
            try
            {
                string xml_ = MDVUtility.GetXmlOfObject(typeof(List<MUAlertsModel>), AlertList);
                return new BLObject<List<MUAlertsModel>>(new DALMUAlerts().UpdateMUAlerts(xml_, IsFromNote));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMUAlerts::UpdateMUAlerts", ex);
                return new BLObject<List<MUAlertsModel>>(null, ex.Message);
            }
        }
        public BLObject<List<MUAlertsModel>> LoadMUAlerts(long PatientId,string ProfileName, bool IsShowAlert = true, string Type = "MU3")
        {
            try
            {
                return new BLObject<List<MUAlertsModel>>(new DALMUAlerts().LoadMUAlerts(PatientId, IsShowAlert, Type, ProfileName));
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMUAlerts::LoadMUAlerts", ex);
                return new BLObject<List<MUAlertsModel>>(null, ex.Message);
            }
        }
    }
}
