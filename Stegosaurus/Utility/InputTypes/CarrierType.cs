namespace Stegosaurus.Utility.InputTypes
{
    public class CarrierType : IInputType
    {
        public string FilePath { get; set; }

        public CarrierType(string _filePath)
        {
            FilePath = _filePath;
        }
        
    }
}
