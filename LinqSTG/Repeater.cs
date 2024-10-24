using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSTG
{
    public struct Repeater
    {
        /// <summary>
        /// Index in repeat.
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Total repeat times in repeat.
        /// </summary>
        public int Total { get; private set; }

        public Repeater(int id, int total)
        {
            ID = id;
            Total = total;
        }

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
    }
}
