using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class UniformVelocityParametric<TTime, TData, TVelocity>(TVelocity velocity)
        : IDerivableParametric<TTime, TData, TVelocity>
        where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
    {
        public TVelocity Velocity => velocity;

        public TData Predict(TTime time) => velocity * time;

        public TVelocity Derive(TTime time) => velocity;
    }
}
