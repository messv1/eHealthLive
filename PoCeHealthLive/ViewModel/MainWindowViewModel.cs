using PoCeHealthLive.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PoCeHealthLive.Model;
using System.Windows;
using PoCeHealthLive.View;
using org.ehealth_connector.common;
using java.text;
using org.ehealth_connector.cda.enums;

namespace PoCeHealthLive.ViewModel
{

    public class MainWindowViewModel : BaseViewModel
    {
        Patient patient = new Patient();
        DemographicData demographicData = new DemographicData();

        private PublishDocumentViewModel childViewModelPublishDocument;
        private EpdDocumentViewModel childViewModelEpdDocument;

        public SearchPatientCommand SearchPatientCommand { get; set; }
        public ConnectToInfomedCommand ConnectToInfomedCommand { get; set; }
        public DisconnectInfomedCommand DisconnectInfomedCommand { get; set; }
        public ShowWindowPublishDocumentCommand ShowWindowPublishDocumentCommand { get; set; }
        public ClearDemographicDataCommand ClearDemographicDataCommand { get; set; }
        public ShowEpdCommand ShowEpdCommand { set; get; }

        // Property backing fields.
        string paraGivenName;
        string paraFamilyName;
        string paraDob;

        /// <summary>
        /// Gets or sets the Given name.
        /// </summary>
        public string ParaGivenName {
            get { return this.paraGivenName; }
            set
            {
                this.paraGivenName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Family name.
        /// </summary>
        public string ParaFamilyName
        {
            get { return this.paraFamilyName; }
            set
            {
                this.paraFamilyName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public string ParaDob {
            get { return this.paraDob; }
            set
            {
                this.paraDob = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DemographicData> PatientInfo { get; set; }

        /// <summary>
        /// MainWindowViewModel constructor
        /// </summary>
        public MainWindowViewModel()
        {
            PatientInfo = new ObservableCollection<DemographicData>();
            childViewModelPublishDocument = new PublishDocumentViewModel(patient);
            childViewModelEpdDocument = new EpdDocumentViewModel();

            this.SearchPatientCommand = new SearchPatientCommand(this);
            this.ConnectToInfomedCommand = new ConnectToInfomedCommand(this);
            this.DisconnectInfomedCommand = new DisconnectInfomedCommand(this);
            this.ShowWindowPublishDocumentCommand = new ShowWindowPublishDocumentCommand(this);
            this.ClearDemographicDataCommand = new ClearDemographicDataCommand(this);
            this.ShowEpdCommand = new ShowEpdCommand(this);
        }


        public void SearchPatient()
        {
            demographicData.GivenName = ParaGivenName;
            demographicData.FamilyName = ParaFamilyName;
            demographicData.Dob = ParaDob;
            //Get demographic patient data from triamed database
            demographicData.getAdministrativeDataFromDB();

            if (!string.IsNullOrEmpty(demographicData.PatID))
            {
                PatientInfo.Clear();
                PatientInfo.Add(demographicData);

                // Create Patient
                patient.addName(new Name(demographicData.GivenName, demographicData.FamilyName));
                patient.setBirthday(DateUtil.date(demographicData.Dob));
                AdministrativeGender sex = AdministrativeGender.MALE; // Abfrage von Datenbank muss noch umgesetzt werden
            }
            else PatientInfo.Clear();
        }

        public void ConnectToInfomed()
        {
            PatientDemographicsQuery request = new PatientDemographicsQuery();
            patient = request.patientDemographicsQueryID(patient);
            java.util.List ids;

            ids= patient.getIds();
            // Set Infomed ID of first patien
            demographicData.IpID = ((Identificator)ids.get(0)).getExtension();
            // get Infomed Folder ID
            demographicData.FolderID = ((Identificator)ids.get(1)).getExtension();

            PatientInfo.Clear();
            PatientInfo.Add(demographicData);
        }

        public void DisconnectInfomed()
        {
            // unklar wie bestehende Id eines Patienten gelöscht werden kann.
            demographicData.IpID = null;
            PatientInfo.Clear();
            PatientInfo.Add(demographicData);
        }

        public void ShowWindowPublishDocument()
        {
            PublishDocumentWindow view = new PublishDocumentWindow();
            view.DataContext = childViewModelPublishDocument;
            // Show Window PublishDocument
            view.ShowDialog();
        }
        public void ClearDemographicParameters()
        {
            ParaGivenName = "";
            ParaFamilyName = "";
            ParaDob = "";
            demographicData.FamilyName = null;
            demographicData.GivenName = null;
            demographicData.Dob = null;
            demographicData.FolderID = null;
            demographicData.IpID = null;
            PatientInfo.Clear();
        }

        public void ShowWindowEpd()
        {
            EpdDocumentWindow view = new EpdDocumentWindow();
            view.DataContext = childViewModelEpdDocument;
            //PublishDocumentWindow
            view.ShowDialog();

        }
    }
}
