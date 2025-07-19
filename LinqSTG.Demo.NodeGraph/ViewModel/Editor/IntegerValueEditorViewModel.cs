using LinqSTG.Demo.NodeGraph.ViewModel;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;

namespace LinqSTG.Demo.NodeGraph.ViewModel.Editor
{
    public class IntegerValueEditorViewModel : ValueEditorViewModel<Contextual<int>>, IContextualValueEditorViewModel<int>
    {
        private static readonly Contextual<int> DefaultValue = Contextual.Create(dict => 0);

        public int RawValue
        {
            get => Value?.Invoke(Parameter.Empty) ?? 0;
            set => Value = Contextual.Create(dict => value);
        }

        public IntegerValueEditorViewModel()
        {
            Value = DefaultValue;
        }
    }
}
