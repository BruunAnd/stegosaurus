using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stegosaurus.Utility
{
    public static class IconExtractor
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public const int FILE_ATTRIBUTE_NORMAL = 0x80;
        public const int SHGFI_ICON = 0x000000100;
        public const int SHGFI_USEFILEATTRIBUTES = 0x000000010;
        public const int SHGFI_SMALLICON = 0x000000001;
  
        public static Icon ExtractIcon(string _extension)
        {
            SHFILEINFO fileInfo = new SHFILEINFO();

            // Set flags
            uint uFlags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;
            uFlags += SHGFI_SMALLICON;

            // Call native GetFileInfo method
            SHGetFileInfo(_extension, FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint) Marshal.SizeOf(fileInfo), uFlags);

            // Get icon from handle and clone, destroy handle afterwards
            Icon extractedIcon = (Icon) Icon.FromHandle(fileInfo.hIcon).Clone();
            DestroyIcon(fileInfo.hIcon);

            return extractedIcon;
        }
    }
}
