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
            .WithImports(
                "System", 
                "System.Drawing", 
                "LinqSTG", 
                "LinqSTG.Pattern", 
                "LinqSTG.Easings", 
                "LinqSTG.Demo.WPF",
                "LinqSTG.Demo.WPF.DegreeMaths");

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
            IPattern<(float r, float v), int> Polyhedron(int nVert, float initRot, int nSegment)
            {
                var vert = Repeat<int>(nVert);
                return vert.SelectMany(
                    _ => Repeat<int>(nSegment),
                    (r1, r2) =>
                    {
                        var x0 = Cos(initRot + r1.Sample01(IntervalType.HeadClosed).MinMax(0, 360f));
                        var y0 = Sin(initRot + r1.Sample01(IntervalType.HeadClosed).MinMax(0, 360f));
                        var x1 = Cos(initRot + r1.Sample01(IntervalType.HeadClosed).MinMax(0, 360f) + 360f / nVert);
                        var y1 = Sin(initRot + r1.Sample01(IntervalType.HeadClosed).MinMax(0, 360f) + 360f / nVert);
                        var k = r2.Sample01(IntervalType.HeadClosed);
                        var x = k * x1 + (1 - k) * x0;
                        var y = k * y1 + (1 - k) * y0;
                        return (Atan2(y, x), Hypot(x, y));
                    });
            }

            var move = RepeatWithInterval(times: 15, interval: 3);

            var pattern = from r1 in RepeatWithInterval(times: 15, interval: 6)
                          from r2 in Polyhedron(4, r1.Sample01(IntervalType.BothClosed).MinMax(0, 180), 10)
                          select (
                            x: r1.ID * 15f, 
                            y: r1.ID * 15f, 
                            r: r2.r, 
                            v: r2.v);
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
            return new PointShooter<(float x, float y, float r, float v)>(r =>
                    t => new(r.x + t * Cos(r.r) * r.v, r.y + t * Sin(r.r) * r.v))
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
            pointPredictions = [.. pointPredictions.Select(pred => new PointPrediction(t =>
            {
                var p = pred.PointFunc(t);
                return new PointF(p.X, -p.Y);
            }, pred.StartTime))];
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
