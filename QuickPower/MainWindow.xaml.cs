using System.Windows;
using QuickPower.ViewModels;

namespace QuickPower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NotifyIconVM NotifyVM{ get; set; }

        public MainWindow()
        {
            InitializeComponent();
            NotifyVM = new NotifyIconVM(this);
            DataContext = NotifyVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Manually hide the notify icon so that it doesn't linger
            // Otherwise, the icon will show until hovered over
            NotifyIcon.Visibility = Visibility.Collapsed;
        }

    }
}
