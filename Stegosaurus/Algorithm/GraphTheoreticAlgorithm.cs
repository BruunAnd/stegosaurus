using System;
using System.Collections.Generic;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm.GraphTheory;
using System.Collections;
using System.Linq;
using System.Text;
using Stegosaurus.Utility;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : IStegoAlgorithm
    {

        private static readonly byte[] GraphTheorySignature = { 0x12, 0x34, 0x56, 078 };
        public ICryptoProvider CryptoProvider { get; set; }

        private int samplesPerVertex => 2;
        private int messageBitsPerVertex => 2;
        
        private List<Vertex> vertices = new List<Vertex>();
        private List<Vertex> reserveVertices = new List<Vertex>();
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
            return ((((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / samplesPerVertex) * messageBitsPerVertex ) / 8) - GraphTheorySignature.Length;
        }

        private List<byte> messageHunks;

        public void Embed(StegoMessage _message)
        {
            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(GraphTheorySignature.Concat(messageArray).ToArray());
            messageHunks = new List<byte>();
            int len = messageInBits.Length / messageBitsPerVertex;
            int index = 0, a = 0;
            byte messageHunk = 0;
            while (index < len)
            {
                messageHunk = new byte();
                a = index++ * messageBitsPerVertex;
                for (int i = 0; i < messageBitsPerVertex; i++)
                {
                    messageHunk += messageInBits[a + i] ? (byte)Math.Pow(2, i) : (byte)0;
                }
                messageHunks.Add(messageHunk);
            }

            GetSamples();

            // Generate random sequence of integers
            IEnumerable<int> numberList = new RandomNumberList(Seed, samples.Count);

            GetVertices(numberList, messageInBits);
            GetEdges();

            foreach (Edge item in edges)
            {
                Console.WriteLine($"{item.ToString()}");
            }

        }

        public StegoMessage Extract()
        {
            throw new NotImplementedException();
        }

        private List<Sample> samples = new List<Sample>();
        private void GetSamples()
        {
            byte[] sample = new byte[CarrierMedia.BytesPerSample];
            long numSamples = CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample;
            long len = CarrierMedia.ByteArray.Length;
            int modFactor = (int)Math.Pow(2, messageBitsPerVertex);
            Sample tempSample;
            for (long i = 0; i < len; i += CarrierMedia.BytesPerSample)
            {
                for (int j = 0; j < CarrierMedia.BytesPerSample; j++)
                {
                    sample[j] = CarrierMedia.ByteArray[i + j];
                }
                tempSample = new Sample(sample);
                int value = 0;
                foreach (byte item in sample)
                {
                    value += (int)item;
                }
                tempSample.Value = value % modFactor;
                samples.Add(tempSample);
            }
        }

        private void GetVertices(IEnumerable<int> _numberList, BitArray _messageInBits)
        {

            int len = samples.Count / samplesPerVertex;
            Vertex temp;
            List<Sample> sampleList;
            int modFactor = (int)Math.Pow(2, messageBitsPerVertex);
            int value = 0;
            for (int i = 0; i < len; i++)
            {
                sampleList = new List<Sample>();
                value = 0;
                for (int index = 0; index < samplesPerVertex; index++)
                {
                    sampleList.Add(samples[_numberList.First()]);
                    value += sampleList[index].Value;
                }
                value %= modFactor;

                temp = new Vertex(sampleList);
                temp.Value = value;

                int dif;
                int mlen = messageHunks.Count;
                if (i >= mlen)
                {
                    foreach (Sample item in temp.Samples)
                    {
                        item.TargetValue = -1;
                    }
                    reserveVertices.Add(temp);
                }
                else if (temp.Value != messageHunks[i])
                {
                    dif = messageHunks[i] - value;
                    foreach (Sample item in temp.Samples)
                    {
                        item.TargetValue = (item.Value + (dif + modFactor)) % modFactor;
                    }
                    vertices.Add(temp);
                }
            }

            
        }

        private void GetEdges()
        {
            int numVertices = vertices.Count;
            Vertex vertexA, vertexB;
            bool isEdge = false;

            for (int i = 0; i < (numVertices - 1); i++)
            {
                vertexA = vertices[i];
                for (int j = i + 1 ; j < numVertices; j++)
                {
                    vertexB = vertices[j];
                    isEdge = false;

                    if (GetDistance(vertexA, vertexB) <= 80)
                    {
                        edges.Add(new Edge(vertexA, vertexB));
                    }

                }
            }
        }
        private long GetDistance(Vertex _vertexA, Vertex _vertexB)
        {
            int bps = CarrierMedia.BytesPerSample;
            long distance = 0;
            long minDistance = 100;
            int temp;
            for (int k = 0; k < samplesPerVertex; k++)
            {
                for (int l = 0; l < samplesPerVertex; l++)
                {
                    if (_vertexA.Samples[k].TargetValue == _vertexB.Samples[l].Value && _vertexA.Samples[k].Value == _vertexB.Samples[l].TargetValue)
                    {
                        distance = 0;
                        for (int i = 0; i < bps; i++)
                        {
                            temp = (_vertexA.Samples[k].Bytes[i] - _vertexB.Samples[l].Bytes[i]);
                            distance += temp * temp;
                        }
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                    

                }
            }
            return distance;
        }

    }
}
