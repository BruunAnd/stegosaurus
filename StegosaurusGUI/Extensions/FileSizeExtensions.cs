using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Stegosaurus.Extensions
{
    public static class FileSizeExtensions
    {
        public static string StringFormatBytes(long byteCount)
        {
            string[] suffixes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB"};
            int logIndex = 0;
            const int order = 1024;
            decimal decByteCount = byteCount;
            
            for (logIndex = 0; decByteCount >= order || decByteCount <= -order; logIndex++)
            {
                decByteCount /= order;
            }

            return $"{decByteCount :0.##} {suffixes[logIndex]}";
        }
    }

}
