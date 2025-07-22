using DynamicData;
using LinqSTG.Demo.NodeGraph.ViewModel;
using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Data
{
    public class ConstantFloatNode : LinqSTGNodeViewModel
    {
        public FloatValueEditorViewModel ValueEditor { get; } = new FloatValueEditorViewModel();
        public ValueNodeOutputViewModel<Contextual<float>> OutputValue { get; }

        public ConstantFloatNode()
        {
            OutputValue = LinqSTGNodeOutputViewModel.Float(editor: ValueEditor);

            AddOutput("value", OutputValue);
            AddEditor("value", ValueEditor);

            Name = "Float";

            TitleColor = NodeColors.Data;
        }
    }
}
