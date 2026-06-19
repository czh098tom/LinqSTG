using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Reactive.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Pattern
{
    using Pattern = LinqSTG.Pattern;

    public class EmptyPatternNode : LinqSTGNodeViewModel
    {
        public LinqSTGNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public EmptyPatternNode()
        {
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddOutput("pattern", OutputPattern);

            Name = "Empty Pattern";

            TitleColor = NodeColors.Pattern;

            OutputPattern.Value = Observable.Return(
                Contextual.Create(_ => Pattern.Empty<Parameter, int>()));
        }
    }
}
