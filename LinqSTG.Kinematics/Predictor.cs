using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Kinematics
{
    public delegate TData Predictor<TTime, TData>(TTime time);

    public static class Predictor
    {
        public static Predictor<TTime, TData> Create<TTime, TData>(Func<TTime, TData> predictor)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new Predictor<TTime, TData>(predictor);
        }

        public static Predictor<TTime, TData> UniformVelocity<TTime, TData, TVelocity>(TVelocity velocity)
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
        {
            return time => velocity * time;
        }

        public static Predictor<TTime, TData> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return time => accel * time * time / (TData.MultiplicativeIdentity + TData.MultiplicativeIdentity);
        }

        public static Predictor<TTime, TData> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TVelocity velocity, TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return time => velocity * time + accel * time * time / (TData.MultiplicativeIdentity + TData.MultiplicativeIdentity);
        }
    }
}
