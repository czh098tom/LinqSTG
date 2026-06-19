using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.PatternOperator
{
    public class SkipWhilePatternNode : LinqSTGNodeViewModel
    {
        public LinqSTGNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> InputPattern { get; }
        public LinqSTGNodeInputViewModel<Contextual<float>?> InputPredicate { get; }
        public LinqSTGNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public SkipWhilePatternNode()
        {
            InputPattern = LinqSTGNodeInputViewModel.Pattern("Pattern");
            InputPredicate = LinqSTGNodeInputViewModel.Float("Predicate");
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddInput("pattern", InputPattern);
            AddInput("predicate", InputPredicate);
            AddOutput("pattern", OutputPattern);

            Name = "Skip While Pattern";

            TitleColor = NodeColors.PatternOperator;

            OutputPattern.Value = InputPattern.ValueChanged
                .CombineLatest(InputPredicate.ValueChanged,
                    (pattern, predicate) => Contextual.Create(dict =>
                        pattern?.Invoke(dict)?.SkipWhile(d => (predicate?.Invoke(d ?? Parameter.Empty) ?? 0f) != 0f)
                            ?? LinqSTG.Pattern.Empty<Parameter, int>()));
        }
    }
}
