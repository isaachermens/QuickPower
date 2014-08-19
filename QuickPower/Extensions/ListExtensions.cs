using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPower.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Clone a list of objects which implement the ICloneable interface.
        /// </summary>
        /// <returns></returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
