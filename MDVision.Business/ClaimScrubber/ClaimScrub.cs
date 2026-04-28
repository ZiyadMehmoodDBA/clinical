using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MDVision.Business.ClaimScrubber
{
    public class ClaimScrub
    {
        public List<Claims> Claims { get; set; }
        public reporterrors ErrorReport { get; set; }

        public ClaimScrub()
        {
            this.Claims = new List<Claims>();
            this.ErrorReport = new reporterrors();
        }
    }

    public class Claims
    {
        public long VisitId { get; set; }
        public string ClaimNumber { get; set; }
        public bool HasErrors { get; set; }
        public int ErrorsCount { get; set; }
    }

    public class reporterrors
    {

        public job job { get; set; }
        public file file { get; set; }
        public summary summary { get; set; }

        public reporterrors()
        {
            this.job = new job();
            this.file = new file();
            this.summary = new summary();
        }
    }

    public class job
    {
        public string number { get; set; }
        public string submitterid { get; set; }

    }
    public class file
    {
        public string fileid { get; set; }
        public batch batch { get; set; }

        public file()
        {
            this.batch = new batch();
        }

    }

    [XmlRoot("batch")]
    public class batch
    {
        public string batchid { get; set; }

        [XmlElement("claim")]
        public List<claim> claim { get; set; }
        public batch()
        {

            this.claim = new List<claim>();
        }

    }
    public class claim
    {
        public string patientid { get; set; }
        public string claimnumber { get; set; }
        public claimerrors claimerrors { get; set; }
        public Items items { get; set; }
        public errmaps errmaps { get; set; }
        public claim()
        {
            this.claimerrors = new claimerrors();
            this.items = new Items();
        }
    }

    [XmlRoot("claimerrors")]
    public class claimerrors
    {
        [XmlElement("claimerror")]
        public List<claimerror> claimerror { get; set; }

        public claimerrors()
        {
            this.claimerror = new List<claimerror>();
        }


    }

    [XmlRoot("items")]
    public class Items
    {
        [XmlElement("item")]
        public List<Item> Item { get; set; }

        public Items()
        {
            this.Item = new List<Item>();
        }
    }

    public class claimerror
    {
        public string claimerrorcode { get; set; }
        public string claimerrorsubcode { get; set; }
        public string claimerrormsg { get; set; }
        public string claimerroraction { get; set; }
        public string claimerrorcategory { get; set; }
        public string claimansireason { get; set; }



    }
    public class Item
    {
        public string lineitemcontrolnumber { get; set; }
        public string codes { get; set; }
        public policyfiles policyfiles { get; set; }
        public string seq { get; set; }

        public string cpt { get; set; }
        public Item()
        {
            this.policyfiles = new policyfiles();
        }
    }

    [XmlRoot("errmap")]
    public class errmaps
    {
        [XmlElement("errmap")]
        public List<errmap> errmap { get; set; }

        public errmaps()
        {
            this.errmap = new List<errmap>();
        }
    }

    public class errmap
    {
        [XmlAttribute]
        public string controlnumber { get; set; }

        [XmlAttribute]
        public string seq { get; set; }

        [XmlAttribute]
        public string claimerrorcode { get; set; }
        [XmlAttribute]
        public string claimerrorsubcode { get; set; }
    }

    [XmlRoot("policyfiles")]
    public class policyfiles
    {
        [XmlElement("policyfile")]
        public List<string> policyfile { get; set; }
        public policyfiles()
        {
            this.policyfile = new List<string>();
        }
    }
    public class summary
    {
        public string totalerrors { get; set; }
        public string totalfileerrors { get; set; }
        public string totalbatcherrors { get; set; }
        public string totalclaimcount { get; set; }
        public string totalclaimerrors { get; set; }
        public string totalbilledfile { get; set; }
        public string totalbillederrors { get; set; }
        public string totalitemcount { get; set; }
        public string totalitemerrors { get; set; }
        public string perclaimerror { get; set; }
        public string perlineerror { get; set; }

    }
}
