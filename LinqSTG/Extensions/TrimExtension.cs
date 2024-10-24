using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal static class TrimExtension
    {
        internal static IEnumerable<PatternNode<TData?, TInterval>> TrimEnd<TData, TInterval>(
            this IEnumerable<PatternNode<TData?, TInterval>> enumerable)
            where TInterval : struct
        {
            var prioneer = enumerable.GetEnumerator();
            var local = enumerable.GetEnumerator();
            while (prioneer.MoveNext())
            {
                if (prioneer.Current.IsData)
                {
                    while (local.MoveNext() && !local.Current.IsData)
                    {
                        yield return local.Current;
                    }
                    yield return local.Current;
                }
            }
        }
    }
}
