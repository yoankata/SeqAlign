using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace SeqAlign.Models
{
    public class AlignedSequence
    {
        public string Name { get; set; }
        public string Sequence { get; set; }

        public AlignedSequence(string s)
        {
            var split = Regex.Split(s, Environment.NewLine);

            if (split.Length != 3) //3 to account for extra null string
                throw new ArgumentException("Irregular sequence from ClustalO!");

            Name = split[0];
            Sequence = split[1];
        }
    }
}