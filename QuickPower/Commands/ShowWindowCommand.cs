using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuickPower.Commands
{
    public class ShowWindowCommand : ICommand
    {
        private Window _targetWindow;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ShowWindowCommand(Window target)
        {
            _targetWindow = target;
        }

        public void Execute(object parameter)
        {
            _targetWindow.Visibility = Visibility.Visible;
        }

        public bool CanExecute(object parameter)
        {
            return _targetWindow != null && _targetWindow.Visibility != Visibility.Visible;
        }
    }
}


