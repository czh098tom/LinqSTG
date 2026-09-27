using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class UniformAccelerationParametric<TTime, TData, TAccel, TVelocity>
        : IDerivableParametric<TTime, TData, TVelocity>
        where TAccel : IMultiplyOperators<TAccel, TTime, TVelocity>
        where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>, IAdditionOperators<TVelocity, TVelocity, TVelocity>
        where TData : IAdditionOperators<TData, TData, TData>, IDivisionOperators<TData, TData, TData>, IMultiplicativeIdentity<TData, TData>
    {
        public TVelocity Velocity { get; }
        public TAccel Accel { get; }

        public UniformAccelerationParametric(TAccel accel) : this(default!, accel) { }

        public UniformAccelerationParametric(TVelocity velocity, TAccel accel)
        {
            Velocity = velocity;
            Accel = accel;
        }

        public TData Predict(TTime time)
            => Velocity * time + Accel * time * time / (TData.MultiplicativeIdentity + TData.MultiplicativeIdentity);

        public TVelocity Derive(TTime time) => Velocity + Accel * time;
    }
}
