using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Kinematics
{
    public static class ParametricExtension
    {
        public static IParametric<TTime, UData> Select<TTime, TData, UData>(this IParametric<TTime, TData> predictor,
            Func<TData, UData> selector)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                selector == null ?
                    throw new ArgumentNullException(nameof(selector)) :
                    new Parametric<TTime, UData>(time => selector(predictor.Predict(time)));
        }

        public static IParametric<TTime, TData> Offset<TTime, TData>(this IParametric<TTime, TData> predictor, TData data)
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new Parametric<TTime, TData>(time => predictor.Predict(time) + data);
        }

        public static IParametric<TTime, TData> AfterTime<TTime, TData>(this IParametric<TTime, TData> predictor,
            TTime time, IParametric<TTime, TData> after)
            where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TTime>
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                after == null ?
                    throw new ArgumentNullException(nameof(after)) :
                    new Parametric<TTime, TData>(t => t < time ? predictor.Predict(t) : after.Predict(t - time) + predictor.Predict(time));
        }
    }
}
