using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;

using LinqSTG.Extensions;

namespace LinqSTG
{
    public static class PatternExtension
    {
        /// <summary>
        /// Map each shoot event in a pattern to another shoot event.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="selector">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, TInterval> Select<TData, TInterval, UData>(
            this IPattern<TData, TInterval> pattern,
            Func<TData?, UData?> selector)
            where TInterval : struct
        {
            return new SelectPattern<TData, TInterval, UData>(pattern, selector);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Interval in subpattern is concatenated to the original pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, TInterval> SelectManyConcat<TData, TInterval, UData>(
            this IPattern<TData, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder)
            where TInterval : struct
        {
            return new SelectManyConcatPattern<TData, TInterval, UData, UData>(pattern, binder, (t, u) => u);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Then map the original pattern shoot event and the sub-pattern shoot event into a new argument.
        /// Interval in sub-pattern is concatenated to the original pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <typeparam name="UData">Sub-pattern shoot event.</typeparam>
        /// <typeparam name="VData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <param name="selector">Mapping function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<VData, TInterval> SelectManyConcat<TData, TInterval, UData, VData>(
            this IPattern<TData, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder,
            Func<TData?, UData?, VData?> selector)
            where TInterval : struct
        {
            return new SelectManyConcatPattern<TData, TInterval, UData, VData>(pattern, binder, selector);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern. Must be able to add, compare and set as 0 or max value.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, TInterval> SelectMany<TData, TInterval, UData>(
            this IPattern<TData, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder)
            where TInterval : struct,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            return new SelectManyPattern<TData, TInterval, UData, UData>(pattern, binder, (t, u) => u);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Then map the original pattern shoot event and the sub-pattern shoot event into a new argument.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern. Must be able to add, compare and set as 0 or max value.</typeparam>
        /// <typeparam name="UData">Sub-pattern shoot event.</typeparam>
        /// <typeparam name="VData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <param name="selector">Mapping function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<VData, TInterval> SelectMany<TData, TInterval, UData, VData>(
            this IPattern<TData, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder,
            Func<TData?, UData?, VData?> selector)
            where TInterval : struct,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            return new SelectManyPattern<TData, TInterval, UData, VData>(pattern, binder, selector);
        }

        /// <summary>
        /// Concatenates a pattern to the end of the given pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern1">First pattern.</param>
        /// <param name="pattern2">The second pattern attached to the end of fitst.</param>
        /// <returns>A new pattern with event concatenated.</returns>
        public static IPattern<TData, TInterval> Concat<TData, TInterval>(this IPattern<TData, TInterval> pattern1,
             IPattern<TData, TInterval> pattern2)
            where TInterval : struct
        {
            return new ConcatPattern<TData, TInterval>(pattern1, pattern2);
        }

        /// <summary>
        /// Reverse the finite pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <returns>A new pattern with interval at the end removed.</returns>
        public static IPattern<TData, TInterval> Reverse<TData, TInterval>(
            this IPattern<TData, TInterval> pattern)
            where TInterval : struct
        {
            return new ReversePattern<TData, TInterval>(pattern);
        }

        /// <summary>
        /// Remove excess interval at the start of pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <returns>A new pattern with interval at the end removed.</returns>
        public static IPattern<TData, TInterval> TrimStart<TData, TInterval>(
            this IPattern<TData, TInterval> pattern)
            where TInterval : struct
        {
            return new TrimStartPattern<TData, TInterval>(pattern);
        }

        /// <summary>
        /// Remove excess interval at the end of pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <returns>A new pattern with interval at the end removed.</returns>
        public static IPattern<TData, TInterval> TrimEnd<TData, TInterval>(
            this IPattern<TData, TInterval> pattern)
            where TInterval : struct
        {
            return new TrimEndPattern<TData, TInterval>(pattern);
        }

        /// <summary>
        /// Remove excess interval at the start of pattern and at the end of pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <returns>A new pattern with interval at the end removed.</returns>
        public static IPattern<TData, TInterval> Trim<TData, TInterval>(
            this IPattern<TData, TInterval> pattern)
            where TInterval : struct
        {
            return new TrimStartPattern<TData, TInterval>(pattern);
        }
    }
}
