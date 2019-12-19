using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeqAlign.Utilities
{
    public static class DirectoryExtensions
    {
        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
        }
    }
}
