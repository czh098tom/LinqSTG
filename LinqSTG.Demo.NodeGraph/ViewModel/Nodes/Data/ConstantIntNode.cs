using DynamicData;
using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
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
    public class ConstantIntNode : LinqSTGNodeViewModel
    {
        public IntegerValueEditorViewModel ValueEditor { get; } = new IntegerValueEditorViewModel();
        public LinqSTGNodeOutputViewModel<Contextual<int>> OutputValue { get; }

        public ConstantIntNode()
        {
            OutputValue = LinqSTGNodeOutputViewModel.Int(editor: ValueEditor);

            AddOutput("value", OutputValue);
            AddEditor("value", ValueEditor);

            Name = "Int";

            TitleColor = NodeColors.Data;
        }
    }
}
