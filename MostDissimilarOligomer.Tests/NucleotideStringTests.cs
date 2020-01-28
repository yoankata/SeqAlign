using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SeqAlign.Shared.Models;
using Shouldly;

namespace MostDissimilarOligomer.UnitTests
{
    [TestClass]
    public class NucleotideStringTests
    {
        [TestMethod]
        public void ToStringShouldReturnEmptyStringWhenNucleotideStringEmpty()
        {
            var expected = "";

            var dnaList = new List<DNA>();

            var actual = (new NucleotideString(dnaList)).ToString();

            actual.Length.ShouldBe(expected.Length);
            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void ToStringShouldReturnCorrectString()
        {
            var expected = "AGTC";

            var dnaList = new List<DNA>();
            dnaList.Add(DNA.A);
            dnaList.Add(DNA.G);
            dnaList.Add(DNA.T);
            dnaList.Add(DNA.C);
            var actual = (new NucleotideString(dnaList)).ToString();

            actual.Length.ShouldBe(expected.Length);
            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void ToCharArrayShouldReturnCorrectString()
        {
            var expected = ("AGTC").ToCharArray();

            var dnaList = new List<DNA>();
            dnaList.Add(DNA.A);
            dnaList.Add(DNA.G);
            dnaList.Add(DNA.T);
            dnaList.Add(DNA.C);
            var actual = (new NucleotideString(dnaList)).ToCharArray();

            actual.Length.ShouldBe(expected.Length);
            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void ToCharArrayShouldReturnEmptyStringWhenNucleotideStringEmpty()
        {
            var expected = ("").ToCharArray();

            var dnaList = new List<DNA>();

            var actual = (new NucleotideString(dnaList)).ToCharArray();

            actual.Length.ShouldBe(expected.Length);
            actual.ShouldBe(expected);
        }
    }
}
