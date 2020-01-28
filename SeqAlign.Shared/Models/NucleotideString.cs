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
        private List<DNA> _dnaString { get; set; }

        public NucleotideString(List<DNA> dna)
        {
            _dnaString = dna;
        }

        public int Count() => _dnaString.Count();
        public DNA this[int key]
        {
            get => _dnaString[key];
            set => _dnaString[key] = value;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                NucleotideString n = (NucleotideString)obj;
                return _dnaString.SequenceEqual(n._dnaString);
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("", _dnaString.Select(s => s.ToString()).ToArray());
        }

        public char[] ToCharArray()
        {
            var dnaArr = ToString().ToCharArray();
            return dnaArr;
        }
    }



}
