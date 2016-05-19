using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stegosaurus.Algorithm.GraphTheory;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using Sample = Stegosaurus.Algorithm.CommonSample.Sample;

namespace Stegosaurus.Algorithm
{
    class NewVertex
    {
        public Sample[] Samples;

        public bool Active { get; set; }

        public byte Value { get; set; }

        public byte Target { get; set; }

        public NewVertex(Sample[] _samples)
        {
            Samples = _samples;
        }

        public byte ModSum(byte _modValue)
        {
            return (byte) (Samples.Sum(s => s.ModValue) & _modValue);
        }

        public byte GetTargetValue(int _index)
        {
            return Samples[_index].TargetModValue;
        }

    }

    public class GTARewrite : StegoAlgorithmBase
    {
        public override string Name => "GTA Rewrite";

        protected override byte[] Signature => new byte[]{ 0x47, 0x54, 0x41, 0x6C };

        public int SamplesPerVertex { get; set; } = 3;

        public int BitsPerVertex { get; set; } = 2;

        private byte modFactor, bitwiseModFactor;

        private Tuple<List<NewVertex>, List<NewVertex>> GetVerticeLists(List<Sample> _sampleList, List<byte> _messageChunks)
        {
            // Find the total amount of vertices in selected carrier.
            int totalNumVertices = _sampleList.Count / SamplesPerVertex;

            // Allocate memory for vertices to contain messages.
            // May allocate more memory than necessary, since some vertices already have the correct value.
            List<NewVertex> messageVertices = new List<NewVertex>(_messageChunks.Count);
            List<NewVertex> reserveVertices = new List<NewVertex>(totalNumVertices - _messageChunks.Count);

            // Iterate through the amount of items to generate.
            RandomNumberList randomNumbers = new RandomNumberList(Seed, _sampleList.Count);
            for (int i = 0; i < totalNumVertices; i++)
            {
                Sample[] tmpSampleArray = new Sample[SamplesPerVertex];

                // Generate SamplesPerVertex items.
                for (int j = 0; j < SamplesPerVertex; j++)
                {
                    tmpSampleArray[j] = _sampleList[randomNumbers.Next];
                }

                // Instantiate vertex.
                // Todo: move ModSdum somewhere else so no object is created if not needed.
                NewVertex vertex = new NewVertex(tmpSampleArray);
                vertex.Value = vertex.ModSum(bitwiseModFactor);

                // If index is more or equal to amount of message, add to reserves.
                // Otherwise - if vertex has a wrong value - add it to message vertices.
                if (i >= _messageChunks.Count)
                {
                    reserveVertices.Add(vertex);
                }
                else if (vertex.Value != _messageChunks[i])
                {
                    messageVertices.Add(vertex);

                    // Calculate delta value (todo: forklar mig).
                    byte deltaValue = (byte)((modFactor - _messageChunks[i] + vertex.Value) & bitwiseModFactor);

                    // Set target value for every sample.
                    foreach (Sample sample in vertex.Samples)
                    {
                        sample.TargetModValue = (byte)((sample.ModValue + deltaValue) & bitwiseModFactor);
                    }

                    // Set vertex target value.
                    vertex.Target = _messageChunks[i];
                }
            }

            return new Tuple<List<NewVertex>, List<NewVertex>>(messageVertices, reserveVertices);
        }

        private List<byte> GetMessageChunks(StegoMessage _message)
        {
            // Combine signature with message and convert to BitArray.
            BitArray messageBitArray = new BitArray(Signature.Concat(_message.ToByteArray(CryptoProvider)).ToArray());

            // Prepare list of bytes.
            int numMessageChunks = messageBitArray.Length / BitsPerVertex;
            List<byte> messageChunklist = new List<byte>(numMessageChunks);

            // Insert each chunk.
            for (int i = 0; i < numMessageChunks; i++)
            {
                // Find current chunk value.
                byte messageValue = 0;
                for (int byteIndex = 0; byteIndex < BitsPerVertex; byteIndex++)
                {
                    messageValue += messageBitArray[i * BitsPerVertex + byteIndex] ? (byte)(1 << byteIndex) : (byte)0;
                }

                // Add chunk to list.
                messageChunklist.Add(messageValue);
            }

            return messageChunklist;
        }

        private void Encode(List<Sample> _sampleList)
        {
            int pos = 0;
            foreach (byte sample in _sampleList.SelectMany(current => current.Values))
            {
                CarrierMedia.ByteArray[pos++] = sample;
            }
        }

        private void Adjust(List<NewVertex> _vertices)
        {
            Random rand = new Random();

            foreach (NewVertex vertex in _vertices)
            {
                int sampleIndex = rand.Next(SamplesPerVertex), byteIndex = rand.Next(CarrierMedia.BytesPerSample);

                // Calculate difference.
                byte valueDifference = (byte)((modFactor + vertex.Samples[sampleIndex].ModValue - vertex.Samples[sampleIndex].TargetModValue) & bitwiseModFactor);

                // Adjust value.
                byte currentValue = vertex.Samples[sampleIndex].Values[byteIndex];
                if ((currentValue + valueDifference) <= byte.MaxValue)
                {
                    vertex.Samples[sampleIndex].Values[byteIndex] += valueDifference;
                }
                else
                {
                    vertex.Samples[sampleIndex].Values[byteIndex] -= (byte)(modFactor - valueDifference);
                }
            }
        }

        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            modFactor = (byte)(1 << BitsPerVertex);
            bitwiseModFactor = (byte)(modFactor - 1);

            // Get a list of all samples.
            List<Sample> sampleList = Sample.GetSampleListFrom(CarrierMedia, bitwiseModFactor);

            // Get message chunks.
            List<byte> messageChunks = GetMessageChunks(_message);
            Console.WriteLine("Message Chunks: {0}", messageChunks.Count);

            // Generate list of vertices.
            // One list contains the vertices that will have to be changed.
            // Other list is used for reserves, i.e. vertices that can be swapped.
            Tuple<List<NewVertex>, List<NewVertex>> verticleTuple = GetVerticeLists(sampleList, messageChunks);
            List<NewVertex> messageVertices = verticleTuple.Item1;
            List<NewVertex> reserveVertices = verticleTuple.Item2;
            Console.WriteLine("{0} message vertices, {1} reserved", messageVertices.Count, reserveVertices.Count);

            // Find edges 
            /*List<NewVertex> unexposed = new List<NewVertex>();
            foreach (NewVertex outerVertex in messageVertices)
            {
                if (outerVertex.Active)
                    continue;
                bool match = false;
                foreach (NewVertex innerVertex in messageVertices)
                {
                    if (innerVertex.Active)
                        continue;

                    if (outerVertex == innerVertex)
                    {
                        continue;
                    }

                    for (int i = 0; i < outerVertex.Samples.Length; i++)
                    {
                        if (outerVertex.Samples[i].TargetModValue == innerVertex.Samples[i].ModValue && outerVertex.Samples[i].ModValue == innerVertex.Samples[i].TargetModValue)
                        {
                            innerVertex.Samples[i].Swap(outerVertex.Samples[i]);
                            innerVertex.Samples[i].UpdateModValue(bitwiseModFactor);
                            outerVertex.Samples[i].UpdateModValue(bitwiseModFactor);
                            if (innerVertex.Samples[i].TargetModValue != innerVertex.Samples[i].ModValue)
                                throw new Exception("gg1");
                            if (outerVertex.Samples[i].TargetModValue != outerVertex.Samples[i].ModValue)
                                throw new Exception("gg2");
                            match = true;
                            innerVertex.Active = true;
                            outerVertex.Active = true;
                            break;
                        }
                    }

                    if (match)
                    {
                        byte innerval = innerVertex.ModSum(bitwiseModFactor);
                        byte outerval = outerVertex.ModSum(bitwiseModFactor);
                        if (innerval != innerVertex.Target)
                            throw new Exception("inner no match");
                        if (outerval != outerVertex.Target)
                            throw new Exception("outer no match");
                        break;
                    }
                }

                if (!match)
                    unexposed.Add(outerVertex);
            }*/

            // Swap edges

            // Adjust unexposed vertices
            // Console.WriteLine("adjust {0}", unexposed.Count);
            Adjust(messageVertices);

            // Encode samples back into carrier.
            Encode(sampleList);
        }


        // todo: make prettier
        private byte[] ReadBytes(RandomNumberList _numberList, int _count)
        {
            BitArray tempBitArray = new BitArray(_count * 8);
            int bps = CarrierMedia.BytesPerSample;
            int bytesPerVertex = bps * SamplesPerVertex;
            int numVertices = _count * 8 / BitsPerVertex;
            int tempValue = 0;
            int byteIndexOffset, bitIndexOffset;

            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                bitIndexOffset = vertexIndex * BitsPerVertex;
                tempValue = 0;
                for (int sampleIndex = 0; sampleIndex < SamplesPerVertex; sampleIndex++)
                {
                    byteIndexOffset = _numberList.Next * bps;
                    for (int byteIndex = 0; byteIndex < bps; byteIndex++)
                    {
                        tempValue += CarrierMedia.ByteArray[byteIndexOffset + byteIndex];
                    }
                }
                tempValue = tempValue & bitwiseModFactor;
                for (int bitIndex = 0; bitIndex < BitsPerVertex; bitIndex++)
                {
                    tempBitArray[bitIndexOffset + bitIndex] = ((tempValue & (1 << bitIndex)) != 0);
                }
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }

        public override StegoMessage Extract()
        {
            modFactor = (byte)(1 << BitsPerVertex);
            bitwiseModFactor = (byte)(modFactor - 1);

            int numSamples = CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample;
            // Generate random numbers
            RandomNumberList randomNumbers = new RandomNumberList(Seed, numSamples);

            // Read bytes and verify GraphTheorySignature
            if (!ReadBytes(randomNumbers, Signature.Length).SequenceEqual(Signature))
            {
                throw new StegoAlgorithmException("Signature is invalid, possibly using a wrong key.");
            }

            // Read length
            int length = BitConverter.ToInt32(ReadBytes(randomNumbers, 4), 0);

            // Read data and return StegoMessage instance
            return new StegoMessage(ReadBytes(randomNumbers, length), CryptoProvider);
        }

        public override long ComputeBandwidth()
        {
            return (CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample / SamplesPerVertex / 8) - Signature.Length;
        }
    }
}
