using DynamicData;
using LinqSTG.Demo.NodeGraph.ViewModel;
using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Pattern
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

            AddInput("times", InputTimes);
            AddInput("interval", InputInterval);
            AddInput("id_key", InputIDFieldName);
            AddInput("total_key", InputTotalFieldName);
            AddOutput("pattern", OutputPattern);
            AddEditor("times", InputTimesEditor);
            AddEditor("interval", InputIntervalEditor);

            Name = "Repeat with Interval Pattern";

            TitleColor = NodeColors.Pattern;

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
