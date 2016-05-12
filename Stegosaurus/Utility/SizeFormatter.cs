using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Utility
{
    public static class SizeFormatter
    {
        /// <summary>
        /// Returns a formatted string from a byte count.
        /// </summary>
        public static string StringFormatBytes(long byteCount)
        {
            string[] suffixes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB" };
            int logIndex = 0;
            const int order = 1024;
            decimal decByteCount = byteCount;

            for (logIndex = 0; decByteCount >= order || decByteCount <= -order; logIndex++)
            {
                decByteCount /= order;
            }

            return $"{decByteCount:0.##} {suffixes[logIndex]}";
        }
    }
}
