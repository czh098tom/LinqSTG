using System;
using System.Numerics;

namespace LinqSTG.Kinematics
{
    public static class ParametricExtension
    {
        public static SelectParametric<TTime, TData, UData> Select<TTime, TData, UData>(
            this IParametric<TTime, TData> predictor, Func<TData, UData> selector)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                selector == null ?
                    throw new ArgumentNullException(nameof(selector)) :
                    new SelectParametric<TTime, TData, UData>(predictor, selector);
        }

        public static OffsetParametric<TTime, TData> Offset<TTime, TData>(
            this IParametric<TTime, TData> predictor, TData offset)
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new OffsetParametric<TTime, TData>(predictor, offset);
        }

        public static AfterTimeParametric<TTime, TData> AfterTime<TTime, TData>(
            this IParametric<TTime, TData> predictor, TTime time, IParametric<TTime, TData> after)
            where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TTime>
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                after == null ?
                    throw new ArgumentNullException(nameof(after)) :
                    new AfterTimeParametric<TTime, TData>(predictor, time, after);
        }
    }
}
