using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqSTG.Extensions
{
    internal class SingleDataPattern<TData, TInterval>(TData data) : IPattern<TData, TInterval>
        where TInterval : struct
    {
        public IEnumerator<PatternNode<TData?, TInterval>> GetEnumerator()
        {
            yield return new PatternNode<TData?, TInterval>(data);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
