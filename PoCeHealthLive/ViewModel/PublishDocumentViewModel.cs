using ikvm.extensions;
using java.net;
using org.ehealth_connector.common;
using org.ehealth_connector.communication;
using org.ehealth_connector.communication.ch.enums;
using org.ehealth_connector.security;
using org.ehealth_connector.security.xua;
using org.openhealthtools.ihe.atna.nodeauth;
using org.openhealthtools.ihe.atna.nodeauth.context;
using org.openhealthtools.ihe.common.hl7v2;
using org.openhealthtools.ihe.xds.document;
using org.openhealthtools.ihe.xds.metadata;
using org.openhealthtools.ihe.xds.metadata.impl;
using org.openhealthtools.ihe.xds.response;
using PoCeHealthLive.ViewModel.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using PoCeHealthLive.Model;
using org.ehealth_connector.cda.enums;

namespace PoCeHealthLive.ViewModel
{
    class PublishDocumentViewModel : BaseViewModel
    {
        private string filePath;
        private string documentTitle;
        private XUAConfig config = XUAConfig.getInstance();
        Patient patient = new Patient();
        XDSResponseType response;

        // Property backing fields.
        string selectedClassCode;
        string selectedTypeCode;
        string selectedHealthCareFacilityCode;
        string selectedConfidentialityCode;
        string selectedPracticeSettingCode;
        string selectedFormatCode;

        public BrowseDocumentsCommand BrowseDocumentsCommand { get; set; }
        public PublishCommand PublishCommand { get; set; }
        public ObservableCollection<CustomCode> ClassCodes { get; set; }
        public ObservableCollection<CustomCode> TypeCodes { get; set; }
        public ObservableCollection<CustomCode> HealthCareFacilityCodes { get; set; }
        public ObservableCollection<CustomCode> ConfidentialityCodes { get; set; }
        public ObservableCollection<CustomCode> PracticeSettingCodes { get; set; }
        public ObservableCollection<CustomCode> FormatCodes { get; set; }

        // Declare a FilePath property of type string:
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }

        public string DocumentTitle
        {
            get { return documentTitle; }
            set
            {
                documentTitle = value;
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
        /// Gets or sets the Type Code.
        /// </summary>
        public string SelectedTypeCode
        {
            get { return this.selectedTypeCode; }
            set
            {
                this.selectedTypeCode = value;
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
        /// Gets or sets the SelectedConfidentiality Code.
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
        /// Gets or sets the SelectedConfidentiality Code.
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
        /// Gets or sets the selectedFormat Code.
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

        public PublishDocumentViewModel()
        {

        }

        public PublishDocumentViewModel(Patient patient)
        {
            this.patient = patient;
            this.BrowseDocumentsCommand = new BrowseDocumentsCommand(this);
            this.PublishCommand = new PublishCommand(this);

            // Instantiet item sources
            this.ClassCodes = new ObservableCollection<CustomCode>();
            this.TypeCodes = new ObservableCollection<CustomCode>();
            this.HealthCareFacilityCodes = new ObservableCollection<CustomCode>();
            this.ConfidentialityCodes = new ObservableCollection<CustomCode>();
            this.PracticeSettingCodes = new ObservableCollection<CustomCode>();
            this.FormatCodes = new ObservableCollection<CustomCode>();

            // Set Document attributes
            this.SetDocumentAttributes();
        }

        /// <summary>
        /// Creates Browse Document Dialog to set the file to select 
        /// the document that should be published
        /// </summary>
        public void BrowseDocuments()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf|XML Files (*.xml)|*.xml";

            // Set Dialog title
            dlg.Title = "Open file";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                // Set file path
                FilePath = filename;
            }
        }

        /// <summary>
        /// Sends document to repository of the Affinity domain and shows state of publication. 
        /// </summary>
        public void PublishDocument()
        {
            //System.Console.WriteLine("Registry Infomed Test");
            initConfig();
            string status = PublishDocumentToRepository();

            // Show status of publication in a Message Box
            if (status.Equals("Success") && string.IsNullOrEmpty(status) == false)
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("Document was published successfully", "Confirmation");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Document was not published successfully", "Error occured");
            }

        }
        public string PublishDocumentToRepository()
        {
            // init security context
            XUAContext context = XUAContext.getInstance();

            SubmissionSetMetadata subSet;
            //String pdfFilePath = FilePath;


            try
            {
                ConvenienceCommunication conCom = getInfomedCommunication();

                // Step 3: Sending PDF document NON-TLS
                // Create instance of document metadata  
                DocumentMetadata metaData = conCom.addDocument(DocumentDescriptor.PDF, FilePath);

                // complete document metadata
                completeMetadata(metaData);

                subSet = this.generateSubmissionSetMetadata(metaData);
                response = conCom.submit(subSet);
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
            return response.getStatus().getLiteral();
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

        private void completeMetadata(DocumentMetadata metaData)
        {
            AdministrativeGender sex = AdministrativeGender.MALE;
            patient.setAdministrativeGender(sex);
            metaData.setPatient(patient);
            metaData.setCodedLanguage(LanguageCode.DEUTSCH_CODE);

            // create a random id accoding to id
            // fixme    long rand = Math.round((java.math.Math.random() * 100000000.0));
            Random rnd = new Random();
            long rand = rnd.Next(0, 99999);
            String docId = config.getEmrId() + "." + rand;
            metaData.setUniqueId(docId);
            // Set Document title
            metaData.setTitle(DocumentTitle + " " + docId);
            // Set ClassCode
            metaData.setClassCode(new Code("2.16.756.5.30.1.120.20.1", SelectedClassCode,
                    "2.16.756.5.30.1.120.20.1^" + SelectedClassCode));
            // Set TypeCode
            metaData.setTypeCode(new Code("2.16.756.5.30.1.120.20.2", SelectedTypeCode,
                "2.16.756.5.30.1.120.20.2^" + SelectedTypeCode));
            // Set FormatCode (default pdf)    
            metaData.setFormatCode(new Code("1.3.6.1.4.1.19376.1.2.3", SelectedFormatCode,
                    "1.3.6.1.4.1.19376.1.2.3^" + SelectedFormatCode));
            // Set HealthcareFacilityTypeCode
            metaData.setHealthcareFacilityTypeCode(new Code("2.16.756.5.30.1.127.3.2.1.19", SelectedHealthCareFacilityCode,
                "2.16.756.5.30.1.127.3.2.1.19^" + SelectedHealthCareFacilityCode));
            // Set PracticeSettingCode
            metaData.setPracticeSettingCode(
                    new Code("2.16.756.5.30.1.127.3.2.1.26", SelectedPracticeSettingCode,
                    "2.16.756.5.30.1.127.3.2.1.26^" + SelectedPracticeSettingCode));
            // Set ConfidentialityCode
            metaData.addConfidentialityCode(new Code("2.16.756.5.30.1.120.20.3", SelectedConfidentialityCode,
                "2.16.756.5.30.1.120.20.3^" + SelectedConfidentialityCode));

            // used for submission-set : XDSSubmissionSet.uniqueId and
            // XDSSubmissionSet.sourceId
            metaData.setDocSourceActorOrganizationId(config.getEmrId());
            java.util.List ids = patient.getIds();
            metaData.setSourcePatientId((Identificator)ids.get(0));
 
            SourcePatientInfoType sourceInfo = Hl7v2Factory.eINSTANCE.createSourcePatientInfoType();
            sourceInfo.setPatientSex(patient.getAdministrativeGenderCode().toString());
            XPN xcn = Hl7v2Factory.eINSTANCE.createXPN();
            xcn.setFamilyName(patient.getName().getFamilyName());
            xcn.setGivenName(patient.getName().getGivenNames());
            sourceInfo.getPatientName().add(xcn);
            sourceInfo.getPatientIdentifier()
            .add(XdsUtil.convertEhcIdentificator((Identificator)patient.getIds().get(0)));
            metaData.getMdhtDocumentEntryType().setSourcePatientInfo(sourceInfo);
            metaData.getMdhtDocumentEntryType().getAuthors().add(this.generateAuthor());
        }

        private AuthorType generateAuthor()
        {
            XCN author = Hl7v2Factory.eINSTANCE.createXCN();

            author.setFamilyName("Infomed client test Oliver Egger");
            author.setIdNumber(config.getEmrId());
            // author.setAssigningAuthorityName("");
            author.setAssigningAuthorityUniversalId("2.16.756.5.30.1.122");
            author.setAssigningAuthorityUniversalIdType("ISO^^^^RI"); // RI -
                                                                      // specify
                                                                      // it's an
                                                                      // application

            org.openhealthtools.ihe.xds.metadata.impl.AuthorTypeImpl ati = (AuthorTypeImpl)MetadataFactory.eINSTANCE
                    .createAuthorType();
            ati.setAuthorPerson(author);

            return ati;
        }

        // build our own metadata (in order to set XDSSubmissionSet.uniqueId and
        // XDSSubmissionSet.sourceId)
        private SubmissionSetMetadata generateSubmissionSetMetadata(DocumentMetadata metaData)
        {
            SubmissionSetMetadata ssm = new SubmissionSetMetadata();

            // set author
            ssm.getOhtSubmissionSetType().setAuthor(this.generateAuthor());

            // XDSSubmissionSet.uniqueId
            // doc id. root = 2.16.756.5.30.1.120.14.101.x (where x > 0)
            // submission id. root = 2.16.756.5.30.1.120.14.101.0.x (where x = same
            // value as doc id)
            ssm.getOhtSubmissionSetType()
                    .setUniqueId(metaData.getUniqueId().replaceAll(config.getEmrId() + ".", config.getEmrId() + ".0."));

            ssm.setAvailabilityStatus(AvailabilityStatus.APPROVED);
            // ssm.setComments(comments);
            ssm.setContentTypeCode(metaData.getFormatCode());
            ssm.setDestinationPatientId(metaData.getPatientId());
            //ssm.setDestinationPatientId((Identificator)patient.getIds().get(1));
            ssm.setSourceId(metaData.getDocSourceActorOrganizationId());
            ssm.setTitle(metaData.getTitle());

            return ssm;
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

        private void SetDocumentAttributes()
        {
            // Set Document Class Codes
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

            //Set Document Type Codes
            this.TypeCodes.Add(new CustomCode("Konsilbericht", "11488-4"));
            this.TypeCodes.Add(new CustomCode("Operationbericht", "34874-8"));
            this.TypeCodes.Add(new CustomCode("Untersuchungsbericht (nicht Labor)", "27899-4"));
            this.TypeCodes.Add(new CustomCode("Einweisung Anfrage", "57830-2"));
            this.TypeCodes.Add(new CustomCode("Konsil Anfrage", "57133-1"));
            this.TypeCodes.Add(new CustomCode("Austrittbericht", "11490-0"));
            this.TypeCodes.Add(new CustomCode("Kurzaustrittbericht", "34106-5"));
            this.TypeCodes.Add(new CustomCode("Notfallbericht", "15507-7"));
            this.TypeCodes.Add(new CustomCode("Befund bildgebende Diagnostik", "18748-4"));
            this.TypeCodes.Add(new CustomCode("Laborbefund", "11502-2"));
            this.TypeCodes.Add(new CustomCode("Pathologiebefund", "27898-6"));
            this.TypeCodes.Add(new CustomCode("Sozialmedizinischer Pflegeverlegungsbericht", "34746-8"));
            this.TypeCodes.Add(new CustomCode("Rezept", "57833-6"));
            this.TypeCodes.Add(new CustomCode("Medikamentliste", "56445-0"));
            this.TypeCodes.Add(new CustomCode("Patient Einwilligung", "59284-0"));

            // Set Document Healthcarefacility Codes
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
            this.ConfidentialityCodes.Add(new CustomCode("Administrative", "A"));
            this.ConfidentialityCodes.Add(new CustomCode("Medical", "N"));
            this.ConfidentialityCodes.Add(new CustomCode("Secret", "V"));
            this.ConfidentialityCodes.Add(new CustomCode("Stigmatizing", "R"));
            this.ConfidentialityCodes.Add(new CustomCode("Utilities", "U"));

            // Set Document PracticeSetting Codes
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
            this.PracticeSettingCodes.Add(new CustomCode("Andere medzini-sche Fachrichtung", "260099"));

            // Set Document Format Codes    
            this.FormatCodes.Add(new CustomCode("application/pdf", "urn:ihe:iti:xds-sd:pdf:2008"));
            this.FormatCodes.Add(new CustomCode("text/plain", "urn:ihe:iti:xds-sd:text:2008"));
            this.FormatCodes.Add(new CustomCode("mage/jpeg", "urn:ihe:iti-fr:xds-sd:jpeg:2010"));
            this.FormatCodes.Add(new CustomCode("image/tiff", "urn:ihe:iti-fr:xds-sd:tiff:2010"));
            this.FormatCodes.Add(new CustomCode("text/xml", "urn:ihe:pcc:xds-ms:2007"));
        }
    }
}
