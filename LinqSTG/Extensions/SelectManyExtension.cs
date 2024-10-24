using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LinqSTG.Extensions
{
    internal static class SelectManyExtension
    {
        internal static IEnumerable<PatternNode<UData?, TInterval>> SelectMany<TData, TInterval, UData>(
            this IEnumerable<PatternNode<TData?, TInterval>> enumerable,
            Func<TData?, IPattern<UData?, TInterval>> binder)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            List<(IEnumerator<PatternNode<UData?, TInterval>> node, TInterval elapsed, bool isValid)> subEnumerators = [];
            TInterval elapsedTime = TInterval.Zero;
            foreach (var n in enumerable)
            {
                if (n.IsData)
                {
                    var subEnumerable = binder(n.Data);
                    var subEnumerator = subEnumerable.GetEnumerator();
                    var idx = subEnumerators.Count;
                    bool isValid = true;
                    while ((isValid = subEnumerator.MoveNext()) && subEnumerator.Current.IsData)
                    {
                        yield return subEnumerator.Current;
                    }
                    subEnumerators.Add((subEnumerator, elapsedTime, isValid));
                }
                else
                {
                    var lastElapsedTime = elapsedTime;
                    var interval = n.Interval;
                    var minElapsedID = CalculateMinElapsed(subEnumerators);
                    while (minElapsedID >= 0 && subEnumerators[minElapsedID].elapsed
                        + subEnumerators[minElapsedID].node.Current.Interval < lastElapsedTime + interval)
                    {
                        var subEnumerator = subEnumerators[minElapsedID].node;
                        var nextInterval = subEnumerator.Current.Interval;
                        var subElapsed = subEnumerators[minElapsedID].elapsed;
                        var nextSub = subElapsed + nextInterval - elapsedTime;
                        if (nextSub != TInterval.Zero)
                        {
                            yield return new PatternNode<UData?, TInterval>(nextSub);
                        }
                        elapsedTime = subElapsed + nextInterval;
                        bool isValid = true;
                        while ((isValid = subEnumerator.MoveNext()) && subEnumerator.Current.IsData)
                        {
                            yield return subEnumerator.Current;
                        }
                        subEnumerators[minElapsedID] = subEnumerators[minElapsedID] with
                        {
                            elapsed = elapsedTime,
                            isValid = isValid
                        };
                        minElapsedID = CalculateMinElapsed(subEnumerators);
                    }
                    var next = interval + lastElapsedTime - elapsedTime;
                    if (next != TInterval.Zero)
                    {
                        yield return new PatternNode<UData?, TInterval>(next);
                    }
                    elapsedTime = lastElapsedTime + interval;
                }
            }
            {
                var minElapsedID = CalculateMinElapsed(subEnumerators);
                while (minElapsedID >= 0)
                {
                    var subEnumerator = subEnumerators[minElapsedID].node;
                    var nextInterval = subEnumerator.Current.Interval;
                    var subElapsed = subEnumerators[minElapsedID].elapsed;
                    var nextSub = subElapsed + nextInterval - elapsedTime;
                    if (nextSub != TInterval.Zero)
                    {
                        yield return new PatternNode<UData?, TInterval>(nextSub);
                    }
                    elapsedTime = subElapsed + nextInterval;
                    bool isValid = true;
                    while ((isValid = subEnumerator.MoveNext()) && subEnumerator.Current.IsData)
                    {
                        yield return subEnumerator.Current;
                    }
                    subEnumerators[minElapsedID] = subEnumerators[minElapsedID] with
                    {
                        elapsed = elapsedTime,
                        isValid = isValid
                    };
                    minElapsedID = CalculateMinElapsed(subEnumerators);
                }
            }
        }

        private static int CalculateMinElapsed<TInterval, UData>(List<(IEnumerator<PatternNode<UData?, TInterval>> node, TInterval elapsed, bool isValid)> subEnumerators)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            TInterval? minElapsed = TInterval.MaxValue;
            var minElapsedID = -1;
            for (int i = 0; i < subEnumerators.Count; i++)
            {
                var node = subEnumerators[i].node;
                if (subEnumerators[i].isValid && node.Current.Interval + subEnumerators[i].elapsed < minElapsed)
                {
                    minElapsed = node.Current.Interval + subEnumerators[i].elapsed;
                    minElapsedID = i;
                }
            }
            return minElapsedID;
        }

        internal static IEnumerable<PatternNode<VData?, TInterval>> SelectMany<TData, TInterval, UData, VData>(
            this IEnumerable<PatternNode<TData?, TInterval>> enumerable,
            Func<TData?, IPattern<UData?, TInterval>> binder,
            Func<TData?, UData?, VData?> resultMapper)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            List<(IEnumerator<PatternNode<UData?, TInterval>> node, TData? original, TInterval elapsed, bool isValid)> subEnumerators = [];
            TInterval elapsedTime = TInterval.Zero;
            foreach (var n in enumerable)
            {
                if (n.IsData)
                {
                    var subEnumerable = binder(n.Data);
                    var subEnumerator = subEnumerable.GetEnumerator();
                    var idx = subEnumerators.Count;
                    bool isValid = true;
                    while ((isValid = subEnumerator.MoveNext()) && subEnumerator.Current.IsData)
                    {
                        yield return new PatternNode<VData?, TInterval>(resultMapper(n.Data, subEnumerator.Current.Data));
                    }
                    subEnumerators.Add((subEnumerator, n.Data, elapsedTime, isValid));
                }
                else
                {
                    var lastElapsedTime = elapsedTime;
                    var interval = n.Interval;
                    var minElapsedID = CalculateMinElapsed(subEnumerators);
                    while (minElapsedID >= 0 && subEnumerators[minElapsedID].elapsed
                        + subEnumerators[minElapsedID].node.Current.Interval < lastElapsedTime + interval)
                    {
                        var subEnumerator = subEnumerators[minElapsedID].node;
                        var nextInterval = subEnumerator.Current.Interval;
                        var subElapsed = subEnumerators[minElapsedID].elapsed;
                        var nextSub = subElapsed + nextInterval - elapsedTime;
                        if (nextSub != TInterval.Zero)
                        {
                            yield return new PatternNode<VData?, TInterval>(nextSub);
                        }
                        elapsedTime = subElapsed + nextInterval;
                        bool isValid = true;
                        while ((isValid = subEnumerator.MoveNext()) && subEnumerator.Current.IsData)
                        {
                            yield return new PatternNode<VData?, TInterval>(resultMapper(subEnumerators[minElapsedID].original, subEnumerator.Current.Data));
                        }
                        subEnumerators[minElapsedID] = subEnumerators[minElapsedID] with
                        {
                            elapsed = elapsedTime,
                            isValid = isValid
                        };
                        minElapsedID = CalculateMinElapsed(subEnumerators);
                    }
                    var next = interval + lastElapsedTime - elapsedTime;
                    if (next != TInterval.Zero)
                    {
                        yield return new PatternNode<VData?, TInterval>(next);
                    }
                    elapsedTime = lastElapsedTime + interval;
                }
            }
            {
                var minElapsedID = CalculateMinElapsed(subEnumerators);
                while (minElapsedID >= 0)
                {
                    var subEnumerator = subEnumerators[minElapsedID].node;
                    var nextInterval = subEnumerator.Current.Interval;
                    var subElapsed = subEnumerators[minElapsedID].elapsed;
                    var nextSub = subElapsed + nextInterval - elapsedTime;
                    if (nextSub != TInterval.Zero)
                    {
                        yield return new PatternNode<VData?, TInterval>(nextSub);
                    }
                    elapsedTime = subElapsed + nextInterval;
                    bool isValid = true;
                    while ((isValid = subEnumerator.MoveNext()) && subEnumerator.Current.IsData)
                    {
                        yield return new PatternNode<VData?, TInterval>(resultMapper(subEnumerators[minElapsedID].original, subEnumerator.Current.Data));
                    }
                    subEnumerators[minElapsedID] = subEnumerators[minElapsedID] with
                    {
                        elapsed = elapsedTime,
                        isValid = isValid
                    };
                    minElapsedID = CalculateMinElapsed(subEnumerators);
                }
            }
        }

        private static int CalculateMinElapsed<TData, TInterval, UData>(List<(IEnumerator<PatternNode<UData?, TInterval>> node, TData? original, TInterval elapsed, bool isValid)> subEnumerators)
            where TInterval : struct,
            IAdditionOperators<TInterval, TInterval, TInterval>,
            IComparisonOperators<TInterval, TInterval, bool>,
            IMinMaxValue<TInterval>,
            INumberBase<TInterval>
        {
            TInterval? minElapsed = TInterval.MaxValue;
            var minElapsedID = -1;
            for (int i = 0; i < subEnumerators.Count; i++)
            {
                var node = subEnumerators[i].node;
                if (subEnumerators[i].isValid && node.Current.Interval + subEnumerators[i].elapsed < minElapsed)
                {
                    minElapsed = node.Current.Interval + subEnumerators[i].elapsed;
                    minElapsedID = i;
                }
            }
            return minElapsedID;
        }

        internal static IEnumerable<PatternNode<UData?, TInterval>> SelectManyConcat<TData, TInterval, UData>(
            this IEnumerable<PatternNode<TData?, TInterval>> enumerable,
            Func<TData?, IPattern<UData?, TInterval>> binder)
            where TInterval : struct
        {
            foreach (var n in enumerable)
            {
                if (n.IsData)
                {
                    foreach (var item in binder(n.Data))
                    {
                        yield return item;
                    }
                }
                else
                {
                    yield return new PatternNode<UData?, TInterval>(n.Interval);
                }
            }
        }

        internal static IEnumerable<PatternNode<VData?, TInterval>> SelectManyConcat<TData, TInterval, UData, VData>(
            this IEnumerable<PatternNode<TData?, TInterval>> enumerable,
            Func<TData?, IPattern<UData?, TInterval>> binder,
            Func<TData?, UData?, VData?> selector)
            where TInterval : struct
        {
            foreach (var n in enumerable)
            {
                if (n.IsData)
                {
                    foreach (var item in binder(n.Data))
                    {
                        if (item.IsData)
                        {
                            yield return new PatternNode<VData?, TInterval>(selector(n.Data, item.Data));
                        }
                        else
                        {
                            yield return new PatternNode<VData?, TInterval>(item.Interval);
                        }
                    }
                }
                else
                {
                    yield return new PatternNode<VData?, TInterval>(n.Interval);
                }
            }
        }
    }
}
