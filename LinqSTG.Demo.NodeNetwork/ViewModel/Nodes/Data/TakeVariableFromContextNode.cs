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

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Data
{
    public class TakeVariableFromContextNode : LinqSTGNodeViewModel
    {
        public ValueNodeInputViewModel<Contextual<string>> InputValue { get; }
        public ValueNodeOutputViewModel<Contextual<float>> OutputValue { get; }

        public TakeVariableFromContextNode()
        {
            InputValue = new ValueNodeInputViewModel<Contextual<string>>()
            {
                Name = "Key",
            };
            OutputValue = new ValueNodeOutputViewModel<Contextual<float>>()
            {
                Name = "Value",
            };

            AddInput("key", InputValue);
            AddOutput("value", OutputValue);

            Name = "Take Variable";

            OutputValue.Value = InputValue.ValueChanged
                .Select(s => Contextual.Create(dict => dict.GetValueOrDefault(s?.Invoke(dict) ?? string.Empty, 0f)));
        }
    }
}
