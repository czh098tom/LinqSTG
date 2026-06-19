namespace LinqSTG.Kinematics
{
    public interface IParametric<in TTime, out TData>
    {
        TData Predict(TTime time);
    }
}
