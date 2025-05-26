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

#if NET7_0_OR_GREATER
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
#else
        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, int> SelectMany<TData, UData>(
            this IPattern<TData, int> pattern,
            Func<TData?, IPattern<UData?, int>> binder)
        {
            return new SelectManyPatternInt<TData, UData, UData>(pattern, binder, (t, u) => u);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Then map the original pattern shoot event and the sub-pattern shoot event into a new argument.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Sub-pattern shoot event.</typeparam>
        /// <typeparam name="VData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <param name="selector">Mapping function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<VData, int> SelectMany<TData, UData, VData>(
            this IPattern<TData, int> pattern,
            Func<TData?, IPattern<UData?, int>> binder,
            Func<TData?, UData?, VData?> selector)
        {
            return new SelectManyPatternInt<TData, UData, VData>(pattern, binder, selector);
        }
        
        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, long> SelectMany<TData, UData>(
            this IPattern<TData, long> pattern,
            Func<TData?, IPattern<UData?, long>> binder)
        {
            return new SelectManyPatternLong<TData, UData, UData>(pattern, binder, (t, u) => u);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Then map the original pattern shoot event and the sub-pattern shoot event into a new argument.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Sub-pattern shoot event.</typeparam>
        /// <typeparam name="VData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <param name="selector">Mapping function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<VData, long> SelectMany<TData, UData, VData>(
            this IPattern<TData, long> pattern,
            Func<TData?, IPattern<UData?, long>> binder,
            Func<TData?, UData?, VData?> selector)
        {
            return new SelectManyPatternLong<TData, UData, VData>(pattern, binder, selector);
        }
        
        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, float> SelectMany<TData, UData>(
            this IPattern<TData, float> pattern,
            Func<TData?, IPattern<UData?, float>> binder)
        {
            return new SelectManyPatternFloat<TData, UData, UData>(pattern, binder, (t, u) => u);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Then map the original pattern shoot event and the sub-pattern shoot event into a new argument.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Sub-pattern shoot event.</typeparam>
        /// <typeparam name="VData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <param name="selector">Mapping function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<VData, float> SelectMany<TData, UData, VData>(
            this IPattern<TData, float> pattern,
            Func<TData?, IPattern<UData?, float>> binder,
            Func<TData?, UData?, VData?> selector)
        {
            return new SelectManyPatternFloat<TData, UData, VData>(pattern, binder, selector);
        }
        
        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<UData, double> SelectMany<TData, UData>(
            this IPattern<TData, double> pattern,
            Func<TData?, IPattern<UData?, double>> binder)
        {
            return new SelectManyPatternDouble<TData, UData, UData>(pattern, binder, (t, u) => u);
        }

        /// <summary>
        /// Map each shoot event argument in a pattern to a new sub-pattern to obtain a new pattern.
        /// Then map the original pattern shoot event and the sub-pattern shoot event into a new argument.
        /// Interval in subpattern is combined into the original pattern timeline.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="UData">Sub-pattern shoot event.</typeparam>
        /// <typeparam name="VData">Target shoot event.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="binder">Projecting function.</param>
        /// <param name="selector">Mapping function.</param>
        /// <returns>A new pattern with event mapped.</returns>
        public static IPattern<VData, double> SelectMany<TData, UData, VData>(
            this IPattern<TData, double> pattern,
            Func<TData?, IPattern<UData?, double>> binder,
            Func<TData?, UData?, VData?> selector)
        {
            return new SelectManyPatternDouble<TData, UData, VData>(pattern, binder, selector);
        }
#endif
        /// <summary>
        /// Filter the pattern by a predicate.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="predicate">filter function.</param>
        /// <returns>A new pattern with event filtered.</returns>
        public static IPattern<TData, TInterval> Where<TData, TInterval>(this IPattern<TData, TInterval> pattern,
            Predicate<TData?> predicate)
            where TInterval : struct
        {
            return new WherePattern<TData, TInterval>(pattern, predicate);
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
        /// Create a pattern skipping first n shooting events and corresponding intervals.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="count">Skip count.</param>
        /// <returns>A new pattern with first n shooting events removed.</returns>
        public static IPattern<TData, TInterval> Skip<TData, TInterval>(
            this IPattern<TData, TInterval> pattern, int count)
            where TInterval : struct
        {
            return new SkipPattern<TData, TInterval>(pattern, count);
        }

        /// <summary>
        /// Create a pattern skipping shooting events match predicate and corresponding intervals.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="predicate">Skipping predicate.</param>
        /// <returns>A new pattern with shooting events removed.</returns>
        public static IPattern<TData, TInterval> SkipWhile<TData, TInterval>(
            this IPattern<TData, TInterval> pattern, Func<TData?, bool> predicate)
            where TInterval : struct
        {
            return new SkipWhilePattern<TData, TInterval>(pattern, predicate);
        }

        /// <summary>
        /// Create a pattern taking first n shooting events and corresponding intervals.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="count">Take count.</param>
        /// <returns>A new pattern with first n shooting events only.</returns>
        public static IPattern<TData, TInterval> Take<TData, TInterval>(
            this IPattern<TData, TInterval> pattern, int count)
            where TInterval : struct
        {
            return new TakePattern<TData, TInterval>(pattern, count);
        }

        /// <summary>
        /// Create a pattern taking shooting events match predicate and corresponding intervals.
        /// </summary>
        /// <typeparam name="TData">Original shoot event.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="pattern">Source pattern.</param>
        /// <param name="predicate">Taking predicate.</param>
        /// <returns>A new pattern with shooting events match predicate only.</returns>
        public static IPattern<TData, TInterval> TakeWhile<TData, TInterval>(
            this IPattern<TData, TInterval> pattern, Func<TData?, bool> predicate)
            where TInterval : struct
        {
            return new TakeWhilePattern<TData, TInterval>(pattern, predicate);
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
