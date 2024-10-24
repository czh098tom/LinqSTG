using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSTG
{
    public struct PatternNode<TData, TInterval>
        where TInterval : struct
    {
        public TData? Data { get; private set; }
        public TInterval Interval { get; private set; }

        public bool IsData { get; private set; }

        public PatternNode() 
        {
            Data = default;
            Interval = default;
            IsData = false;
        }

        internal PatternNode(TData right)
        {
            Data = right;
            Interval = default;
            IsData = true;
        }

        internal PatternNode(TInterval left)
        {
            Data = default;
            Interval = left;
            IsData = false;
        }
    }
}
