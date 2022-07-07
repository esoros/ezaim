using System;
using System.Windows.Input;

namespace ezaim {
    public class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;


        private readonly Predicate<object?> _canExecute;
        private readonly Action<object?> _execue;

        public DelegateCommand(Predicate<object?> canExecute, Action<object?> execute) {
            _canExecute = canExecute;
            _execue = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execue(parameter);
        }
    }
}