using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class SingleIntervalPattern<TData, TInterval>(TInterval interval) : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            yield return new PatternNode<TData?, TInterval>(interval);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
