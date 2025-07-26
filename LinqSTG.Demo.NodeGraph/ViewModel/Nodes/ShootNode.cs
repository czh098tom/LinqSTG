using DynamicData;
using LinqSTG.Demo.NodeGraph;
using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes
{
    using Pattern = Pattern;

    public class ShootNode : LinqSTGNodeViewModel
    {
        public LinqSTGNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> InputPattern { get; }
        public LinqSTGNodeInputViewModel<Contextual<Predictor<int, Vector2>>?> InputMovement { get; }

        public IObservable<Contextual<IEnumerable<PointPrediction>>> Result { get; }

        public ShootNode()
        {
            InputPattern = LinqSTGNodeInputViewModel.Pattern("Pattern");
            InputMovement = LinqSTGNodeInputViewModel.Movement("Movement");

            AddInput("pattern", InputPattern);
            AddInput("movement", InputMovement);

            Name = "Shoot";

            TitleColor = NodeColors.Shoot;

            Result = InputPattern.ValueChanged
                .CombineLatest(InputMovement.ValueChanged, (pattern, movement)
                    => Contextual.Create(dict => new PointShooter<Parameter>(dict => movement?.Invoke(dict ?? Parameter.Empty))
                        .Shoot(pattern?.Invoke(dict) ?? LinqSTG.Pattern.Empty<Parameter, int>())));
        }
    }
}
