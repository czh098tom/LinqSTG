using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.WPF
{
    public readonly record struct PointPrediction(Func<int, PointF> PointFunc, int StartTime)
    {
    }
}
