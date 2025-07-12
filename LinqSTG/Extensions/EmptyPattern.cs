using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class EmptyPattern<TData, TInterval> : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public static readonly EmptyPattern<TData, TInterval> Instance = new();

        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }
}
