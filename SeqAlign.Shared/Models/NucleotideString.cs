using System;
using System.Collections.Generic;
using System.Linq;

namespace SeqAlign.Shared.Models
{
    public enum DNA
    {
        A = 1,
        C,
        T,
        G
    }

    public class NucleotideString
    {
        private List<DNA> _dnaLetters { get; set; }

        public NucleotideString(int length)
        {
            _dnaLetters = new List<DNA>(length);
        }
        public NucleotideString(List<DNA> dna)
        {
            _dnaLetters = dna;
        }

        public int Count() => _dnaLetters.Count();
        public DNA this[int key]
        {
            get => _dnaLetters[key];
            set => _dnaLetters[key] = value;
        }

        public override string ToString()
        {
            return string.Join("", _dnaLetters.Select(s => s.ToString()).ToArray());
        }

        public List<DNA> ToDNAList()
        {
            return new List<DNA>(_dnaLetters);
        }

        public char[] ToCharArray()
        {
            var dnaArr = ToString().ToCharArray();
            return dnaArr;
        }
    }



}
