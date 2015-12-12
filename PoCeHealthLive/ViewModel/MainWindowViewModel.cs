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
        private string firstName;
        private string lastName;
        private string dob;
        private string paraFirstName;
        private string paraLastName;
        private string paraDob;
        Patient patient = new Patient();
        TriaMedPatient pat = new TriaMedPatient();
        ObservableCollection<TriaMedPatient> infos;
        private PublishDocumentViewModel childViewModelPublishDocument;
        private EpdDocumentViewModel childViewModelEpdDocument;

        /// <summary>
        /// MainWindowViewModel constructor
        /// </summary>
        public MainWindowViewModel()
        {
            PatientInfo = new ObservableCollection<TriaMedPatient>();
            childViewModelPublishDocument = new PublishDocumentViewModel(patient);
            childViewModelEpdDocument = new EpdDocumentViewModel();

            this.SearchPatientCommand = new SearchPatientCommand(this);
            this.ConnectToInfomedCommand = new ConnectToInfomedCommand(this);
            this.DisconnectInfomedCommand = new DisconnectInfomedCommand(this);
            this.ShowWindowPublishDocumentCommand = new ShowWindowPublishDocumentCommand(this);
            this.ShowEpdCommand = new ShowEpdCommand(this);
        }
        public SearchPatientCommand SearchPatientCommand { get; set; }
        public ConnectToInfomedCommand ConnectToInfomedCommand { get; set; }
        public DisconnectInfomedCommand DisconnectInfomedCommand { get; set; }
        public ShowWindowPublishDocumentCommand ShowWindowPublishDocumentCommand { get; set; }
        public ShowEpdCommand ShowEpdCommand { set; get; }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get
            { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        public string Dob
        {
            get
            { return dob; }
            set
            {
                dob = value;
                OnPropertyChanged();
            }
        }

        public string ParaFirstName
        {
            get { return paraFirstName; }
            set { paraFirstName = value; }
        }

        public string ParaLastName
        {
            get { return paraLastName; }
            set { paraLastName = value; }
        }

        public string ParaDob
        {
            get { return paraDob; }
            set { paraDob = value; }
        }

        public ObservableCollection<TriaMedPatient> PatientInfo
        {
            get
            {
                return infos;
            }
            set
            {
                infos = value;
                OnPropertyChanged("PatientInfo");
            }
        }

        public void SearchPatient()
        {
            pat.FirstName = ParaFirstName;
            pat.LastName = ParaLastName;
            pat.Dob = ParaDob;
            pat.GetTriaMedPatient();
            PatientInfo.Add(pat);

            // Patient
            Name patientName = new Name(pat.FirstName, pat.LastName);
            patient.addName(patientName);
            patient.setBirthday(DateUtil.date(pat.Dob));
            AdministrativeGender sex = AdministrativeGender.MALE; // Abfrage von Datenbank muss noch umgesetzt werden
            patient.setAdministrativeGender(sex);

            //testzwecke
            System.Console.WriteLine(patient.getName().getFamilyName());
            System.Console.WriteLine(patient.getName().getGivenNames());

        }

        public void ConnectToInfomed()
        {
            Debug.WriteLine("Start: MPI Pdq Infomed Test");
            Debug.WriteLine("====================");
            PatientDemographicsQuery request = new PatientDemographicsQuery();
            Debug.WriteLine(request.patientDemographicsQueryID(pat, patient));
            PatientInfo.Add(pat);       
        }

        public void DisconnectInfomed()
        {
            // unklar wie bestehende Id eines Patienten gelöscht werden kann.
            pat.IpID = null;
            PatientInfo.Add(pat);
        }

        public void ShowWindowPublishDocument()
        {
            PublishDocumentWindow view = new PublishDocumentWindow();
            view.DataContext = childViewModelPublishDocument;
            // Show Window PublishDocument
            view.ShowDialog();
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
