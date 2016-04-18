using Stegosaurus.Exceptions;
using System.IO;

namespace Stegosaurus
{
    public class InputFile
    {
        public string Name { get; }
        public byte[] Content { get; }

        public InputFile(string _filePath)
        {
            FileInfo fileInfo = new FileInfo(_filePath);
            if (!fileInfo.Exists)
                throw new InvalidFileException("File does not exist.", _filePath);

            // Set name
            Name = fileInfo.Name;

            // Set content
            Content = File.ReadAllBytes(_filePath);
        }
    }
}
