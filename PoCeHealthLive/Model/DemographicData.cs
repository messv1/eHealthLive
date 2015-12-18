﻿using System;
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
            DateTime birthDate = new DateTime(); //(1947, 05, 15);
            int status = 0;

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




                //foreach (var result in q)
                //{

                //    if (String.IsNullOrEmpty(result.inNummer.ToString()))
                //    {

                //        PatID = result.inNummer.ToString();
                //    }
                //    else
                //    {
                //        PatID = null;
                //        FamilyName = null;
                //        GivenName = null;
                //        Dob = null;
                //        IpID = null;
                //        FolderID = null;
                //    }
                //}            
            }
            catch { }
        }
    }
}
