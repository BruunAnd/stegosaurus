using System;
using System.Collections.Generic;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm.GraphTheory;
using System.Collections;
using System.Linq;
using Stegosaurus.Utility;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : IStegoAlgorithm
    {

        private static readonly byte[] GraphTheorySignature = { 0x12, 0x34, 0x56, 078 };
        public ICryptoProvider CryptoProvider { get; set; }

        private int samplesPerVertex => 2;
        private int modFactor => 4;


        private byte[][] sampleBytes;
        private List<Vertex> vertices = new List<Vertex>();
        private List<Edge> edges = new List<Edge>();

        public ICarrierMedia CarrierMedia
        {
            get; set;
        }

        public string Name => "Graph Theoretic Algorithm";
        public string CryptoKey { get; set; }

        public int Seed => CryptoProvider?.Seed ?? 0;

        public long ComputeBandwidth()
        {
            throw new NotImplementedException();
        }

        public void Embed(StegoMessage _message)
        {
            // Combine LsbSignature with byteArray and convert to bitArray
            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(GraphTheorySignature.Concat(messageArray).ToArray());


            GetSamples();

            // Generate random sequence of integers
            IEnumerable<int> numberList = new RandomNumberList(Seed, samples.Count);

            GetVertices(numberList, messageInBits);


            GetEdges();


        }

        public StegoMessage Extract()
        {
            throw new NotImplementedException();
        }

        private List<Sample> samples = new List<Sample>();
        private void GetSamples()
        {
            byte[] sample = new byte[CarrierMedia.BytesPerSample];
            int numSamples = CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample;
            int len = CarrierMedia.ByteArray.Length;
            for (int i = 0; i < len; i += CarrierMedia.BytesPerSample)
            {
                for (int j = 0; j < CarrierMedia.BytesPerSample; j++)
                {
                    sample[j] = CarrierMedia.ByteArray[i + j];
                }
                samples.Add(new Sample(sample));
            }
        }

        private void GetVertices(IEnumerable<int> _numberList, BitArray _messageInBits)
        {
            int len = samples.Count / samplesPerVertex;
            int[] sampleIndices = new int[samplesPerVertex];
            int sampleLength = sampleIndices.Length;
            int messageIndex = 0;
            int TargetVal;
            List<Sample> sampleList;

            for (int i = 0; i < len; i++)
            {
                sampleList = new List<Sample>();
                for (int index = 0; index < samplesPerVertex; index++)
                {
                    sampleList.Add(samples[_numberList.First()]);
                }
                

                vertices.Add(new Vertex(sampleList));

            }

            
        }

        private void GetEdges()
        {
            throw new NotImplementedException();
        }


    }
}
