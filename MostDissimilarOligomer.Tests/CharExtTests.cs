using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeqAlign.Shared.Extensions;
using Shouldly;

namespace MostDissimilarOligomer.UnitTests
{
    [TestClass]
    public class CharExtTests
    {
        [TestMethod]
        public void GetColorValueReturnsLightCoralForA()
        {
            var expected = "lightcoral";
            var actual = 'A'.GetColorValue();

            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void GetColorValueReturnsLightBlueForC()
        {
            var expected = "lightblue";
            var actual = 'C'.GetColorValue();

            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void GetColorValueReturnsLightPinkForT()
        {
            var expected = "lightpink";
            var actual = 'T'.GetColorValue();

            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void GetColorValueReturnsLightSkyBlueForG()
        {
            var expected = "lightskyblue";
            var actual = 'G'.GetColorValue();

            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void GetColorValueReturnsLavenderBlushForP() // any letter other than ACTG
        {
            var expected = "lavendarblush";
            var actual = 'P'.GetColorValue();

            actual.ShouldBe(expected);
        }
    }
}
