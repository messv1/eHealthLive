using java.net;
using org.ehealth_connector.common;
using org.ehealth_connector.communication;
using org.ehealth_connector.security;
using org.ehealth_connector.security.xua;
using org.openhealthtools.ihe.atna.nodeauth;
using org.openhealthtools.ihe.atna.nodeauth.context;
using System;
using System.Diagnostics;

namespace PoCeHealthLive.Model
{
    class PatientDemographicsQuery
    {
        private XUAConfig config = XUAConfig.getInstance();

        public PatientDemographicsQuery()
        {
            this.initConfig(); // initalize configuration
        }
        public void initConfig()
        {
            config.setSTSEndpoint("https://www-test.infomed-vs.ch/dpp-habilitation/X509UserSAMLService");
            config.setAssertionEndpoint("https://www-test.infomed-vs.ch/dpp-habilitation/AssertionSAMLService");
            config.setXDSRegistryEndpoint("https://www-test.infomed-vs.ch/dpp-xdsreg");
            config.setXDSRepositoryEndpoint("https://www-test.infomed-vs.ch/dpp-xdsrep");

            config.setAuthorId("7601003220445");
            config.setEmrId("2.16.756.5.30.1.120.14.101");
            config.setEmrName("Infomed client test Oliver Egger");
            config.setEmrVersion("1.1");
            config.setLoginAsApplication(new java.lang.Boolean(true));
            config.setCertificateSource(XUAConfig.CertificateSource.JKS);
            config.setX509KeystoreAlias("oliver.egger");
            config.setX509KeystoreAliasPassword("ahdis");
            config.setX509KeystorePassword("ahdiskeystore");
            config.setX509KeystorePath(System.IO.Path.Combine("rsc", "oliver.egger.jks"));
            config.setLoginWithInstitutionContext(new java.lang.Boolean(true));
            config.setInstitutionGLN("7601001401310");

            try
            {
                java.util.Properties properties = new java.util.Properties();

                properties.put("javax.net.ssl.keyStore", System.IO.Path.Combine("rsc", "empty.jks"));
                properties.put("javax.net.ssl.keyStorePassword", "changeit");
                properties.put("javax.net.ssl.trustStore", System.IO.Path.Combine("rsc", "truststoreinfomed.jks"));
                properties.put("javax.net.ssl.trustStorePassword", "infomed");

                SecurityDomain sd = new SecurityDomain("infomed", properties);

                NodeAuthModuleContext.getContext().getSecurityDomainManager().registerSecurityDomain(sd);
                NodeAuthModuleContext.getContext().getSecurityDomainManager()
                        .registerURItoSecurityDomain(new java.net.URI("https://www-test.infomed-vs.ch:443"), "infomed");
                ConvenienceSecurity.initSecurity();
            }
            catch (SecurityDomainException e1)
            {
                e1.printStackTrace();
            }
            catch (Exception)
            {
            }
        }

        public AffinityDomain getInfomedAffinityDomian()
        {
            Destination infomedReg = null;
            Destination infomedRep = null;
            try
            {
                infomedReg = new Destination("2.16.756.5.30.1.120.10.1", // new
                                                                         // URI("http://localhost:8080/dpp-xdsreg"));
                        new URI("https://www-test.infomed-vs.ch/dpp-xdsreg"));
                infomedRep = new Destination("2.16.756.5.30.1.120.10.1",
                        new URI("https://www-test.infomed-vs.ch/dpp-xdsrep"));
                // new URI("http://localhost:8080/dpp-xdsrep"));
            }
            catch (URISyntaxException)
            {
            }
            AffinityDomain affinityDomain = new AffinityDomain(null, infomedReg, infomedRep);

            String endpointPdq = "https://www-test.infomed-vs.ch/PdqInEJB/PDQSupplier_Service/PDQSupplier_PortType";
            Destination dest = new Destination();

            dest.setSenderApplicationOid("1.2.840.114350.1.13.99998.8735");
            dest.setSenderFacilityOid("1.2.840.114350.1.13.99998");

            dest.setReceiverApplicationOid("1.3.6.1.4.1.21367.2011.2.2.7872");

            try
            {
                dest.setUri(new URI(endpointPdq));
            }
            catch (URISyntaxException e)
            {
                e.printStackTrace();
            }
            affinityDomain.setPdqDestination(dest);
            return affinityDomain;
        }

        //public bool patientDemographicsQueryID(TriaMedPatient pat, Patient patient)
        public bool patientDemographicsQueryID(DemographicData pat, Patient patient)
        {
            
            AffinityDomain affinityDomain = getInfomedAffinityDomian();
            MasterPatientIndexQuery mpiQuery = new MasterPatientIndexQuery(affinityDomain.getPdqDestination());
            Name name = new Name(patient.getName().getGivenNames(), patient.getName().getFamilyName());

            mpiQuery.addPatientName(true, name);

            if (patient.getBirthday() != null)
            {         
                mpiQuery.setPatientDateOfBirth(patient.getBirthday());
            }
            MasterPatientIndexQueryResponse ret = ConvenienceMasterPatientIndexV3.queryPatientDemographics(mpiQuery,
                affinityDomain);

            Debug.WriteLine("succes " + ret.getSuccess());
            Debug.WriteLine("totalNumbers " + ret.getTotalNumbers());

            java.util.List patients = ret.getPatients();
            java.util.List ids;

            if (patients != null)
            {
                // get IDs of first patient
                ids = ((Patient)patients.get(0)).getIds();
                // get Infomed ID of first patient
                pat.IpID = ((Identificator)ids.get(0)).getExtension();
                patient.addId((Identificator)ids.get(0));
            }
            return ret.getSuccess();
        }
    }
}
