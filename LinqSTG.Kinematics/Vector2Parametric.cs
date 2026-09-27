using System.Numerics;

namespace LinqSTG.Kinematics
{
    /// <summary>
    /// A uniform velocity motion on the frame-count time axis (<c>TTime = int</c>).
    /// Frame-time counterpart of <see cref="UniformVelocityParametric{TTime, TData, TVelocity}"/>;
    /// see <see cref="Vector2Parametric"/> for why the generic factory cannot cover this axis.
    /// </summary>
    public sealed class UniformVelocityByFramesParametric<T>(Vector2<T> velocity)
        : IDerivableParametric<int, Vector2<T>, Vector2<T>>
        where T : struct, INumber<T>
    {
        public Vector2<T> Velocity => velocity;

        public Vector2<T> Predict(int time) => velocity * T.CreateChecked(time);

        public Vector2<T> Derive(int time) => velocity;
    }

    /// <summary>
    /// A uniformly accelerated motion on the frame-count time axis (<c>TTime = int</c>).
    /// Frame-time counterpart of <see cref="UniformAccelerationParametric{TTime, TData, TAccel, TVelocity}"/>.
    /// </summary>
    public sealed class UniformAccelerationByFramesParametric<T>
        : IDerivableParametric<int, Vector2<T>, Vector2<T>>
        where T : struct, INumber<T>
    {
        public Vector2<T> Velocity { get; }
        public Vector2<T> Accel { get; }

        public UniformAccelerationByFramesParametric(Vector2<T> accel) : this(Vector2<T>.Zero, accel) { }

        public UniformAccelerationByFramesParametric(Vector2<T> velocity, Vector2<T> accel)
        {
            Velocity = velocity;
            Accel = accel;
        }

        public Vector2<T> Predict(int time)
        {
            var t = T.CreateChecked(time);
            return Velocity * t + Accel * t * t / (Vector2<T>.MultiplicativeIdentity + Vector2<T>.MultiplicativeIdentity);
        }

        public Vector2<T> Derive(int time) => Velocity + Accel * T.CreateChecked(time);
    }

    /// <summary>
    /// Frame-time factories for <see cref="Vector2{T}"/> motions, mirroring
    /// <see cref="Parametric"/> for the <c>TTime = int</c> axis.
    /// The generic factories multiply velocity by time through
    /// <see cref="IMultiplyOperators{TSelf, TOther, TResult}"/>, and a single type cannot
    /// implement multiplication by both its scalar type and <c>int</c>: the two interface
    /// constructions would unify for <c>T = int</c> (CS0695), and the check ignores
    /// constraints. <see cref="Vector2{T}"/> therefore implements the scalar multiply
    /// (covering <c>TTime = T</c>, e.g. float), while these factories cover frame time by
    /// converting the frame count to the scalar explicitly
    /// (<see cref="INumberBase{TSelf}.CreateChecked{TOther}"/>) — generic code has no
    /// implicit <c>int</c>-to-scalar conversion, even though concrete call sites do.
    /// </summary>
    public static class Vector2Parametric
    {
        /// <summary>Creates a uniform velocity motion measured in frames.</summary>
        public static UniformVelocityByFramesParametric<T> UniformVelocityByFrames<T>(Vector2<T> velocity)
            where T : struct, INumber<T>
            => new(velocity);

        /// <summary>Creates a uniformly accelerated motion from rest, measured in frames.</summary>
        public static UniformAccelerationByFramesParametric<T> UniformAccelerationByFrames<T>(Vector2<T> accel)
            where T : struct, INumber<T>
            => new(accel);

        /// <summary>Creates a uniformly accelerated motion, measured in frames.</summary>
        public static UniformAccelerationByFramesParametric<T> UniformAccelerationByFrames<T>(Vector2<T> velocity, Vector2<T> accel)
            where T : struct, INumber<T>
            => new(velocity, accel);
    }
}
