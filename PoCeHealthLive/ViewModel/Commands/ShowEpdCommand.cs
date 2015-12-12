using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoCeHealthLive.ViewModel.Commands
{
    public class ShowEpdCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainWindowViewModel ViewModel { get; set; }
        public ShowEpdCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.ViewModel.ShowWindowEpd();
        }
    }
}
