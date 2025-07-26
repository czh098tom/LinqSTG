using LinqSTG.Kinematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph
{
    public readonly record struct PointPrediction(Predictor<int, Vector2> PointFunc, int StartTime)
    {
    }
}
