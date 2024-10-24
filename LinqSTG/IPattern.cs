using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSTG
{
    public interface IPattern<TData, TInterval> : IEnumerable<PatternNode<TData, TInterval>>
        where TInterval : struct
    {
    }
}
