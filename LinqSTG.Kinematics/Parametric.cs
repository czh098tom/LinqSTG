using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Kinematics
{
    public interface IParametric<in TTime, out TData>
    {
        TData Predict(TTime time);
    }

    public sealed class Parametric<TTime, TData>(Func<TTime, TData> func) : IParametric<TTime, TData>
    {
        public TData Predict(TTime time) => func(time);
    }

    public static class Parametric
    {
        public static IParametric<TTime, TData> Create<TTime, TData>(Func<TTime, TData> predictor)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new Parametric<TTime, TData>(predictor);
        }

        public static IParametric<TTime, TData> UniformVelocity<TTime, TData, TVelocity>(TVelocity velocity)
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
        {
            return new Parametric<TTime, TData>(time => velocity * time);
        }

        public static IParametric<TTime, TData> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return new Parametric<TTime, TData>(time => accel * time * time / (TData.MultiplicativeIdentity + TData.MultiplicativeIdentity));
        }

        public static IParametric<TTime, TData> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TVelocity velocity, TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return new Parametric<TTime, TData>(time => velocity * time + accel * time * time / (TData.MultiplicativeIdentity + TData.MultiplicativeIdentity));
        }
    }
}
