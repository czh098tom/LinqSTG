using System;

namespace LinqSTG.Kinematics
{
    public sealed class SelectParametric<TTime, TData, UData>(
        IParametric<TTime, TData> source, Func<TData, UData> selector) : IParametric<TTime, UData>
    {
        public IParametric<TTime, TData> Source => source;
        public Func<TData, UData> Selector => selector;

        public UData Predict(TTime time) => selector(source.Predict(time));
    }
}
