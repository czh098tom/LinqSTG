using LinqSTG.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return new Decorator<Repeater, TInterval>(RepeatImpl<TInterval>(times));
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
            return new Decorator<Repeater, TInterval>(RepeatWithIntervalImpl(times, interval));
        }

        private static IEnumerable<PatternNode<Repeater, TInterval>> RepeatImpl<TInterval>(int times)
            where TInterval : struct
        {
            for (int i = 0; i < times; i++)
            {
                yield return new PatternNode<Repeater, TInterval>(new Repeater(i, times));
            }
        }

        private static IEnumerable<PatternNode<Repeater, TInterval>> RepeatWithIntervalImpl<TInterval>(int times, TInterval interval)
            where TInterval : struct
        {
            for (int i = 0; i < times; i++)
            {
                yield return new PatternNode<Repeater, TInterval>(new Repeater(i, times));
                yield return new PatternNode<Repeater, TInterval>(interval);
            }
        }
    }
}
