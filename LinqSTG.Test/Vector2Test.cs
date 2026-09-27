using LinqSTG.Easings;
using LinqSTG.Kinematics;
using static LinqSTG.Kinematics.Parametric;
using static LinqSTG.Kinematics.Vector2Parametric;
using Vector2F = LinqSTG.Kinematics.Vector2<float>;

namespace LinqSTG.Test
{
    public class Vector2Test
    {
        [Test]
        public void TestUniformVelocityWithFrameTime()
        {
            var velocity = new Vector2F(3f, -1.5f);
            var movement = UniformVelocityByFrames(velocity);
            var at60 = movement.Predict(60);
            Assert.Multiple(() =>
            {
                Assert.That(at60.X, Is.EqualTo(180f));
                Assert.That(at60.Y, Is.EqualTo(-90f));
                Assert.That(movement.Derive(10), Is.EqualTo(velocity));
                Assert.That(movement.Velocity, Is.EqualTo(velocity));
            });
        }

        [Test]
        public void TestUniformVelocityWithScalarTime()
        {
            var movement = UniformVelocity<float, Vector2F, Vector2F>(new Vector2F(0.5f, 2f));
            var at = movement.Predict(1.5f);
            Assert.Multiple(() =>
            {
                Assert.That(at.X, Is.EqualTo(0.75f).Within(1e-6f));
                Assert.That(at.Y, Is.EqualTo(3f).Within(1e-6f));
                Assert.That(movement.Velocity, Is.EqualTo(new Vector2F(0.5f, 2f)));
            });
        }

        [Test]
        public void TestUniformVelocityWithDoubleComponents()
        {
            var movement = UniformVelocity<double, Vector2<double>, Vector2<double>>(new Vector2<double>(0.5, 2.0));
            var at = movement.Predict(3.0);
            Assert.Multiple(() =>
            {
                Assert.That(at.X, Is.EqualTo(1.5).Within(1e-9));
                Assert.That(at.Y, Is.EqualTo(6.0).Within(1e-9));
            });
        }

        [Test]
        public void TestUniformAccelerationWithFrameTime()
        {
            var movement = UniformAccelerationByFrames(
                velocity: new Vector2F(1f, 0f), accel: new Vector2F(0.5f, -0.25f));
            var at10 = movement.Predict(10);
            var derive10 = movement.Derive(10);
            Assert.Multiple(() =>
            {
                // v0 * t + a * t^2 / 2 = (10 + 25, -12.5)
                Assert.That(at10.X, Is.EqualTo(35f).Within(1e-4f));
                Assert.That(at10.Y, Is.EqualTo(-12.5f).Within(1e-4f));
                // v0 + a * t = (6, -2.5)
                Assert.That(derive10.X, Is.EqualTo(6f).Within(1e-4f));
                Assert.That(derive10.Y, Is.EqualTo(-2.5f).Within(1e-4f));
            });
        }

        [Test]
        public void TestOffsetAndAfterTimeComposition()
        {
            IDerivableParametric<int, Vector2F, Vector2F> movement =
                UniformVelocityByFrames(new Vector2F(1f, 2f))
                    .AfterTime(60, UniformAccelerationByFrames(
                        velocity: new Vector2F(0f, 0f), accel: new Vector2F(0.1f, -0.1f)))
                    .Offset(new Vector2F(100f, 100f));

            var at30 = movement.Predict(30);
            var at60 = movement.Predict(60);
            var at70 = movement.Predict(70);
            var derive70 = movement.Derive(70);
            Assert.Multiple(() =>
            {
                // Still on the uniform velocity: (30, 60) + offset.
                Assert.That(at30.X, Is.EqualTo(130f).Within(1e-2f));
                Assert.That(at30.Y, Is.EqualTo(160f).Within(1e-2f));
                // Continuous at the switch: (60, 120) + a * 0 + offset.
                Assert.That(at60.X, Is.EqualTo(160f).Within(1e-2f));
                Assert.That(at60.Y, Is.EqualTo(220f).Within(1e-2f));
                // After 10 frames of acceleration: (60, 120) + (5, -5) + offset.
                Assert.That(at70.X, Is.EqualTo(165f).Within(1e-2f));
                Assert.That(at70.Y, Is.EqualTo(215f).Within(1e-2f));
                // Derivative comes from the after motion: a * 10 = (1, -1).
                Assert.That(derive70.X, Is.EqualTo(1f).Within(1e-2f));
                Assert.That(derive70.Y, Is.EqualTo(-1f).Within(1e-2f));
            });
        }

        [Test]
        public void TestOperators()
        {
            var a = new Vector2F(1f, 2f);
            var b = new Vector2F(3f, 5f);
            Assert.Multiple(() =>
            {
                Assert.That(a + b, Is.EqualTo(new Vector2F(4f, 7f)));
                Assert.That(a - b, Is.EqualTo(new Vector2F(-2f, -3f)));
                // Component-wise (Hadamard) product.
                Assert.That(a * b, Is.EqualTo(new Vector2F(3f, 10f)));
                Assert.That(a * 2f, Is.EqualTo(new Vector2F(2f, 4f)));
                Assert.That(3f * a, Is.EqualTo(new Vector2F(3f, 6f)));
                // An int operand reaches the scalar multiply via implicit conversion.
                Assert.That(a * 3, Is.EqualTo(new Vector2F(3f, 6f)));
                Assert.That(a / 2f, Is.EqualTo(new Vector2F(0.5f, 1f)));
                Assert.That(new Vector2F(6f, 8f) / new Vector2F(2f, 4f), Is.EqualTo(new Vector2F(3f, 2f)));
                Assert.That(-a, Is.EqualTo(new Vector2F(-1f, -2f)));
                Assert.That(a.Dot(b), Is.EqualTo(13f).Within(1e-6f));
                Assert.That(new Vector2F(3f, 4f).LengthSquared(), Is.EqualTo(25f).Within(1e-6f));
                Assert.That(a, Is.EqualTo(new Vector2F(1f, 2f)));
                Assert.That(a == new Vector2F(1f, 2f), Is.True);
                Assert.That(a != b, Is.True);
                Assert.That(Vector2F.Zero, Is.EqualTo(new Vector2F(0f, 0f)));
                Assert.That(Vector2F.One, Is.EqualTo(new Vector2F(1f, 1f)));
                Assert.That(Vector2F.AdditiveIdentity, Is.EqualTo(Vector2F.Zero));
                Assert.That(Vector2F.MultiplicativeIdentity, Is.EqualTo(Vector2F.One));
            });
        }

        [Test]
        public void TestMinMaxEasingPerAxis()
        {
            var value = new Vector2F(0.25f, 0.75f);
            var min = new Vector2F(-10f, 0f);
            var max = new Vector2F(10f, 4f);
            var result = value.MinMax(min, max);
            Assert.Multiple(() =>
            {
                // x: 10 * 0.25 + (-10) * 0.75 = -5; y: 4 * 0.75 + 0 * 0.25 = 3.
                Assert.That(result.X, Is.EqualTo(-5f).Within(1e-5f));
                Assert.That(result.Y, Is.EqualTo(3f).Within(1e-5f));
            });
        }

        [Test]
        public void TestFloatingPointHelpers()
        {
            var v = new Vector2F(3f, 4f);
            var normalized = v.Normalize();
            Assert.Multiple(() =>
            {
                Assert.That(v.Length(), Is.EqualTo(5f).Within(1e-6f));
                Assert.That(v.Distance(Vector2F.Zero), Is.EqualTo(5f).Within(1e-6f));
                Assert.That(normalized.X, Is.EqualTo(0.6f).Within(1e-6f));
                Assert.That(normalized.Y, Is.EqualTo(0.8f).Within(1e-6f));
                Assert.That(normalized.Length(), Is.EqualTo(1f).Within(1e-6f));
            });
        }

        [Test]
        public void TestIntegerComponents()
        {
            // Integer components work through the frame factories and at call sites alike:
            // with only the scalar multiply operator left, `vector * int` is no longer
            // ambiguous for them (the int operand converts to the scalar exactly).
            var a = new Vector2<int>(1, 2);
            var b = new Vector2<int>(3, 4);
            var movement = UniformVelocityByFrames(new Vector2<int>(2, -1));
            Assert.Multiple(() =>
            {
                Assert.That(a + b, Is.EqualTo(new Vector2<int>(4, 6)));
                Assert.That(a * 5, Is.EqualTo(new Vector2<int>(5, 10)));
                var at10 = movement.Predict(10);
                Assert.That(at10.X, Is.EqualTo(20));
                Assert.That(at10.Y, Is.EqualTo(-10));
            });
        }
    }
}
