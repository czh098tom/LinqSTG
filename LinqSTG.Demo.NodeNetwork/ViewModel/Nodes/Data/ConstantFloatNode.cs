using DynamicData;
using LinqSTG.Demo.NodeNetwork.ViewModel.Editor;
using LinqSTG.Kinematics;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Data
{
    public class ConstantFloatNode : LinqSTGNodeViewModel
    {
        public FloatValueEditorViewModel ValueEditor { get; } = new FloatValueEditorViewModel();
        public ValueNodeOutputViewModel<Contextual<float>> OutputValue { get; }

        public ConstantFloatNode()
        {
            OutputValue = new ValueNodeOutputViewModel<Contextual<float>>()
            {
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged,
            };

            AddOutput("value", OutputValue);
            AddEditor("value", ValueEditor);

            Name = "Float";
        }
    }
}
