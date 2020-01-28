using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeqAlign.Shared.Extensions;
using SeqAlign.Shared.Models;
using Shouldly;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MostDissimilarOligomer.UnitTests
{
    [TestClass]
    public class NucleotideStringExtTests
    {
        [TestMethod]
        public void ComplementShouldReturnEmptyStringWhenNucleotideStringEmpty()
        {
            var expectedNucleotideString = new List<DNA>();
            var expected = new NucleotideString(expectedNucleotideString);

            var actualNucleotideString = new List<DNA>();
            var actual = (new NucleotideString(actualNucleotideString)).Complement();

            actual.Count().ShouldBe(expected.Count());
            actual.ShouldBe(expected);
        }

        [TestMethod]
        public void ComplementShouldReturnCorrectString()
        {
            var expectedNucleotideString = new List<DNA>();
            expectedNucleotideString.Add(DNA.T);
            expectedNucleotideString.Add(DNA.C);
            expectedNucleotideString.Add(DNA.A);
            expectedNucleotideString.Add(DNA.G);
            var expected = new NucleotideString(expectedNucleotideString);

            var actualNucleotideString = new List<DNA>();
            actualNucleotideString.Add(DNA.A);
            actualNucleotideString.Add(DNA.G);
            actualNucleotideString.Add(DNA.T);
            actualNucleotideString.Add(DNA.C);
            var actual = (new NucleotideString(actualNucleotideString)).Complement();

            actual.Count().ShouldBe(expected.Count());
            actual.ShouldBe(expected);
        }

        [TestMethod]
        public async Task GetOccurenceDictionaryDiffernetLegnthsShouldThrow()
        {
            var dictionary1 = new Dictionary<DNA, uint>();
            dictionary1.Add(DNA.A, 2);
            dictionary1.Add(DNA.C, 3);
            dictionary1.Add(DNA.T, 1);
            dictionary1.Add(DNA.G, 0);

            var dictionary2 = new Dictionary<DNA, uint>();
            dictionary2.Add(DNA.A, 1);
            dictionary2.Add(DNA.C, 2);
            dictionary2.Add(DNA.T, 2);
            dictionary2.Add(DNA.G, 1);

            var expected = new List<Dictionary<DNA, uint>>();
            expected.Add(dictionary1);
            expected.Add(dictionary2);

            var dnaString1 = new List<DNA>();
            dnaString1.Add(DNA.A);
            dnaString1.Add(DNA.C);
            dnaString1.Add(DNA.T);
            dnaString1.Add(DNA.G);

            var dnaString2 = new List<DNA>();
            dnaString2.Add(DNA.A);
            dnaString2.Add(DNA.A);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            var nucleotideStrings = new List<NucleotideString> 
            { 
                new NucleotideString(dnaString1),
                new NucleotideString(dnaString2),
            };

            Should.Throw<ArgumentOutOfRangeException>(async () => 
                await nucleotideStrings.GetOccurenceDictionary());
        }

        [TestMethod]
        public async Task GetOccurenceDictionaryShouldReturnCorrectDictionary()
        {
            var dictionary1 = new Dictionary<DNA, uint>();
            dictionary1.Add(DNA.A, 2);
            dictionary1.Add(DNA.C, 1);

            var dictionary2 = new Dictionary<DNA, uint>();
            dictionary2.Add(DNA.A, 1);
            dictionary2.Add(DNA.C, 2);

            var expected = new List<Dictionary<DNA, uint>>();
            expected.Add(dictionary1);
            expected.Add(dictionary2);

            var dnaString1 = new List<DNA>();
            dnaString1.Add(DNA.A);
            dnaString1.Add(DNA.C);

            var dnaString2 = new List<DNA>();
            dnaString2.Add(DNA.A);
            dnaString2.Add(DNA.A);

            var dnaString3 = new List<DNA>();
            dnaString3.Add(DNA.C);
            dnaString3.Add(DNA.C);

            var nucleotideStrings = new List<NucleotideString>
            {
                new NucleotideString(dnaString1),
                new NucleotideString(dnaString2),
                new NucleotideString(dnaString3),
            };

            var actual = await nucleotideStrings.GetOccurenceDictionary();

            actual.Count.ShouldBe(2);
            (actual[0][DNA.A] == 2).ShouldBeTrue();
            (actual[0][DNA.C] == 1).ShouldBeTrue();
            (actual[1][DNA.A] == 1).ShouldBeTrue();
            (actual[1][DNA.C] == 2).ShouldBeTrue();
        }

        [TestMethod]
        public async Task GetMostDifferentFromNucleotideSetShouldThrowWhenLessThan2()
        {
            var expectedDnaList = new List<DNA>();
            expectedDnaList.Add(DNA.A);
            expectedDnaList.Add(DNA.C);
            expectedDnaList.Add(DNA.T);
            expectedDnaList.Add(DNA.G);
            var expected = new NucleotideString(expectedDnaList);

            var dnaString1 = new List<DNA>();
            dnaString1.Add(DNA.A);
            dnaString1.Add(DNA.C);
            dnaString1.Add(DNA.T);
            dnaString1.Add(DNA.G);

            var nucleotideStrings = new List<NucleotideString>
            {
                new NucleotideString(dnaString1),
            };

            Should.Throw<ArgumentOutOfRangeException>(async () =>
                await nucleotideStrings.GetMostDifferentFromNucleotideSet());
        }

        [TestMethod]
        public async Task GetMostDifferentFromNucleotideSetShouldThrowWhenDifferentLengths()
        {
            var expectedDnaList = new List<DNA>();
            expectedDnaList.Add(DNA.A);
            expectedDnaList.Add(DNA.C);
            expectedDnaList.Add(DNA.T);
            expectedDnaList.Add(DNA.G);
            var expected = new NucleotideString(expectedDnaList);

            var dnaString1 = new List<DNA>();
            dnaString1.Add(DNA.A);
            dnaString1.Add(DNA.C);
            dnaString1.Add(DNA.T);
            dnaString1.Add(DNA.G);

            var dnaString2 = new List<DNA>();
            dnaString2.Add(DNA.A);
            dnaString2.Add(DNA.A);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.T);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            dnaString2.Add(DNA.G);
            var nucleotideStrings = new List<NucleotideString>
            {
                new NucleotideString(dnaString1),
                new NucleotideString(dnaString2),
            };

            Should.Throw<ArgumentOutOfRangeException>(async () =>
                await nucleotideStrings.GetMostDifferentFromNucleotideSet());
        }


        [TestMethod]
        public async Task GetMostDifferentFromNucleotideSetShouldReturnCorrect()
        {
            var expectedDnaList = new List<DNA>();
            expectedDnaList.Add(DNA.G);
            expectedDnaList.Add(DNA.T);
            expectedDnaList.Add(DNA.A);
            expectedDnaList.Add(DNA.A);
            var expected = new NucleotideString(expectedDnaList);

            var dnaString1 = new List<DNA>();
            dnaString1.Add(DNA.A);
            dnaString1.Add(DNA.C);
            dnaString1.Add(DNA.T);
            dnaString1.Add(DNA.G);

            var dnaString2 = new List<DNA>();
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.A);
            dnaString2.Add(DNA.C);
            dnaString2.Add(DNA.C);

            var dnaString3 = new List<DNA>();
            dnaString3.Add(DNA.T);
            dnaString3.Add(DNA.A);
            dnaString3.Add(DNA.G);
            dnaString3.Add(DNA.C);

            var nucleotideStrings = new List<NucleotideString>
            {
                new NucleotideString(dnaString1),
                new NucleotideString(dnaString2),
                new NucleotideString(dnaString3),
            };

            var actual = await nucleotideStrings.GetMostDifferentFromNucleotideSet();
            
            actual.Count().ShouldBe(expected.Count());
            actual.ShouldBe(expected);
        }
    }
}
