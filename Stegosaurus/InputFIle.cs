using Stegosaurus.Exceptions;
using System.IO;

namespace Stegosaurus
{
    public class InputFile
    {
        public string Name { get; }
        public byte[] Content { get; }

        public InputFile(string _name, byte[] _content)
        {
            Name = _name;
            Content = _content;
        }

        public InputFile(string _filePath)
        {
            FileInfo fileInfo = new FileInfo(_filePath);
            if (!fileInfo.Exists)
                throw new InvalidCarrierFileException("File does not exist.", _filePath);

            Name = fileInfo.Name;
            Content = File.ReadAllBytes(_filePath);
        }

        public void SaveTo(string _destination)
        {
            File.WriteAllBytes(_destination, Content);
        }
    }
}
