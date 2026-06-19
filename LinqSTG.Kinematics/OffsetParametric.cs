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
}
