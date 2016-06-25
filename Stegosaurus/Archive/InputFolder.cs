using System.Collections.Generic;
using System.IO;
using Stegosaurus.Utility.Extensions;
using System.Linq;
using System;

namespace Stegosaurus.Archive
{
    public class InputFolder : ArchiveItem
    {
        public InputFolder Parent { get; private set; }
        public List<ArchiveItem> Items = new List<ArchiveItem>();

        public InputFolder(string _name)
        {
            Name = _name;
        }

        public override void WriteToStream(Stream _stream)
        {
            // Write name of folder.
            _stream.Write(Name);

            // Write folders.
            WriteTypeToStream(typeof(InputFolder), _stream);

            // Write files.
            WriteTypeToStream(typeof(InputFile), _stream);
        }

        private void WriteTypeToStream<T>(T _t, Stream _stream)
        {
            List<T> matchingItems = Items.OfType<T>().ToList();
            _stream.Write(matchingItems.Count);
            Console.WriteLine("write {0}/{1}", matchingItems.Count, Items.Count);
            matchingItems.ForEach(i => (i as ArchiveItem).WriteToStream(_stream));
        }

        public static InputFolder FromStream(Stream stream)
        {
            InputFolder folder = new InputFolder(stream.ReadString());

            // Read amount of folders.
            int amountInputFolders = stream.ReadInt();

            // Read folders.
            for (int i = 0; i < amountInputFolders; i++)
            {
                folder.Items.Add(FromStream(stream));
            }

            // Read amount of files.
            int amountInputFiles = stream.ReadInt();

            // Read files.
            for (int i = 0; i < amountInputFiles; i++)
            {
                folder.Items.Add(new InputFile(stream.ReadString(), stream.ReadBytes()));
            }

            return folder;
        }

        public override void SaveTo(string _destination)
        {
            Directory.CreateDirectory(_destination);

            // Save sub-items to combined destination.
            Items.ForEach(i => i.SaveTo(Path.Combine(_destination, i.Name)));
        }
    }
}
