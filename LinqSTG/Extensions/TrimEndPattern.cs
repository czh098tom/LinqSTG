using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class TrimEndPattern<TData, TInterval>(
        IPattern<TData, TInterval> pattern) : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            var prioneer = pattern.GetEnumerator();
            var local = pattern.GetEnumerator();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
