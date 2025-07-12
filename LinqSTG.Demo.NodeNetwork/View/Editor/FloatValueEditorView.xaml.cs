using LinqSTG.Demo.NodeNetwork.ViewModel.Editor;
using ReactiveUI;
using System.Windows;

namespace LinqSTG.Demo.NodeNetwork.View.Editor
{
    public partial class FloatValueEditorView : IViewFor<FloatValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(FloatValueEditorViewModel), typeof(FloatValueEditorView), new PropertyMetadata(null));

        public FloatValueEditorViewModel? ViewModel
        {
            get => (FloatValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (FloatValueEditorViewModel?)value;
        }
        #endregion

        public FloatValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d => d(
                this.Bind(ViewModel, vm => vm.RawValue, v => v.TextBox.Text)
            ));
        }
    }
}
