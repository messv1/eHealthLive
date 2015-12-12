using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoCeHealthLive.ViewModel.Commands
{
    class BrowseDocumentsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public BrowseDocumentsCommand(PublishDocumentViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public PublishDocumentViewModel ViewModel { get; set; }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.ViewModel.BrowseDocuments();
        }
    }
}
