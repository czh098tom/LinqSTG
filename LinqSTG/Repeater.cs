using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSTG
{
    /// <summary>
    /// Represent a item created by repeat.
    /// </summary>
    /// <param name="ID">Index in repeat.</param>
    /// <param name="Total">Total repeat times in repeat.</param>
    public record struct Repeater(int ID, int Total)
    {
        public readonly float Sample01(IntervalType sampleMethod)
        {
            return sampleMethod switch
            {
                IntervalType.Open => (ID + 1) / (float)(Total + 1),
                IntervalType.HeadClosed => ID / (float)Total,
                IntervalType.TailClosed => (ID + 1) / (float)Total,
                IntervalType.BothClosed => ID / (float)(Total - 1),
                _ => ID / (float)Total,
            };
        }

        public readonly double Sample01Double(IntervalType sampleMethod)
        {
            return sampleMethod switch
            {
                IntervalType.Open => (ID + 1) / (double)(Total + 1),
                IntervalType.HeadClosed => ID / (double)Total,
                IntervalType.TailClosed => (ID + 1) / (double)Total,
                IntervalType.BothClosed => ID / (double)(Total - 1),
                _ => ID / (double)Total,
            };
        }
    }
}
