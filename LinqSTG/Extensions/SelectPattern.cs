using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class SelectPattern<TData, TInterval, UData>(
        IPattern<TData, TInterval> @internal,
        Func<TData?, UData?> selector) : IPattern<UData, TInterval>
        where TInterval : struct
    {
        IEnumerator<PatternNode<UData?, TInterval>> IEnumerable<PatternNode<UData?, TInterval>>.GetEnumerator()
        {
            foreach (var n in @internal)
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<PatternNode<UData?, TInterval>>)(this)).GetEnumerator();
        }
    }
}
