using System;
using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class Parametric<TTime, TData>(Func<TTime, TData> func) : IParametric<TTime, TData>
    {
        public TData Predict(TTime time) => func(time);
    }

    public static class Parametric
    {
        public static Parametric<TTime, TData> Create<TTime, TData>(Func<TTime, TData> predictor)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new Parametric<TTime, TData>(predictor);
        }

        public static UniformVelocityParametric<TTime, TData, TVelocity> UniformVelocity<TTime, TData, TVelocity>(TVelocity velocity)
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
        {
            return new UniformVelocityParametric<TTime, TData, TVelocity>(velocity);
        }

        public static UniformAccelerationParametric<TTime, TData, TAccel, TVelocity> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return new UniformAccelerationParametric<TTime, TData, TAccel, TVelocity>(accel);
        }

        public static UniformAccelerationParametric<TTime, TData, TAccel, TVelocity> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TVelocity velocity, TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return new UniformAccelerationParametric<TTime, TData, TAccel, TVelocity>(velocity, accel);
        }
    }
}
