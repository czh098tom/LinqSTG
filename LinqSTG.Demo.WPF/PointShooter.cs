using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.WPF
{
    public class PointShooter<TData>(Func<TData?, Func<int, PointF>> createPrediction) 
        : IShooter<TData, int, IEnumerable<PointPrediction>>
    {
        public IEnumerable<PointPrediction> Shoot(IPattern<TData, int> pattern)
        {
            var startTime = 0;
            foreach (var data in pattern)
            {
                if (data.IsData)
                {
                    yield return new PointPrediction(createPrediction(data.Data), startTime);
                }
                else
                {
                    startTime += data.Interval;
                }
            }
        }
    }
}
