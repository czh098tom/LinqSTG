using System.Numerics;
using System.Runtime.CompilerServices;

namespace LinqSTG.Kinematics
{
    /// <summary>
    /// A two-component vector over a generic scalar type <typeparamref name="T"/>,
    /// implementing the generic math operator interfaces so that it can be used as
    /// TData, TVelocity or TAccel of the parametric motions in this library,
    /// e.g. <c>Parametric.UniformVelocity&lt;float, Vector2&lt;float&gt;, Vector2&lt;float&gt;&gt;(velocity)</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The implemented scalar multiplication is the one by <typeparamref name="T"/>, so the
    /// parametric factories run with a <typeparamref name="T"/> time axis (e.g. <c>float</c>
    /// seconds). Multiplication by <c>int</c> cannot also be an interface member: C# forbids
    /// implementing both constructions, since they would unify for <c>T = int</c> (CS0695),
    /// and the check ignores constraints. At concrete call sites an <c>int</c> operand still
    /// works through its implicit conversion to the scalar; the frame-time factories
    /// (<c>TTime = int</c>, see <see cref="Vector2Parametric"/>) convert explicitly via
    /// <c>INumberBase.CreateChecked</c>, since generic code has no implicit
    /// <c>int</c>-to-scalar conversion.
    /// </para>
    /// <para>
    /// Vector-by-vector operators are component-wise (Hadamard product), which is the algebra
    /// the generic constraints expect: dividing by <see cref="MultiplicativeIdentity"/> +
    /// <see cref="MultiplicativeIdentity"/> halves both axes, and component-wise lerp
    /// (<c>MinMax</c> in LinqSTG.Easings) works per axis.
    /// </para>
    /// <para>
    /// This is a readonly struct of two scalars (8 bytes for <c>float</c>): it never allocates,
    /// and all operators are aggressively inlined, so instantiations over primitive scalars
    /// compile to the same machine code as hand-written scalar math.
    /// </para>
    /// </remarks>
    public readonly struct Vector2<T> :
        IAdditionOperators<Vector2<T>, Vector2<T>, Vector2<T>>,
        ISubtractionOperators<Vector2<T>, Vector2<T>, Vector2<T>>,
        IMultiplyOperators<Vector2<T>, Vector2<T>, Vector2<T>>,
        IMultiplyOperators<Vector2<T>, T, Vector2<T>>,
        IDivisionOperators<Vector2<T>, Vector2<T>, Vector2<T>>,
        IUnaryNegationOperators<Vector2<T>, Vector2<T>>,
        IAdditiveIdentity<Vector2<T>, Vector2<T>>,
        IMultiplicativeIdentity<Vector2<T>, Vector2<T>>,
        IEqualityOperators<Vector2<T>, Vector2<T>, bool>,
        IEquatable<Vector2<T>>
        where T : struct, INumber<T>
    {
        /// <summary>The x component.</summary>
        public T X { get; }

        /// <summary>The y component.</summary>
        public T Y { get; }

        /// <summary>Creates a vector from two components.</summary>
        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }

        /// <summary>The zero vector (0, 0).</summary>
        public static Vector2<T> Zero => default;

        /// <summary>The all-ones vector (1, 1).</summary>
        public static Vector2<T> One => new(T.MultiplicativeIdentity, T.MultiplicativeIdentity);

        /// <summary>The additive identity, the zero vector.</summary>
        public static Vector2<T> AdditiveIdentity => default;

        /// <summary>The multiplicative identity, the all-ones vector.</summary>
        public static Vector2<T> MultiplicativeIdentity => One;

        /// <summary>Adds two vectors component-wise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator +(Vector2<T> left, Vector2<T> right)
            => new(left.X + right.X, left.Y + right.Y);

        /// <summary>Subtracts two vectors component-wise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right)
            => new(left.X - right.X, left.Y - right.Y);

        /// <summary>Negates both components.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator -(Vector2<T> value)
            => new(-value.X, -value.Y);

        /// <summary>Multiplies two vectors component-wise (Hadamard product).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator *(Vector2<T> left, Vector2<T> right)
            => new(left.X * right.X, left.Y * right.Y);

        /// <summary>
        /// Scales both components by a scalar. This is the interface multiply, so the
        /// parametric factories work with a <typeparamref name="T"/> time axis (e.g. float).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator *(Vector2<T> value, T scalar)
            => new(value.X * scalar, value.Y * scalar);

        /// <summary>Scales both components by a scalar.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator *(T scalar, Vector2<T> value)
            => new(scalar * value.X, scalar * value.Y);

        /// <summary>Divides two vectors component-wise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator /(Vector2<T> left, Vector2<T> right)
            => new(left.X / right.X, left.Y / right.Y);

        /// <summary>Divides both components by a scalar.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2<T> operator /(Vector2<T> value, T scalar)
            => new(value.X / scalar, value.Y / scalar);

        /// <summary>Compares both components for equality.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2<T> left, Vector2<T> right)
            => left.X == right.X && left.Y == right.Y;

        /// <summary>Compares whether any component differs.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2<T> left, Vector2<T> right)
            => left.X != right.X || left.Y != right.Y;

        /// <summary>Dot product of two vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Dot(Vector2<T> other)
            => X * other.X + Y * other.Y;

        /// <summary>Squared euclidean length of the vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LengthSquared()
            => Dot(this);

        /// <inheritdoc/>
        public bool Equals(Vector2<T> other)
            => X == other.X && Y == other.Y;

        /// <inheritdoc/>
        public override bool Equals(object? obj)
            => obj is Vector2<T> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
            => HashCode.Combine(X, Y);

        /// <inheritdoc/>
        public override string ToString()
            => $"({X}, {Y})";
    }
}
