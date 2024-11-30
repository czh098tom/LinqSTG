using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class InfiniteWithIntervalPattern<TInterval>(TInterval interval, int startID = 0, bool startWithInterval = false)
        : IPattern<int, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<int, TInterval>> GetEnumerator()
        {
            if (startWithInterval)
            {
                yield return new PatternNode<int, TInterval>(interval);
            }
            for (int i = startID; ; i++)
            {
                yield return new PatternNode<int, TInterval>(i);
                yield return new PatternNode<int, TInterval>(interval);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
