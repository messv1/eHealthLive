using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoCeHealthLive.ViewModel.Commands
{
    class SearchDocumentsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public SearchDocumentsCommand(EpdDocumentViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }
        public EpdDocumentViewModel ViewModel { get; set; }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.ViewModel.SearchDocumentsInRegistry();
        }
    }
}
