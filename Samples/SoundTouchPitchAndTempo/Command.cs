using System;
using System.Windows.Input;

namespace SoundTouchPitchAndTempo
{
    public class Command : ICommand
    {
        private readonly Action<object> _executeWithParameter;
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public Command(Action<object> execute)
        {
            _executeWithParameter = execute;
            _canExecute = () => true;
        }

        public Command(Action execute)
        {
            _execute = execute;
            _canExecute = () => true;
        }

        public void Execute(object parameter)
        {
            if(parameter != null)
            {
                _executeWithParameter(parameter);
            }
            else
            {
                _execute();
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}