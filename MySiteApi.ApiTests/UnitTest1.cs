using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private List<int> list = new List<int>();

        [SetUp]
        public void Setup()
        {
            //list = new List<int>(); // uncomment to pass all the tests
        }

        [TestCaseSource(nameof(pairs))]
        [MaxTime(100)]
        public void Test1(int actual, int expected)
        {
            list.Should().BeEmpty();
            list.Add(actual + expected);
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