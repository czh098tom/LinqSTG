using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LinqSTG.Extensions
{
    internal class Decorator<TData, TInterval>(IEnumerable<PatternNode<TData, TInterval>> @internal) 
        : IPattern<TData, TInterval>
        where TInterval : struct
    {
        private readonly IEnumerable<PatternNode<TData, TInterval>> @internal = @internal;

        public IEnumerator<PatternNode<TData, TInterval>> GetEnumerator()
        {
            return @internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)@internal).GetEnumerator();
        }
    }
}
