//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PoCeHealthLive
{
    using System;
    using System.Collections.Generic;
    
    public partial class TadrStamm
    {
        public int IDStamm { get; set; }
        public short shStammArt { get; set; }
        public short shKorrespondenzsprache { get; set; }
        public bool tfInaktiv { get; set; }
        public int inMandantID { get; set; }
        public string txEANNummer { get; set; }
        public string txErfassungscode { get; set; }
        public string txAnrede { get; set; }
        public string txName1 { get; set; }
        public string txName2 { get; set; }
        public string moBemerkungen { get; set; }
        public Nullable<int> inBildID { get; set; }
        public string txName1Phonem { get; set; }
        public string txName2Phonem { get; set; }
        public int inLastUpdateUser { get; set; }
        public System.DateTime dtLastUpdate { get; set; }
        public int inCreationUser { get; set; }
        public System.DateTime dtCreation { get; set; }
        public byte[] timestamp { get; set; }
    }
}
