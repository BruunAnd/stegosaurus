namespace Stegosaurus.Exceptions
{
    public class InvalidWaveFileException : InvalidCarrierFileException
    {
        public InvalidWaveFileException(string _message, string _fileName)
            : base($"WAVE file was invalid. {_message}", _fileName)
        {
        }
    }
}
