using QuickPower.API;
using QuickPower.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuickPower.Extensions;
using System.ComponentModel;
using QuickPower.Properties;
using QuickPower.CommandModels;

namespace QuickPower.ViewModels
{
    public class NotifyIconVM
    {
        private Window _host;

        public LambdaCommand ChangeDischargingScheme { get; set; }
        public LambdaCommand ChangeChargingScheme { get; set; }
        public LambdaCommand SetActiveScheme { get; set; }
        public PowerActionCommand PowerCommand { get; set; }

        public ShowWindowCommand ShowWindow { get; set; }
        public LambdaCommand CloseWindow { get { return new LambdaCommand(() =>_host.Close()); } }

        public ObservableCollection<PowerScheme> DischargingPowerSchemes { get; set; }
        public ObservableCollection<PowerScheme> ChargingPowerSchemes { get; set; }
        public ObservableCollection<PowerScheme> PowerSchemes { get; set; }
        public ObservableCollection<PowerAction> PowerActions { get; set; }
        
        public NotifyIconVM(Window target)
        {
            _host = target;

            ShowWindow = new ShowWindowCommand(_host);
            ChangeDischargingScheme = new LambdaCommand(ChangeDischarge);
            ChangeChargingScheme = new LambdaCommand(ChangeCharge);
            SetActiveScheme = new LambdaCommand(ChangeActive);
            PowerCommand = new PowerActionCommand();
            InitiallizeCollections();    
        }

        private void InitiallizeCollections()
        {
            var schemes = PowerManagement.GetPowerSchemes();
            // Create clones of the list of schemes to use for charging, discharging, and active scheme menus
            PowerSchemes = new ObservableCollection<PowerScheme>(schemes.Clone());
            DischargingPowerSchemes = new ObservableCollection<PowerScheme>(ActivateScheme(schemes.Clone(), Settings.Default.DischargingScheme));
            ChargingPowerSchemes = new ObservableCollection<PowerScheme>(ActivateScheme(schemes.Clone(), Settings.Default.ChargingScheme));

            PowerActions = new ObservableCollection<PowerAction>(PowerAction.GetActions());
        }

        public void UpdateActiveScheme(Guid schemeId)
        {
            foreach (var scheme in PowerSchemes)
            {
                if (scheme.ID == schemeId)
                    scheme.Active = true;
            }
        }

        /// <summary>
        /// Sets the Active field of each power scheme in the provided list according to 
        /// whether or not the scheme's Guid equals the provided Guid
        /// </summary>
        /// <returns>The modified list of power schemes</returns>
        private IList<PowerScheme> ActivateScheme(IList<PowerScheme> schemes, Guid target)
        {
            foreach (var scheme in schemes)
            {
                scheme.Active = scheme.ID == target;
            }
            return schemes;
        }

        /// <summary>
        /// Set the chosen Power Scheme to use when the battery is discharging
        /// </summary>
        /// <param name="arg">Guid of the Power Scheme to use when discharging</param>
        private void ChangeDischarge(object arg)
        {
            var guid = (Guid)arg;
            var scheme = DischargingPowerSchemes.FirstOrDefault(c => c.ID == guid);
            // If no match was found or the match is not active (it was unchecked), set an empty GUID
            // Otherwise set to the appropriate GUID
            Settings.Default.DischargingScheme = scheme != null && scheme.Active ? scheme.ID : Guid.Empty;
            Settings.Default.Save();
        }

        /// <summary>
        /// Set the chosen Power Scheme to use when the battery is charging
        /// </summary>
        /// <param name="arg">Guid of the Power Scheme to use when charging</param>
        private void ChangeCharge(object arg)
        {
            var guid = (Guid)arg;
            var scheme = ChargingPowerSchemes.FirstOrDefault(c => c.ID == guid);
            // If no match was found or the match is not active (it was unchecked), set an empty GUID
            // Otherwise set to the appropriate GUID
            Settings.Default.ChargingScheme = scheme != null && scheme.Active ? scheme.ID : Guid.Empty;
            Settings.Default.Save();
        }

        /// <summary>
        /// Set the active power scheme to the provided Guid
        /// Behavior is undefined is arg is not Guid
        /// The case of an unchecked option is ignored, since having no active power scheme is invalid behavior
        /// </summary>
        /// <param name="arg"></param>
        private void ChangeActive(object arg)
        {
            PowerManagement.SetActiveScheme((Guid)arg);
        }
    }
}
