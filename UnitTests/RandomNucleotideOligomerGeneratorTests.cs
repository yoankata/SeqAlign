using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeqAlign.Shared.Data;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;

namespace MostDissimilarOligomer.UnitTests
{
    [TestClass]
    public class RandomNucleotideOligomerGeneratorTests
    {
        [TestMethod]
        public async Task RandomNucleotideOligomerGeneratorGenerates()
        {
            var stringLength = 6;
            var setSize = 400;

            var actual = 
                await RandomNucleotideOligomerGenerator.RandomSetOfNuclotides(stringLength, setSize);

            actual.Count().ShouldBe(setSize);
            actual.All(s => s.Count() == stringLength).ShouldBeTrue();
            actual.All(s => s == actual.First()).ShouldBeFalse();
        }
    }
}
