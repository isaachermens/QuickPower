using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuickPower.Commands
{
    public class PowerActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public PowerActionCommand()
        {
        }

        public void Execute(object parameter)
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}


