using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Pattern
{
    using Pattern = LinqSTG.Pattern;

    public class SingleDataPatternNode : LinqSTGNodeViewModel
    {
        public LinqSTGNodeInputViewModel<Contextual<Parameter>?> InputTransformation { get; }
        public LinqSTGNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public SingleDataPatternNode()
        {
            InputTransformation = LinqSTGNodeInputViewModel.Transformation("Transformation");
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddInput("transformation", InputTransformation);
            AddOutput("pattern", OutputPattern);

            Name = "Single Pattern";

            TitleColor = NodeColors.Pattern;

            OutputPattern.Value = InputTransformation.ValueChanged
                .Select(trans => Contextual.Create(dict =>
                    Pattern.Single<Parameter, int>(trans?.Invoke(dict) ?? new Parameter(dict))));
        }
    }
}
