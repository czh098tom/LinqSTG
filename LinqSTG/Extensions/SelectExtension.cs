using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal static class SelectExtension
    {
        internal static IEnumerable<PatternNode<UData?, TInterval>> Select<TData, TInterval, UData>(
            this IEnumerable<PatternNode<TData?, TInterval>> enumerable,
            Func<TData?, UData?> selector)
            where TInterval : struct
        {
            foreach (var n in enumerable)
            {
                if (n.IsData)
                {
                    yield return new PatternNode<UData?, TInterval>(selector(n.Data));
                }
                else
                {
                    yield return new PatternNode<UData?, TInterval>(n.Interval);
                }
            }
        }
    }
}
