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
    
    public partial class TadrPersonen
    {
        public int inStammID { get; set; }
        public Nullable<short> shSexID { get; set; }
        public Nullable<System.DateTime> dtGeburtstag { get; set; }
        public string txTitel { get; set; }
        public string txVornamenkürzel { get; set; }
        public string txGeburtsname { get; set; }
        public string txNickName { get; set; }
        public string txBeruf { get; set; }
        public string txAHVNr { get; set; }
        public Nullable<int> inNationalitätLandID { get; set; }
        public Nullable<short> shMuttersprache { get; set; }
        public Nullable<short> shZivilstandID { get; set; }
        public Nullable<short> shKonfessionID { get; set; }
        public Nullable<System.DateTime> dtExitus { get; set; }
        public Nullable<int> inHeimatOrt { get; set; }
        public Nullable<int> inHeimatLand { get; set; }
        public Nullable<int> inGeburtsOrt { get; set; }
        public Nullable<int> inGeburtsLand { get; set; }
        public string txGeburtsnamePhonem { get; set; }
        public int inLastUpdateUser { get; set; }
        public System.DateTime dtLastUpdate { get; set; }
        public int inCreationUser { get; set; }
        public System.DateTime dtCreation { get; set; }
        public byte[] timestamp { get; set; }
    }
}
