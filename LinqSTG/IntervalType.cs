using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSTG
{
    /// <summary>
    /// Interval type when sampling repeaters.
    /// </summary>
    public enum IntervalType
    {
        /// <summary>
        /// Include neither first value nor last value.
        /// e.g. -o-o-o-
        /// </summary>
        Open = 0b00,
        /// <summary>
        /// Include first value only.
        /// e.g. o-o-o-
        /// </summary>
        HeadClosed = 0b01,
        /// <summary>
        /// Include last value only.
        /// e.g. -o-o-o
        /// </summary>
        TailClosed = 0b10,
        /// <summary>
        /// Include both first value and last value.
        /// e.g. o-o-o
        /// </summary>
        BothClosed = 0b11,
    }
}
