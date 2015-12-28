using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive.Model
{
   public class DemographicData
    {
        // Make database instance
        PraxiCenterEntities ctx = new PraxiCenterEntities();

        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Dob { get; set; }
        public string PatID { get; set; }
        public string IpID { get; set; }
        public string FolderID { get; set; }

        public DemographicData()
        {

        }
        public DemographicData(string givenName, string familyName, string dob)
        {
            this.GivenName = givenName;
            this.FamilyName = familyName;
            this.Dob = dob;
            this.IpID = null;
            this.FolderID = null;
        }
        
        public void getAdministrativeDataFromDB()
        {
            DateTime birthDate = new DateTime();
            int status = 0;

            try
            {
                birthDate = Convert.ToDateTime(Dob);
            }
            catch { }

            /// Query database PraxiCenter.
            /// Query Paramter: FamilyName, GivenName, brithdate
            /// Return list with Results q: (Family Name, Given Name, 
            /// Birthdate, Triamed Patient ID)
            try
            {
                var q = (from a in ctx.TadrStamm
                         join b in ctx.TadrPersonen on a.IDStamm equals b.inStammID
                         join c in ctx.TpatNummer on a.IDStamm equals c.inPatientID
                         where a.txName1 == FamilyName
                         where a.txName2 == GivenName
                         where b.dtGeburtstag == birthDate
                         select new { a.txName1, a.txName2, b.dtGeburtstag, c.inNummer });

                /// Loop through Return list q
                foreach (var result in q)
                {
                    PatID = result.inNummer.ToString();
                    status = 1;              
                }

                if(status == 0)
                {
                    PatID = null;
                    FamilyName = null;
                    GivenName = null;
                    Dob = null;
                    IpID = null;
                    FolderID = null;
                }      
            }
            catch { }
        }
    }
}
