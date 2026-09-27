namespace LinqSTG.Kinematics
{
    /// <summary>
    /// A parametric motion that is derivable.
    /// Besides predicting the value at any time, it also gives the first-order derivative at that time.
    /// For example, a displacement motion can yield the instantaneous velocity directly
    /// instead of numerically differencing the prediction.
    /// </summary>
    /// <typeparam name="TTime">Time type.</typeparam>
    /// <typeparam name="TData">Predicted value type.</typeparam>
    /// <typeparam name="TDerivative">First-order derivative type.</typeparam>
    public interface IDerivableParametric<in TTime, out TData, out TDerivative> : IParametric<TTime, TData>
    {
        /// <summary>
        /// Get the first-order derivative of the motion at the given time.
        /// </summary>
        /// <param name="time">The time to derive at.</param>
        /// <returns>The derivative at that time, for the same time as Predict.</returns>
        TDerivative Derive(TTime time);
    }
}
