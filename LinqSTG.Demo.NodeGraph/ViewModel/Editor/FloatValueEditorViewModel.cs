using LinqSTG.Demo.NodeGraph.ViewModel;
using NodeNetwork.Toolkit.ValueNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Editor
{
    public class FloatValueEditorViewModel : ValueEditorViewModel<Contextual<float>>, IContextualValueEditorViewModel<float>
    {
        private static readonly Contextual<float> DefaultValue = Contextual.Create(dict => 0f);

        public float RawValue
        {
            get => Value?.Invoke(Parameter.Empty) ?? 0f;
            set => Value = Contextual.Create(dict => value);
        }

        public FloatValueEditorViewModel()
        {
            Value = DefaultValue;
        }
    }
}
