using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LinqSTG.Demo.WPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static readonly ScriptOptions DefaultOptions = ScriptOptions.Default
            .WithReferences(typeof(IPattern<,>).Assembly, typeof(MainViewModel).Assembly, typeof(Point).Assembly)
            .WithImports("System", "System.Drawing", "LinqSTG", "LinqSTG.Easings", "LinqSTG.Demo.WPF");

        public string PatternScript
        {
            get => patternScript;
            set
            {
                patternScript = value;
                RaisePropertyChanged();
            }
        }
        private string patternScript =
            """
            var pattern = 
                from r1 in Pattern.Repeat<int>(times: 6)
                from r2 in Pattern.RepeatWithInterval(times: 9, interval: 12)
                select r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f)
                    + r2.Sample01(IntervalType.BothClosed).MinMax(-15f, 15f);
            """;

        public string PositionScript
        {
            get => positionScript;
            set
            {
                positionScript = value;
                RaisePropertyChanged();
            }
        }
        private string positionScript =
            """
            return new PointShooter<float>(r => 
                t => 
                    new PointF(Convert.ToSingle(t * Math.Cos(r * Math.PI / 180)), 
                        Convert.ToSingle(t * Math.Sin(r * Math.PI / 180))))
                .Shoot(pattern);
            """;

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

        private IEnumerable<PointPrediction> pointPredictions = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public async void GeneratePredictions()
        {
            pointPredictions = await CSharpScript.EvaluateAsync<IEnumerable<PointPrediction>>(
                $"{PatternScript}\n{PositionScript}", DefaultOptions);
            pointPredictions = pointPredictions.Select(pred => new PointPrediction(t =>
            {
                var p = pred.PointFunc(t);
                return new PointF(p.X + 100, p.Y + 100);
            }, pred.StartTime));
            UpdatePrediction();
        }

        private void UpdatePrediction()
        {
            Points.Clear();
            foreach (var pred in pointPredictions)
            {
                if (Time >= pred.StartTime)
                {
                    Points.Add(pred.PointFunc.Invoke(Time - pred.StartTime));
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
