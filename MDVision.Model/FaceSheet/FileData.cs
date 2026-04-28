using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Model.FaceSheet
{
    public class FileData
    {
        public string PatDocId { get; set; }
        public string FilePath { get; set; }
        public byte[] FileStream { get; set; }
        public string FileType { get; set; }
        public string Url { get; set; }

    }
}
