using LinqSTG.Kinematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork
{
    public readonly record struct PointPrediction(Predictor<int, PointF> PointFunc, int StartTime)
    {
    }
}
