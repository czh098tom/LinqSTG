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

        /// <summary>
        /// Generates a spiral bullet pattern with increasing radius.
        /// </summary>
        public static IEnumerable<PointPrediction> TestSpiral()
        {
            // Create a pattern with 20 arms, each rotating with slight angle increments
            var pattern =
                from r1 in RepeatWithInterval(times: 20, interval: 5)
                from r2 in Repeat<int>(times: 10)
                select (
                    r: r1.Sample01(IntervalType.BothClosed).MinMax(0f, 360f) + r2.ID * 10f, // Spiral angle increases with r2
                    v: r2.Sample01(IntervalType.BothClosed).MinMax(0.8f, 1.2f) // Random speed variation
                );

            // Use PointShooter to convert angles and speeds into polar coordinates
            return new PointShooter<(float r, float v)>(r =>
                t => new(t * Cos(r.r) * r.v, t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        /// <summary>
        /// Generates a wave-like bullet pattern with oscillating angles.
        /// </summary>
        public static IEnumerable<PointPrediction> TestWave()
        {
            // Create a pattern with 8 directions, each with oscillating sub-bullets
            var pattern =
                from r1 in Repeat<int>(times: 8)
                from r2 in RepeatWithInterval(times: 12, interval: 8)
                select (
                    r: r1.Sample01(IntervalType.HeadClosed).MinMax(0f, 360f) +
                       Sin(r2.ID * 30f) * 20f, // Oscillate angle using sine function
                    v: 1f + 0.2f * Cos(r2.ID * 15f) // Vary speed with cosine for wave effect
                );

            // Convert to polar coordinates with PointShooter
            return new PointShooter<(float r, float v)>(r =>
                t => new(t * Cos(r.r) * r.v, t * Sin(r.r) * r.v))
                .Shoot(pattern);
        }

        public static IEnumerable<PointPrediction> TestRect()
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
