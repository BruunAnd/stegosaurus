namespace Stegosaurus.Exceptions
{
    public class InvalidCarrierFileException : StegosaurusException
    {
        public string FileName { get; }

        public InvalidCarrierFileException(string _message, string _fileName)
            : base(_message)
        {
            FileName = _fileName;
        }
    }
}
