using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(nameof(pairs))]
        [MaxTime(100)]
        public void Test1(int actual, int expected)
        {
            actual.Should().Be(expected);
        }

        static object[] pairs =
        {
            new object[] {1, 1},
            new object[] {2, 2},
            new object[] {3, 3}
        };
    }
}