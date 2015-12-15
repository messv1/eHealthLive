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
        //TriaMedPatient pat = new TriaMedPatient();
        DemographicData pat = new DemographicData();


        private PublishDocumentViewModel childViewModelPublishDocument;
        private EpdDocumentViewModel childViewModelEpdDocument;

        public SearchPatientCommand SearchPatientCommand { get; set; }
        public ConnectToInfomedCommand ConnectToInfomedCommand { get; set; }
        public DisconnectInfomedCommand DisconnectInfomedCommand { get; set; }
        public ShowWindowPublishDocumentCommand ShowWindowPublishDocumentCommand { get; set; }
        public ClearDemographicDataCommand ClearDemographicDataCommand { get; set; }
        public ShowEpdCommand ShowEpdCommand { set; get; }


        string paraGivenName;
        string paraFamilyName;
        string paraDob;

        public string ParaGivenName {
            get { return this.paraGivenName; }
            set
            {
                this.paraGivenName = value;
                OnPropertyChanged();
            }
        }

        public string ParaFamilyName
        {
            get { return this.paraFamilyName; }
            set
            {
                this.paraFamilyName = value;
                OnPropertyChanged();
            }
        }
        public string ParaDob {
            get { return this.paraDob; }
            set
            {
                this.paraDob = value;
                OnPropertyChanged();
            }
        }

        //public ObservableCollection<TriaMedPatient> PatientInfo { get; set; }
        public ObservableCollection<DemographicData> PatientInfo { get; set; }

        /// <summary>
        /// MainWindowViewModel constructor
        /// </summary>
        public MainWindowViewModel()
        {
            /*PatientInfo = new ObservableCollection<TriaMedPatient>()*/;
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
            pat.GivenName = ParaGivenName;
            pat.FamilyName = ParaFamilyName;
            pat.Dob = ParaDob;
            pat.getAdministrativeDataFromDB();

            if (!string.IsNullOrEmpty(pat.PatID))
            {
                PatientInfo.Clear();
                PatientInfo.Add(pat);
            }
            else PatientInfo.Clear();
            

            // Patient
            Name patientName = new Name(pat.GivenName, pat.FamilyName);
            patient.addName(patientName);
            patient.setBirthday(DateUtil.date(pat.Dob));
            AdministrativeGender sex = AdministrativeGender.MALE; // Abfrage von Datenbank muss noch umgesetzt werden

        }

        public void ConnectToInfomed()
        {
            PatientDemographicsQuery request = new PatientDemographicsQuery();
            Debug.WriteLine(request.patientDemographicsQueryID(pat, patient));
            PatientInfo.Clear();
            PatientInfo.Add(pat);
        }

        public void DisconnectInfomed()
        {
            // unklar wie bestehende Id eines Patienten gelöscht werden kann.
            pat.IpID = null;
            PatientInfo.Clear();
            PatientInfo.Add(pat);
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
            pat.FamilyName = null;
            pat.GivenName = null;
            pat.Dob = null;
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
