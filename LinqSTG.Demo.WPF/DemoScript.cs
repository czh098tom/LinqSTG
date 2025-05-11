using System;
using System.Drawing;
using LinqSTG;
using static LinqSTG.Pattern;
using LinqSTG.Easings;
using LinqSTG.Demo.WPF;
using static LinqSTG.Demo.WPF.DegreeMaths;

namespace LinqSTG.Demo.WPF
{
    public static class DemoScript
    {
        public static IEnumerable<PointPrediction> Test1()
        {
            var pattern =
                from r1 in Repeat<int>(times: 6)
                from r2 in RepeatWithInterval(times: 9, interval: 12)
                select r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f)
                    + r2.Sample01(IntervalType.BothClosed).MinMax(-15f, 15f);
            return new PointShooter<float>(r => t => new(t * Cos(r), t * Sin(r)))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> Test2()
        {
            var pattern =
                from r1 in Repeat<int>(times: 12)
                from r2 in RepeatWithInterval(times: 9, interval: 12)
                select (r: r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f)
                    + r2.Sample01(IntervalType.BothClosed).MinMax(-15f, 15f)
                    , v: r2.Sample01(IntervalType.BothClosed).MinMax(1.2f, 1f));
            return new PointShooter<(float r, float v)>(r =>
                    t => new(t * Cos(r.r) * r.v, t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> TestPolyhedron()
        {
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

            return new PointShooter<(float x, float y, float r, float v)>(r =>
                    t => new(r.x + t * Cos(r.r) * r.v, r.y + t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }
    }
}
