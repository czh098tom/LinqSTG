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

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Pattern
{
    using Pattern = LinqSTG.Pattern;

    public class RepeatWithIntervalPatternNode : LinqSTGNodeViewModel
    {
        public IntegerValueEditorViewModel InputTimesEditor { get; } = new() { RawValue = 1 };
        public IntegerValueEditorViewModel InputIntervalEditor { get; } = new();
        public ValueNodeInputViewModel<Contextual<string>?> InputIDFieldName { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputTotalFieldName { get; }
        public ValueNodeInputViewModel<Contextual<int>?> InputTimes { get; }
        public ValueNodeInputViewModel<Contextual<int>?> InputInterval { get; }
        public ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public RepeatWithIntervalPatternNode()
        {
            InputTimes = new ValueNodeInputViewModel<Contextual<int>?>()
            {
                Name = "Times",
                Editor = InputTimesEditor,
            };
            InputInterval = new ValueNodeInputViewModel<Contextual<int>?>()
            {
                Name = "Interval",
                Editor = InputIntervalEditor,
            };
            InputIDFieldName = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "ID Key",
            };
            InputTotalFieldName = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "Total Key",
            };
            OutputPattern = new ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = "Pattern"
            };
            Inputs.Add(InputTimes);
            Inputs.Add(InputInterval);
            Inputs.Add(InputIDFieldName);
            Inputs.Add(InputTotalFieldName);
            Outputs.Add(OutputPattern);
            Name = "Repeat with Interval Pattern";

            OutputPattern.Value = InputTimes.ValueChanged
                .CombineLatest(InputInterval.ValueChanged, InputIDFieldName.ValueChanged, InputTotalFieldName.ValueChanged,
                    (times, interval, id, total) => Contextual.Create(dict => 
                        Pattern.RepeatWithInterval(times?.Invoke(dict) ?? 0, interval?.Invoke(dict) ?? 0)
                        .Select(r => new Parameter(dict)
                        {
                            [id?.Invoke(dict) ?? "ID"] = r.ID,
                            [total?.Invoke(dict) ?? "Total"] = r.Total,
                        })));
        }
    }
}
