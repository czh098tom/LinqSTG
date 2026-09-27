using System.Numerics;

namespace LinqSTG.Kinematics
{
    /// <summary>
    /// Length-based helpers for <see cref="Vector2{T}"/> over floating-point scalars.
    /// Kept out of the struct because square root is not defined for all scalars
    /// (e.g. integers), and the operator interfaces alone suffice for the parametrics.
    /// </summary>
    public static class Vector2Extension
    {
        /// <summary>Euclidean length of the vector.</summary>
        public static T Length<T>(this Vector2<T> value)
            where T : struct, IFloatingPointIeee754<T>
            => T.Sqrt(value.LengthSquared());

        /// <summary>
        /// The vector scaled to unit length. A zero vector yields NaN components.
        /// </summary>
        public static Vector2<T> Normalize<T>(this Vector2<T> value)
            where T : struct, IFloatingPointIeee754<T>
            => value / T.Sqrt(value.LengthSquared());

        /// <summary>Euclidean distance between two points.</summary>
        public static T Distance<T>(this Vector2<T> value, Vector2<T> other)
            where T : struct, IFloatingPointIeee754<T>
            => (value - other).Length();
    }
}
