using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Pattern
{
    using Pattern = LinqSTG.Pattern;

    public class SingleIntervalPatternNode : LinqSTGNodeViewModel
    {
        public IntegerValueEditorViewModel InputIntervalEditor { get; } = new();
        public LinqSTGNodeInputViewModel<Contextual<int>?> InputInterval { get; }
        public LinqSTGNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public SingleIntervalPatternNode()
        {
            InputInterval = LinqSTGNodeInputViewModel.Int("Interval", InputIntervalEditor);
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddInput("interval", InputInterval);
            AddOutput("pattern", OutputPattern);
            AddEditor("interval", InputIntervalEditor);

            Name = "Single Interval Pattern";

            TitleColor = NodeColors.Pattern;

            OutputPattern.Value = InputInterval.ValueChanged
                .Select(interval => Contextual.Create(dict =>
                    Pattern.SingleInterval<Parameter, int>(interval?.Invoke(dict) ?? 0)));
        }
    }
}
