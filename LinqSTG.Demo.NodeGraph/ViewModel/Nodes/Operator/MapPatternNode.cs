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
    public class MapPatternNode : LinqSTGNodeViewModel
    {
        private static readonly Contextual<Parameter> DefaultMapper = dict => new(dict);

        public ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>?> InputPattern { get; }
        public ValueNodeInputViewModel<Contextual<Parameter>?> InputMapper { get; }
        public ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public MapPatternNode()
        {
            InputPattern = LinqSTGNodeInputViewModel.Pattern("Pattern");
            InputMapper = LinqSTGNodeInputViewModel.Transformation("Transformation");
            OutputPattern = LinqSTGNodeOutputViewModel.Pattern("Pattern");

            AddInput("pattern", InputPattern);
            AddInput("mapper", InputMapper);
            AddOutput("pattern", OutputPattern);

            Name = "Map Pattern";

            TitleColor = NodeColors.PatternOperator;

            OutputPattern.Value = InputPattern.ValueChanged
                .CombineLatest(InputMapper.ValueChanged, 
                    (pattern, mapper) => Contextual.Create(dict =>
                        pattern?.Invoke(dict)?.Select(d => (mapper ?? DefaultMapper).Invoke(d ?? Parameter.Empty)) 
                            ?? LinqSTG.Pattern.Empty<Parameter, int>()));
        }
    }
}
