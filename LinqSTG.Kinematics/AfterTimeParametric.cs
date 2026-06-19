using System.Numerics;

namespace LinqSTG.Kinematics
{
    public sealed class AfterTimeParametric<TTime, TData>(
        IParametric<TTime, TData> source, TTime switchTime, IParametric<TTime, TData> after) : IParametric<TTime, TData>
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TTime>
        where TData : IAdditionOperators<TData, TData, TData>
    {
        public IParametric<TTime, TData> Source => source;
        public TTime SwitchTime => switchTime;
        public IParametric<TTime, TData> After => after;

        public TData Predict(TTime time)
            => time < switchTime ? source.Predict(time) : after.Predict(time - switchTime) + source.Predict(switchTime);
    }
}
