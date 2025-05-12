using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class InfinitePattern<TInterval>(int startID = 0) : IPattern<int, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<int, TInterval>> GetEnumerator()
        {
            for (int i = startID; ; i++)
            {
                yield return new PatternNode<int, TInterval>(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
