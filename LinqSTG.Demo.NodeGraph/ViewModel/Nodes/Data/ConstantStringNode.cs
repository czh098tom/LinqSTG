using DynamicData;
using LinqSTG.Demo.NodeGraph.ViewModel;
using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Data
{
    public class ConstantStringNode : LinqSTGNodeViewModel
    {
        public StringValueEditorViewModel ValueEditor { get; } = new StringValueEditorViewModel();
        public ValueNodeOutputViewModel<Contextual<string>> OutputValue { get; }

        public ConstantStringNode()
        {
            OutputValue = new ValueNodeOutputViewModel<Contextual<string>>()
            {
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged,
            };

            AddOutput("value", OutputValue);
            AddEditor("value", ValueEditor);

            Name = "String";

            TitleColor = NodeColors.Data;
        }
    }
}
