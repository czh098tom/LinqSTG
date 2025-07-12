using LinqSTG.Demo.NodeNetwork.View.Editor;
using LinqSTG.Demo.NodeNetwork.ViewModel.Editor;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Data;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Movement;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Operator;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Pattern;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Transformation;
using NodeNetwork;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.Views;
using ReactiveUI;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LinqSTG.Demo.NodeNetwork
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            NNViewRegistrar.RegisterSplat();

            Splat.Locator.CurrentMutable.Register(() => new IntegerValueEditorView(), typeof(IViewFor<IntegerValueEditorViewModel>));
            Splat.Locator.CurrentMutable.Register(() => new StringValueEditorView(), typeof(IViewFor<StringValueEditorViewModel>));
            Splat.Locator.CurrentMutable.Register(() => new FloatValueEditorView(), typeof(IViewFor<FloatValueEditorViewModel>));

            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<ShootNode>));

            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<ConstantFloatNode>));
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<ConstantIntNode>));
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<ConstantStringNode>));
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<TakeVariableFromContextNode>));

            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<MapPatternNode>));
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<ExtrudePatternNode>));

            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<RepeatWithIntervalPatternNode>));

            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<UniformVelocityMovementNode>));

            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<SampleMinMaxNode>));
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<MinMaxNode>));
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<Sample01Node>));
        }
    }

}
