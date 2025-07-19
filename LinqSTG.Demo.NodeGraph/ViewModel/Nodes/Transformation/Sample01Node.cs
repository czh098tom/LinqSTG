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

namespace LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Transformation
{
    public class Sample01Node : LinqSTGNodeViewModel
    {
        public ValueNodeInputViewModel<Contextual<Parameter>> InputTransformation { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyID { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyTotal { get; }
        public ValueNodeInputViewModel<Contextual<string>?> InputKeyTarget { get; }
        public ValueNodeOutputViewModel<Contextual<Parameter>> OutputTransformation { get; }

        public Sample01Node()
        {
            InputTransformation = new ValueNodeInputViewModel<Contextual<Parameter>>()
            {
                Name = "Transformation",
            };

            InputKeyID = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "ID Key",
            };

            InputKeyTotal = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "Total Key",
            };

            InputKeyTarget = new ValueNodeInputViewModel<Contextual<string>?>()
            {
                Name = "Target Key",
            };

            OutputTransformation = new ValueNodeOutputViewModel<Contextual<Parameter>>()
            {
                Name = "Transformation",
            };

            AddInput("transformation", InputTransformation);
            AddInput("id_key", InputKeyID);
            AddInput("total_key", InputKeyTotal);
            AddInput("target", InputKeyTarget);
            AddOutput("transformation", OutputTransformation);

            Name = "Sample01";

            TitleColor = NodeColors.Transformation;

            OutputTransformation.Value = InputTransformation.ValueChanged
                .CombineLatest(InputKeyID.ValueChanged, InputKeyTotal.ValueChanged, InputKeyTarget.ValueChanged,
                    (trans, id, total, target) => Contextual.Create(dict =>
                    {
                        dict = trans?.Invoke(dict) ?? dict;

                        var parameter = new Parameter(dict);
                        var idKey = id?.Invoke(dict) ?? "ID";
                        var totalKey = total?.Invoke(dict) ?? "Total";
                        var repeater = new Repeater(Convert.ToInt32(dict.GetValueOrDefault(idKey, 0)),
                            Convert.ToInt32(dict.GetValueOrDefault(totalKey, 0)));
                        var value = repeater.Sample01(IntervalType.HeadClosed);
                        parameter[target?.Invoke(dict) ?? "Value"] = value;
                        return parameter;
                    }));
        }
    }
}
