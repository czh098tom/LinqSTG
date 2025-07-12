using DynamicData;
using LinqSTG.Demo.NodeNetwork.ViewModel.Editor;
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
    public class ConstantIntNode : LinqSTGNodeViewModel
    {
        public IntegerValueEditorViewModel ValueEditor { get; } = new IntegerValueEditorViewModel();
        public ValueNodeOutputViewModel<Contextual<int>> OutputValue { get; }

        public ConstantIntNode()
        {
            OutputValue = new ValueNodeOutputViewModel<Contextual<int>>()
            {
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged,
            };
            Outputs.Add(OutputValue);

            Name = "Int";
        }
    }
}
