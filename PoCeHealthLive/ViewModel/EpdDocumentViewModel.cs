using java.net;
using org.ehealth_connector.cda.enums;
using org.ehealth_connector.common;
using org.ehealth_connector.communication;
using org.ehealth_connector.communication.ch.enums;
using org.ehealth_connector.communication.xd.storedquery;
using org.ehealth_connector.security;
using org.ehealth_connector.security.xua;
using org.openhealthtools.ihe.atna.nodeauth;
using org.openhealthtools.ihe.atna.nodeauth.context;
using org.openhealthtools.ihe.common.ebxml._3._0.rim;
using org.openhealthtools.ihe.xds.response;
using PoCeHealthLive.Model;
using PoCeHealthLive.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive.ViewModel
{
    class EpdDocumentViewModel : BaseViewModel
    {
        public SearchDocumentsCommand SearchDocumentsCommand { get; set; }
        public ObservableCollection<DocumentAttributes> ClassCodes { get; set; }
        public ObservableCollection<DocumentAttributes> HealthCareFacilityCodes { get; set; }
        public ObservableCollection<DocumentAttributes> ConfidentialityCodes { get; set; }
        public ObservableCollection<DocumentAttributes> PracticeSettingCodes { get; set; }
        public ObservableCollection<DocumentAttributes> FormatCodes { get; set; }
        public ObservableCollection<DocumentReference> RecievedDocumentReferences { get; set; }

        private XUAConfig config = XUAConfig.getInstance();

        // Property backing fields.
        string selectedClassCode;
        string selectedHealthCareFacilityCode;
        string selectedConfidentialityCode;
        string selectedPracticeSettingCode;
        string selectedFormatCode;


        /// <summary>
        /// Gets or sets the Class Code.
        /// </summary>
        public string SelectedClassCode
        {
            get { return this.selectedClassCode; }
            set
            {
                this.selectedClassCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the HealthCareFacility Code.
        /// </summary>
        public string SelectedHealthCareFacilityCode
        {
            get { return this.selectedHealthCareFacilityCode; }
            set
            {
                this.selectedHealthCareFacilityCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the HealthCareFacility Code.
        /// </summary>
        public string SelectedConfidentialityCode
        {
            get { return this.selectedConfidentialityCode; }
            set
            {
                this.selectedConfidentialityCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the PracticeSetting Code.
        /// </summary>
        public string SelectedPracticeSettingCode
        {
            get { return this.selectedPracticeSettingCode; }
            set
            {
                this.selectedPracticeSettingCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SelectedFormat Code.
        /// </summary>
        public string SelectedFormatCode
        {
            get { return this.selectedFormatCode; }
            set
            {
                this.selectedFormatCode = value;
                OnPropertyChanged();
            }
        }
   
        public EpdDocumentViewModel()
        {
            this.SearchDocumentsCommand = new SearchDocumentsCommand(this);
            // Instantiet item sources
            this.ClassCodes = new ObservableCollection<DocumentAttributes>();
            this.HealthCareFacilityCodes = new ObservableCollection<DocumentAttributes>();
            this.ConfidentialityCodes = new ObservableCollection<DocumentAttributes>();
            this.PracticeSettingCodes = new ObservableCollection<DocumentAttributes>();
            this.FormatCodes = new ObservableCollection<DocumentAttributes>();
            this.RecievedDocumentReferences = new ObservableCollection<DocumentReference>();

            // Set Document attributes
            this.SetDocumentAttributes();
            
        }

        public void SetDocumentAttributes()
        {
            // Set Document Class Codes
            this.ClassCodes.Add(new DocumentAttributes("Alle", null));
            this.ClassCodes.Add(new DocumentAttributes("Konsultation Notizen", "DTC01"));
            this.ClassCodes.Add(new DocumentAttributes("Verlauf Notizen", "DTC02"));
            this.ClassCodes.Add(new DocumentAttributes("Behandlungen Notizen", "DTC03"));
            this.ClassCodes.Add(new DocumentAttributes("Untersuchung Verordnung", "DTC04"));
            this.ClassCodes.Add(new DocumentAttributes("Behandlung Verordnungen", "DTC05"));
            this.ClassCodes.Add(new DocumentAttributes("Episoden Zusamenfassung", "DTC06"));
            this.ClassCodes.Add(new DocumentAttributes("Verlaufsberichte", "DTC07"));
            this.ClassCodes.Add(new DocumentAttributes("Untersuchungs Resultate", "DTC08"));
            this.ClassCodes.Add(new DocumentAttributes("Benachrichtigungen", "DTC09"));
            this.ClassCodes.Add(new DocumentAttributes("Krankengeschichte Zusammenfassungen", "DTC10"));
            this.ClassCodes.Add(new DocumentAttributes("Aktuelle Zustandszusammenfassungen", "DTC11"));
            this.ClassCodes.Add(new DocumentAttributes("Behandlungsplan", "DTC12"));
            this.ClassCodes.Add(new DocumentAttributes("Warnungen", "DTC13"));
            this.ClassCodes.Add(new DocumentAttributes("Andere", "DTC90"));

            // Set Document Healthcarefacility Codes
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Alle", null));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Institut für medizinische Diagnostik", "190001"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Notfalleinrichtung/Rettungswesen", "190002"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Gesundheitsbehörde", "190003"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Spitex", "190004"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Spital", "190005"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Psychiatrie Spital", "190006"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Gesundheitseinrichtung in der Haftanstalt", "190007"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Organisation für stationäre Krankenpflege", "190008"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Apotheken", "190009"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Hausarztpraxis", "190010"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Facharztpraxis", "190011"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Organisation für stationäre Rehabilitation", "190012"));
            this.HealthCareFacilityCodes.Add(new DocumentAttributes("Andere", "190999"));

            // Set Document Confidentiality Codes
            this.ConfidentialityCodes.Add(new DocumentAttributes("Alle", null));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Administrative", "A"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Medical", "N"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Secret", "V"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Stigmatizing", "R"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Utilities", "U"));

            // Set Document PracticeSetting Codes
            this.PracticeSettingCodes.Add(new DocumentAttributes("Alle", null));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Allergologie und klinische Immunologie", "260001"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Allgemeinmedizin", "260002"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Anästhesiologie", "260003"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Angiologie", "260004"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Pharmakologie", "260005"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Arbeitsmedizin", "260006"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Augenoptik", "260007"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Chiropraktik", "260008"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Chirurgie", "260009"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Dermatologie/Venerologie", "260010"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Endokrinologie/Diabetologie", "260011"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Ergotherapie", "260012"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Ernährungsberatung", "260013"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Geriatrie", "260014"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Gastroenterologie", "260015"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Gynäkologie/Geburtshilfe", "260016"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Handchirurgie", "260017"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Hebamme", "260018"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Herz- und thorakale Gefässchirurgie", "260019"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Infektiologie", "260020"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Innere Medizin", "260021"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Intensivmedizin", "260022"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Kardiologie", "260023"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Kinder-/Jugendmedizin", "260024"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Kinder/Jugend-psychiatrie/-psychotherapie", "260025"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Kinderchirurgie", "260026"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Klinische Psychologie", "260027"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Kur-/Präventions-Einrichtung", "260028"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Labordiagnostik", "260029"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Logopädie", "260030"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Medizinische Genetik", "260031"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Medizinische Onkologie", "260032"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Mund-, Kiefer- und Gesichtschirurgie", "260033"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Nephrologie", "260034"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Neurochirurgie", "260035"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Neurologie", "260036"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Nuklearmedizin", "260037"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Ophthalmologie", "260038"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Orthopädische Chirurgie und Traumatologie des Bewegungsapparates", "260039"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Osteopathie", "260040"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Oto-Rhino-Laryngologie", "260041"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Palliativmedizin", "260042"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Pathologie", "260043"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Pflege ambulant (zu Hause)", "260044"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Pflege stationär", "260045"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Physikalische Medizin", "260046"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Physiotherapie", "260047"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Plastische, Rekonstruktive und Ästhetische Chirurgie", "260048"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Pneumologie", "260049"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Podologie", "260050"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Prävention", "260051"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Psychiatrie und Psychotherapie", "260052"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Psychosomatik", "260053"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Radiologie", "260054"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Radio-Onkologie/Strahlentherapie", "260055"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Rechtsmedizin", "260056"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Rehabilitation", "260057"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Rettungsmedizin", "260058"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Rheumatologie", "260059"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Sozialdienst", "260060"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Thoraxchirurgie", "260061"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Transfusionsmedizin", "260062"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Tropen-/Reisemedizin", "260063"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Unfallchirurgie", "260064"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Urologie", "260065"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Zahnheilkunde", "260066"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Zahn-, Mund- und Kieferheilkunde", "260067"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Hämatologie", "260068"));
            this.PracticeSettingCodes.Add(new DocumentAttributes("Andere medzinische Fachrichtung", "260099"));

            // Set Document Format Codes
            this.FormatCodes.Add(new DocumentAttributes("Alle", null));
            this.FormatCodes.Add(new DocumentAttributes("application/pdf", "urn:ihe:iti:xds-sd:pdf:2008"));
            this.FormatCodes.Add(new DocumentAttributes("text/plain", "urn:ihe:iti:xds-sd:text:2008"));
            this.FormatCodes.Add(new DocumentAttributes("mage/jpeg", "urn:ihe:iti-fr:xds-sd:jpeg:2010"));
            this.FormatCodes.Add(new DocumentAttributes("image/tiff", "urn:ihe:iti-fr:xds-sd:tiff:2010"));
            this.FormatCodes.Add(new DocumentAttributes("text/xml", "urn:ihe:pcc:xds-ms:2007"));
        }

        public void SearchDocumentsInRegistry()
        {
            ConvenienceCommunication conCom = getInfomedCommunication();
            XDSQueryResponseType qr;

            initConfig();

            // Set Parameters for stored query request
            qr = conCom.queryDocumentsReferencesOnly(SetSearchAttributes());

            //
            List<ObjectRefType> docReferences = new List<ObjectRefType>();
            qr.getReferences().size();

            for (int i = 0; i < qr.getReferences().size(); i++)
            {
                ObjectRefType objectRefType = (ObjectRefType)qr.getReferences().get(i);
                objectRefType.getId();
                docReferences.Add(objectRefType);
            }

            DisplayDocumentsReferencesResponse(docReferences);
        }
        private void DisplayDocumentsReferencesResponse(List<ObjectRefType> docReferences)
        {            
           RecievedDocumentReferences.Clear();

            for (int i = 0; i < docReferences.Count(); i++)
            {
                RecievedDocumentReferences.Add(new DocumentReference(docReferences[i].getId()));
            }
        }

        public FindDocumentsQuery SetSearchAttributes()
        {
            // Identificator patientId = new Identificator("2.16.756.5.30.1.120.10.1", "214211038"); Messerli Vinzenz
            Identificator patientId = new Identificator("2.16.756.5.30.1.120.10.1", "102836133"); // ersetzen
            AdministrativeGender sex = AdministrativeGender.MALE;

            // Set classCode common DTC06: Episode Zusamenfassungen
            //new Code("2.16.756.5.30.1.120.20.1", "DTC01", "2.16.756.5.30.1.120.20.1^DTC01")
            Code[] classCodes = new Code[1];
            if (!string.IsNullOrEmpty(SelectedClassCode))
            {
                classCodes[0] =  new Code("2.16.756.5.30.1.120.20.1", SelectedClassCode, 
                    "2.16.756.5.30.1.120.20.1^" + SelectedClassCode);
            }
            else { classCodes = null; }

            // Set DateTimeRange Attribute
            DateTimeRangeAttributes name = DateTimeRangeAttributes.CREATION_TIME;
            java.util.Date from = new java.util.Date(2015, 12, 11);
            java.util.Date to = new java.util.Date(2015, 12, 11);
            DateTimeRange[] dateTimeRange = new DateTimeRange[4];
            dateTimeRange[0] = new DateTimeRange(name, (java.util.Date)from, to);

            // Set healthCareFacilityCodes
            //Code[] healthCareFacilityCodes = { new Code("2.16.756.5.30.1.127.3.2.1.19",
            //    "190010", "2.16.756.5.30.1.127.3.2.1.19^190010") };
            Code[] healthCareFacilityCodes = new Code[1];
            if (!string.IsNullOrEmpty(SelectedHealthCareFacilityCode))
            {
                healthCareFacilityCodes[0] = new Code("2.16.756.5.30.1.127.3.2.1.19", SelectedHealthCareFacilityCode,
                    "2.16.756.5.30.1.127.3.2.1.19^" + SelectedHealthCareFacilityCode);
            }
            else { healthCareFacilityCodes = null; }

            // Set confidentialityCode "N" = normal, "R" = restricted, "V" = very restricted
            //Code[] confidentialityCodes = { new Code("2.16.756.5.30.1.120.20.3", "N",
            //    "2.16.756.5.30.1.120.20.3^N") };
            Code[] confidentialityCodes = new Code[1];
            if (!string.IsNullOrEmpty(SelectedConfidentialityCode))
            {
                confidentialityCodes[0] = new Code("2.16.756.5.30.1.120.20.3", SelectedConfidentialityCode, 
                    "2.16.756.5.30.1.120.20.3^" + SelectedConfidentialityCode);
            }
            else { confidentialityCodes = null; }

            // Set practiceSettingCode 260003: Anästhesiologie
            //Code[] practiceSettingCodes = { new Code("2.16.756.5.30.1.127.3.2.1.26",
            //    "260059", "2.16.756.5.30.1.127.3.2.1.26^260059") };
            Code[] practiceSettingCodes = new Code[1];
            if (!string.IsNullOrEmpty(SelectedPracticeSettingCode))
            {
                practiceSettingCodes[0] = new Code("2.16.756.5.30.1.127.3.2.1.26", SelectedPracticeSettingCode, 
                    "2.16.756.5.30.1.127.3.2.1.26^" + SelectedPracticeSettingCode);
            }
            else { practiceSettingCodes = null; }

            // Set formatCode urn:ihe:iti:xds-sd:pdf:2008 = application/pdf
            //Code[] formatCodes = { new Code("1.3.6.1.4.1.19376.1.2.3", "urn:ihe:iti:xds-sd:pdf:2008",
            //        "1.3.6.1.4.1.19376.1.2.3^urn:ihe:iti:xds-sd:pdf:2008") };
            Code[] formatCodes = new Code[1];
            if (!string.IsNullOrEmpty(SelectedFormatCode))
            {
                formatCodes[0] = new Code("1.3.6.1.4.1.19376.1.2.3", SelectedFormatCode,
                    "1.3.6.1.4.1.19376.1.2.3^" + SelectedFormatCode);
            }
            else { formatCodes = null; }

                FindDocumentsQuery fdq = new FindDocumentsQuery(patientId, classCodes, null, practiceSettingCodes,
                healthCareFacilityCodes, confidentialityCodes, formatCodes, null, AvailabilityStatus.APPROVED);
            //FindDocumentsQuery fdq = new FindDocumentsQuery(patientId, null, null, null,
            //    null, null, null, null, AvailabilityStatus.APPROVED);
            return fdq;
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
        private ConvenienceCommunication getInfomedCommunication()
        {
            AffinityDomain affinityDomain = getInfomedAffinityDomian();
            ConvenienceCommunication conCom = new ConvenienceCommunication(affinityDomain);
            return conCom;
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
    }
}
