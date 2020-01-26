using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeqAlign.Shared.Models;

namespace UnitTests
{
    [TestClass]
    public class RegionsModelIntegrationTests
    {
        private static ICollection<Region> GetRegularTestRegions()
        {
            return new List<Region>
            {
                new Region(1, 131m, 132m),
                new Region(1, 117m, 544m),
                new Region(1, 649m, 870m),
                new Region(1, 799m, 922m),
                new Region(1, 563m, 659m)
            };
        }
        private static ICollection<Region> GetCompactTestRegions()
        {
            return new List<Region>
            {
                new Region(131m, 132m, 1),
                new Region(117m, 544m, 1),
                new Region(649m, 870m, 1),
                new Region(799m, 922m, 1),
                new Region(563m, 659m, 1)
            };
        }

        private static List<List<Region>> GetRegularStackingRegionsResult()
        {
            return new List<List<Region>>
            {
                new List<Region>
                {
                    new Region(1,   117, 544m),
                    new Region(1,   563, 659m),
                    new Region(1,   799, 922m)

                },
                new List<Region>
                {
                    new Region(1, 131, 132m),
                    new Region(1, 649, 870m)
                }
            };
        }

        private static List<List<Region>> GetCompactStackingRegionsResult()
        {
            return new List<List<Region>>
            {
                new List<Region>
                {
                    new Region(117m, 130, 1),
                    new Region(131m, 131, 2),
                    new Region(132m, 544, 1),

                    new Region(563m, 648, 1),
                    new Region(649m, 658, 2),
                    new Region(659m, 798, 1),
                    new Region(799m, 869, 2),
                    new Region(870m, 922, 1)
                }
            };
        }

        [TestMethod]
        public void RegularStackingRegionsReturnsCorrectStack()
        {

            var actual = new RegionsModel(GetRegularTestRegions(), StackingMethod.RegularStacking).Rows;
            var expected = GetRegularStackingRegionsResult();
            actual.Should().BeEquivalentTo(expected);

        }

        [TestMethod]
        public void CompactStackingRegionsReturnsCorrectStack()
        {

            var actual = new RegionsModel(GetCompactTestRegions(), StackingMethod.CompactStacking).Rows;
            var expected = GetCompactStackingRegionsResult();
            actual.Should().BeEquivalentTo(expected);

        }
    }
}
