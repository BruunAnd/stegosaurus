using Stegosaurus.Exceptions;
using System.IO;
using System;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Archive
{
    public class InputFile : ArchiveItem
    {

        /// <summary>
        /// A byte array containing the files contents.
        /// </summary>
        public byte[] Content { get; }

        /// <summary>
        /// A constructor to construct a InputFile from byte array.
        /// </summary>
        public InputFile(string _name, byte[] _content)
        {
            Name = _name;
            Content = _content;
        }

        /// <summary>
        /// A constructor to construct a InputFile from a file path.
        /// </summary>
        public InputFile(string _filePath)
        {
            FileInfo fileInfo = new FileInfo(_filePath);
            if (!fileInfo.Exists)
            {
                throw new InvalidCarrierFileException("File does not exist.", _filePath);
            }

            Name = fileInfo.Name;
            Content = File.ReadAllBytes(_filePath);
        }

        /// <summary>
        /// This method is used to save the file in the file system of the operating system.
        /// </summary>
        public override void SaveTo(string _destination)
        {
            File.WriteAllBytes(_destination, Content);
        }

        public override void WriteToStream(Stream _stream)
        {
            _stream.Write(this);
        }
    }
}
