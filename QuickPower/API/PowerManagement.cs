using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPower.API
{
    public static class PowerManagement
    {
        public static List<PowerScheme> GetPowerSchemes()
        {
            return new List<PowerScheme> { new PowerScheme(Guid.NewGuid(), "Low", false), new PowerScheme(Guid.NewGuid(), "Bob", false) };
        }

        public static void SetActiveScheme(Guid powerSchemeId)
        {
            //todo implement
        }
    }
}
