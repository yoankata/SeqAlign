using SeqAlign.Models;
using SeqAlign.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static SequenceExt;

namespace SeqAlign.Models
{
    public class AlignmentModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory() + "/TestFolder/";
        public ICollection<string> ClustalOSequences { get; set; } = new List<string>();
        public ICollection<string> RawSequences { get; set; } = new List<string>();
        public ICollection<AlignedSequence> AlignedSequences { get; set; } = new List<AlignedSequence>();
        public string AlignmentError { get; set; }

        public AlignmentModel()
        {

        }
        public AlignmentModel(List<string> rawSequences, string fileName)
        {
            if (rawSequences is null || !rawSequences.Any())
                throw new ArgumentException("Raw Sequences must be provided!");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name must be provided!");

            FileName = fileName;
            RawSequences = rawSequences;
            ClustalOSequences = RawSequences.ToClustalOSequences();
            FileUtilities.WriteFileContents(ClustalOSequences, FileName);

            AlignedSequences = GetClustalWOlignment();

        }

        private ICollection<AlignedSequence> GetClustalWOlignment()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = Directory.GetCurrentDirectory() + "/clustal-omega-1.2.2-win64/clustalo.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "-i " + FilePath + FileName
            };
            
            try
            {
                using (var process = Process.Start(startInfo))
                {
                    var output = process.StandardOutput.ReadToEnd();                    
                    AlignmentError = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    return output?.SplitClustalOAlignmentSequences();
                }
            }
            catch(Exception e)
            {
                AlignmentError = "Failed to start ClustalO process! Fatal error:" + e.Message + e.InnerException?.Message;
                return null;
            }
        }
    }
}

public static class SequenceExt
{
    public static ICollection<string> ToClustalOSequences(this ICollection<string> rawSequences)
    {
        var clustalOSequences = new List<string>();
        foreach (var rawSequence in rawSequences)
        {
            var trimmedSequence =rawSequence
                .RemoveWhiteSpaces(); // cleanup spaces
            var noSpaces = trimmedSequence
                .ToClustalOLetters(); // translate digits to letters so clustal can handle htem
            var prepended = noSpaces.PrependMetaData();  // add > sign if missing

            clustalOSequences.Add(prepended);
        }

        return clustalOSequences;
    }

    public static string RemoveWhiteSpaces(this string rawSequence)
    {
        return Regex.Replace(rawSequence, @"\s+", ""); 
    }

    public static string PrependMetaData(this string rawSequence)
    {
        if (!rawSequence.Contains(">"))
            return Regex.Replace(rawSequence, @"^", $">{Math.Abs(rawSequence.GetHashCode()).ToString()}\n");

        return rawSequence;
    }

    public static string ToClustalOLetters(this string rawSequence)
    {
        for (int i = 0; i <= 9; i++)
        {
            var ch = i.ToString().First();
            rawSequence = rawSequence.Replace(ch, ch.ToClustalChar());
        }

        return rawSequence;
    }

    public static ICollection<AlignedSequence> SplitClustalOAlignmentSequences(this string rawSequence)
    {
        var pattern = @">";
        var sequences = Regex.Split(rawSequence, pattern, RegexOptions.Multiline);
        var splitSequences = sequences
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => new AlignedSequence(s))
            .ToHashSet();

        return splitSequences;
    }

    public enum ClustalOAlphabet
    {
        A = 0,
        C = 1,
        D = 2,
        E = 3,
        F = 4,
        G = 5,
        H = 6,
        I = 7,
        K = 8,
        L = 9
    }
}
public static class ClustalOAlphabetExt
{
    public static char ToClustalChar(this char ch)
    {
        if (!char.IsDigit(ch))
            throw new ArgumentOutOfRangeException($"{ch} needs to be a valid digit!");

        return ((ClustalOAlphabet)int.Parse(ch.ToString())).ToString().First();
    }
}

