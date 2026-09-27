using System;
using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class Parametric<TTime, TData>(Func<TTime, TData> func) : IParametric<TTime, TData>
    {
        public TData Predict(TTime time) => func(time);
    }

    /// <summary>
    /// A derivable motion defined by delegates: prediction and derivation are each given by a delegate.
    /// </summary>
    public sealed class DerivableParametric<TTime, TData, TDerivative>(
        Func<TTime, TData> predictor, Func<TTime, TDerivative> derivator) : IDerivableParametric<TTime, TData, TDerivative>
    {
        public Func<TTime, TData> Predictor => predictor;
        public Func<TTime, TDerivative> Derivator => derivator;

        public TData Predict(TTime time) => predictor(time);

        public TDerivative Derive(TTime time) => derivator(time);
    }

    public static class Parametric
    {
        public static Parametric<TTime, TData> Create<TTime, TData>(Func<TTime, TData> predictor)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                new Parametric<TTime, TData>(predictor);
        }

        public static DerivableParametric<TTime, TData, TDerivative> Derivable<TTime, TData, TDerivative>(
            Func<TTime, TData> predictor, Func<TTime, TDerivative> derivator)
        {
            return predictor == null ?
                throw new ArgumentNullException(nameof(predictor)) :
                derivator == null ?
                    throw new ArgumentNullException(nameof(derivator)) :
                    new DerivableParametric<TTime, TData, TDerivative>(predictor, derivator);
        }

        public static UniformVelocityParametric<TTime, TData, TVelocity> UniformVelocity<TTime, TData, TVelocity>(TVelocity velocity)
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
        {
            return new UniformVelocityParametric<TTime, TData, TVelocity>(velocity);
        }

        public static UniformAccelerationParametric<TTime, TData, TAccel, TVelocity> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>, IAdditionOperators<TVelocity, TVelocity, TVelocity>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return new UniformAccelerationParametric<TTime, TData, TAccel, TVelocity>(accel);
        }

        public static UniformAccelerationParametric<TTime, TData, TAccel, TVelocity> UniformAcceleration<TTime, TData, TAccel, TVelocity>(TVelocity velocity, TAccel accel)
            where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
            where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>, IAdditionOperators<TVelocity, TVelocity, TVelocity>
            where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
        {
            return new UniformAccelerationParametric<TTime, TData, TAccel, TVelocity>(velocity, accel);
        }
    }
}
