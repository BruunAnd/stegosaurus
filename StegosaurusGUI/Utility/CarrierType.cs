namespace StegosaurusGUI.Utility
{
    internal class CarrierType : IInputType
    {
        public string FilePath { get; set; }

        public CarrierType(string _filePath)
        {
            FilePath = _filePath;
        }
    }
}
