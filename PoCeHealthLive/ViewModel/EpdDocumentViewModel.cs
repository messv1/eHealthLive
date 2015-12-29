using java.net;
using org.apache.commons.io;
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
using org.openhealthtools.ihe.xds.document;
using org.openhealthtools.ihe.xds.metadata;
using org.openhealthtools.ihe.xds.metadata.impl;
using org.openhealthtools.ihe.xds.response;
using PoCeHealthLive.Model;
using PoCeHealthLive.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PoCeHealthLive.ViewModel
{
    class EpdDocumentViewModel : BaseViewModel
    {
        // Property backing fields.
        string selectedClassCode;
        string selectedHealthCareFacilityCode;
        string selectedConfidentialityCode;
        string selectedPracticeSettingCode;
        string selectedFormatCode;
        string documentReferenceCounter;

        DocumentEntryAttributes selectedDocumentEntryType;

        Patient patient = new Patient();
        private XUAConfig config = XUAConfig.getInstance();

        public SearchDocumentsCommand SearchDocumentsCommand { get; set; }
        public OpenDocumentCommand OpenDocumentCommand { get; set; }
        public ObservableCollection<CustomCode> ClassCodes { get; set; }
        public ObservableCollection<CustomCode> HealthCareFacilityCodes { get; set; }
        public ObservableCollection<CustomCode> ConfidentialityCodes { get; set; }
        public ObservableCollection<CustomCode> PracticeSettingCodes { get; set; }
        public ObservableCollection<CustomCode> FormatCodes { get; set; }
        public ObservableCollection<DocumentEntryAttributes> RecievedDocumentEntries { get; set; }
        //public ObservableCollection<DocumentEntryType> RecievedDocumentEntries { get; set; }

        public string  DocumentReferencesCounter {
            get { return this.documentReferenceCounter; }
            set
            {
                documentReferenceCounter = value;
                OnPropertyChanged();
            }
        }

        public DocumentEntryAttributes SelectedDocumentEntryType
        {
            get { return this.selectedDocumentEntryType; }
            set
            {
                this.selectedDocumentEntryType = value;
                OnPropertyChanged();
            }
        }

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
   
        public EpdDocumentViewModel(Patient patient)
        {
            this.patient = patient;
            this.SearchDocumentsCommand = new SearchDocumentsCommand(this);
            this.OpenDocumentCommand = new OpenDocumentCommand(this);
            // Instantiet item sources
            this.ClassCodes = new ObservableCollection<CustomCode>();
            this.HealthCareFacilityCodes = new ObservableCollection<CustomCode>();
            this.ConfidentialityCodes = new ObservableCollection<CustomCode>();
            this.PracticeSettingCodes = new ObservableCollection<CustomCode>();
            this.FormatCodes = new ObservableCollection<CustomCode>();
            this.RecievedDocumentEntries = new ObservableCollection<DocumentEntryAttributes>();

            // Set Document attributes
            this.SetDocumentAttributes();
            
        }

        public void SetDocumentAttributes()
        {
            // Set Document Class Codes
            this.ClassCodes.Add(new CustomCode("Alle", null));
            this.ClassCodes.Add(new CustomCode("Konsultation Notizen", "DTC01"));
            this.ClassCodes.Add(new CustomCode("Verlauf Notizen", "DTC02"));
            this.ClassCodes.Add(new CustomCode("Behandlungen Notizen", "DTC03"));
            this.ClassCodes.Add(new CustomCode("Untersuchung Verordnung", "DTC04"));
            this.ClassCodes.Add(new CustomCode("Behandlung Verordnungen", "DTC05"));
            this.ClassCodes.Add(new CustomCode("Episoden Zusamenfassung", "DTC06"));
            this.ClassCodes.Add(new CustomCode("Verlaufsberichte", "DTC07"));
            this.ClassCodes.Add(new CustomCode("Untersuchungs Resultate", "DTC08"));
            this.ClassCodes.Add(new CustomCode("Benachrichtigungen", "DTC09"));
            this.ClassCodes.Add(new CustomCode("Krankengeschichte Zusammenfassungen", "DTC10"));
            this.ClassCodes.Add(new CustomCode("Aktuelle Zustandszusammenfassungen", "DTC11"));
            this.ClassCodes.Add(new CustomCode("Behandlungsplan", "DTC12"));
            this.ClassCodes.Add(new CustomCode("Warnungen", "DTC13"));
            this.ClassCodes.Add(new CustomCode("Andere", "DTC90"));

            // Set Document Healthcarefacility Codes
            this.HealthCareFacilityCodes.Add(new CustomCode("Alle", null));
            this.HealthCareFacilityCodes.Add(new CustomCode("Institut für medizinische Diagnostik", "190001"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Notfalleinrichtung/Rettungswesen", "190002"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Gesundheitsbehörde", "190003"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Spitex", "190004"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Spital", "190005"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Psychiatrie Spital", "190006"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Gesundheitseinrichtung in der Haftanstalt", "190007"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Organisation für stationäre Krankenpflege", "190008"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Apotheken", "190009"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Hausarztpraxis", "190010"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Facharztpraxis", "190011"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Organisation für stationäre Rehabilitation", "190012"));
            this.HealthCareFacilityCodes.Add(new CustomCode("Andere", "190999"));

            // Set Document Confidentiality Codes
            this.ConfidentialityCodes.Add(new CustomCode("Alle", null));
            this.ConfidentialityCodes.Add(new CustomCode("Administrative", "A"));
            this.ConfidentialityCodes.Add(new CustomCode("Medical", "N"));
            this.ConfidentialityCodes.Add(new CustomCode("Secret", "V"));
            this.ConfidentialityCodes.Add(new CustomCode("Stigmatizing", "R"));
            this.ConfidentialityCodes.Add(new CustomCode("Utilities", "U"));

            // Set Document PracticeSetting Codes
            this.PracticeSettingCodes.Add(new CustomCode("Alle", null));
            this.PracticeSettingCodes.Add(new CustomCode("Allergologie und klinische Immunologie", "260001"));
            this.PracticeSettingCodes.Add(new CustomCode("Allgemeinmedizin", "260002"));
            this.PracticeSettingCodes.Add(new CustomCode("Anästhesiologie", "260003"));
            this.PracticeSettingCodes.Add(new CustomCode("Angiologie", "260004"));
            this.PracticeSettingCodes.Add(new CustomCode("Pharmakologie", "260005"));
            this.PracticeSettingCodes.Add(new CustomCode("Arbeitsmedizin", "260006"));
            this.PracticeSettingCodes.Add(new CustomCode("Augenoptik", "260007"));
            this.PracticeSettingCodes.Add(new CustomCode("Chiropraktik", "260008"));
            this.PracticeSettingCodes.Add(new CustomCode("Chirurgie", "260009"));
            this.PracticeSettingCodes.Add(new CustomCode("Dermatologie/Venerologie", "260010"));
            this.PracticeSettingCodes.Add(new CustomCode("Endokrinologie/Diabetologie", "260011"));
            this.PracticeSettingCodes.Add(new CustomCode("Ergotherapie", "260012"));
            this.PracticeSettingCodes.Add(new CustomCode("Ernährungsberatung", "260013"));
            this.PracticeSettingCodes.Add(new CustomCode("Geriatrie", "260014"));
            this.PracticeSettingCodes.Add(new CustomCode("Gastroenterologie", "260015"));
            this.PracticeSettingCodes.Add(new CustomCode("Gynäkologie/Geburtshilfe", "260016"));
            this.PracticeSettingCodes.Add(new CustomCode("Handchirurgie", "260017"));
            this.PracticeSettingCodes.Add(new CustomCode("Hebamme", "260018"));
            this.PracticeSettingCodes.Add(new CustomCode("Herz- und thorakale Gefässchirurgie", "260019"));
            this.PracticeSettingCodes.Add(new CustomCode("Infektiologie", "260020"));
            this.PracticeSettingCodes.Add(new CustomCode("Innere Medizin", "260021"));
            this.PracticeSettingCodes.Add(new CustomCode("Intensivmedizin", "260022"));
            this.PracticeSettingCodes.Add(new CustomCode("Kardiologie", "260023"));
            this.PracticeSettingCodes.Add(new CustomCode("Kinder-/Jugendmedizin", "260024"));
            this.PracticeSettingCodes.Add(new CustomCode("Kinder/Jugend-psychiatrie/-psychotherapie", "260025"));
            this.PracticeSettingCodes.Add(new CustomCode("Kinderchirurgie", "260026"));
            this.PracticeSettingCodes.Add(new CustomCode("Klinische Psychologie", "260027"));
            this.PracticeSettingCodes.Add(new CustomCode("Kur-/Präventions-Einrichtung", "260028"));
            this.PracticeSettingCodes.Add(new CustomCode("Labordiagnostik", "260029"));
            this.PracticeSettingCodes.Add(new CustomCode("Logopädie", "260030"));
            this.PracticeSettingCodes.Add(new CustomCode("Medizinische Genetik", "260031"));
            this.PracticeSettingCodes.Add(new CustomCode("Medizinische Onkologie", "260032"));
            this.PracticeSettingCodes.Add(new CustomCode("Mund-, Kiefer- und Gesichtschirurgie", "260033"));
            this.PracticeSettingCodes.Add(new CustomCode("Nephrologie", "260034"));
            this.PracticeSettingCodes.Add(new CustomCode("Neurochirurgie", "260035"));
            this.PracticeSettingCodes.Add(new CustomCode("Neurologie", "260036"));
            this.PracticeSettingCodes.Add(new CustomCode("Nuklearmedizin", "260037"));
            this.PracticeSettingCodes.Add(new CustomCode("Ophthalmologie", "260038"));
            this.PracticeSettingCodes.Add(new CustomCode("Orthopädische Chirurgie und Traumatologie des Bewegungsapparates", "260039"));
            this.PracticeSettingCodes.Add(new CustomCode("Osteopathie", "260040"));
            this.PracticeSettingCodes.Add(new CustomCode("Oto-Rhino-Laryngologie", "260041"));
            this.PracticeSettingCodes.Add(new CustomCode("Palliativmedizin", "260042"));
            this.PracticeSettingCodes.Add(new CustomCode("Pathologie", "260043"));
            this.PracticeSettingCodes.Add(new CustomCode("Pflege ambulant (zu Hause)", "260044"));
            this.PracticeSettingCodes.Add(new CustomCode("Pflege stationär", "260045"));
            this.PracticeSettingCodes.Add(new CustomCode("Physikalische Medizin", "260046"));
            this.PracticeSettingCodes.Add(new CustomCode("Physiotherapie", "260047"));
            this.PracticeSettingCodes.Add(new CustomCode("Plastische, Rekonstruktive und Ästhetische Chirurgie", "260048"));
            this.PracticeSettingCodes.Add(new CustomCode("Pneumologie", "260049"));
            this.PracticeSettingCodes.Add(new CustomCode("Podologie", "260050"));
            this.PracticeSettingCodes.Add(new CustomCode("Prävention", "260051"));
            this.PracticeSettingCodes.Add(new CustomCode("Psychiatrie und Psychotherapie", "260052"));
            this.PracticeSettingCodes.Add(new CustomCode("Psychosomatik", "260053"));
            this.PracticeSettingCodes.Add(new CustomCode("Radiologie", "260054"));
            this.PracticeSettingCodes.Add(new CustomCode("Radio-Onkologie/Strahlentherapie", "260055"));
            this.PracticeSettingCodes.Add(new CustomCode("Rechtsmedizin", "260056"));
            this.PracticeSettingCodes.Add(new CustomCode("Rehabilitation", "260057"));
            this.PracticeSettingCodes.Add(new CustomCode("Rettungsmedizin", "260058"));
            this.PracticeSettingCodes.Add(new CustomCode("Rheumatologie", "260059"));
            this.PracticeSettingCodes.Add(new CustomCode("Sozialdienst", "260060"));
            this.PracticeSettingCodes.Add(new CustomCode("Thoraxchirurgie", "260061"));
            this.PracticeSettingCodes.Add(new CustomCode("Transfusionsmedizin", "260062"));
            this.PracticeSettingCodes.Add(new CustomCode("Tropen-/Reisemedizin", "260063"));
            this.PracticeSettingCodes.Add(new CustomCode("Unfallchirurgie", "260064"));
            this.PracticeSettingCodes.Add(new CustomCode("Urologie", "260065"));
            this.PracticeSettingCodes.Add(new CustomCode("Zahnheilkunde", "260066"));
            this.PracticeSettingCodes.Add(new CustomCode("Zahn-, Mund- und Kieferheilkunde", "260067"));
            this.PracticeSettingCodes.Add(new CustomCode("Hämatologie", "260068"));
            this.PracticeSettingCodes.Add(new CustomCode("Andere medzinische Fachrichtung", "260099"));

            // Set Document Format Codes
            this.FormatCodes.Add(new CustomCode("Alle", null));
            this.FormatCodes.Add(new CustomCode("application/pdf", "urn:ihe:iti:xds-sd:pdf:2008"));
            this.FormatCodes.Add(new CustomCode("text/plain", "urn:ihe:iti:xds-sd:text:2008"));
            this.FormatCodes.Add(new CustomCode("mage/jpeg", "urn:ihe:iti-fr:xds-sd:jpeg:2010"));
            this.FormatCodes.Add(new CustomCode("image/tiff", "urn:ihe:iti-fr:xds-sd:tiff:2010"));
            this.FormatCodes.Add(new CustomCode("text/xml", "urn:ihe:pcc:xds-ms:2007"));
        }

        public void SearchDocumentsInRegistry()
        {
            ConvenienceCommunication conCom = getInfomedCommunication();
            XDSQueryResponseType qr;

            initConfig();

            List<DocumentEntryType> documentEntries = new List<DocumentEntryType>();

            qr = conCom.queryDocuments(SetSearchAttributes());

            for (int i = 0; i < qr.getDocumentEntryResponses().size(); i++)
            {
                DocumentEntryResponseType documentEntryType = (DocumentEntryResponseType)qr
                    .getDocumentEntryResponses().get(i);
                documentEntries.Add((DocumentEntryType)documentEntryType.
                    getDocumentEntry());
            }

            DisplayDocumentsEntryResponse(documentEntries);
        }
        private void DisplayDocumentsEntryResponse(List<DocumentEntryType> documentEntries)
        {            
           RecievedDocumentEntries.Clear();

            for (int i = 0; i < documentEntries.Count(); i++)
            {
                // extract DocumentEntry title
                string sKeyWordBegin = "value: ";
                string sKeyWordEnd = ")";
                InternationalStringTypeImpl title = (InternationalStringTypeImpl)documentEntries[i].getTitle();
                string sourceString = title.getGroup().getValue(0).ToString();
                int iKeyWordBegin = sourceString.IndexOf(sKeyWordBegin) + 7;
                string sTitle = sourceString.Substring(iKeyWordBegin);
                int iKeyWordEnd = sTitle.IndexOf(sKeyWordEnd);
                sTitle = sTitle.Substring(0, iKeyWordEnd);
                System.Console.WriteLine(sTitle);


                // DocumentReference umbenennen
                RecievedDocumentEntries.Add(new DocumentEntryAttributes(documentEntries[i].getEntryUUID().ToString(),
                    sTitle, documentEntries[i].getClassCode().getCode(),
                    documentEntries[i].getTypeCode().getCode(), documentEntries[i].getHealthCareFacilityTypeCode().getCode(),
                    documentEntries[i].getPracticeSettingCode().getCode(), documentEntries[i].getFormatCode().getCode(),
                    documentEntries[i].getCreationTime(), documentEntries[i]));
            }

            // Set the DocumentReferencesCounter 
            DocumentReferencesCounter = documentEntries.Count().ToString() + " documents found";
        }

        public FindDocumentsQuery SetSearchAttributes()
        {
            // Identificator patientId = new Identificator("2.16.756.5.30.1.120.10.1", "214211038"); Messerli Vinzenz
            //Identificator patientId = new Identificator("2.16.756.5.30.1.120.10.1", "102836133"); // ersetzen
            java.util.List ids = patient.getIds();
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

                FindDocumentsQuery fdq = new FindDocumentsQuery((Identificator)patient.getIds().get(0), classCodes, null, practiceSettingCodes,
                healthCareFacilityCodes, confidentialityCodes, formatCodes, null, AvailabilityStatus.APPROVED);
            //FindDocumentsQuery fdq = new FindDocumentsQuery(patientId, null, null, null,
            //    null, null, null, null, AvailabilityStatus.APPROVED);
            return fdq;
        }

        /// <summary>
        /// Open a document from DocumentRequestResponse
        /// </summary>
        public void OpenDocument()
        {
            ConvenienceCommunication conCom = getInfomedCommunication();
            DocumentEntryType docEntry = SelectedDocumentEntryType.DocumentEntryType;

            DocumentRequest documentRequest = new DocumentRequest(docEntry.getRepositoryUniqueId(),
                        conCom.getAffinityDomain().getRepositoryDestination().getUri(), docEntry.getUniqueId());
            XDSRetrieveResponseType rrt = conCom.retrieveDocument(documentRequest);

            try
            {
                storeDocument(rrt, SelectedDocumentEntryType.Title + ".pdf");
            }
            catch (URISyntaxException e)
            {
                e.printStackTrace();
            }
            catch (java.io.IOException e)
            {
                e.printStackTrace();
            }
        }
        /// <summary>
        /// Store Document in folder Retrieved Documents
        /// </summary>
        /// <param name="docEntry"></param>
        /// <param name="rrt"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool storeDocument(XDSRetrieveResponseType rrt, String filename)
        {
            string path = "Retrieved Documents/";
            if (rrt.getAttachments() == null)
            {
            }

            else if (rrt.getAttachments().size() > 0)
            {

                if (rrt.getErrorList() != null)
                {
                    MessageBoxResult msgBoxError = MessageBox.Show("Error(s): occured: " + 
                        rrt.getErrorList().getHighestSeverity().getName(), "Error");
                }

                XDSDocument document = (XDSDocument)rrt.getAttachments().get(0);
                java.io.InputStream docIS = document.getStream();
                java.io.File targetFile = new java.io.File(path + filename);
                FileUtils.copyInputStreamToFile(docIS, targetFile);

                MessageBoxResult msgBoxSuccess = MessageBox.Show("Document was stored to: " + 
                    Environment.NewLine + targetFile.getCanonicalPath(), "Document stored successful");
            }
            else
            {
                return false;
            }
            return true;
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
