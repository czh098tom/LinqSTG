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

    /// <summary>
    /// Derivable variant of AfterTimeParametric.
    /// The derivative comes from the source motion before the switch time and from the after motion from it on,
    /// matching how Predict splits at the switch time (the after motion's derivative is used at the switch time itself).
    /// </summary>
    public sealed class AfterTimeParametric<TTime, TData, TDerivative>(
        IDerivableParametric<TTime, TData, TDerivative> source,
        TTime switchTime,
        IDerivableParametric<TTime, TData, TDerivative> after) : IDerivableParametric<TTime, TData, TDerivative>
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TTime>
        where TData : IAdditionOperators<TData, TData, TData>
    {
        public IDerivableParametric<TTime, TData, TDerivative> Source => source;
        public TTime SwitchTime => switchTime;
        public IDerivableParametric<TTime, TData, TDerivative> After => after;

        public TData Predict(TTime time)
            => time < switchTime ? source.Predict(time) : after.Predict(time - switchTime) + source.Predict(switchTime);

        public TDerivative Derive(TTime time)
            => time < switchTime ? source.Derive(time) : after.Derive(time - switchTime);
    }
}
