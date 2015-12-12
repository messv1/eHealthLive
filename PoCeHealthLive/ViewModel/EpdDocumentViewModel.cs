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
        public ObservableCollection<ClassCode> ClassCodes { get; set; }
        public ObservableCollection<HealthCareFacilityCode> HealthCareFacilityCodes { get; set; }
        public ObservableCollection<ConfidentialityCode> ConfidentialityCodes { get; set; }
        public ObservableCollection<PracticeSettingCode> PracticeSettingCodes { get; set; }
        public ObservableCollection<FormatCode> FormatCodes { get; set; }

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
            this.ClassCodes = new ObservableCollection<ClassCode>();
            this.HealthCareFacilityCodes = new ObservableCollection<HealthCareFacilityCode>();
            this.ConfidentialityCodes = new ObservableCollection<ConfidentialityCode>();
            this.PracticeSettingCodes = new ObservableCollection<PracticeSettingCode>();
            this.FormatCodes = new ObservableCollection<FormatCode>();

            // Set Document attributes
            this.SetDocumentAttributes();
            
        }

        public void SetDocumentAttributes()
        {    
            // Set Document Class Codes
            this.ClassCodes.Add(new ClassCode("Konsultation Notizen", "DTC01"));
            this.ClassCodes.Add(new ClassCode("Verlauf Notizen", "DTC02"));
            this.ClassCodes.Add(new ClassCode("Behandlungen Notizen", "DTC03"));
            this.ClassCodes.Add(new ClassCode("Untersuchung Verordnung", "DTC04"));
            this.ClassCodes.Add(new ClassCode("Behandlung Verordnungen", "DTC05"));
            this.ClassCodes.Add(new ClassCode("Episoden Zusamenfassung", "DTC06"));
            this.ClassCodes.Add(new ClassCode("Verlaufsberichte", "DTC07"));
            this.ClassCodes.Add(new ClassCode("Untersuchungs Resultate", "DTC08"));
            this.ClassCodes.Add(new ClassCode("Benachrichtigungen", "DTC09"));
            this.ClassCodes.Add(new ClassCode("Krankengeschichte Zusammenfassungen", "DTC10"));
            this.ClassCodes.Add(new ClassCode("Aktuelle Zustandszusammenfassungen", "DTC11"));
            this.ClassCodes.Add(new ClassCode("Behandlungsplan", "DTC12"));
            this.ClassCodes.Add(new ClassCode("Warnungen", "DTC13"));
            this.ClassCodes.Add(new ClassCode("Andere", "DTC90"));

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
            this.ConfidentialityCodes.Add(new ConfidentialityCode("Administrative", "A"));
            this.ConfidentialityCodes.Add(new ConfidentialityCode("Medical", "N"));
            this.ConfidentialityCodes.Add(new ConfidentialityCode("Secret", "V"));
            this.ConfidentialityCodes.Add(new ConfidentialityCode("Stigmatizing", "R"));
            this.ConfidentialityCodes.Add(new ConfidentialityCode("Utilities", "U"));

            // Set Document PracticeSetting Codes
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Allergologie und klinische Immunologie", "260001"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Allgemeinmedizin", "260002"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Anästhesiologie", "260003"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Angiologie", "260004"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Pharmakologie", "260005"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Arbeitsmedizin", "260006"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Augenoptik", "260007"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Chiropraktik", "260008"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Chirurgie", "260009"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Dermatologie/Venerologie", "260010"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Endokrinologie/Diabetologie", "260011"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Ergotherapie", "260012"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Ernährungsberatung", "260013"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Geriatrie", "260014"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Gastroenterologie", "260015"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Gynäkologie/Geburtshilfe", "260016"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Handchirurgie", "260017"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Hebamme", "260018"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Herz- und thorakale Gefässchirurgie", "260019"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Infektiologie", "260020"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Innere Medizin", "260021"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Intensivmedizin", "260022"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Kardiologie", "260023"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Kinder-/Jugendmedizin", "260024"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Kinder/Jugend-psychiatrie/-psychotherapie", "260025"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Kinderchirurgie", "260026"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Klinische Psychologie", "260027"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Kur-/Präventions-Einrichtung", "260028"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Labordiagnostik", "260029"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Logopädie", "260030"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Medizinische Genetik", "260031"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Medizinische Onkologie", "260032"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Mund-, Kiefer- und Gesichtschirurgie", "260033"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Nephrologie", "260034"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Neurochirurgie", "260035"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Neurologie", "260036"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Nuklearmedizin", "260037"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Ophthalmologie", "260038"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Orthopädische Chirurgie und Traumatologie des Bewegungsapparates", "260039"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Osteopathie", "260040"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Oto-Rhino-Laryngologie", "260041"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Palliativmedizin", "260042"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Pathologie", "260043"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Pflege ambulant (zu Hause)", "260044"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Pflege stationär", "260045"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Physikalische Medizin", "260046"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Physiotherapie", "260047"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Plastische, Rekonstruktive und Ästhetische Chirurgie", "260048"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Pneumologie", "260049"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Podologie", "260050"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Prävention", "260051"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Psychiatrie und Psychotherapie", "260052"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Psychosomatik", "260053"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Radiologie", "260054"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Radio-Onkologie/Strahlentherapie", "260055"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Rechtsmedizin", "260056"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Rehabilitation", "260057"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Rettungsmedizin", "260058"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Rheumatologie", "260059"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Sozialdienst", "260060"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Thoraxchirurgie", "260061"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Transfusionsmedizin", "260062"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Tropen-/Reisemedizin", "260063"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Unfallchirurgie", "260064"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Urologie", "260065"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Zahnheilkunde", "260066"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Zahn-, Mund- und Kieferheilkunde", "260067"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Hämatologie", "260068"));
            this.PracticeSettingCodes.Add(new PracticeSettingCode("Andere medzini-sche Fachrichtung", "260099"));

            // Set Document Format Codes    
            this.FormatCodes.Add(new FormatCode("application/pdf", "urn:ihe:iti:xds-sd:pdf:2008"));
            this.FormatCodes.Add(new FormatCode("text/plain", "urn:ihe:iti:xds-sd:text:2008"));
            this.FormatCodes.Add(new FormatCode("mage/jpeg", "urn:ihe:iti-fr:xds-sd:jpeg:2010"));
            this.FormatCodes.Add(new FormatCode("image/tiff", "urn:ihe:iti-fr:xds-sd:tiff:2010"));
            this.FormatCodes.Add(new FormatCode("text/xml", "urn:ihe:pcc:xds-ms:2007"));
        }

        public void SearchDocumentsInRegistry()
        {
            Debug.WriteLine("Methode SearchDocumentInRegistry ausgeführt");
        }
    }
}
