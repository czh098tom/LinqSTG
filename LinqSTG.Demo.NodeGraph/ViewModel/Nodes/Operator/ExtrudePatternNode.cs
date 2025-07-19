using DynamicData;
using LinqSTG.Demo.NodeGraph.ViewModel;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Operator
{
    public class ExtrudePatternNode : LinqSTGNodeViewModel
    {
        public ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>> InputPattern { get; }
        public ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>> InputSubPattern { get; }
        public ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public ExtrudePatternNode()
        {
            InputPattern = new ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = "Pattern",
            };
            InputSubPattern = new ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = "Sub Pattern",
            };
            OutputPattern = new ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = "Pattern",
            };

            AddInput("pattern", InputPattern);
            AddInput("sub_pattern", InputSubPattern);
            AddOutput("pattern", OutputPattern);

            Name = "Extrude Pattern";

            TitleColor = NodeColors.PatternOperator;

            OutputPattern.Value = InputPattern.ValueChanged
                .CombineLatest(InputSubPattern.ValueChanged,
                    (pattern, subPattern) => Contextual.Create(dict =>
                        pattern?.Invoke(dict)?.SelectMany(d => (subPattern?.Invoke(d ?? Parameter.Empty) 
                            ?? LinqSTG.Pattern.Single<Parameter, int>(d ?? Parameter.Empty))!)
                            ?? LinqSTG.Pattern.Empty<Parameter, int>()));
        }
    }
}
