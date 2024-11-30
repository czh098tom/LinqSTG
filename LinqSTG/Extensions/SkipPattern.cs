using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class SkipPattern<TData, TInterval>(IPattern<TData, TInterval> pattern, int count)
        : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            var currSkiped = 0;
            foreach (var node in pattern)
            {
                if (currSkiped < count)
                {
                    if (node.IsData)
                    {
                        currSkiped++;
                    }
                }
                else
                {
                    yield return node;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
