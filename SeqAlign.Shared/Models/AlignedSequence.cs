using System;
using System.Text.RegularExpressions;

namespace SeqAlign.Shared.Models
{
    public class AlignedSequence
    {
        public string Name { get; set; }
        public string Sequence { get; set; }

        public AlignedSequence(string s)
        {
            var namePattern = @"^.*" + Environment.NewLine; //.+\r\n[A-Z-]+\r\n
            Name = Regex.Match(s, namePattern).Value.TrimEnd(Environment.NewLine.ToCharArray());
            Sequence = Regex.Replace(s, namePattern, "").Replace(Environment.NewLine, "");
        }
    }
}