using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.PatternOperator
{
    public class SkipPatternNode : LinqSTGNodeViewModel
    {
        public IntegerValueEditorViewModel InputCountEditor { get; } = new();
        public LinqSTGNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> InputPattern { get; }
        public LinqSTGNodeInputViewModel<Contextual<int>?> InputCount { get; }
        public LinqSTGNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public SkipPatternNode()
        {
            InputPattern = LinqSTGNodeInputViewModel.Pattern("Pattern");
            InputCount = LinqSTGNodeInputViewModel.Int("Count", InputCountEditor);
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddInput("pattern", InputPattern);
            AddInput("count", InputCount);
            AddOutput("pattern", OutputPattern);
            AddEditor("count", InputCountEditor);

            Name = "Skip Pattern";

            TitleColor = NodeColors.PatternOperator;

            OutputPattern.Value = InputPattern.ValueChanged
                .CombineLatest(InputCount.ValueChanged,
                    (pattern, count) => Contextual.Create(dict =>
                        pattern?.Invoke(dict)?.Skip(count?.Invoke(dict) ?? 0)
                            ?? LinqSTG.Pattern.Empty<Parameter, int>()));
        }
    }
}
