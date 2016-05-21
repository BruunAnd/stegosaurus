namespace StegosaurusGUI.Utility
{
    internal class ContentType : IInputType
    {
        public string FilePath { get; set; }

        public ContentType(string _filePath)
        {
            FilePath = _filePath;
        }
    }
}
