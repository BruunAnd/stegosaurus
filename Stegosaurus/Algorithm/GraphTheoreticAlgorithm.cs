using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Stegosaurus.Utility;
using System.ComponentModel;
using System.Threading;
using Stegosaurus.Algorithm.GraphTheory;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : StegoAlgorithmBase
    {
        public override string Name => "Graph Theoretic Algorithm";

        protected override byte[] Signature => new byte[] { 0x47, 0x54, 0x41, 0x6C };

        private byte samplesPerVertex = 2;
        [Category("Algorithm"), Description("The number of samples collected in each vertex. Higher numbers means less bandwidth but more imperceptibility.(Default = 2, Max = 4.)")]
        public byte SamplesPerVertex
        {
            get { return samplesPerVertex; }
            set { samplesPerVertex = (value <= 4) ? ((value >= 1) ? value : (byte)1) : (byte)4; }
        }

        private byte messageBitsPerVertex = 2;
        [Category("Algorithm"), Description("The number of bits hidden in each vertex. Higher numbers means more bandwidth but less imperceptibility.(Default = 2, Max = 4.)")]
        public byte MessageBitsPerVertex
        {
            get { return messageBitsPerVertex; }
            set
            {
                byte temp = (byte)(1 << ((int)Math.Log(value, 2)));
                messageBitsPerVertex = (temp <= 4) ? ((temp >= 1) ? temp : (byte)1) : (byte)4;
            }
        }

        private int distanceMax = 8;
        [Category("Algorithm"), Description("The maximum distance between single samplevalues for an edge to be valid. Higher numbers means less visual imperceptibility but more statistical imperceptibility. Higher numbers might also decrease performance, depending on DistancePrecision. (Default = 32, Min-Max = 2-128.)")]
        public int DistanceMax
        {
            get { return distanceMax; }
            set { distanceMax = (value <= 128) ? ((value >= 2) ? value : 2) : 128; }
        }

        private int distancePrecision = 2;
        [Category("Algorithm"), Description("The distance precision. Higher numbers significantly decreases performance with high DistanceMax. (Default = 8, Min-Max = 2-32.)")]
        public int DistancePrecision
        {
            get { return 1 << distancePrecision; }
            set { distancePrecision = (value <= 32) ? ((value >= 0) ? (int)Math.Log(value, 2) : 0) : 32; }
        }

        private int verticesPerMatching = 50000;
        [Category("Algorithm"), Description("The maximum number of vertices to find edges for at a time. Higher numbers means more memory usage but better imperceptibility. (Default = 150,000, Min = 10,000.)")]
        public int VerticesPerMatching
        {
            get { return verticesPerMatching; }
            set { verticesPerMatching = (value >= 10000) ? value : 10000; }
        }

        private int progress, progressCounter, progressUpdateInterval;
        private byte modFactor;
        private byte bitwiseModFactor;

        public override long ComputeBandwidth()
        {
            return ((((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / samplesPerVertex) * messageBitsPerVertex) / 8) - Signature.Length;
        }

        #region Embed
        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            modFactor = (byte)(1 << messageBitsPerVertex);
            bitwiseModFactor = (byte)(modFactor - 1);
            progress = 0;
            progressCounter = 0;

            // Verify that the carrier is supported by the algorithm.
            if (CarrierMedia.BytesPerSample != 3)
            {
                throw new StegoAlgorithmException("The selected carrier is not supported by this algorithm.");
            }

            // Convert StegoMessage into chunks of bytes.
            List<byte> messageChunks = GetMessageChunks(_message);//GetMessage(_message, _progress, _ct, 10);

            // Convert bytes CarrierMedia to a list of Samples.
            List<Sample> sampleList = Sample.GetSampleListFrom(CarrierMedia, bitwiseModFactor);

            // Get Vertex lists.
            Tuple<List<Vertex>, List<Vertex>> verticeTuple = GetVerticeLists(sampleList, messageChunks);
            List<Vertex> messageVertexList = verticeTuple.Item1;
            // List<Vertex> reserveVertexList = verticeTuple.Item2;
            int messageVertexCount = messageVertexList.Count;

            // Find and swap edges.
            // Returned value is a list of vertices that could not be changed.
            List<Vertex> unmatchedVertexList = FindEdgesAndSwap(messageVertexList, _progress, _ct, 100);

            //DoReserveMatching();
            // Adjust vertices that could not be swapped.
            Adjust(unmatchedVertexList);
            Console.WriteLine("Adjusted {0} vertices ({1}% of total).", unmatchedVertexList.Count, (unmatchedVertexList.Count / (float)messageVertexCount) * 100);

            // Finally encode the samples back into the CarrierMedia.
            Encode(sampleList);
            _progress?.Report(100);
        }

        private void Encode(List<Sample> _sampleList)
        {
            int pos = 0;
            foreach (byte sample in _sampleList.SelectMany(current => current.Values))
            {
                CarrierMedia.ByteArray[pos++] = sample;
            }
        }

        /// <summary>
        /// Adjust a list of vertices, so all vertices have their target value.
        /// There is a randomly selected sample and byteIndex that will be edited for each vertex.
        /// </summary>
        private void Adjust(List<Vertex> _vertices)
        {
            Random rand = new Random();

            foreach (Vertex vertex in _vertices)
            {
                int sampleIndex = rand.Next(SamplesPerVertex), byteIndex = rand.Next(CarrierMedia.BytesPerSample);

                // Calculate difference.
                byte valueDifference = (byte)((modFactor - vertex.Samples[sampleIndex].ModValue + vertex.Samples[sampleIndex].TargetModValue) & bitwiseModFactor);

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

        /// <summary>
        /// Returns a Tuple containing two lists of vertices.
        /// The first list contains the vertices that have been assigned a target value.
        /// The second list contains vertices with no targets, that can be used as a reserve to exchange.
        /// </summary>
        private Tuple<List<Vertex>, List<Vertex>> GetVerticeLists(List<Sample> _sampleList, List<byte> _messageChunks)
        {
            // Find the total amount of vertices in selected carrier.
            int totalNumVertices = _sampleList.Count / SamplesPerVertex;

            // Allocate memory for vertices to contain messages.
            // May allocate more memory than necessary, since some vertices already have the correct value.
            List<Vertex> messageVertices = new List<Vertex>(_messageChunks.Count);
            List<Vertex> reserveVertices = new List<Vertex>(totalNumVertices - _messageChunks.Count);

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

                // Calculate mod value of vertex.
                byte vertexModValue = (byte)(tmpSampleArray.Sum(val => val.ModValue) & bitwiseModFactor);

                // If index is more or equal to amount of message, add to reserves.
                // Otherwise add to message vertices.
                if (i >= _messageChunks.Count)
                {
                    Vertex reserveVertex = new Vertex(tmpSampleArray) { Value = vertexModValue };
                    reserveVertices.Add(reserveVertex);
                }
                else
                {
                    Vertex messageVertex = new Vertex(tmpSampleArray) { Value = vertexModValue };
                    messageVertices.Add(messageVertex);

                    // Calculate delta value.
                    byte deltaValue = (byte)((modFactor + _messageChunks[i] - messageVertex.Value) & bitwiseModFactor);

                    // Set target values.
                    foreach (Sample sample in messageVertex.Samples)
                    {
                        sample.TargetModValue = (byte)((sample.ModValue + deltaValue) & bitwiseModFactor);
                    }
                }
            }

            return new Tuple<List<Vertex>, List<Vertex>>(messageVertices, reserveVertices);
        }

        private List<Vertex> FindEdgesAndSwap(List<Vertex> _vertices, IProgress<int> _progress, CancellationToken _ct, int _progressWeight)
        {
            int numRounds = (int)Math.Ceiling((decimal)_vertices.Count / VerticesPerMatching), roundProgressWeight = _progressWeight / numRounds;
            int verticesPerRound = _vertices.Count / numRounds + 1, maxCarryoverPerRound = VerticesPerMatching / 4;
            int verticeOffset = 0, startNumVertices = _vertices.Count;
            List<Vertex> leftoverVertexList = new List<Vertex>();

            // Continue until we have gone through all vertices.
            while (verticeOffset < startNumVertices)
            {
                //Console.WriteLine("Round: {0} ({1}/{2})", ++roundNumber, verticeOffset, startNumVertices);

                // Calculate how many vertices to use this round.
                int verticesThisRound = verticesPerRound > _vertices.Count ? _vertices.Count : verticesPerRound;
                verticeOffset += verticesThisRound;

                // Take this amount of vertices.
                List<Vertex> tmpVertexList = _vertices.GetRange(0, verticesThisRound);

                // Remove them from the main list.
                _vertices.RemoveRange(0, verticesThisRound);

                // Calculate how many leftover vertices to carry over.
                int leftoverCarryover = maxCarryoverPerRound > leftoverVertexList.Count ? leftoverVertexList.Count : maxCarryoverPerRound;

                // Add leftover vertices to tmpVertexList.
                tmpVertexList.AddRange(leftoverVertexList.GetRange(0, leftoverCarryover));

                // Remove the transfered vertices from the list.
                leftoverVertexList.RemoveRange(0, leftoverCarryover);

                // Get edges for subset.
                GetEdges(tmpVertexList, _progress, _ct, roundProgressWeight);

                // Swap edges found for subset and add leftovers to list.
                leftoverVertexList.AddRange(Swap(tmpVertexList));

                // Clear edges for subset.
                tmpVertexList.ForEach(v => v.Edges.Clear());
            }


            return leftoverVertexList;
        }

        private List<Tuple<int, byte>>[,,,,] GetArray(List<Vertex> _vertices, int _dimensionSize, CancellationToken _ct)
        {
            List<Tuple<int, byte>>[,,,,] array = new List<Tuple<int, byte>>[_dimensionSize, _dimensionSize, _dimensionSize, modFactor, modFactor];
            Tuple<int, byte> vertexRef;
            List<Tuple<int, byte>> vertexRefs;
            Sample sample;
            int numVertices = _vertices.Count;

            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                for (byte sampleIndex = 0; sampleIndex < samplesPerVertex; sampleIndex++)
                {
                    vertexRef = Tuple.Create(vertexIndex, sampleIndex);
                    sample = _vertices[vertexIndex].Samples[sampleIndex];
                    vertexRefs = array[sample.Values[0] >> distancePrecision, sample.Values[1] >> distancePrecision, sample.Values[2] >> distancePrecision, sample.ModValue, sample.TargetModValue];
                    if (vertexRefs != null)
                    {
                        vertexRefs.Add(vertexRef);
                    }
                    else
                    {
                        array[sample.Values[0] >> distancePrecision, sample.Values[1] >> distancePrecision, sample.Values[2] >> distancePrecision, sample.ModValue, sample.TargetModValue] = new List<Tuple<int, byte>>();
                        array[sample.Values[0] >> distancePrecision, sample.Values[1] >> distancePrecision, sample.Values[2] >> distancePrecision, sample.ModValue, sample.TargetModValue].Add(vertexRef);
                    }
                }
            }
            return array;
        }

        private void GetEdges(List<Vertex> _vertexList, IProgress<int> _progress, CancellationToken _ct, int _progressWeight)
        {
            //Console.WriteLine("Debug GetEdges:");
            int numVertices = _vertexList.Count;
            List<Tuple<int, byte>> vertexRefs;
            Vertex vertex;
            Sample sample;
            byte dimMax = (byte)(byte.MaxValue >> distancePrecision), maxDelta = (byte)(distanceMax >> distancePrecision);
            //Console.WriteLine($"Debug GetEdges: maxDelta {maxDelta} , dimMax {dimMax}");
            List<Tuple<int, byte>>[,,,,] array = GetArray(_vertexList, dimMax + 1, _ct); //dimMax + 1 to account for 0 based indexes.
            int bytesPerSample = CarrierMedia.BytesPerSample;
            Edge newEdge;
            byte[] outerSampleValues, innerSampleValues;
            short distance;
            int temp;
            int[] minValues = new int[bytesPerSample], maxValues = new int[bytesPerSample];
            byte sampleTargetValue, sampleModValue;
            byte[] bestSwaps = new byte[2];
            progressCounter = 1;
            progressUpdateInterval = numVertices / _progressWeight;

            bool firstXY, isHere;


            for (int numVertex = 0; numVertex < numVertices; numVertex++, progressCounter++)
            {
                _ct.ThrowIfCancellationRequested();
                vertex = _vertexList[numVertex];

                for (byte sampleIndex = 0; sampleIndex < samplesPerVertex; sampleIndex++)
                {
                    sample = vertex.Samples[sampleIndex];
                    outerSampleValues = sample.Values;
                    sampleTargetValue = sample.TargetModValue;
                    sampleModValue = sample.ModValue;
                    bestSwaps[0] = sampleIndex;

                    for (int byteIndex = 0; byteIndex < bytesPerSample; byteIndex++)
                    {
                        temp = (outerSampleValues[byteIndex] >> distancePrecision);
                        minValues[byteIndex] = temp;
                        maxValues[byteIndex] = ((temp + maxDelta) > dimMax) ? dimMax : (temp + maxDelta);
                    }
                    firstXY = true;
                    isHere = true;
                    for (int x = minValues[0]; x <= maxValues[0]; x++)
                    {
                        for (int y = minValues[1]; y <= maxValues[1]; y++)
                        {
                            for (int z = minValues[2]; z <= maxValues[2]; z++)
                            {
                                vertexRefs = array[x, y, z, sampleTargetValue, sampleModValue];
                                if (vertexRefs != null)
                                {
                                    foreach (Tuple<int, byte> vertexRef in vertexRefs)
                                    {
                                        if (isHere && vertexRef.Item1 <= numVertex)
                                        {
                                            continue;
                                        }
                                        innerSampleValues = _vertexList[vertexRef.Item1].Samples[vertexRef.Item2].Values;
                                        bestSwaps[1] = vertexRef.Item2;

                                        distance = 0;
                                        for (int valueIndex = 0; valueIndex < bytesPerSample; valueIndex++)
                                        {
                                            temp = outerSampleValues[valueIndex] - innerSampleValues[valueIndex];
                                            distance += (short)(temp * temp);
                                        }

                                        newEdge = new Edge(numVertex, vertexRef.Item1, distance, bestSwaps);

                                        foreach (int vertexId in newEdge.Vertices)
                                        {
                                            _vertexList[vertexId].Edges.Add(newEdge);
                                        }
                                    }
                                }
                                isHere = false;
                            }
                            if (firstXY)
                            {
                                minValues[1] = outerSampleValues[1] > distanceMax ? (outerSampleValues[1] - distanceMax) >> distancePrecision : 0;
                                minValues[2] = outerSampleValues[2] > distanceMax ? (outerSampleValues[2] - distanceMax) >> distancePrecision : 0;
                                firstXY = false;
                            }
                        }
                    }
                }

                // Update progress counter.
                if (progressCounter == progressUpdateInterval)
                {
                    progressCounter = 1;
                    _progress?.Report(++progress);
                    Console.WriteLine($"... {numVertex} of {numVertices} handled. {(decimal)numVertex / numVertices:p}");
                }
            }
            //Console.WriteLine("GetEdges: Successful.");
        }

        private List<Vertex> Swap(List<Vertex> _vertexList)
        {
            List<Vertex> leftoverVertexList = new List<Vertex>();

            // Sort input list of vertices by amount of edges.
            List<Vertex> sortedVertexList = _vertexList.Select(x => x).ToList();
            sortedVertexList.Sort((v1, v2) => v1.Edges.Count - v2.Edges.Count);

            // Iterate through all vertices.
            foreach (Vertex vertex in sortedVertexList)
            {
                // Only swap valid vertices.
                if (vertex.IsValid)
                {
                    bool swapped = false;

                    // Sort current vertex edges by weight.
                    vertex.Edges.Sort((e1, e2) => e1.Weight - e2.Weight);

                    // Iterate through edges of this vertex.
                    foreach (Edge edge in vertex.Edges)
                    {
                        Vertex firstVertex = _vertexList[edge.Vertices[0]];
                        Vertex secondVertex = _vertexList[edge.Vertices[1]];

                        // Skip if either vertex is invalid.
                        if (firstVertex == secondVertex || !firstVertex.IsValid || !secondVertex.IsValid)
                        {
                            continue;
                        }

                        // Swap samples.
                        Sample firstSample = firstVertex.Samples[edge.BestSwaps[0]];
                        Sample secondSample = secondVertex.Samples[edge.BestSwaps[1]];
                        firstSample.Swap(secondSample);

                        // Disable vertices.
                        firstVertex.IsValid = false;
                        secondVertex.IsValid = false;

                        swapped = true;
                        break;
                    }

                    // Add to unmatched if it could not be swapped.
                    if (!swapped)
                    {
                        leftoverVertexList.Add(vertex);
                    }
                }
            }

            return leftoverVertexList;
        }
        #endregion

        #region Extract
        public override StegoMessage Extract()
        {
            int numSamples = CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample;
            modFactor = (byte)(1 << messageBitsPerVertex);
            bitwiseModFactor = (byte)(modFactor - 1);

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

        private List<byte> GetMessageChunks(StegoMessage _message)
        {
            // Combine signature with message and convert to BitArray.
            BitArray messageBitArray = new BitArray(Signature.Concat(_message.ToByteArray(CryptoProvider)).ToArray());

            // Prepare list of bytes.
            int numMessageChunks = messageBitArray.Length / MessageBitsPerVertex;
            List<byte> messageChunklist = new List<byte>(numMessageChunks);

            // Insert each chunk.
            int indexOffset = 0;
            for (int i = 0; i < numMessageChunks; i++)
            {
                // Find current chunk value.
                byte messageValue = 0;
                for (int byteIndex = 0; byteIndex < messageBitsPerVertex; byteIndex++)
                {
                    messageValue += messageBitArray[indexOffset + byteIndex] ? (byte)(1 << byteIndex) : (byte)0;
                }

                // Increment offset.
                indexOffset += messageBitsPerVertex;

                // Add chunk to list.
                messageChunklist.Add(messageValue);
            }

            return messageChunklist;
        }

        private byte[] ReadBytes(RandomNumberList _numberList, int _count)
        {
            BitArray tempBitArray = new BitArray(_count * 8);
            int bps = CarrierMedia.BytesPerSample;
            int numVertices = (_count * 8) / messageBitsPerVertex;

            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                int bitIndexOffset = vertexIndex * messageBitsPerVertex;
                int tempValue = 0;
                for (int sampleIndex = 0; sampleIndex < samplesPerVertex; sampleIndex++)
                {
                    var byteIndexOffset = _numberList.Next * bps;
                    for (int byteIndex = 0; byteIndex < bps; byteIndex++)
                    {
                        tempValue += CarrierMedia.ByteArray[byteIndexOffset + byteIndex];
                    }
                }
                tempValue = tempValue & bitwiseModFactor;
                for (int bitIndex = 0; bitIndex < messageBitsPerVertex; bitIndex++)
                {
                    tempBitArray[bitIndexOffset + bitIndex] = ((tempValue & (1 << bitIndex)) != 0);
                }
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }
        #endregion

    }
}