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
        public static IEnumerable<PointPrediction> SimpleTest()
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

        public static IEnumerable<PointPrediction> TestV()
        {
            var vdir1 = Repeat<int>(times: 3)
                .Select(r =>
                    (r: r.Sample01(IntervalType.BothClosed).MinMax(0, 5),
                    v: r.Sample01(IntervalType.BothClosed).MinMax(1, 0.9f)));

            var v = vdir1.Concat(vdir1.Select(t => (r: -t.r, v: t.v)).Reverse());

            var pattern = from r1 in Repeat<int>(times: 9)
                          from r2 in v
                          select (r: r1.Sample01(IntervalType.BothClosed).MinMax(-60f, 60f) + r2.r,
                                  v: r2.v * (r1.ID % 2 == 0 ? 1f : 1.2f));

            return new PointShooter<(float r, float v)>(r =>
                    t => new(t * Cos(r.r) * r.v, t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> TestSpiral()
        {
            var pattern =
                from r1 in RepeatWithInterval(times: 20, interval: 5)
                from r2 in Repeat<int>(times: 10)
                select (
                    r: r1.Sample01(IntervalType.BothClosed).MinMax(0f, 360f) + r2.ID * 10f,
                    v: r2.Sample01(IntervalType.BothClosed).MinMax(0.8f, 1.2f)
                );

            return new PointShooter<(float r, float v)>(r =>
                t => new(t * Cos(r.r) * r.v, t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> TestWave()
        {
            var pattern =
                from r1 in Repeat<int>(times: 8)
                from r2 in RepeatWithInterval(times: 12, interval: 8)
                select (
                    r: r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f) +
                       Sin(r2.ID * 30f) * 20f,
                    v: 1f + 0.2f * Cos(r2.ID * 15f)
                );

            return new PointShooter<(float r, float v)>(r =>
                t => new(t * Cos(r.r) * r.v, t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> TestBlock()
        {
            var spr = RepeatWithInterval(times: 15, interval: 5)
                .Select(r => r.Sample01(IntervalType.BothClosed).MinMax(-180, 360));

            var rdiff = Repeat<int>(times: 3)
                .Select(r => r.Sample01(IntervalType.BothClosed).MinMax(-3, 3));

            var vext = Repeat<int>(times: 5)
                .Select(r => r.Sample01(IntervalType.BothClosed).MinMax(1, 1.4f));

            var pattern = from r1 in spr
                          from r2 in rdiff
                          from r3 in vext
                          select (x: 50 * Cos(r1), y: 50 * Sin(r1), r: r1 + r2 - 90, v: r3 * 2);

            return new PointShooter<(float x, float y, float r, float v)>(r =>
                    t => new(r.x + t * Cos(r.r) * r.v, r.y + t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> TestMultipleSpeed()
        {
            var golden = RepeatWithInterval(100, 1);

            var spread = Repeat<int>(4);

            var speed = Repeat<int>(2);

            var pattern = from r1 in golden
                          from r2 in spread
                          from r3 in speed
                          let r0 = r1.ID * 137.5f
                          select (x: 5 * Cos(r0),
                                  y: 5 * Sin(r0),
                                  r: r0 + r2.Sample01(IntervalType.BothClosed).MinMax(-2f, 2f),
                                  v: r3.Sample01(IntervalType.BothClosed).MinMax(1, 1.05f)
                                    * r1.Sample01(IntervalType.BothClosed).MinMax(1.5f, 1f));

            const float v1mul = 2f;
            const float v2mul = 0.1f;
            const float v3mul = 1f;
            const int t1 = 60;
            const int t2 = 120;
            const int t3 = 150;

            return new PointShooter<(float x, float y, float r, float v)>(r =>
                t => t switch
                {
                    < t1 => new(
                        r.x + Cos(r.r) * r.v * (v1mul * t + 0.5f * (v2mul - v1mul) / t1 * t * t),
                        r.y + Sin(r.r) * r.v * (v1mul * t + 0.5f * (v2mul - v1mul) / t1 * t * t)),
                    >= t1 and < t2 => new(
                        r.x + Cos(r.r) * r.v * ((v2mul + v1mul) * t1 / 2 + (t - t1) * v2mul),
                        r.y + Sin(r.r) * r.v * ((v2mul + v1mul) * t1 / 2 + (t - t1) * v2mul)),
                    >= t2 and < t3 => new(
                        r.x + Cos(r.r) * r.v * ((v2mul + v1mul) * t1 / 2 + (t2 - t1) * v2mul + v2mul * (t - t2) + 0.5f * (v3mul - v2mul) / (t3 - t2) * (t - t2) * (t - t2)),
                        r.y + Sin(r.r) * r.v * ((v2mul + v1mul) * t1 / 2 + (t2 - t1) * v2mul + v2mul * (t - t2) + 0.5f * (v3mul - v2mul) / (t3 - t2) * (t - t2) * (t - t2))),
                    _ => new(
                        r.x + Cos(r.r) * r.v * ((v2mul + v1mul) * t1 / 2 + (t2 - t1) * v2mul + (v3mul + v2mul) * (t3 - t2) / 2 + (t - t3) * v3mul),
                        r.y + Sin(r.r) * r.v * ((v2mul + v1mul) * t1 / 2 + (t2 - t1) * v2mul + (v3mul + v2mul) * (t3 - t2) / 2 + (t - t3) * v3mul)),
                }).Shoot(pattern);
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

        public static IEnumerable<PointPrediction> TestRandom() 
        {
            var randomizer = new Random(Convert.ToInt32(DateTime.Now.Ticks % (1L + int.MaxValue)));
            var randomXY = RepeatWithInterval(times: 10, interval: 10)
                .Select(r => (
                    x: randomizer.NextSingle() * 100 - 50, 
                    y: randomizer.NextSingle() * 100 - 50,
                    v: 1f + randomizer.NextSingle() * 0.5f));

            var circle = Repeat<int>(24)
                .Select(r => r.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f));

            var pattern = from r1 in randomXY
                          from r2 in circle
                          select (x: r1.x,
                                  y: r1.y,
                                  r: r2,
                                  v: r1.v);

            return new PointShooter<(float x, float y, float r, float v)>(r =>
                    t => new(r.x + t * Cos(r.r) * r.v, r.y + t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }
    }
}
