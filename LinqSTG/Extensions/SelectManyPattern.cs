using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
    internal class SelectManyPattern<TData, TInterval, UData, VData>(
        IPattern<TData, TInterval> pattern,
        Func<TData?, IPattern<UData?, TInterval>> binder,
        Func<TData?, UData?, VData?> resultMapper) : IPattern<VData, TInterval>
        where TInterval : struct, IComparisonOperators<TInterval, TInterval, bool>,
        IMinMaxValue<TInterval>, INumberBase<TInterval>
    {
        public IEnumerator<PatternNode<VData?, TInterval>> GetEnumerator()
        {
            List<(IEnumerator<PatternNode<UData?, TInterval>> node, TData? original, TInterval elapsed, bool isValid)> subEnumerators = [];
            TInterval elapsedTime = TInterval.Zero;
            foreach (var n in pattern)
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

        private static int CalculateMinElapsed(List<(IEnumerator<PatternNode<UData?, TInterval>> node, TData? original, TInterval elapsed, bool isValid)> subEnumerators)
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
