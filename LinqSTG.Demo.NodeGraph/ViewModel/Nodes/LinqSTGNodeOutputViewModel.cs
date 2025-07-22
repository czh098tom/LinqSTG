using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes
{
    public static class LinqSTGNodeOutputViewModel
    {
        public static ValueNodeOutputViewModel<Contextual<int>> Int(string? name = null, ValueEditorViewModel<Contextual<int>>? editor = null)
        {
            return new ValueNodeOutputViewModel<Contextual<int>>()
            {
                Name = name,
                Editor = editor,
                Value = editor?.ValueChanged,
                Port = new LinqSTGPortViewModel(PortColor.Int),
            };
        }

        public static ValueNodeOutputViewModel<Contextual<float>> Float(string? name = null, ValueEditorViewModel<Contextual<float>>? editor = null)
        {
            return new ValueNodeOutputViewModel<Contextual<float>>()
            {
                Name = name,
                Editor = editor,
                Value = editor?.ValueChanged,
                Port = new LinqSTGPortViewModel(PortColor.Float),
            };
        }

        public static ValueNodeOutputViewModel<Contextual<string>> String(string? name = null, ValueEditorViewModel<Contextual<string>>? editor = null)
        {
            return new ValueNodeOutputViewModel<Contextual<string>>()
            {
                Name = name,
                Editor = editor,
                Value = editor?.ValueChanged,
                Port = new LinqSTGPortViewModel(PortColor.String),
            };
        }

        public static ValueNodeOutputViewModel<Contextual<Parameter>> Transformation(string name)
        {
            return new ValueNodeOutputViewModel<Contextual<Parameter>>()
            {
                Name = name,
                Port = new LinqSTGPortViewModel(PortColor.Transformation),
            };
        }

        public static ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> Pattern(string name)
        {
            return new ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = name,
                Port = new LinqSTGPortViewModel(PortColor.Pattern),
            };
        }

        public static ValueNodeOutputViewModel<Contextual<Predictor<int, PointF>>> Movement(string name)
        {
            return new ValueNodeOutputViewModel<Contextual<Predictor<int, PointF>>>()
            {
                Name = name,
                Port = new LinqSTGPortViewModel(PortColor.Movement),
            };
        }
    }
}
