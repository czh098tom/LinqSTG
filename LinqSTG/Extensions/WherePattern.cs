using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class WherePattern<TData, TInterval>(
        IPattern<TData, TInterval> @internal, Predicate<TData?> predicate)
        : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            foreach (var n in @internal)
            {
                if (n.IsData)
                {
                    if (predicate(n.Data))
                    {
                        yield return new PatternNode<TData?, TInterval>(n.Data);
                    }
                }
                else
                {
                    yield return new PatternNode<TData?, TInterval>(n.Interval);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
