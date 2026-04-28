using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.PhysicalExam
{
    public class PatientPhysicalExamModel
    {
        public PatientPhysicalExamModel()
        {
            Systems = new List<PhysicalExamSystemModel>();
        }
        public string PatientPhysicalExamId { get; set; }
        public string PatientId { get; set; }
        public string PatientPhysicalExamDate { get; set; }


        private bool? _bNormal;
        public bool? bNormal
        {
            get
            {
                if (_bNormal == null)
                    return false;
                else
                    return _bNormal;
            }
            set { _bNormal = value; }
        }

        private bool? _isActive;

        public bool? isActive
        {
            get
            {
                if (_isActive == null)
                    return true;
                else
                    return _isActive;
            }
            set { _isActive = value; }
        }

        public string Comments { get; set; }

        public string NormalComments { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SoapText { get; set; }
        public string commandType { get; set; }
        public string NormalExamsDetail { get; set; }
        public long NotesId { get; set; }
        public long TemplateId { get; set; }

        //Start 10-02-2016 Humaira Yousaf for Ids of normal systems
        public List<int> NormalSystemIds { get; set; }
        //End 10-02-2016 Humaira Yousaf for Ids of normal systems

        // public bool isFromNormalComments { get; set; }

        private bool? _isFromNormalComments;

        public bool? isFromNormalComments
        {
            get
            {
                if (_isFromNormalComments == null)
                    return false;
                else
                    return _isFromNormalComments;
            }
            set { _isFromNormalComments = value; }
        }


        public List<PhysicalExamSystemModel> Systems { get; set; }
    }
}