using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeqAlign.Shared.Extensions
{
    public static class CharExt
    {
        public static string GetColorValue(this char character)
        {
            switch (character)
            {
                case 'A':
                    return "red";
                case 'C':
                    return "blue";
                case 'T':
                    return "darkred";
                case 'G':
                    return "darkblue";
                default:
                    return "white";
            }
        }
    }
}
