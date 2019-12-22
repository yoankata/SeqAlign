using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeqAlign.Models;

namespace UnitTests
{
    [TestClass]
    public class RegionUnitTests
    {
        [TestMethod]
        public void TestMethRegionConstructorWithoutOverlapShouldHaveOverlap1()
        {
            var region = new Region(0, 1700000000000, 17000000000000);

            region.Overlap.Should().Be(1);
        }

        [TestMethod]
        public void RegionConstructorWithoutIdShouldHaveIdMinus1()
        {
            var region = new Region(1700000000000, 17000000000000, 1);

            region.Id.Should().Be(-1);
        }
    }
}

