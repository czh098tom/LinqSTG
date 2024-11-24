using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinqSTG.Extensions;

namespace LinqSTG
{
    public static class Pattern
    {
        /// <summary>
        /// Create a pattern shoot n times with repeat event inside.
        /// </summary>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="times">Shooting times.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<Repeater, TInterval> Repeat<TInterval>(int times)
            where TInterval : struct
        {
            return new RepeatPattern<TInterval>(times);
        }

        /// <summary>
        /// Create a pattern shoot n times with repeat event inside.
        /// </summary>
        /// <typeparam name="TInterval"></typeparam>
        /// <param name="times"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static IPattern<Repeater, TInterval> RepeatWithInterval<TInterval>(int times, TInterval interval)
            where TInterval : struct
        {
            return new RepeatWithIntervalPattern<TInterval>(times, interval);
        }
    }
}
