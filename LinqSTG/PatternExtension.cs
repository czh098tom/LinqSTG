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
        public static IPattern<UData?, TInterval> Select<TData, TInterval, UData>(
            this IPattern<TData?, TInterval> pattern,
            Func<TData?, UData?> selector)
            where TInterval : struct
        {
            return new Decorator<UData?, TInterval>(((IEnumerable<PatternNode<TData?, TInterval>>)pattern).Select(selector));
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
        public static IPattern<UData?, TInterval> SelectManyConcat<TData, TInterval, UData>(
            this IPattern<TData?, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder)
            where TInterval : struct
        {
            return new Decorator<UData?, TInterval>(((IEnumerable<PatternNode<TData?, TInterval>>)pattern).SelectManyConcat(binder));
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
        public static IPattern<VData?, TInterval> SelectManyConcat<TData, TInterval, UData, VData>(
            this IPattern<TData?, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder,
            Func<TData?, UData?, VData?> selector)
            where TInterval : struct
        {
            return new Decorator<VData?, TInterval>(((IEnumerable<PatternNode<TData?, TInterval>>)pattern).SelectManyConcat(binder, selector));
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
        public static IPattern<UData?, TInterval> SelectMany<TData, TInterval, UData>(
            this IPattern<TData?, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            return new Decorator<UData?, TInterval>(((IEnumerable<PatternNode<TData?, TInterval>>)pattern).SelectMany(binder));
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
        public static IPattern<VData?, TInterval> SelectMany<TData, TInterval, UData, VData>(
            this IPattern<TData?, TInterval> pattern,
            Func<TData?, IPattern<UData?, TInterval>> binder,
            Func<TData?, UData?, VData?> selector)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            return new Decorator<VData?, TInterval>(((IEnumerable<PatternNode<TData?, TInterval>>)pattern).SelectMany(binder, selector));
        }

        /// <summary>
        /// Remove excess interval at the end of pattern.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <returns>A new pattern with interval at the end removed.</returns>
        public static IPattern<TData?, TInterval> TrimEnd<TData, TInterval>(
            this IPattern<TData?, TInterval> pattern)
            where TInterval : struct
        {
            return new Decorator<TData?, TInterval>(((IEnumerable<PatternNode<TData?, TInterval>>)pattern).TrimEnd());
        }
    }
}
