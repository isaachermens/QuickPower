using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickPower.Commands;

namespace QuickPower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ShowWindowCommand ShowWindow { get; set; }
        public LambdaCommand CloseWindow { get { return new LambdaCommand(Close); } }

        public MainWindow()
        {
            InitializeComponent();
            ShowWindow = new ShowWindowCommand(this);
            DataContext = this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Manually hide the notify icon so that it doesn't linger
            // Otherwise, the icon will show until hovered over
            NotifyIcon.Visibility = Visibility.Collapsed;
        }

    }
}
