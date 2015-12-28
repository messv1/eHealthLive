using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoCeHealthLive.ViewModel.Commands
{
    class OpenDocumentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public EpdDocumentViewModel ViewModel { get; set; }

        public OpenDocumentCommand(EpdDocumentViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.ViewModel.OpenDocument();
        }
    }
}
