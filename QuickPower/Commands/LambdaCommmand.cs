using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickPower.Commands
{
    public class LambdaCommand : ICommand
    {
        private Action _action;
        private bool _canExecute;
        /// <summary>
        /// Initializes a new instance of the LambdaCommand class.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="canExecute">if set to <c>true</c> [can execute].</param>
        public LambdaCommand(Action action, bool canExecute = true)
        {
            //  Set the action.
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _action != null && _canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
