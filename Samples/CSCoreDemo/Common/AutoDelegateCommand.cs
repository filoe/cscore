using System;
using System.Windows.Input;

namespace CSCoreDemo
{
    public class AutoDelegateCommand : DelegateCommand, ICommand
    {
        public AutoDelegateCommand(Action<object> execute)
            : base(execute)
        {
        }

        public AutoDelegateCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}