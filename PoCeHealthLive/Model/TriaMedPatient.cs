using PoCeHealthLive.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive
{
    public class TriaMedPatient : BaseViewModel
    {
        // Make database instance
        PraxiCenterEntities ctx = new PraxiCenterEntities();

        private string firstName;
        private string lastName;
        private string dob;
        private string patID;
        private string ipID;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Dob
        {
            get { return dob; }
            set { dob = value; }
        }

        public string PatID
        {
            get { return patID; }
            set { patID = value; }
        }

        public string IpID
        {
            get { return ipID; }
            set { ipID = value; }
        }

        public TriaMedPatient()
        {

        }

        /// <summary>
        /// Constructor of TriaMedPatient
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dob"></param>
        public TriaMedPatient(string firstName, string lastName, string dob)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.dob = dob;
        }

        public TriaMedPatient(string patID, string firstName, string lastName, string dob, string ipID)
        {
            this.patID = patID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.dob = dob;
            this.ipID = ipID;
        }

        /// <summary>
        /// Get Patient from TriaMed database based on first name, last name and birthdate
        /// </summary>
        /// 
        public void GetTriaMedPatient()
        {
            DateTime birthDate = new DateTime(); //(1947, 05, 15);

            //Convert birthdate from string to Datetime
            try
            {
                birthDate = Convert.ToDateTime(dob);
            }
            catch { }

            try
            {
                var q = (from a in ctx.TadrStamm
                         join b in ctx.TadrPersonen on a.IDStamm equals b.inStammID
                         join c in ctx.TpatNummer on a.IDStamm equals c.inPatientID
                         where a.txName1 == this.lastName //"Müller"
                         where a.txName2 == this.firstName //"Anita"
                         where b.dtGeburtstag == birthDate // "15.05.1947"
                         select new { a.txName1, a.txName2, b.dtGeburtstag, c.inNummer });
                foreach (var result in q)
                {
                    lastName = result.txName1;
                    firstName = result.txName2;
                    //dob = String.Format("{0:MM/dd/yyyy}", result.dtGeburtstag);
                    dob = String.Format("{0:dd/MM/yyyy}", result.dtGeburtstag);
                    patID = result.inNummer.ToString();
                    ipID = null;
                }
            }
            catch { }
        }
    }
}
