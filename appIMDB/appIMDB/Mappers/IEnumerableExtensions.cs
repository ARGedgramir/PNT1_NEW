using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appIMDB.Models;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {

        public static ISet<T> ToSet<T>(this IEnumerable<T> target)
        {
            return new HashSet<T>(target);
        }

        public static void RemoveWhere<T>(this ICollection<T> target, Func<T, bool> removePredicate)
        {
            foreach (var item in target.ToList())
            {
                if (removePredicate(item))
                {
                    target.Remove(item);
                }
            }
        }
    }
}