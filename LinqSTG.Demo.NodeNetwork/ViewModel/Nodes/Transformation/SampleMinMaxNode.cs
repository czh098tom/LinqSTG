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
    public class SampleMinMaxNode : LinqSTGNodeViewModel
    {
        public FloatValueEditorViewModel InputLowerBoundEditor { get; } = new();
        public FloatValueEditorViewModel InpuUpperBoundEditor { get; } = new();

        public ValueNodeInputViewModel<Contextual<Parameter>> InputTransformation { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyID { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyTotal { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyTarget { get; }
        public ValueNodeInputViewModel<Contextual<float>?> InputLowerBound { get; }
        public ValueNodeInputViewModel<Contextual<float>?> InpuUpperBound { get; }
        public ValueNodeOutputViewModel<Contextual<Parameter>> OutputTransformation { get; }

        public SampleMinMaxNode()
        {
            InputTransformation = new ValueNodeInputViewModel<Contextual<Parameter>>()
            {
                Name = "Transformation",
            };

            InputKeyID = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "ID Key",
            };

            InputKeyTotal = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "Total Key",
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

            AddInput("transformation", InputTransformation);
            AddInput("id_key", InputKeyID);
            AddInput("total_key", InputKeyTotal);
            AddInput("target", InputKeyTarget);
            AddInput("lower_bound", InputLowerBound);
            AddInput("upper_bound", InpuUpperBound);
            AddOutput("transformation", OutputTransformation);
            AddEditor("lower_bound", InputLowerBoundEditor);
            AddEditor("upper_bound", InpuUpperBoundEditor);

            Name = "Sample MinMax";

            OutputTransformation.Value = InputTransformation.ValueChanged
                .CombineLatest(InputKeyID.ValueChanged, InputKeyTotal.ValueChanged, InputKeyTarget.ValueChanged,
                    InputLowerBound.ValueChanged, InpuUpperBound.ValueChanged,
                    (trans, id, total, target, lower, upper) => Contextual.Create(dict =>
                    {
                        dict = trans?.Invoke(dict) ?? dict;

                        var parameter = new Parameter(dict);
                        var idKey = id?.Invoke(dict) ?? "ID";
                        var totalKey = total?.Invoke(dict) ?? "Total";
                        var repeater = new Repeater(Convert.ToInt32(dict.GetValueOrDefault(idKey, 0)),
                            Convert.ToInt32(dict.GetValueOrDefault(totalKey, 0)));

                        var lowerValue = lower?.Invoke(dict) ?? 0f;
                        var upperValue = upper?.Invoke(dict) ?? 0f;
                        var value = repeater.Sample01(IntervalType.HeadClosed).MinMax(lowerValue, upperValue);

                        parameter[target?.Invoke(dict) ?? "Value"] = value;
                        return parameter;
                    }));
        }
    }
}
