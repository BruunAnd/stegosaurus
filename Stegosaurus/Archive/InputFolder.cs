using System.Collections.Generic;
using System.IO;
using Stegosaurus.Utility.Extensions;
using System.Linq;
using System;
using System.CodeDom;
using System.Runtime.CompilerServices;

namespace Stegosaurus.Archive
{
    public class InputFolder : ArchiveItem
    {
        public InputFolder Parent { get; set; }
        public List<ArchiveItem> Items = new List<ArchiveItem>();

        public InputFolder(string _name)
        {
            Name = _name;
        }

        public void AddDirectory(string _name)
        {
            InputFolder newFolder = new InputFolder(_name) {Parent = this};
            Items.Add(newFolder);
        }

        public override void WriteToStream(Stream _stream)
        {
            // Write name of folder.
            _stream.Write(Name);

            // Write folders.
            var folders = Items.FindAll(i => i is InputFolder).ToList();
            _stream.Write(folders.Count);
            folders.ForEach(f => f.WriteToStream(_stream));

            // Write files.
            var files = Items.FindAll(i => i is InputFile).ToList();
            _stream.Write(files.Count);
            files.ForEach(f => f.WriteToStream(_stream));
        }

        public static InputFolder FromStream(Stream stream)
        {
            InputFolder folder = new InputFolder(stream.ReadString());

            // Read amount of folders.
            int amountInputFolders = stream.ReadInt();

            // Read folders.
            for (int i = 0; i < amountInputFolders; i++)
            {
                InputFolder readFolder = FromStream(stream);
                readFolder.Parent = folder;
                folder.Items.Add(readFolder);
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
