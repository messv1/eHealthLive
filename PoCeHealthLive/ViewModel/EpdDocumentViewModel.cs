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

            // Set Document attributes
            this.SetDocumentAttributes();
            
        }

        public void SetDocumentAttributes()
        {    
            // Set Document Class Codes
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
            this.ConfidentialityCodes.Add(new DocumentAttributes("Administrative", "A"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Medical", "N"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Secret", "V"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Stigmatizing", "R"));
            this.ConfidentialityCodes.Add(new DocumentAttributes("Utilities", "U"));

            // Set Document PracticeSetting Codes
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
            this.PracticeSettingCodes.Add(new DocumentAttributes("Andere medzini-sche Fachrichtung", "260099"));

            // Set Document Format Codes    
            this.FormatCodes.Add(new DocumentAttributes("application/pdf", "urn:ihe:iti:xds-sd:pdf:2008"));
            this.FormatCodes.Add(new DocumentAttributes("text/plain", "urn:ihe:iti:xds-sd:text:2008"));
            this.FormatCodes.Add(new DocumentAttributes("mage/jpeg", "urn:ihe:iti-fr:xds-sd:jpeg:2010"));
            this.FormatCodes.Add(new DocumentAttributes("image/tiff", "urn:ihe:iti-fr:xds-sd:tiff:2010"));
            this.FormatCodes.Add(new DocumentAttributes("text/xml", "urn:ihe:pcc:xds-ms:2007"));
        }

        public void SearchDocumentsInRegistry()
        {
            Debug.WriteLine("Methode SearchDocumentInRegistry ausgeführt");
        }
    }
}
