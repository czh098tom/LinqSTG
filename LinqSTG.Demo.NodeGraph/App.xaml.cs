using LinqSTG.Demo.NodeGraph.ViewModel.Editor;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Data;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Movement;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Pattern;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Transformation;
using LinqSTG.Demo.NodeGraph.View;
using LinqSTG.Demo.NodeGraph.View.Editor;
using NodeNetwork;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.Views;
using ReactiveUI;
using System.Configuration;
using System.Data;
using System.Windows;
using LinqSTG.Demo.NodeGraph.ViewModel;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.PatternOperator;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.IntrinsicOperator;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.MovementOperator;

namespace LinqSTG.Demo.NodeGraph
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

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGPortView(), typeof(IViewFor<LinqSTGPortViewModel>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGConnectionView(), typeof(IViewFor<LinqSTGConnectionViewModel>));
            Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<ContextAwareNodeInputViewModel>));
            Splat.Locator.CurrentMutable.Register(() => new NodeOutputView(), typeof(IViewFor<ContextAwareNodeOutputViewModel>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<ConstantFloatNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<ConstantIntNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<ConstantStringNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<RepeaterKeyNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<Vector2FromRotationDistanceNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<Vector2Node>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<AddNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<FloatToIntNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<IntToFloatNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<MinMaxNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<Sample01Node>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<Sample01MinMaxNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<TakeRepeaterFromContextNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<TakeVariableFromContextNode>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<StationaryMovementNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<UniformVelocityMovementNode>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<MovementSumNode>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<RepeatWithIntervalPatternNode>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<MapPatternNode>));
            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<ExtrudePatternNode>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<AssignNode>));

            Splat.Locator.CurrentMutable.Register(() => new LinqSTGNodeView(), typeof(IViewFor<ShootNode>));
        }
    }

}
