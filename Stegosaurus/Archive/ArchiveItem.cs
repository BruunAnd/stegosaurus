using System.IO;

namespace Stegosaurus.Archive
{
    public abstract class ArchiveItem
    {
        public string Name { get; protected set; }

        public abstract void WriteToStream(Stream _stream);
        public abstract void SaveTo(string _destination);
    }
}
