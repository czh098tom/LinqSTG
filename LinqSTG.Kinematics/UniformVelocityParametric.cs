using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class UniformVelocityParametric<TTime, TData, TVelocity>(TVelocity velocity) : IParametric<TTime, TData>
        where TVelocity : IMultiplyOperators<TVelocity, TTime, TData>
    {
        public TVelocity Velocity => velocity;

        public TData Predict(TTime time) => velocity * time;
    }
}
