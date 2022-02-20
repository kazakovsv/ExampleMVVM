using System;
using System.Windows.Input;

namespace ExampleMVVM.DesktopClient.ViewModels
{
    public class CommandViewModel : BaseViewModel
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            DisplayName = displayName;
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public ICommand Command { get; }
    }
}
