/* Author: M Ahmad Imran
 * OverView: Model for Complaints
 * Date : Feb 11, 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDVision.IEHR.EMR.Model.Medical
{
    public class ComplaintModel
    {
        public string commandType { get; set; }
        public string PatientId { get; set; }
        public string DateCaptured { get; set; }
        public string OverallComments { get; set; }
        public string NotesId { get; set; }
        public string ComplaintId { get; set; }
        public string RowsPerPage { get; set; }
        public string PageNumber { get; set; }
        public List<ComplaintDetailModel> ComplaintDetails { get; set; }
        public string UserId { get; set; }
        
 
    }
}