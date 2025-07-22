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
    public class LinqSTGNodeInputViewModel
    {
        public static ValueNodeInputViewModel<Contextual<int>?> Int(string name, ValueEditorViewModel<Contextual<int>>? editor = null)
        {
            return new ValueNodeInputViewModel<Contextual<int>?>
            {
                Name = name,
                Editor = editor,
                Port = new LinqSTGPortViewModel(PortColor.Int),
            };
        }

        public static ValueNodeInputViewModel<Contextual<float>?> Float(string name, ValueEditorViewModel<Contextual<float>>? editor = null)
        {
            return new ValueNodeInputViewModel<Contextual<float>?>
            {
                Name = name,
                Editor = editor,
                Port = new LinqSTGPortViewModel(PortColor.Float),
            };
        }

        public static ValueNodeInputViewModel<Contextual<string>?> String(string name, ValueEditorViewModel<Contextual<string>>? editor = null)
        {
            return new ValueNodeInputViewModel<Contextual<string>?>
            {
                Name = name,
                Editor = editor,
                Port = new LinqSTGPortViewModel(PortColor.String),
            };
        }

        public static ValueNodeInputViewModel<Contextual<Parameter>?> Transformation(string name)
        {
            return new ValueNodeInputViewModel<Contextual<Parameter>?>
            {
                Name = name,
                Port = new LinqSTGPortViewModel(PortColor.Transformation),
            };
        }

        public static ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> Pattern(string name)
        {
            return new ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>?>
            {
                Name = name,
                Port = new LinqSTGPortViewModel(PortColor.Pattern),
            };
        }

        public static ValueNodeInputViewModel<Contextual<Predictor<int, PointF>>?> Movement(string name)
        {
            return new ValueNodeInputViewModel<Contextual<Predictor<int, PointF>>?>
            {
                Name = name,
                Port = new LinqSTGPortViewModel(PortColor.Movement),
            };
        }
    }
}
