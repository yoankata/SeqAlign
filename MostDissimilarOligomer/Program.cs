using SeqAlign.Shared.Data;
using static SeqAlign.Shared.Data.RandomNucleotideOligomerGenerator;
using SeqAlign.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeqAlign.Shared.Extensions;

namespace MostDissimilarOligomer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var oligomerLength = 6;
            var setSize = 400;
            Console.WriteLine("Hello, Bioinformaticians!");
            Console.WriteLine("Generating random nucleotide input set...");
            var nucleotideSet = await RandomSetOfNuclotides(oligomerLength, setSize);
            Console.WriteLine("Done generating input!\n\n");

            foreach (var oligomer in nucleotideSet)
            {
                Console.WriteLine($"{oligomer}");
            }

            var occurenceSet = await nucleotideSet.GetOccurenceDictionary();

            Console.WriteLine("\n\nOccurence summary:");

            for (var pos = 0; pos < occurenceSet.Count; pos++)
            {
                Console.Write($"Position {pos}. ");
                foreach (var dic in occurenceSet[pos] )
                {
                    Console.Write($"{dic.Key}: {dic.Value}\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            var mostDissimilarNucleotideString = await nucleotideSet.GetMostDifferentFromNucleotideSet();
            Console.WriteLine($"The most dissimilar nucleotide oligomer is: {mostDissimilarNucleotideString}");
            
            var nucleotideComplement = mostDissimilarNucleotideString.Complement();
            Console.WriteLine($"Its complement is: {nucleotideComplement}");

            Console.ReadKey();
        }
    }
}
