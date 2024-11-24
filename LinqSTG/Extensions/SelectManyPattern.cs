using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Extensions
{
#if NET7_0_OR_GREATER
    internal class SelectManyPattern<TData, TInterval, UData, VData>(
        IPattern<TData, TInterval> pattern,
        Func<TData?, IPattern<UData?, TInterval>> binder,
        Func<TData?, UData?, VData?> resultMapper)
        : SelectManyPatternBase<TData, TInterval, UData, VData>(pattern, binder, resultMapper)
        where TInterval : struct, IComparisonOperators<TInterval, TInterval, bool>,
        IMinMaxValue<TInterval>, INumberBase<TInterval>
    {
        protected override TInterval GetZero() => TInterval.Zero;
        protected override TInterval GetMax() => TInterval.MaxValue;
        protected override bool IsZero(TInterval interval) => interval == TInterval.Zero;
        protected override TInterval Add(TInterval interval1, TInterval interval2) => interval1 + interval2;
        protected override TInterval Substract(TInterval interval1, TInterval interval2) => interval1 - interval2;
        protected override bool IsSmallerThan(TInterval interval1, TInterval interval2) => interval1 < interval2;
    }
#else
    internal class SelectManyPatternInt<TData, UData, VData>(
        IPattern<TData, int> pattern,
        Func<TData?, IPattern<UData?, int>> binder,
        Func<TData?, UData?, VData?> resultMapper)
        : SelectManyPatternBase<TData, int, UData, VData>(pattern, binder, resultMapper)
    {
        protected override int GetZero() => 0;
        protected override int GetMax() => int.MaxValue;
        protected override bool IsZero(int interval) => interval == 0;
        protected override int Add(int interval1, int interval2) => interval1 + interval2;
        protected override int Substract(int interval1, int interval2) => interval1 - interval2;
        protected override bool IsSmallerThan(int interval1, int interval2) => interval1 < interval2;
    }

    internal class SelectManyPatternLong<TData, UData, VData>(
        IPattern<TData, long> pattern,
        Func<TData?, IPattern<UData?, long>> binder,
        Func<TData?, UData?, VData?> resultMapper)
        : SelectManyPatternBase<TData, long, UData, VData>(pattern, binder, resultMapper)
    {
        protected override long GetZero() => 0;
        protected override long GetMax() => long.MaxValue;
        protected override bool IsZero(long interval) => interval == 0;
        protected override long Add(long interval1, long interval2) => interval1 + interval2;
        protected override long Substract(long interval1, long interval2) => interval1 - interval2;
        protected override bool IsSmallerThan(long interval1, long interval2) => interval1 < interval2;
    }

    internal class SelectManyPatternFloat<TData, UData, VData>(
        IPattern<TData, float> pattern,
        Func<TData?, IPattern<UData?, float>> binder,
        Func<TData?, UData?, VData?> resultMapper)
        : SelectManyPatternBase<TData, float, UData, VData>(pattern, binder, resultMapper)
    {
        protected override float GetZero() => 0;
        protected override float GetMax() => float.MaxValue;
        protected override bool IsZero(float interval) => interval == 0;
        protected override float Add(float interval1, float interval2) => interval1 + interval2;
        protected override float Substract(float interval1, float interval2) => interval1 - interval2;
        protected override bool IsSmallerThan(float interval1, float interval2) => interval1 < interval2;
    }

    internal class SelectManyPatternDouble<TData, UData, VData>(
        IPattern<TData, double> pattern,
        Func<TData?, IPattern<UData?, double>> binder,
        Func<TData?, UData?, VData?> resultMapper)
        : SelectManyPatternBase<TData, double, UData, VData>(pattern, binder, resultMapper)
    {
        protected override double GetZero() => 0;
        protected override double GetMax() => double.MaxValue;
        protected override bool IsZero(double interval) => interval == 0;
        protected override double Add(double interval1, double interval2) => interval1 + interval2;
        protected override double Substract(double interval1, double interval2) => interval1 - interval2;
        protected override bool IsSmallerThan(double interval1, double interval2) => interval1 < interval2;
    }
#endif
    internal abstract class SelectManyPatternBase<TData, TInterval, UData, VData>(
        IPattern<TData, TInterval> pattern,
        Func<TData?, IPattern<UData?, TInterval>> binder,
        Func<TData?, UData?, VData?> resultMapper) : IPattern<VData, TInterval>
        where TInterval : struct
    {
        protected abstract TInterval GetZero();
        protected abstract TInterval GetMax();
        protected abstract bool IsZero(TInterval interval);
        protected abstract TInterval Add(TInterval interval1, TInterval interval2);
        protected abstract TInterval Substract(TInterval interval1, TInterval interval2);
        protected abstract bool IsSmallerThan(TInterval interval1, TInterval interval2);

        public IEnumerator<PatternNode<VData?, TInterval>> GetEnumerator()
        {
            List<(IEnumerator<PatternNode<UData?, TInterval>> node, TData? original, TInterval elapsed, bool isValid)> subEnumerators = [];
            TInterval elapsedTime = GetZero();
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
                    while (minElapsedID >= 0 && IsSmallerThan(Add(subEnumerators[minElapsedID].elapsed,
                        subEnumerators[minElapsedID].node.Current.Interval), Add(lastElapsedTime, interval)))
                    {
                        var subEnumerator = subEnumerators[minElapsedID].node;
                        var nextInterval = subEnumerator.Current.Interval;
                        var subElapsed = subEnumerators[minElapsedID].elapsed;
                        var nextSub = Substract(Add(subElapsed, nextInterval), elapsedTime);
                        if (!IsZero(nextSub))
                        {
                            yield return new PatternNode<VData?, TInterval>(nextSub);
                        }
                        elapsedTime = Add(subElapsed, nextInterval);
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
                    var next = Substract(Add(interval, lastElapsedTime), elapsedTime);
                    if (!IsZero(next))
                    {
                        yield return new PatternNode<VData?, TInterval>(next);
                    }
                    elapsedTime = Add(lastElapsedTime, interval);
                }
            }
            {
                var minElapsedID = CalculateMinElapsed(subEnumerators);
                while (minElapsedID >= 0)
                {
                    var subEnumerator = subEnumerators[minElapsedID].node;
                    var nextInterval = subEnumerator.Current.Interval;
                    var subElapsed = subEnumerators[minElapsedID].elapsed;
                    var nextSub = Substract(Add(subElapsed, nextInterval), elapsedTime);
                    if (!IsZero(nextSub))
                    {
                        yield return new PatternNode<VData?, TInterval>(nextSub);
                    }
                    elapsedTime = Add(subElapsed, nextInterval);
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

        private int CalculateMinElapsed(List<(IEnumerator<PatternNode<UData?, TInterval>> node, TData? original, TInterval elapsed, bool isValid)> subEnumerators)
        {
            TInterval minElapsed = GetMax();
            var minElapsedID = -1;
            for (int i = 0; i < subEnumerators.Count; i++)
            {
                var node = subEnumerators[i].node;
                if (subEnumerators[i].isValid && IsSmallerThan(Add(node.Current.Interval, subEnumerators[i].elapsed), minElapsed))
                {
                    minElapsed = Add(node.Current.Interval, subEnumerators[i].elapsed);
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
