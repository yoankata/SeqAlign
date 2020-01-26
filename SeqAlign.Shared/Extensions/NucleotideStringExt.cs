using SeqAlign.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeqAlign.Shared.Extensions
{
    public static class NucleotideStringExt
    {
        public static async Task<NucleotideString> GetMostDifferentFromNucleotideSet(this IEnumerable<NucleotideString> nucleotideStringSet)
        {
            var dnaOccurenceDictionary = await nucleotideStringSet.GetOccurenceDictionary();
            var strLength = nucleotideStringSet.First().Count();
            var mostDifferentDNALetters = new List<DNA>(strLength);
            foreach (var position in dnaOccurenceDictionary)
            {
                // get the non-occuring DNA letters
                var nonPresentDNALetters = Enum.GetValues(typeof(DNA)).Cast<DNA>().ToList().Except(position.Keys);

                if (nonPresentDNALetters.Any())
                    mostDifferentDNALetters.Add(nonPresentDNALetters.First());
                else
                    mostDifferentDNALetters.Add(position.OrderBy(l => l.Value).First().Key);
            }

            return new NucleotideString(mostDifferentDNALetters);
        }

        public static Task<List<Dictionary<DNA, uint>>> GetOccurenceDictionary(this IEnumerable<NucleotideString> nucleotideStringSet)
        {
            if (nucleotideStringSet is null || !nucleotideStringSet.Any())
                throw new ArgumentNullException("Nucleotide string set cannot be null or empty!");

            var letterCount = nucleotideStringSet.First().Count();
            var dictionaryOfOccurence = new List<Dictionary<DNA, uint>>(letterCount);
            for (var i = 0; i < letterCount; i++)
            {
                dictionaryOfOccurence.Add(new Dictionary<DNA, uint>());
            }

            for (var position = 0; position < letterCount; position++)
            {
                foreach (var str in nucleotideStringSet)
                {
                    if (dictionaryOfOccurence[position].ContainsKey(str[position]))
                    {
                        dictionaryOfOccurence[position][str[position]]++;
                    }
                    else
                    {
                        dictionaryOfOccurence[position].Add(str[position], 1);
                    }
                }
            }

            return Task.FromResult<List<Dictionary<DNA, uint>>>(dictionaryOfOccurence);
        }

        public static NucleotideString Complement(this NucleotideString ns)
        {
            var complement = new List<DNA>(ns.Count());
            for (var l = 0; l < ns.Count(); l++)
            {
                complement.Add(ns[l].Complement());
            }
            return new NucleotideString(complement);
        }
        public static DNA Complement(this DNA n)
        {
            switch (n)
            {
                case DNA.A:
                    return DNA.T;

                case DNA.C:
                    return DNA.G;

                case DNA.G:
                    return DNA.C;

                case DNA.T:
                    return DNA.A;

                default:
                    throw new ArgumentOutOfRangeException($"Unrecognized input nucleotide: {n}.");
            }
        }
    }
}
