using System.Windows;
using QuickPower.ViewModels;
using Microsoft.Win32;
using Forms = System.Windows.Forms;
using QuickPower.API;
using QuickPower.Properties;
using System;
using Hardcodet.Wpf.TaskbarNotification;

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
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
            NotifyVM = new NotifyIconVM(this);
            DataContext = NotifyVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Manually hide the notify icon so that it doesn't linger
            // Otherwise, the icon will show until hovered over
            NotifyIcon.Visibility = Visibility.Collapsed;
        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs args)
        {
            if(args.Mode != PowerModes.StatusChange)
            {
                return;
            }
            // Todo, base this only on charging/discharging? Or on percentage status?
            Guid id;
            var message = string.Empty;
            switch (Forms.SystemInformation.PowerStatus.PowerLineStatus)
            {
                case Forms.PowerLineStatus.Online:
                    id = Settings.Default.ChargingScheme;
                    if (id != Guid.Empty)
                    {
                        PowerManagement.SetActiveScheme(id);
                        message = String.Format("The power scheme has been automatically set to {0}", PowerManagement.ReadFriendlyName(id));
                    }
                    break;
                case Forms.PowerLineStatus.Offline:
                    id = Settings.Default.DischargingScheme;
                    if (id != Guid.Empty)
                    {
                        PowerManagement.SetActiveScheme(id);
                        message = String.Format("The power scheme has been automatically set to {0}", PowerManagement.ReadFriendlyName(id));
                    }
                    break;
                case Forms.PowerLineStatus.Unknown:
                    // todo, what does unknown indicate? How to best handle this case?
                    message = "Unknown? Panic!";
                    break;
                default:
                    message = "Default? Hmmm";
                    break;
            }
            NotifyVM.UpdateActiveScheme(PowerManagement.GetActiveScheme());
            NotifyIcon.ShowBalloonTip("QuickPower", message, BalloonIcon.Info);
        }
    }
}
