using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class SelectManyConcatPattern<TData, TInterval, UData, VData>(
        IPattern<TData, TInterval> pattern,
        Func<TData?, IPattern<UData?, TInterval>> binder,
        Func<TData?, UData?, VData?> resultMapper) : IPattern<VData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<VData?, TInterval>> GetEnumerator()
        {
            foreach (var n in pattern)
            {
                if (n.IsData)
                {
                    foreach (var item in binder(n.Data))
                    {
                        if (item.IsData)
                        {
                            yield return new PatternNode<VData?, TInterval>(resultMapper(n.Data, item.Data));
                        }
                        else
                        {
                            yield return new PatternNode<VData?, TInterval>(item.Interval);
                        }
                    }
                }
                else
                {
                    yield return new PatternNode<VData?, TInterval>(n.Interval);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
