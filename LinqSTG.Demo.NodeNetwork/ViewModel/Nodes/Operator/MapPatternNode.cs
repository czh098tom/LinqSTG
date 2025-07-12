using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Operator
{
    public class MapPatternNode : LinqSTGNodeViewModel
    {
        private static readonly Contextual<Parameter> DefaultMapper = dict => new(dict);

        public ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>> InputPattern { get; }
        public ValueNodeInputViewModel<Contextual<Parameter>> InputMapper { get; }
        public ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>> OutputPattern { get; }

        public MapPatternNode()
        {
            InputPattern = new ValueNodeInputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = "Pattern",
            };
            InputMapper = new ValueNodeInputViewModel<Contextual<Parameter>>()
            {
                Name = "Transformation",
            };
            OutputPattern = new ValueNodeOutputViewModel<Contextual<IPattern<Parameter, int>>>()
            {
                Name = "Pattern",
            };

            AddInput("pattern", InputPattern);
            AddInput("mapper", InputMapper);
            AddOutput("pattern", OutputPattern);

            Name = "Map Pattern";

            OutputPattern.Value = InputPattern.ValueChanged
                .CombineLatest(InputMapper.ValueChanged, 
                    (pattern, mapper) => Contextual.Create(dict =>
                        pattern?.Invoke(dict)?.Select(d => (mapper ?? DefaultMapper).Invoke(d ?? Parameter.Empty)) 
                            ?? LinqSTG.Pattern.Empty<Parameter, int>()));
        }
    }
}
