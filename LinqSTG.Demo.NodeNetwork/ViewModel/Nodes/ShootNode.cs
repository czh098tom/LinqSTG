using DynamicData;
using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes
{
    using Pattern = LinqSTG.Pattern;

    public class ShootNode : LinqSTGNodeViewModel
    {
        public ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> InputPattern { get; }
        public ValueNodeInputViewModel<Contextual<Predictor<int, PointF>>?> InputMovement { get; }

        public IObservable<Contextual<IEnumerable<PointPrediction>>> Result { get; }

        public ShootNode()
        {
            InputPattern = new ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>?>()
            {
                Name = "Pattern",
            };

            InputMovement = new ValueNodeInputViewModel<Contextual<Predictor<int, PointF>>?>()
            {
                Name = "Movement",
            };

            AddInput("pattern", InputPattern);
            AddInput("movement", InputMovement);

            Name = "Shoot";

            CanBeRemovedByUser = false;

            Result = InputPattern.ValueChanged
                .CombineLatest(InputMovement.ValueChanged, (pattern, movement)
                    => Contextual.Create(dict => new PointShooter<Parameter>(dict => movement?.Invoke(dict ?? Parameter.Empty))
                        .Shoot(pattern?.Invoke(dict) ?? LinqSTG.Pattern.Empty<Parameter, int>())));
        }
    }
}
