using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus;
using Stegosaurus.Exceptions;
using System;

namespace StegosaurusTest
{
    [TestClass]
    public class InputFileTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCarrierFileException))]
        public void RandomInputFile_ThrowsInvalidFileException()
        {
            InputFile file = new InputFile(Guid.NewGuid().ToString());
        }
    }
}
