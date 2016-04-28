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
        [ExpectedException(typeof(InvalidFileException))]
        public void TestInvalidFileException()
        {
            new InputFile(Guid.NewGuid().ToString());
        }
    }
}
