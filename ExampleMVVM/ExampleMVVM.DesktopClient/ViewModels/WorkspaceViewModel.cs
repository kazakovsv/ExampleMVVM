using ExampleMVVM.DesktopClient.Common;
using System;
using System.Windows.Input;

namespace ExampleMVVM.DesktopClient.ViewModels
{
    public abstract class WorkspaceViewModel : BaseViewModel
    {
        private ICommand _closeCommand;

        protected WorkspaceViewModel()
        {
        }

        public ICommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(OnRequestClose));

        public event EventHandler RequestClose;

        protected virtual void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
