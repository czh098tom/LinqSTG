using DynamicData;
using LinqSTG.Demo.NodeGraph;
using LinqSTG.Demo.NodeGraph.Serialization;
using LinqSTG.Demo.NodeGraph.ViewModel;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes;
using LinqSTG.Demo.NodeGraph.Properties;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Data;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Movement;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Pattern;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.Transformation;
using Newtonsoft.Json;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.PatternOperator;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.IntrinsicOperator;
using LinqSTG.Demo.NodeGraph.ViewModel.Nodes.MovementOperator;

namespace LinqSTG.Demo.NodeGraph
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
            network.ConnectionFactory = (input, output) => new LinqSTGConnectionViewModel(network, input, output);

            var modifyNodes = network.Nodes
                .Connect()
                .ToCollection()
                .SelectMany(c => c
                    .OfType<ShootNode>()
                    .Select(s => s.Result)
                    .CombineLatest())
                .Subscribe(ls =>
                {
                    pointPredictions = ls.SelectMany(pred => pred.Invoke(Parameter.Empty));
                    UpdatePrediction();
                });

            NodeList.AddNodeType(() => new ConstantFloatNode());
            NodeList.AddNodeType(() => new ConstantIntNode());
            NodeList.AddNodeType(() => new ConstantStringNode());
            NodeList.AddNodeType(() => new RepeaterKeyNode());
            NodeList.AddNodeType(() => new Vector2FromRotationDistanceNode());
            NodeList.AddNodeType(() => new Vector2Node());

            NodeList.AddNodeType(() => new AddNode());
            NodeList.AddNodeType(() => new FloatToIntNode());
            NodeList.AddNodeType(() => new IntToFloatNode());
            NodeList.AddNodeType(() => new MinMaxNode());
            NodeList.AddNodeType(() => new Sample01Node());
            NodeList.AddNodeType(() => new Sample01MinMaxNode());
            NodeList.AddNodeType(() => new TakeRepeaterFromContextNode());
            NodeList.AddNodeType(() => new TakeVariableFromContextNode());

            NodeList.AddNodeType(() => new StationaryMovementNode());
            NodeList.AddNodeType(() => new UniformVelocityMovementNode());

            NodeList.AddNodeType(() => new MovementSumNode());

            NodeList.AddNodeType(() => new RepeatWithIntervalPatternNode());

            NodeList.AddNodeType(() => new MapPatternNode());
            NodeList.AddNodeType(() => new ExtrudePatternNode());

            NodeList.AddNodeType(() => new AssignNode());

            NodeList.AddNodeType(() => new ShootNode());
        }

        private void UpdatePrediction()
        {
            Points.Clear();
            foreach (var pred in pointPredictions)
            {
                if (Time >= pred.StartTime)
                {
                    var point = pred.PointFunc.Invoke(Time - pred.StartTime);
                    if (float.IsNaN(point.X) || float.IsNaN(point.Y)) continue;
                    Points.Add(new PointF(point.X, -point.Y));
                }
            }
        }

        public void Save()
        {
            try
            {
                Settings.Default.Temp = JsonConvert.SerializeObject(NetworkModel.FromViewModel(network));
                Settings.Default.Save();
            }
            catch (Exception)
            {
                Settings.Default.Temp = string.Empty;
                Settings.Default.Save();
            }
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(Settings.Default.Temp))
            {
                return;
            }
            try
            {
                var model = JsonConvert.DeserializeObject<NetworkModel>(Settings.Default.Temp);
                model?.ApplyToNetwork(network);
            }
            catch (Exception)
            {
                network.Connections.Clear();
                network.Nodes.Clear();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
