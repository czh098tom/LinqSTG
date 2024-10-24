namespace LinqSTG.Test
{
    public class PatternTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test0()
        {
            var pattern0 = Pattern.Repeat<int>(0);
            Assert.That(pattern0.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Test1()
        {
            var pattern1 = Pattern.Repeat<int>(1);
            Assert.Multiple(() =>
            {
                Assert.That(pattern1.Count(), Is.EqualTo(1));
                Assert.That(pattern1.ToArray()[0].Data.ID, Is.EqualTo(0));
                Assert.That(pattern1.ToArray()[0].Data.Total, Is.EqualTo(1));
            });
        }

        [Test]
        public void TestM1()
        {
            var patternM1 = Pattern.Repeat<int>(-1);
            Assert.That(patternM1.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Test123()
        {
            var pattern123 = Pattern.Repeat<int>(3);
            Assert.Multiple(() =>
            {
                Assert.That(pattern123.Count(), Is.EqualTo(3));
                Assert.That(pattern123.ToArray()[0].Data.ID, Is.EqualTo(0));
                Assert.That(pattern123.ToArray()[0].Data.Total, Is.EqualTo(3));
                Assert.That(pattern123.ToArray()[1].Data.ID, Is.EqualTo(1));
                Assert.That(pattern123.ToArray()[1].Data.Total, Is.EqualTo(3));
                Assert.That(pattern123.ToArray()[2].Data.ID, Is.EqualTo(2));
                Assert.That(pattern123.ToArray()[2].Data.Total, Is.EqualTo(3));
            });
        }

        [Test]
        public void Test0i()
        {
            var pattern0i = Pattern.RepeatWithInterval(0, 2);
            Assert.That(pattern0i.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Test1i()
        {
            var pattern1i = Pattern.RepeatWithInterval(1, 2);
            Assert.Multiple(() =>
            {
                Assert.That(pattern1i.Count(), Is.EqualTo(2));

                Assert.That(pattern1i.ToArray()[0].IsData, Is.EqualTo(true));
                Assert.That(pattern1i.ToArray()[0].Data.ID, Is.EqualTo(0));
                Assert.That(pattern1i.ToArray()[0].Data.Total, Is.EqualTo(1));
                Assert.That(pattern1i.ToArray()[1].IsData, Is.EqualTo(false));
                Assert.That(pattern1i.ToArray()[1].Interval, Is.EqualTo(2));
            });
        }

        [Test]
        public void TestM1i()
        {
            var patternM1i = Pattern.RepeatWithInterval(-1, 2);
            Assert.That(patternM1i.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Test123i()
        {
            var pattern123i = Pattern.RepeatWithInterval(3, 2);
            Assert.Multiple(() =>
            {
                Assert.That(pattern123i.Count(), Is.EqualTo(6));

                Assert.That(pattern123i.ToArray()[0].IsData, Is.EqualTo(true));
                Assert.That(pattern123i.ToArray()[0].Data.ID, Is.EqualTo(0));
                Assert.That(pattern123i.ToArray()[0].Data.Total, Is.EqualTo(3));
                Assert.That(pattern123i.ToArray()[1].IsData, Is.EqualTo(false));
                Assert.That(pattern123i.ToArray()[1].Interval, Is.EqualTo(2));
                Assert.That(pattern123i.ToArray()[2].IsData, Is.EqualTo(true));
                Assert.That(pattern123i.ToArray()[2].Data.ID, Is.EqualTo(1));
                Assert.That(pattern123i.ToArray()[2].Data.Total, Is.EqualTo(3));
                Assert.That(pattern123i.ToArray()[3].IsData, Is.EqualTo(false));
                Assert.That(pattern123i.ToArray()[3].Interval, Is.EqualTo(2));
                Assert.That(pattern123i.ToArray()[4].IsData, Is.EqualTo(true));
                Assert.That(pattern123i.ToArray()[4].Data.ID, Is.EqualTo(2));
                Assert.That(pattern123i.ToArray()[4].Data.Total, Is.EqualTo(3));
                Assert.That(pattern123i.ToArray()[5].IsData, Is.EqualTo(false));
                Assert.That(pattern123i.ToArray()[5].Interval, Is.EqualTo(2));
            });
        }
    }
}