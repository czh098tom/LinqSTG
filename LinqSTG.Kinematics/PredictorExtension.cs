using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Kinematics
{
    public static class PredictorExtension
    {
        public static Predictor<TTime, UData> Select<TTime, TData, UData>(this Predictor<TTime, TData> predictor,
            Func<TData, UData> selector)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                selector == null ?
                    throw new ArgumentNullException(nameof(selector)) :
                    new Predictor<TTime, UData>(time => selector(predictor(time)));
        }

        public static Predictor<TTime, TData> Offset<TTime, TData>(this Predictor<TTime, TData> predictor, TData data)
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new Predictor<TTime, TData>(time => predictor(time) + data);
        }

        public static Predictor<TTime, TData> AfterTime<TTime, TData>(this Predictor<TTime, TData> predictor,
            TTime time, Predictor<TTime, TData> after)
            where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TTime>
            where TData : IAdditionOperators<TData, TData, TData>
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                after == null ?
                    throw new ArgumentNullException(nameof(after)) :
                    new Predictor<TTime, TData>(t => t < time ? predictor(t) : after(t - time) + predictor(time));
        }
    }
}
