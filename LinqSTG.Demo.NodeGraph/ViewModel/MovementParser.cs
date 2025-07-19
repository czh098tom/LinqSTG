using LinqSTG.Kinematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel
{
    public delegate Predictor<TTime, TData> MovementParser<TTime, TData>(Dictionary<string, float> param);
}
