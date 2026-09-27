using LinqSTG.Kinematics;
using static LinqSTG.Kinematics.Parametric;

namespace LinqSTG.Test
{
    public class KinematicsTest
    {
        [Test]
        public void TestUniformVelocityDerive()
        {
            var movement = UniformVelocity<float, float, float>(3f);
            Assert.Multiple(() =>
            {
                Assert.That(movement.Derive(0f), Is.EqualTo(3f));
                Assert.That(movement.Derive(10f), Is.EqualTo(3f));
                Assert.That(movement.Predict(5f), Is.EqualTo(15f));
            });
        }

        [Test]
        public void TestUniformAccelerationDerive()
        {
            var movement = UniformAcceleration<float, float, float, float>(velocity: 2f, accel: 0.5f);
            Assert.Multiple(() =>
            {
                Assert.That(movement.Derive(0f), Is.EqualTo(2f));
                Assert.That(movement.Derive(4f), Is.EqualTo(2f + 0.5f * 4f).Within(1e-5f));
                Assert.That(movement.Predict(4f), Is.EqualTo(2f * 4f + 0.5f * 16f / 2f).Within(1e-5f));
            });
        }

        [Test]
        public void TestOffsetPreservesDerive()
        {
            var movement = UniformVelocity<float, float, float>(2f);
            var offsetted = movement.Offset(10f);
            Assert.Multiple(() =>
            {
                Assert.That(offsetted.Predict(3f), Is.EqualTo(16f));
                Assert.That(offsetted.Derive(3f), Is.EqualTo(2f));
                Assert.That(offsetted, Is.InstanceOf<IDerivableParametric<float, float, float>>());
            });
        }

        [Test]
        public void TestOffsetNonDerivableSource()
        {
            var plain = Create<int, float>(t => t * t);
            var offsetted = plain.Offset(100f);
            Assert.Multiple(() =>
            {
                Assert.That(offsetted.Predict(3), Is.EqualTo(109f));
                Assert.That(offsetted, Is.Not.InstanceOf<IDerivableParametric<int, float, float>>());
            });
        }

        [Test]
        public void TestAfterTimeDerive()
        {
            // Uniform acceleration from rest (accel = 2), switching to uniform velocity (3) after 60 units of time.
            IDerivableParametric<float, float, float> movement =
                UniformAcceleration<float, float, float, float>(2f)
                    .AfterTime(60f, UniformVelocity<float, float, float>(3f));
            Assert.Multiple(() =>
            {
                // Before the switch time: derivative is a*t.
                Assert.That(movement.Derive(10f), Is.EqualTo(20f).Within(1e-5f));
                // At and after the switch time (matching Predict's branch split): constant derivative 3.
                Assert.That(movement.Derive(60f), Is.EqualTo(3f).Within(1e-5f));
                Assert.That(movement.Derive(100f), Is.EqualTo(3f).Within(1e-5f));
                // Position is continuous at the switch time: a*t^2/2 = 2*3600/2 = 3600.
                Assert.That(movement.Predict(60f), Is.EqualTo(3600f).Within(1e-2f));
                Assert.That(movement.Predict(90f), Is.EqualTo(3690f).Within(1e-2f));
            });
        }

        [Test]
        public void TestAfterTimeDeriveMatchesFiniteDifference()
        {
            // Same multi-stage speed chain as DemoScript.TestMultipleSpeed; verify Derive against finite differences of Predict.
            const float v1 = 2f, v2 = 0.1f, v3 = 1f;
            const int t1 = 60, t2 = 120, t3 = 150;
            var movement =
                UniformAcceleration<float, float, float, float>(v1, (v2 - v1) / t1)
                    .AfterTime(t1, UniformVelocity<float, float, float>(v2))
                    .AfterTime(t2, UniformAcceleration<float, float, float, float>(v2, (v3 - v2) / (t3 - t2)))
                    .AfterTime(t3, UniformVelocity<float, float, float>(v3))
                    .Offset(5f);

            foreach (var t in new[] { 5f, 30f, 59.5f, 90f, 130f, 200f })
            {
                var dt = 1e-2f;
                var slope = (movement.Predict(t + dt) - movement.Predict(t - dt)) / (2 * dt);
                Assert.That(movement.Derive(t), Is.EqualTo(slope).Within(1e-2f), $"time = {t}");
            }
        }

        [Test]
        public void TestDerivableFactory()
        {
            var movement = Derivable<double, double, double>(
                t => t * t,
                t => 2 * t);
            Assert.Multiple(() =>
            {
                Assert.That(movement.Predict(3.0), Is.EqualTo(9.0));
                Assert.That(movement.Derive(3.0), Is.EqualTo(6.0));
            });
        }
    }
}
