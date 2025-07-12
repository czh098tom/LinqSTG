using DynamicData;
using LinqSTG.Demo.NodeNetwork.ViewModel.Editor;
using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Movement
{
    public class UniformVelocityMovementNode : LinqSTGNodeViewModel
    {
        public FloatValueEditorViewModel InputSpeedDirectionEditor { get; } = new FloatValueEditorViewModel();
        public FloatValueEditorViewModel InputSpeedEditor { get; } = new FloatValueEditorViewModel() { RawValue = 3 };
        public ValueNodeInputViewModel<Contextual<float>?> InputSpeedDirection { get; }
        public ValueNodeInputViewModel<Contextual<float>?> InputSpeed { get; }
        public ValueNodeOutputViewModel<Contextual<Predictor<int, PointF>>> OutputMovement { get; }

        public UniformVelocityMovementNode()
        {
            InputSpeedDirection = new ValueNodeInputViewModel<Contextual<float>?>()
            {
                Name = "Direction",
                Editor = InputSpeedDirectionEditor,
            };
            InputSpeed = new ValueNodeInputViewModel<Contextual<float>?>()
            {
                Name = "Speed",
                Editor = InputSpeedEditor,
            };
            OutputMovement = new ValueNodeOutputViewModel<Contextual<Predictor<int, PointF>>>()
            {
                Name = "Movement",
            };

            AddInput("speed_dir", InputSpeedDirection);
            AddInput("speed", InputSpeed);
            AddOutput("movement", OutputMovement);
            AddEditor("speed_dir", InputSpeedDirectionEditor);
            AddEditor("speed", InputSpeedEditor);

            Name = "Uniform Velocity Movement";

            OutputMovement.Value = InputSpeed.ValueChanged
                .CombineLatest(InputSpeedDirection.ValueChanged, 
                    (speed, direction) => Contextual.Create(dict => new Predictor<int, PointF>(t => new PointF(
                        t * (speed?.Invoke(dict) ?? 0) * (float)DegreeMaths.Cos((direction?.Invoke(dict) ?? 0)),
                        t * (speed?.Invoke(dict) ?? 0) * (float)DegreeMaths.Sin((direction?.Invoke(dict) ?? 0))))));
        }
    }
}
