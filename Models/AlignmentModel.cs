using SeqAlign.Models;
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
        public string FilePath { get; set; }
        public ICollection<string> ClustalOSequences => RawSequences.ToClustalOSequences();
        public ICollection<string> RawSequences { get; set; } = new List<string>();
        public ICollection<AlignedSequence> AlignedSequences => 
            ClustalOSequences.Any() ? GetClustalWOlignment() : new List<AlignedSequence>();
        public string AlignmentError { get; set; }

        private ICollection<AlignedSequence> GetClustalWOlignment()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = Directory.GetCurrentDirectory() 
                + "/clustal-omega-1.2.2-win64/clustalo.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-i " + Directory.GetCurrentDirectory() + "/TestFolder/" + FileName;

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
                AlignmentError = "Failed to start ClustalO process! It returned an error:" + e.Message + e.InnerException?.Message;
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
            var trimmedSequence =
                rawSequence
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
        var pattern = @">"; //.+\r\n[A-Z-]+\r\n
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

