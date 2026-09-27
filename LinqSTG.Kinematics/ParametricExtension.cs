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

        /// <summary>
        /// Offset a derivable motion.
        /// Translating by a constant offset does not change the derivative, so the result stays derivable.
        /// This overload is preferred over the IParametric overload when the source is derivable.
        /// </summary>
        public static OffsetParametric<TTime, TData, TDerivative> Offset<TTime, TData, TDerivative>(
            this IDerivableParametric<TTime, TData, TDerivative> predictor, TData offset)
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new OffsetParametric<TTime, TData, TDerivative>(predictor, offset);
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

        /// <summary>
        /// Switch a derivable motion to another derivable motion after a given time.
        /// The result stays derivable: the source derivative is used before the switch time and the after derivative from it on.
        /// This overload is preferred over the IParametric overload when both motions are derivable.
        /// </summary>
        public static AfterTimeParametric<TTime, TData, TDerivative> AfterTime<TTime, TData, TDerivative>(
            this IDerivableParametric<TTime, TData, TDerivative> predictor,
            TTime time,
            IDerivableParametric<TTime, TData, TDerivative> after)
            where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TTime>
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                after == null ?
                    throw new ArgumentNullException(nameof(after)) :
                    new AfterTimeParametric<TTime, TData, TDerivative>(predictor, time, after);
        }
    }
}
