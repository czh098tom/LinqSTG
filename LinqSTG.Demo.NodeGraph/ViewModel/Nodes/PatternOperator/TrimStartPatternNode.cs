using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.PatternOperator
{
    public class TrimStartPatternNode : LinqSTGNodeViewModel
    {
        public LinqSTGNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> InputPattern { get; }
        public LinqSTGNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public TrimStartPatternNode()
        {
            InputPattern = LinqSTGNodeInputViewModel.Pattern("Pattern");
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddInput("pattern", InputPattern);
            AddOutput("pattern", OutputPattern);

            Name = "Trim Start";

            TitleColor = NodeColors.PatternOperator;

            OutputPattern.Value = InputPattern.ValueChanged
                .Select(pattern => Contextual.Create(dict =>
                    pattern?.Invoke(dict)?.TrimStart() ?? LinqSTG.Pattern.Empty<Parameter, int>()));
        }
    }
}
