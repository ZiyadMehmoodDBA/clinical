using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.Native.Patient
{
    public class UploadPDFVM : NativeBaseModel
    {
        public string PatientId { get; set; }
        public string PdfBase64String { get; set; }

        public string Filename { get; set; }
        public string DocumentFolder { get; set; }
        public string DocumentFolderId { get; set; }

    }
}
