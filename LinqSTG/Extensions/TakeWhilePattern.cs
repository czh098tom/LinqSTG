using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class TakeWhilePattern<TData, TInterval>(IPattern<TData, TInterval> pattern, Func<TData?, bool> predicate)
        : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            foreach (var node in pattern)
            {
                if (node.IsData)
                {
                    if (predicate(node.Data))
                    {
                        yield return node;
                    }
                    else
                    {
                        yield break;
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
