using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class OffsetParametric<TTime, TData>(
        IParametric<TTime, TData> source, TData offset) : IParametric<TTime, TData>
        where TData : IAdditionOperators<TData, TData, TData>
    {
        public IParametric<TTime, TData> Source => source;
        public TData Offset => offset;

        public TData Predict(TTime time) => source.Predict(time) + offset;
    }

    /// <summary>
    /// Derivable variant of OffsetParametric.
    /// Translating a motion by a constant offset does not change its derivative,
    /// so the derivative is that of the source motion.
    /// </summary>
    public sealed class OffsetParametric<TTime, TData, TDerivative>(
        IDerivableParametric<TTime, TData, TDerivative> source, TData offset)
        : IDerivableParametric<TTime, TData, TDerivative>
        where TData : IAdditionOperators<TData, TData, TData>
    {
        public IDerivableParametric<TTime, TData, TDerivative> Source => source;
        public TData Offset => offset;

        public TData Predict(TTime time) => source.Predict(time) + offset;

        public TDerivative Derive(TTime time) => source.Derive(time);
    }
}
