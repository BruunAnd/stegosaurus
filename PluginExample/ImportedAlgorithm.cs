using Stegosaurus.Algorithm;
using System;
using Stegosaurus;
using System.Threading;
using Stegosaurus.Exceptions;

namespace PluginExample
{
    public class ImportedAlgorithm : StegoAlgorithmBase
    {
        public override string Name => "Imported Algorithm";

        protected override byte[] Signature => new byte[] { 0 };

        public override long ComputeBandwidth()
        {
            return 0;
        }

        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            throw new StegoAlgorithmException("Embedding has not been implemented.");
        }

        public override StegoMessage Extract()
        {
            throw new StegoAlgorithmException("Extraction has not been implemented.");
        }
    }
}
