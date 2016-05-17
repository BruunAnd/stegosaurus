namespace Stegosaurus.Utility
{
    public static class SizeFormatter
    {
        /// <summary>
        /// Returns a formatted string from a byte count.
        /// </summary>
        public static string StringFormatBytes(long _byteCount)
        {
            string[] suffixes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB" };
            int logIndex;
            const int order = 1024;
            decimal decByteCount = _byteCount;

            for (logIndex = 0; decByteCount >= order || decByteCount <= -order; logIndex++)
            {
                decByteCount /= order;
            }

            return $"{decByteCount:0.##} {suffixes[logIndex]}";
        }
    }
}
