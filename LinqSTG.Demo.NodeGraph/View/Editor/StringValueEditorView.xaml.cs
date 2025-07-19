using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using ReactiveUI;
using System.Windows;

namespace LinqSTG.Demo.NodeGraph.View.Editor
{
    public partial class StringValueEditorView : IViewFor<StringValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(StringValueEditorViewModel), typeof(StringValueEditorView), new PropertyMetadata(null));

        public StringValueEditorViewModel? ViewModel
        {
            get => (StringValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StringValueEditorViewModel?)value;
        }
        #endregion

        public StringValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d => d(
                this.Bind(ViewModel, vm => vm.RawValue, v => v.TextBox.Text)
            ));
        }
    }
}
