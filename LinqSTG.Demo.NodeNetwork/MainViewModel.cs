using DynamicData;
using LinqSTG.Demo.NodeNetwork.ViewModel;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Data;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Movement;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Operator;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Pattern;
using LinqSTG.Demo.NodeNetwork.ViewModel.Nodes.Transformation;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.NodeNetwork
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PointF> Points { get; set; } = [];

        public int Time
        {
            get => time;
            set
            {
                time = value;
                RaisePropertyChanged();
                UpdatePrediction();
            }
        }
        private int time;

        public NetworkViewModel Network
        {
            get => network;
            set
            {
                network = value;
                RaisePropertyChanged();
            }
        }
        private NetworkViewModel network = new();

        private ShootNode shootNode;

        public NodeListViewModel NodeList { get; } = new();

        private IEnumerable<PointPrediction> pointPredictions = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            shootNode = new ShootNode();
            network.Nodes.Add(shootNode);

            shootNode.Result
                .Subscribe(pred =>
                {
                    pointPredictions = [.. pred.Invoke(Parameter.Empty)];
                    UpdatePrediction();
                });

            //NodeList.AddNodeType(() => new IntLiteralNode());
            //NodeList.AddNodeType(() => new TextLiteralNode());
            //NodeList.AddNodeType(() => new FloatLiteralNode());
            NodeList.AddNodeType(() => new RepeatWithIntervalPatternNode());

            NodeList.AddNodeType(() => new ConstantFloatNode());
            NodeList.AddNodeType(() => new ConstantIntNode());
            NodeList.AddNodeType(() => new ConstantStringNode());
            NodeList.AddNodeType(() => new TakeVariableFromContextNode());

            NodeList.AddNodeType(() => new MapPatternNode());
            NodeList.AddNodeType(() => new ExtrudePatternNode());
            //NodeList.AddNodeType(() => new MapTransformationNode());

            NodeList.AddNodeType(() => new UniformVelocityMovementNode());

            NodeList.AddNodeType(() => new SampleMinMaxNode());
            NodeList.AddNodeType(() => new MinMaxNode());
            NodeList.AddNodeType(() => new Sample01Node());
        }

        public void GeneratePredictions()
        {
            UpdatePrediction();
        }

        private void UpdatePrediction()
        {
            Points.Clear();
            foreach (var pred in pointPredictions)
            {
                if (Time >= pred.StartTime)
                {
                    var point = pred.PointFunc.Invoke(Time - pred.StartTime);
                    Points.Add(new PointF(point.X, -point.Y));
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
