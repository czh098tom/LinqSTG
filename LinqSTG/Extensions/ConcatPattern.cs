using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class ConcatPattern<TData, TInterval>(
        IPattern<TData, TInterval> pattern1, IPattern<TData, TInterval> pattern2) : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            foreach (var item in pattern1)
            {
                yield return item;
            }
            foreach (var item in pattern2)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
