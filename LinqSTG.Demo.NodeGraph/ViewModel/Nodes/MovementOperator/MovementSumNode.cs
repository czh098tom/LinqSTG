using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.MovementOperator
{
    public class MovementSumNode : LinqSTGNodeViewModel
    {
        public LinqSTGNodeInputViewModel<Contextual<Predictor<int, Vector2>>?> InputMovement1 { get; }
        public LinqSTGNodeInputViewModel<Contextual<Predictor<int, Vector2>>?> InputMovement2 { get; }
        public LinqSTGNodeOutputViewModel<Contextual<Predictor<int, Vector2>>> OutputMovement { get; }

        public MovementSumNode()
        {
            InputMovement1 = LinqSTGNodeInputViewModel.Movement();
            InputMovement2 = LinqSTGNodeInputViewModel.Movement();
            OutputMovement = LinqSTGNodeOutputViewModel.Movement();

            AddInput("movement1", InputMovement1);
            AddInput("movement2", InputMovement2);
            AddOutput("movement", OutputMovement);

            Name = "Movement Sum";
            TitleColor = NodeColors.Movement;

            OutputMovement.Value = InputMovement1.ValueChanged
                .CombineLatest(InputMovement2.ValueChanged, (m1, m2)
                    => Contextual.Create(dict
                        => new Predictor<int, Vector2>(t 
                            => (m1?.Invoke(dict)?.Invoke(t) ?? Vector2.Zero) + (m2?.Invoke(dict)?.Invoke(t) ?? Vector2.Zero))));
        }
    }
}
