namespace Stegosaurus.Utility.InputTypes
{
    public class ContentType : IInputType
    {
        public string FilePath { get; set; }

        public ContentType(string _filePath)
        {
            FilePath = _filePath;
        }
    }
}
