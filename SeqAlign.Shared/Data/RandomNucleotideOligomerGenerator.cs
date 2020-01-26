using SeqAlign.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeqAlign.Shared.Data
{
    public static class RandomNucleotideOligomerGenerator
    {
        public static Random rng = new Random();
        public static DNA Next => (DNA)rng.Next(1, 5);

        public static Task<IEnumerable<NucleotideString>> RandomSetOfNuclotides(int wordLength, int numberOfWords)
        {
            var dnaWords = new List<NucleotideString>(numberOfWords);
            for (int i = 0; i < numberOfWords; i++)
            {
                var word = new List<DNA>();

                for (int j = 0; j < wordLength; j++)
                {
                    word.Add(Next);
                }

                dnaWords.Add(new NucleotideString(word));
            }

            return Task.FromResult<IEnumerable<NucleotideString>>(dnaWords.OrderBy(x => x.ToString()));
        }
    }

}
