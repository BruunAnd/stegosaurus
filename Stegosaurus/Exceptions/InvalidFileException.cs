namespace Stegosaurus.Exceptions
{
    public class InvalidFileException : StegosaurusException
    {
        /// <summary>
        /// Get the file name of the invalid file
        /// </summary>
        public string FileName
        {
            get;
        }

        public InvalidFileException(string _message, string _fileName)
            : base(_message)
        {
            FileName = _fileName;
        }
    }
}
