using SeqAlign.Models;
using SeqAlign.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeqAlign.Models
{
    public class AlignmentModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; } = Directory.GetCurrentDirectory() + "/TestFolder/";
        public ICollection<string> ClustalOSequences { get; set; } = new List<string>();
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
            ClustalOSequences = rawSequences;
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
                    return output?.SplitSequences();
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
    public static string RemoveWhiteSpaces(this string rawSequence)
    {
        return Regex.Replace(rawSequence, @"\s+", ""); 
    }

    public static ICollection<AlignedSequence> SplitSequences(this string rawSequence)
    {
        var pattern = @"(?=>)";
        var sequences = Regex.Split(rawSequence, pattern, RegexOptions.Multiline);
        var splitSequences = sequences
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => new AlignedSequence(s))
            .ToHashSet();

        return splitSequences;
    }
}

