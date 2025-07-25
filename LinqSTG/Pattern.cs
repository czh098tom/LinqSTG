﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LinqSTG.Extensions;

namespace LinqSTG
{
    public static class Pattern
    {
        /// <summary>
        /// Create a pattern that shoot n times emitting repeat event.
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
        /// Create a pattern that shoot n times emitting repeat event in between with intervals.
        /// </summary>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="times">Shooting times.</param>
        /// <param name="interval">Shooting interval.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<Repeater, TInterval> RepeatWithInterval<TInterval>(int times, TInterval interval)
            where TInterval : struct
        {
            return new RepeatWithIntervalPattern<TInterval>(times, interval);
        }

        /// <summary>
        /// Create a pattern that shoot infinite times emitting integer value as ids.
        /// </summary>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="startID">First index to emit.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<int, TInterval> Infinite<TInterval>(int startID = 0)
            where TInterval : struct
        {
            return new InfinitePattern<TInterval>(startID);
        }

        /// <summary>
        /// Create a pattern that shoot infinite times emitting integer value as ids in between with intervals.
        /// </summary>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="startID">First index to emit.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<int, TInterval> InfiniteWithInterval<TInterval>(TInterval interval, int startID = 0)
            where TInterval : struct
        {
            return new InfiniteWithIntervalPattern<TInterval>(interval, startID);
        }

        /// <summary>
        /// Create a pattern with single shoot.
        /// </summary>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="data">Data to emit.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<TData, TInterval> Single<TData, TInterval>(TData data)
            where TInterval : struct
        {
            return new SingleDataPattern<TData, TInterval>(data);
        }

        /// <summary>
        /// Create a pattern with single interval.
        /// </summary>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="interval">Interval to emit.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<TData, TInterval> SingleInterval<TData, TInterval>(TInterval interval)
            where TInterval : struct
        {
            return new SingleIntervalPattern<TData, TInterval>(interval);
        }

        /// <summary>
        /// Create a pattern with enumerable data.
        /// </summary>
        /// <typeparam name="TData">Data type in pattern.</typeparam>
        /// <typeparam name="TInterval">Interval type in pattern.</typeparam>
        /// <param name="data">Data sequence.</param>
        /// <returns>A pattern created.</returns>
        public static IPattern<TData, TInterval> FromEnumerable<TData, TInterval>(this IEnumerable<TData> data)
            where TInterval : struct
        {
            return new FromEnumerablePattern<TData, TInterval>(data);
        }

        /// <summary>
        /// Creates an empty pattern that matches no data and spans no intervals.
        /// </summary>
        /// <typeparam name="TData">The type of data associated with the pattern.</typeparam>
        /// <typeparam name="TInterval">The type of interval associated with the pattern. Must be a value type.</typeparam>
        /// <returns>An instance of an empty pattern.</returns>
        public static IPattern<TData, TInterval> Empty<TData, TInterval>()
            where TInterval : struct
        {
            return EmptyPattern<TData, TInterval>.Instance;
        }
    }
}
