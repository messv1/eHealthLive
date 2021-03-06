﻿using ikvm.extensions;
using java.io;
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
using PoCeHealthLive.View;
using PoCeHealthLive.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PoCeHealthLive.Model;

namespace PoCeHealthLive.ViewModel
{
    class PublishDocumentViewModel : BaseViewModel
    {
        private string filePath;
        private string documentTitle;
        private XUAConfig config = XUAConfig.getInstance();
        Patient patient = new Patient();
        XDSResponseType response;

        public BrowseDocumentsCommand BrowseDocumentsCommand { get; set; }
        public PublishCommand PublishCommand { get; set; }
        public ObservableCollection<Model.ClassCode> ClassCodes { get; set; }
        public ObservableCollection<Model.TypeCode> TypeCodes { get; set; }
        public ObservableCollection<HealthCareFacilityCode> HealthCareFacilityCodes { get; set; }
        public ObservableCollection<Model.ConfidentialityCode> ConfidentialityCodes { get; set; }
        public ObservableCollection<Model.PracticeSettingCode> PracticeSettingCodes { get; set; }
        public ObservableCollection<Model.FormatCode> FormatCodes { get; set; }

        // Property backing fields.
        string selectedClassCode;
        string selectedTypeCode;
        string selectedHealthCareFacilityCode;
        string selectedConfidentialityCode;
        string selectedPracticeSettingCode;
        string selectedFormatCode;


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
            this.ClassCodes = new ObservableCollection<Model.ClassCode>();
            this.TypeCodes = new ObservableCollection<Model.TypeCode>();
            this.HealthCareFacilityCodes = new ObservableCollection<HealthCareFacilityCode>();
            this.ConfidentialityCodes = new ObservableCollection<Model.ConfidentialityCode>();
            this.PracticeSettingCodes = new ObservableCollection<Model.PracticeSettingCode>();
            this.FormatCodes = new ObservableCollection<Model.FormatCode>();

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
                MessageBoxResult result = MessageBox.Show("Document was successfully published", "Confirmation");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Document was not successfully published", "Confirmation");
            }

        }
        public string PublishDocumentToRepository()
        {
            // init security context
            XUAContext context = XUAContext.getInstance();

            SubmissionSetMetadata subSet;
            String pdfFilePath = FilePath;


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
            //java.util.List ids;
            //ids = patient.getIds();
            // Will be removed
            Patient patient = new Patient();
            patient.addId(new Identificator("2.16.756.5.30.1.120.10.1", "214211038")); // Patient Messerli Vinzenz
            metaData.setPatient(patient);
            
            metaData.setCodedLanguage(LanguageCode.DEUTSCH_CODE);
            metaData.setTypeCode(
                    new Code("2.16.840.1.113883.6.1", "34817-7", "Otorhinolaryngology Evaluation and Management Note"));

            // create a random id according to id
            // fixme    long rand = Math.round((java.math.Math.random() * 100000000.0));
            Random rnd = new Random();
            long rand = rnd.Next(0, 99999);

            String docId = config.getEmrId() + "." + rand;
            metaData.setUniqueId(docId);

            metaData.setTitle("test Oliver Document No " + docId);

            // metaData.setLanguage("de-CH");

            // metaData.setTypeCode(new Code("2.16.840.1.113883.6.1", "34817-7",
            // "Otorhinolaryngology Evaluation and Management Note"));
            metaData.setTypeCode(new Code("2.16.756.5.30.1.120.20.2", "11490-0", "2.16.756.5.30.1.120.20.2^11490-0"));
            // metaData.setClassCode(new Code("1.3.6.1.4.1.21367.100.1",
            // "DEMO-Procedure", "Procedure"));
            metaData.setClassCode(new Code("2.16.756.5.30.1.120.20.1", "DTC06", "2.16.756.5.30.1.120.20.1^DTC06"));

            // metaData.setFormatCode(new Code("1.3.6.1.4.1.19376.1.2.3",
            // "urn:ihe:rad:TEXT", "urn:ihe:rad:TEXT"));
            metaData.setFormatCode(new Code("1.3.6.1.4.1.19376.1.2.3", "urn:ihe:iti:xds-sd:pdf:2008",
                    "1.3.6.1.4.1.19376.1.2.3^urn:ihe:iti:xds-sd:pdf:2008"));

            // used for submission-set : XDSSubmissionSet.uniqueId and
            // XDSSubmissionSet.sourceId
            metaData.setDocSourceActorOrganizationId(config.getEmrId());

            metaData.setHealthcareFacilityTypeCode(new Code("2.16.840.1.113883.5.11", "AMB", "Ambulance"));

            // metaData.setPracticeSettingCode(new Code("2.16.840.1.113883.6.96",
            // "408478003", "Critical Care Medicine"));
            metaData.setPracticeSettingCode(
                    new Code("2.16.756.5.30.1.127.3.2.1.26", "260002", "2.16.756.5.30.1.127.3.2.1.26^260002"));
            // 2.16.756.5.30.1.127.3.2.1.19

            metaData.addConfidentialityCode(new Code("2.16.756.5.30.1.120.20.3", "N", "2.16.756.5.30.1.120.20.3^N"));

            // metaData.addAuthor(new Author(new Name("","Infomed client test Oliver
            // Egger"),"2.16.756.5.30.1.120.14.101"));

            metaData.setSourcePatientId(new Identificator("2.16.756.5.30.1.120.10.1", "214211038"));

            //metaData.setSourcePatientId((Identificator)ids.get(0));
            //Debug.WriteLine((Identificator)ids.get(0));

            SourcePatientInfoType sourceInfo = Hl7v2Factory.eINSTANCE.createSourcePatientInfoType();
            sourceInfo.setPatientSex(patient.getAdministrativeGenderCode().toString());
            //sourceInfo.setPatientSex("M");
            XPN xcn = Hl7v2Factory.eINSTANCE.createXPN();
            //xcn.setFamilyName("Messerli");
            xcn.setFamilyName(patient.getName().getFamilyName());
            //xcn.setGivenName("Vinzenz");
            xcn.setGivenName(patient.getName().getGivenNames());
            sourceInfo.getPatientName().add(xcn);
            sourceInfo.getPatientIdentifier()
                .add(XdsUtil.convertEhcIdentificator(new Identificator("2.16.756.5.30.1.120.10.1", "214211038")));

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
            this.ClassCodes.Add(new Model.ClassCode("Konsultation Notizen", "DTC01"));
            this.ClassCodes.Add(new Model.ClassCode("Verlauf Notizen", "DTC02"));
            this.ClassCodes.Add(new Model.ClassCode("Behandlungen Notizen", "DTC03"));
            this.ClassCodes.Add(new Model.ClassCode("Untersuchung Verordnung", "DTC04"));
            this.ClassCodes.Add(new Model.ClassCode("Behandlung Verordnungen", "DTC05"));
            this.ClassCodes.Add(new Model.ClassCode("Episoden Zusamenfassung", "DTC06"));
            this.ClassCodes.Add(new Model.ClassCode("Verlaufsberichte", "DTC07"));
            this.ClassCodes.Add(new Model.ClassCode("Untersuchungs Resultate", "DTC08"));
            this.ClassCodes.Add(new Model.ClassCode("Benachrichtigungen", "DTC09"));
            this.ClassCodes.Add(new Model.ClassCode("Krankengeschichte Zusammenfassungen", "DTC10"));
            this.ClassCodes.Add(new Model.ClassCode("Aktuelle Zustandszusammenfassungen", "DTC11"));
            this.ClassCodes.Add(new Model.ClassCode("Behandlungsplan", "DTC12"));
            this.ClassCodes.Add(new Model.ClassCode("Warnungen", "DTC13"));
            this.ClassCodes.Add(new Model.ClassCode("Andere", "DTC90"));

            //Set Document Type Codes
            this.TypeCodes.Add(new Model.TypeCode("Konsilbericht", "11488-4"));
            this.TypeCodes.Add(new Model.TypeCode("Operationbericht", "34874-8"));
            this.TypeCodes.Add(new Model.TypeCode("Untersuchungsbericht (nicht Labor)", "27899-4"));
            this.TypeCodes.Add(new Model.TypeCode("Einweisung Anfrage", "57830-2"));
            this.TypeCodes.Add(new Model.TypeCode("Konsil Anfrage", "57133-1"));
            this.TypeCodes.Add(new Model.TypeCode("Austrittbericht", "11490-0"));
            this.TypeCodes.Add(new Model.TypeCode("Kurzaustrittbericht", "34106-5"));
            this.TypeCodes.Add(new Model.TypeCode("Notfallbericht", "15507-7"));
            this.TypeCodes.Add(new Model.TypeCode("Befund bildgebende Diagnostik", "18748-4"));
            this.TypeCodes.Add(new Model.TypeCode("Laborbefund", "11502-2"));
            this.TypeCodes.Add(new Model.TypeCode("Pathologiebefund", "27898-6"));
            this.TypeCodes.Add(new Model.TypeCode("Sozialmedizinischer Pflegeverlegungsbericht", "34746-8"));
            this.TypeCodes.Add(new Model.TypeCode("Rezept", "57833-6"));
            this.TypeCodes.Add(new Model.TypeCode("Medikamentliste", "56445-0"));
            this.TypeCodes.Add(new Model.TypeCode("Patient Einwilligung", "59284-0"));

            // Set Document Healthcarefacility Codes
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Institut für medizinische Diagnostik", "190001"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Notfalleinrichtung/Rettungswesen", "190002"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Gesundheitsbehörde", "190003"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Spitex", "190004"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Spital", "190005"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Psychiatrie Spital", "190006"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Gesundheitseinrichtung in der Haftanstalt", "190007"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Organisation für stationäre Krankenpflege", "190008"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Apotheken", "190009"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Hausarztpraxis", "190010"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Facharztpraxis", "190011"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Organisation für stationäre Rehabilitation", "190012"));
            this.HealthCareFacilityCodes.Add(new HealthCareFacilityCode("Andere", "190999"));

            // Set Document Confidentiality Codes
            this.ConfidentialityCodes.Add(new Model.ConfidentialityCode("Administrative", "A"));
            this.ConfidentialityCodes.Add(new Model.ConfidentialityCode("Medical", "N"));
            this.ConfidentialityCodes.Add(new Model.ConfidentialityCode("Secret", "V"));
            this.ConfidentialityCodes.Add(new Model.ConfidentialityCode("Stigmatizing", "R"));
            this.ConfidentialityCodes.Add(new Model.ConfidentialityCode("Utilities", "U"));

            // Set Document PracticeSetting Codes
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Allergologie und klinische Immunologie", "260001"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Allgemeinmedizin", "260002"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Anästhesiologie", "260003"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Angiologie", "260004"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Pharmakologie", "260005"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Arbeitsmedizin", "260006"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Augenoptik", "260007"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Chiropraktik", "260008"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Chirurgie", "260009"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Dermatologie/Venerologie", "260010"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Endokrinologie/Diabetologie", "260011"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Ergotherapie", "260012"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Ernährungsberatung", "260013"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Geriatrie", "260014"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Gastroenterologie", "260015"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Gynäkologie/Geburtshilfe", "260016"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Handchirurgie", "260017"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Hebamme", "260018"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Herz- und thorakale Gefässchirurgie", "260019"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Infektiologie", "260020"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Innere Medizin", "260021"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Intensivmedizin", "260022"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Kardiologie", "260023"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Kinder-/Jugendmedizin", "260024"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Kinder/Jugend-psychiatrie/-psychotherapie", "260025"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Kinderchirurgie", "260026"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Klinische Psychologie", "260027"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Kur-/Präventions-Einrichtung", "260028"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Labordiagnostik", "260029"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Logopädie", "260030"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Medizinische Genetik", "260031"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Medizinische Onkologie", "260032"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Mund-, Kiefer- und Gesichtschirurgie", "260033"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Nephrologie", "260034"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Neurochirurgie", "260035"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Neurologie", "260036"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Nuklearmedizin", "260037"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Ophthalmologie", "260038"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Orthopädische Chirurgie und Traumatologie des Bewegungsapparates", "260039"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Osteopathie", "260040"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Oto-Rhino-Laryngologie", "260041"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Palliativmedizin", "260042"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Pathologie", "260043"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Pflege ambulant (zu Hause)", "260044"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Pflege stationär", "260045"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Physikalische Medizin", "260046"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Physiotherapie", "260047"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Plastische, Rekonstruktive und Ästhetische Chirurgie", "260048"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Pneumologie", "260049"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Podologie", "260050"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Prävention", "260051"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Psychiatrie und Psychotherapie", "260052"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Psychosomatik", "260053"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Radiologie", "260054"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Radio-Onkologie/Strahlentherapie", "260055"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Rechtsmedizin", "260056"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Rehabilitation", "260057"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Rettungsmedizin", "260058"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Rheumatologie", "260059"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Sozialdienst", "260060"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Thoraxchirurgie", "260061"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Transfusionsmedizin", "260062"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Tropen-/Reisemedizin", "260063"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Unfallchirurgie", "260064"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Urologie", "260065"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Zahnheilkunde", "260066"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Zahn-, Mund- und Kieferheilkunde", "260067"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Hämatologie", "260068"));
            this.PracticeSettingCodes.Add(new Model.PracticeSettingCode("Andere medizinische Fachrichtung", "260099"));

            // Set Document Format Codes    
            this.FormatCodes.Add(new Model.FormatCode("application/pdf", "urn:ihe:iti:xds-sd:pdf:2008"));
            this.FormatCodes.Add(new Model.FormatCode("text/plain", "urn:ihe:iti:xds-sd:text:2008"));
            this.FormatCodes.Add(new Model.FormatCode("mage/jpeg", "urn:ihe:iti-fr:xds-sd:jpeg:2010"));
            this.FormatCodes.Add(new Model.FormatCode("image/tiff", "urn:ihe:iti-fr:xds-sd:tiff:2010"));
            this.FormatCodes.Add(new Model.FormatCode("text/xml", "urn:ihe:pcc:xds-ms:2007"));
        }
    }
}
