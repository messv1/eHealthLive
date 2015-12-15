using System;
using System.Collections.Generic;
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

        public DemographicData()
        {

        }
        public DemographicData(string givenName, string familyName, string dob)
        {
            this.GivenName = givenName;
            this.FamilyName = familyName;
            this.Dob = dob;
            this.IpID = null;
        }
        public void getAdministrativeDataFromDB()
        {
            DateTime birthDate = new DateTime(); //(1947, 05, 15);

            try
            {
                birthDate = Convert.ToDateTime(Dob);
            }
            catch { }

            try
            {
                var q = (from a in ctx.TadrStamm
                         join b in ctx.TadrPersonen on a.IDStamm equals b.inStammID
                         join c in ctx.TpatNummer on a.IDStamm equals c.inPatientID
                         where a.txName1 == FamilyName //"Müller"
                         where a.txName2 == GivenName //"Anita"
                         where b.dtGeburtstag == birthDate // "15.05.1947"
                         select new { a.txName1, a.txName2, b.dtGeburtstag, c.inNummer });
                   
                foreach (var result in q)
                {
  
                    if ((result.inNummer).ToString() != null)
                    {

                        PatID = result.inNummer.ToString();
                    }
                    else
                    {
                        PatID = null;
                        FamilyName = null;
                        GivenName = null;
                        Dob = null;
                    }
                    //administrativeData[0] = result.txName1;
                    //administrativeData[1] = result.txName2;
                    //administrativeData[2] = String.Format("{0:dd/MM/yyyy}", result.dtGeburtstag);
                    //administrativeData[3] = result.inNummer.ToString();
                    //administrativeData[4] = null;
                    //lastName = result.txName1;
                    //firstName = result.txName2;
                    ////dob = String.Format("{0:MM/dd/yyyy}", result.dtGeburtstag);
                    //dob = String.Format("{0:dd/MM/yyyy}", result.dtGeburtstag);
                    //patID = result.inNummer.ToString();
                    //ipID = null;
                }            
            }
            catch { }
        }
    }
}
