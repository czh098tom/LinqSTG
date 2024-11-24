using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class TrimStartPattern<TData, TInterval>(
        IPattern<TData, TInterval> pattern) : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            var curr = pattern.GetEnumerator();
            bool started = false;
            while (curr.MoveNext())
            {
                if (curr.Current.IsData)
                {
                    started = true;
                }
                if (started)
                {
                    yield return curr.Current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
