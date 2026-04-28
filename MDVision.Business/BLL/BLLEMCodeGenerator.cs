using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using MDVision.Model.Clinical.EMCodeGenerator;
using MDVision.DataAccess.DAL.Clinical;
namespace MDVision.Business.BLL
{
    public class BLLEMCodeGenerator
    {

        #region Constructors
        public BLLEMCodeGenerator()
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
        public EMCodeGeneratorDataHolder LoadEMCodeGeneratorData(long PatientId, long NotesId, long UserId)
        {
            EMCodeGeneratorDataHolder EMCodeData = null;
            try
            {
                EMCodeData = new DALEMCodeGenerator().LoadEMCodeGeneratorData(PatientId, NotesId, UserId);
                return EMCodeData;
            }
            catch (Exception e)
            {
                EMCodeData.ErrorMessage = e.Message;
                return EMCodeData;
            }
        }

    }
}
