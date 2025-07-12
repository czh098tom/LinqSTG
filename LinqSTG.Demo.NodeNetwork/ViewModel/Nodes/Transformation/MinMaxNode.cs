using DynamicData;
using LinqSTG.Demo.NodeNetwork.ViewModel.Editor;
using LinqSTG.Easings;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Transformation
{
    public class MinMaxNode : LinqSTGNodeViewModel
    {
        public FloatValueEditorViewModel InputLowerBoundEditor { get; } = new();
        public FloatValueEditorViewModel InpuUpperBoundEditor { get; } = new();

        public ValueNodeInputViewModel<Contextual<Parameter>?> InputTransformation { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyInput { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyTarget { get; }
        public ValueNodeInputViewModel<Contextual<float>?> InputLowerBound { get; }
        public ValueNodeInputViewModel<Contextual<float>?> InpuUpperBound { get; }
        public ValueNodeOutputViewModel<Contextual<Parameter>> OutputTransformation { get; }

        public MinMaxNode()
        {
            InputTransformation = new ValueNodeInputViewModel<Contextual<Parameter>?>()
            {
                Name = "Transformation",
            };

            InputKeyInput = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "Input Key",
            };

            InputKeyTarget = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "Target Key",
            };

            InputLowerBound = new ValueNodeInputViewModel<Contextual<float>?>()
            {
                Name = "Lower Bound",
                Editor = InputLowerBoundEditor,
            };

            InpuUpperBound = new ValueNodeInputViewModel<Contextual<float>?>()
            {
                Name = "Upper Bound",
                Editor = InpuUpperBoundEditor,
            };

            OutputTransformation = new ValueNodeOutputViewModel<Contextual<Parameter>>()
            {
                Name = "Transformation",
            };

            Inputs.Add(InputTransformation);
            Inputs.Add(InputKeyInput);
            Inputs.Add(InputKeyTarget);
            Inputs.Add(InputLowerBound);
            Inputs.Add(InpuUpperBound);
            Outputs.Add(OutputTransformation);

            Name = "MinMax";

            OutputTransformation.Value = InputTransformation.ValueChanged
                .CombineLatest(InputKeyInput.ValueChanged, InputKeyTarget.ValueChanged, InputLowerBound.ValueChanged, InpuUpperBound.ValueChanged,
                    (trans, input, target, lower, upper) => Contextual.Create(dict =>
                    {
                        dict = trans?.Invoke(dict) ?? dict;

                        var parameter = new Parameter(dict);
                        var inputKey = input?.Invoke(dict) ?? "Value";
                        var targetKey = target?.Invoke(dict) ?? "Value";
                        var lowerValue = lower?.Invoke(dict) ?? 0f;
                        var upperValue = upper?.Invoke(dict) ?? 0f;
                        var value = dict.GetValueOrDefault(inputKey, 0f).MinMax(lowerValue, upperValue);
                        parameter[targetKey] = value;
                        return parameter;
                    }));
        }
    }
}
