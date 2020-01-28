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
                    return "lightcoral";
                case 'C':
                    return "lightblue";
                case 'T':
                    return "lightpink";
                case 'G':
                    return "lightskyblue";
                default:
                    return "lavendarblush";
            }
        }
    }
}
