using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickPower.API
{
    public static class PowerManagement
    {
        [DllImport("PowrProf.dll")]
        public static extern UInt32 PowerEnumerate(IntPtr RootPowerKey, IntPtr SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, UInt32 AcessFlags, UInt32 Index, ref Guid Buffer, ref UInt32 BufferSize);
        [DllImport("PowrProf.dll")]
        public static extern UInt32 PowerReadFriendlyName(IntPtr RootPowerKey, ref Guid SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, IntPtr PowerSettingGuid, IntPtr Buffer, ref UInt32 BufferSize);
        [DllImport("PowrProf.dll")]
        public static extern uint PowerGetActiveScheme(IntPtr UserRootPowerKey, ref IntPtr ActivePolicyGuid);

        [DllImport("PowrProf.dll")]
        public static extern uint PowerSetActiveScheme(IntPtr UserRootPowerKey, ref Guid SchemeGuid);

        public enum AccessFlags : uint
        {
            ACCESS_SCHEME = 16,
            ACCESS_SUBGROUP = 17,
            ACCESS_INDIVIDUAL_SETTING = 18
        }

        private static IEnumerable<Guid> FindAllGuids()
        {
            var schemeGuid = Guid.Empty;

            uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
            uint schemeIndex = 0;

            while(PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 
                (uint)AccessFlags.ACCESS_SCHEME, schemeIndex, 
                ref schemeGuid, ref sizeSchemeGuid) == 0)
            {
                yield return schemeGuid;
                schemeIndex++;
            }
        }

        public static string ReadFriendlyName(Guid schemeGuid)
        {
            uint sizeName = 1024;
            IntPtr pSizeName = Marshal.AllocHGlobal((int)sizeName);

            string friendlyName;

            try
            {
                PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, pSizeName, ref sizeName);
                friendlyName = Marshal.PtrToStringUni(pSizeName);
            }
            finally
            {
                Marshal.FreeHGlobal(pSizeName);
            }

            return friendlyName;
        }

        public static List<PowerScheme> GetPowerSchemes()
        {
            var result = new List<PowerScheme>();
            var activeId = GetActiveScheme();
            foreach (var guid in FindAllGuids())
            {
                var current = new PowerScheme(guid, ReadFriendlyName(guid), guid == activeId);
                result.Add(current);
            }
            return result;
        }

        public static void SetActiveScheme(Guid powerSchemeId)
        {
            var schemeGuid = powerSchemeId;

            PowerSetActiveScheme(IntPtr.Zero, ref schemeGuid);
        }

        // todo, find a way to keep up to date
        // may mean polling, or finding an appropriate event.
        public static Guid GetActiveScheme()
        {
            IntPtr pCurrentSchemeGuid = IntPtr.Zero;

            PowerGetActiveScheme(IntPtr.Zero, ref pCurrentSchemeGuid);

            var currentSchemeGuid = (Guid)Marshal.PtrToStructure(pCurrentSchemeGuid, typeof(Guid));

            return currentSchemeGuid;
        }
    }
}
