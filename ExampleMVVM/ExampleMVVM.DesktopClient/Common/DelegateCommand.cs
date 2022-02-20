using System;
using System.Windows.Input;

namespace ExampleMVVM.DesktopClient.Common
{
    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action execute)
            : base(parameter => execute())
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : base(parameter => execute(), parameter => canExecute())
        {
        }

        public DelegateCommand(Action<object> execute)
            : base(execute)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion // ICommand Members
    }
}
