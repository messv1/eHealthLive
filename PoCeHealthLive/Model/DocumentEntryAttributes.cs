using org.openhealthtools.ihe.xds.metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive.Model
{
    class DocumentEntryAttributes
    {
        public string UUID { get; set; }
        public string Title { get; set; }
        public string ClassCode { get; set; }
        public string TypeCode { get; set; }
        public string FacilityTypeCode { get; set; }
        public string PracticeSettingCode { get; set; }
        public string FormatCode { get; set; }
        public string CreationTime { get; set; }
        public DocumentEntryType DocumentEntryType { get; set; }
        //public string ConfidentialityCode { get; set; }

        public DocumentEntryAttributes(string uuid, string title, 
            string classCode, string typeCode, string facilityTypeCode, 
            string practiceSettingCode, string formatCode, string creationTime, DocumentEntryType documentEntryType)
        {
            this.UUID = uuid;
            this.Title = title;
            this.ClassCode = classCode;
            this.TypeCode = typeCode;
            this.FacilityTypeCode = facilityTypeCode;
            this.PracticeSettingCode = practiceSettingCode;
            this.FormatCode = formatCode;
            this.CreationTime = creationTime;
            this.DocumentEntryType = documentEntryType;
            //this.ConfidentialityCode = confidentialityCode;

        }
    }
}
