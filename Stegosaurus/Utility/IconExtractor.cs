using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stegosaurus.Utility
{
    public static class IconExtractor
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string _pszPath, uint _dwFileAttributes, ref Shfileinfo _psfi, uint _cbFileInfo, uint _uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr _hIcon);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct Shfileinfo
        {
            public readonly IntPtr hIcon;
            private readonly int iIcon;
            private readonly uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            private readonly string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            private readonly string szTypeName;
        }

        public const int FileAttributeNormal = 0x80;
        public const int ShgfiIcon = 0x000000100;
        public const int ShgfiUsefileattributes = 0x000000010;
        public const int ShgfiSmallicon = 0x000000001;
  
        /// <summary>
        /// Returns an instance of Icon using a file extension.
        /// </summary>
        public static Icon ExtractIcon(string _extension)
        {
            Shfileinfo fileInfo = new Shfileinfo();

            // Set flags.
            uint uFlags = ShgfiIcon | ShgfiUsefileattributes;
            uFlags += ShgfiSmallicon;

            // Call native GetFileInfo method.
            SHGetFileInfo(_extension, FileAttributeNormal, ref fileInfo, (uint) Marshal.SizeOf(fileInfo), uFlags);

            // Get icon from handle and clone, destroy handle afterwards.
            Icon extractedIcon = (Icon) Icon.FromHandle(fileInfo.hIcon).Clone();
            DestroyIcon(fileInfo.hIcon);

            return extractedIcon;
        }
    }
}
