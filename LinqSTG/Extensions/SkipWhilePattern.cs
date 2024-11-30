using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqSTG.Extensions
{
    internal class SkipWhilePattern<TData, TInterval>(IPattern<TData, TInterval> pattern, Func<TData?, bool> predicate)
        : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            var predicateUsed = false;
            foreach (var node in pattern)
            {
                if (!predicateUsed)
                {
                    if (node.IsData && !predicate(node.Data))
                    {
                        predicateUsed = true;
                        yield return node;
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
