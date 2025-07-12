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
    public class MapTransformationNode : LinqSTGNodeViewModel
    {
        public ValueNodeInputViewModel<Contextual<Parameter>> InputTransformation { get; }
        public ValueNodeInputViewModel<Contextual<Parameter>> InputMapper { get; }
        public ValueNodeOutputViewModel<Contextual<Parameter>> OutputTransformation { get; }

        public MapTransformationNode()
        {
            InputTransformation = new ValueNodeInputViewModel<Contextual<Parameter>>()
            {
                Name = "Input Transformation",
            };
            InputMapper = new ValueNodeInputViewModel<Contextual<Parameter>>()
            {
                Name = "Input Mapper",
            };
            OutputTransformation = new ValueNodeOutputViewModel<Contextual<Parameter>>()
            {
                Name = "Output Transformation",
            };
            Inputs.Add(InputTransformation);
            Inputs.Add(InputMapper);
            Outputs.Add(OutputTransformation);

            Name = "Map Transformation";

            OutputTransformation.Value = InputTransformation.ValueChanged
                .CombineLatest(InputMapper.ValueChanged, 
                    (transformation, mapper) => Contextual.Create(dict =>
                        mapper?.Invoke(transformation?.Invoke(dict) ?? Parameter.Empty)
                            ?? Parameter.Empty));
        }
    }
}
