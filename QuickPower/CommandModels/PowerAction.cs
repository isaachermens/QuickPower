using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPower.CommandModels
{
    public class PowerAction
    {
        public string Description { get; set; }
        public ActionType Action {get; set;}

        public enum ActionType
        {
            Shutdown,
            Restart,
            LogOff,
            Lock,
            Hibernate,
            Sleep
        }

        public PowerAction(string description, ActionType action)
        {
            Description = description;
            Action = action;
        }

        public static List<PowerAction> GetActions()
        {
            return new List<PowerAction>{
                new PowerAction("Shutdown", ActionType.Shutdown),
                new PowerAction("Restart", ActionType.Restart)
            };
        }
    }
}
