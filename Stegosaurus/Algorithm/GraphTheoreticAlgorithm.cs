using System;
using System.Collections.Generic;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm;
using System.Collections;
using System.Linq;
using System.Text;
using Stegosaurus.Utility;
using Stegosaurus.Forms;
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

        private int distanceMax = 16;
        [Category("Algorithm"), Description("The maximum distance between single samplevalues for an edge to be valid. Higher numbers means less visual imperceptibility but more statistical imperceptibility. Higher numbers might also decrease performance, depending on DistancePrecision. (Default = 32, Min-Max = 2-128.)")]
        public int DistanceMax
        {
            get { return distanceMax; }
            set { distanceMax = (value <= 128) ? ((value >= 2) ? value : 2) : 128; }
        }

        private int distancePrecision = 4;
        [Category("Algorithm"), Description("The distance precision. Higher numbers significantly decreases performance with high DistanceMax. (Default = 8, Min-Max = 2-32.)")]
        public int DistancePrecision
        {
            get { return distancePrecision; }
            set { distancePrecision = (value <= 32) ? ((value >= 2) ? value : 2) : 32; }
        }

        private int verticesPerMatching = 100000;
        [Category("Algorithm"), Description("The maximum number of vertices to find edges for at a time. Higher numbers means more memory usage but better imperceptibility. (Default = 150,000, Min = 10,000.)")]
        public int VerticesPerMatching
        {
            get { return verticesPerMatching; }
            set { verticesPerMatching = (value >= 10000) ? value : 10000; }
        }

        //private int reserveMatching = 1;
        //[Category("Algorithm"), Description("The number of times to try matching leftover vertices with reserve samples. (Default = 2, Min-Max = 0-8.)")]
        //public int ReserveMatching
        //{
        //    get { return reserveMatching; }
        //    set { reserveMatching = (value <= 8) ? ((value >= 0) ? value : 0) : 8; }
        //}

        private int progress, progressCounter, progressUpdateInterval;
        private byte modFactor;
        private byte bitwiseModFactor;
        private byte shiftFactor;
        
        public override long ComputeBandwidth()
        {
            return ((((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / samplesPerVertex) * messageBitsPerVertex ) / 8) - Signature.Length;
        }

        #region Embed
        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            modFactor = (byte)(1 << messageBitsPerVertex);
            bitwiseModFactor = (byte)(modFactor - 1);
            int logDistanceMax = (int)(Math.Ceiling(Math.Log(DistanceMax, 2))), logDistancePrecision = (int)(Math.Ceiling(Math.Log(distancePrecision, 2)));
            shiftFactor = (logDistanceMax > logDistancePrecision) ? (byte)(logDistanceMax - logDistancePrecision) : (byte)0;

            // Verify that the carrier is supported by the algorithm.
            int bps = CarrierMedia.BytesPerSample;
            if (bps != 3)
            {
                throw new StegoAlgorithmException("The selected carrier is not supported by this algorithm.");
            }

            // Convert StegoMessage into chunks of bytes.
            List<byte> messageChunks = GetMessageChunks(_message);//GetMessage(_message, _progress, _ct, 10);

            // Convert bytes CarrierMedia to a list of Samples.
            List<Sample> samples = Sample.GetSampleListFrom(CarrierMedia, bitwiseModFactor);

            // Get Vertex lists.
            Tuple<List<Vertex>, List<Vertex>> verticeTuple = GetVerticeLists(samples, messageChunks);

            List<Vertex> messageVertices = verticeTuple.Item1;
            List<Vertex> reserveVertices = verticeTuple.Item2;

            //GetEdges(vertices, _progress, _ct, 40);
            //List<Vertex> leftovers = DoSwap(vertices, _progress, _ct, 10);

            List<Vertex> leftovers = FindEdgesAndSwap(messageVertices, _progress, _ct, 50, samples, messageChunks);
            //PrintMessage("FindEdgesAndSwap:", samples, message);

            //DoReserveMatching();

            DoAdjust(leftovers, _progress, _ct, 5);
            //PrintDebug("DoAdjust:", samples, message);

            DoEncode(samples);
            //PrintDebug("DoEncode:", samples, message);

            _progress?.Report(100);
        }
        
        // Gets the encrypted message and seperates the bit pattern into chunks of size messageBitsPerVertex which are added to the messageHunk list.
        private List<byte> GetMessage(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct, int _progressWeight)
        {
            Console.WriteLine("Debug GetMessageHunks:");
            List<byte> message = new List<byte>();
            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(Signature.Concat(messageArray).ToArray());
            int numMessageParts = messageInBits.Length / messageBitsPerVertex;
            byte messageValue;
            
            progressUpdateInterval = numMessageParts / _progressWeight;
            progressCounter = 0;
            for (int index = 0, indexOffset = 0; index < numMessageParts; index++, indexOffset += messageBitsPerVertex, progressCounter++)
            {
                _ct.ThrowIfCancellationRequested();
                messageValue = new byte();
                messageValue = 0;
                for (int byteIndex = 0; byteIndex < messageBitsPerVertex; byteIndex++)
                {
                    messageValue += messageInBits[indexOffset + byteIndex] ? (byte)(1 << byteIndex) : (byte)0;
                }
                message.Add(messageValue);

                if (progressCounter == progressUpdateInterval)
                {
                    progressCounter = 0;
                    _progress?.Report(++progress);
                    Console.WriteLine($"... {index} of {numMessageParts} messageHunks handled.");
                }
            }

            Console.WriteLine("GetMessageHunks: Succesful.");
            return message;
        }

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
                    Vertex reserveVertex = new Vertex(tmpSampleArray);
                    reserveVertex.Value = vertexModValue;
                    reserveVertices.Add(reserveVertex);
                }
                else
                {
                    Vertex messageVertex = new Vertex(tmpSampleArray);
                    messageVertex.Value = vertexModValue;
                    messageVertices.Add(messageVertex);

                    // Calculate delta value.
                    byte deltaValue = (byte) ((modFactor + _messageChunks[i] - messageVertex.Value) & bitwiseModFactor );

                    // Set target values.
                    foreach (Sample sample in messageVertex.Samples)
                    {
                        sample.TargetModValue = (byte) ((sample.ModValue + deltaValue) & bitwiseModFactor);
                    }
                }
            }

            return new Tuple<List<Vertex>, List<Vertex>>(messageVertices, reserveVertices);
        }

        private List<Vertex> FindEdgesAndSwap(List<Vertex> _vertices, IProgress<int> _progress, CancellationToken _ct, int _progressWeight, List<Sample> _samples, List<byte> _message)
        {
            Console.WriteLine("Debug FindEdgesAndSwap:");
            List<Vertex> leftovers = new List<Vertex>(), tempLeftovers = new List<Vertex>(), tempVertices = new List<Vertex>();
            List<Edge> edges = new List<Edge>();
            int numRuns = (int)Math.Ceiling((decimal)_vertices.Count / verticesPerMatching);
            int verticesPerRun = _vertices.Count / numRuns;
            int indexOffset = 0;
            int maxLeftovers = (VerticesPerMatching >> 2);
            int numToTrim;
            int curProgress = progress, weight = ((_progressWeight >> 1) / numRuns) > 0 ? ((_progressWeight >> 1) / numRuns) : 1;
            for (int i = 0; i < numRuns; i++)
            {
                Console.WriteLine($"FEAS Iteration {i} of {numRuns}.");
                _ct.ThrowIfCancellationRequested();
                tempVertices.Clear();
                if (tempLeftovers.Count > maxLeftovers)
                {
                    numToTrim = tempLeftovers.Count - maxLeftovers;
                    leftovers.AddRange(tempLeftovers.GetRange(0, numToTrim));
                    tempLeftovers = tempLeftovers.GetRange(numToTrim, maxLeftovers);
                }
                tempVertices.AddRange(tempLeftovers);
                tempVertices.AddRange(_vertices.GetRange(indexOffset, (i < (numRuns - 1) ? verticesPerRun : (_vertices.Count - indexOffset))));
                GetEdges(tempVertices, _progress, _ct, weight);
                tempLeftovers = DoSwap(tempVertices, _progress, _ct, weight);
                //PrintDebug("DoSwap:", _samples, _message);
                ClearVertexEdges(tempVertices);
                indexOffset += verticesPerRun;
            }
            leftovers.AddRange(tempLeftovers);
            Console.WriteLine($"Total of {leftovers.Count} vertices not swapped.");
            progress = curProgress + _progressWeight;
            _progress?.Report(progress);

            ClearVertexEdges(tempVertices);
            Console.WriteLine("FindEdgesAndSwap: Succesful.");
            return leftovers;
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
                    vertexRefs = array[sample.Values[0] >> shiftFactor, sample.Values[1] >> shiftFactor, sample.Values[2] >> shiftFactor, sample.ModValue, sample.TargetModValue];
                    if (vertexRefs != null)
                    {
                        vertexRefs.Add(vertexRef);
                    }
                    else
                    {
                        array[sample.Values[0] >> shiftFactor, sample.Values[1] >> shiftFactor, sample.Values[2] >> shiftFactor, sample.ModValue, sample.TargetModValue] = new List<Tuple<int, byte>>();
                        array[sample.Values[0] >> shiftFactor, sample.Values[1] >> shiftFactor, sample.Values[2] >> shiftFactor, sample.ModValue, sample.TargetModValue].Add(vertexRef);
                    }
                }
            }
            return array;
        }

        private void GetEdges(List<Vertex> _vertexList, IProgress<int> _progress, CancellationToken _ct, int _progressWeight)
        {
            Console.WriteLine("Debug GetEdges:");
            int numVertices = _vertexList.Count;
            List<Edge> edges = new List<Edge>();
            List<Tuple<int, byte>> vertexRefs;
            Vertex vertex;
            Sample sample;
            byte dimMax = (byte)(byte.MaxValue >> shiftFactor), maxDelta = (byte)(distanceMax >> shiftFactor);
            Console.WriteLine($"Debug GetEdges: maxDelta {maxDelta} , dimMax {dimMax}");
            List<Tuple<int, byte>>[,,,,] array = GetArray(_vertexList, dimMax + 1, _ct); //dimMax + 1 to account for 0 based indexes.
            int bytesPerSample = CarrierMedia.BytesPerSample;
            Edge newEdge;
            byte[] outerSampleValues, innerSampleValues;
            byte[] sampleDeltaValues = new byte[bytesPerSample];
            short distance;
            int temp;
            int[] minValues = new int[bytesPerSample], maxValues = new int[bytesPerSample];
            byte sampleTargetValue, sampleModValue;
            byte[] bestSwaps = new byte[2];
            //Dictionary<Tuple<byte, byte, byte, byte, byte>, List<Tuple<int, byte>>> locationDictionary = GetDictionary(_vertexList, _ct);
            //Tuple<byte, byte, byte, byte, byte> location;
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
                        temp = (outerSampleValues[byteIndex] >> shiftFactor);
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
                                //location = Tuple.Create((byte)x, (byte)y, (byte)z, sampleTargetValue, sampleModValue);
                                //if (locationDictionary.TryGetValue(location, out vertexRefs))
                                //{
                                //    foreach (Tuple<int, byte> vertexRef in vertexRefs)
                                //    {
                                //        if (isHere && vertexRef.Item1 <= numVertex)
                                //        {
                                //            continue;
                                //        }
                                //        innerVertex = _vertexList[vertexRef.Item1];
                                //        innerSampleValues = innerVertex.Samples[vertexRef.Item2].Values;
                                //        bestSwaps[1] = vertexRef.Item2;

                                //        distance = 0;
                                //        deltaValue = 0;
                                //        for (int valueIndex = 0; valueIndex < bps; valueIndex++)
                                //        {
                                //            deltaValue = outerSampleValues[valueIndex] - innerSampleValues[valueIndex];
                                //            distance += (short)(deltaValue * deltaValue);
                                //        }

                                //        newEdge = new Edge(outerVertex, _vertexList[vertexRef.Item1], distance, bestSwaps);
                                //        foreach (Vertex vertex in newEdge.Vertices)
                                //        {
                                //            vertex.Edges.Add(newEdge);
                                //            vertex.numEdges++;
                                //        }
                                //        edges.Add(newEdge);
                                //    }
                                //}
                                isHere = false;
                            }
                            if (firstXY)
                            {
                                minValues[1] = (outerSampleValues[1] > distanceMax) ? (byte)((outerSampleValues[1] - distanceMax) >> shiftFactor) : (byte)0;
                                minValues[2] = (outerSampleValues[2] > distanceMax) ? (byte)((outerSampleValues[2] - distanceMax) >> shiftFactor) : (byte)0;
                                firstXY = false;
                            }
                        }
                    }
                }
                if (progressCounter == progressUpdateInterval)
                {
                    progressCounter = 1;
                    _progress?.Report(++progress);
                    Console.WriteLine($"... {numVertex} of {numVertices} handled. {(decimal)numVertex / numVertices :p}");
                }
            }
            Console.WriteLine("GetEdges: Succesfull.");

            return;
        }
        
        // returns a dictionary of lists of Tuple vertex references, with keys containing the sample Values, ModValue and TargetValue.
        private Dictionary<Tuple<byte, byte, byte, byte, byte>, List<Tuple<int, byte>>> GetDictionary(List<Vertex> _vertices, CancellationToken _ct)
        {
            Dictionary<Tuple<byte, byte, byte, byte, byte>, List<Tuple<int, byte>>> locationDictionary = new Dictionary<Tuple<byte, byte, byte, byte, byte>, List<Tuple<int, byte>>>();

            Sample vertexSample;
            List<Tuple<int, byte>> vertexReferenceList;
            Tuple <int, byte> vertexRef;
            Tuple<byte, byte, byte, byte, byte> location;
            int numVertices = _vertices.Count;
            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                _ct.ThrowIfCancellationRequested();
                for (byte sampleIndex = 0; sampleIndex < samplesPerVertex; sampleIndex++)
                {
                    vertexSample = _vertices[vertexIndex].Samples[sampleIndex];
                    vertexRef = Tuple.Create(vertexIndex, sampleIndex);
                    location = Tuple.Create((byte)(vertexSample.Values[0] >> shiftFactor), (byte)(vertexSample.Values[1] >> shiftFactor), (byte)(vertexSample.Values[2] >> shiftFactor), vertexSample.ModValue, vertexSample.TargetModValue);

                    if (locationDictionary.TryGetValue(location, out vertexReferenceList))
                    {
                        vertexReferenceList.Add(vertexRef);
                    }
                    else
                    {
                        vertexReferenceList = new List<Tuple<int, byte>>();
                        vertexReferenceList.Add(vertexRef);
                        locationDictionary.Add(location, vertexReferenceList);
                    }
                }
            }
            return locationDictionary;
        }
        
        // receives a list of vertices, sorts them, performs swaps starting with the vertice with least edges and returns a list of the vertices that couldnt be swapped.
        private List<Vertex> DoSwap(List<Vertex> _vertices, IProgress<int> _progress, CancellationToken _ct, int _progressWeight)
        {
            Console.WriteLine("Debug DoSwap:");
            List<Vertex> leftoverVertices = new List<Vertex>();
            List<Vertex> sortedVertices = _vertices.GetRange(0, _vertices.Count);
            //Console.WriteLine("... Sorting for edges");
            sortedVertices.Sort((v1, v2) => v1.Edges.Count - v2.Edges.Count);
            //Console.WriteLine("... Sorted.");
            bool swapped;
            int numVertices = _vertices.Count;
            Vertex vertex;
            int bytesPerSample = CarrierMedia.BytesPerSample;
            byte[] tempSampleBytes = new byte[bytesPerSample];

            byte[] iValues, oValues;

            progressUpdateInterval = numVertices / _progressWeight;
            progressCounter = 1;
            for (int i = 0; i < numVertices; i++, progressCounter++)
            {
                _ct.ThrowIfCancellationRequested();
                vertex = sortedVertices[i];
                if (vertex.IsValid)
                {
                    swapped = false;
                    vertex.Edges.Sort((e1, e2) => e1.Weight - e2.Weight);
                    foreach (Edge edge in vertex.Edges)
                    {
                        if ((_vertices[edge.Vertices[0]].IsValid && _vertices[edge.Vertices[1]].IsValid) && (edge.Vertices[0] != edge.Vertices[1]))
                        {
                            ////swap sample bytes.
                            //tempSampleBytes = _vertices[edge.Vertices[0]].Samples[edge.BestSwaps[0]].Values;
                            //_vertices[edge.Vertices[0]].Samples[edge.BestSwaps[0]].Values = _vertices[edge.Vertices[1]].Samples[edge.BestSwaps[1]].Values;
                            //_vertices[edge.Vertices[1]].Samples[edge.BestSwaps[1]].Values = tempSampleBytes;

                            oValues = _vertices[edge.Vertices[0]].Samples[edge.BestSwaps[0]].Values;
                            iValues = _vertices[edge.Vertices[1]].Samples[edge.BestSwaps[1]].Values;
                            for (int byteIndex = 0; byteIndex < bytesPerSample; byteIndex++)
                            {
                                tempSampleBytes[byteIndex] = oValues[byteIndex];
                                oValues[byteIndex] = iValues[byteIndex];
                                iValues[byteIndex] = tempSampleBytes[byteIndex];
                            }

                        
                            foreach (int edgeVertexId in edge.Vertices)
                            {
                                _vertices[edgeVertexId].IsValid = false;
                            }
                            swapped = true;
                            break;
                        }                
                    }
                    if (!swapped)
                    {
                        vertex.IsValid = false;
                        leftoverVertices.Add(vertex);
                    }
                }
                if (progressCounter == progressUpdateInterval)
                {
                    progressCounter = 1;
                    _progress?.Report(++progress);
                    Console.WriteLine($"... {i} of {numVertices} vertices handled. {(decimal)i / numVertices:p}");
                }
            }
            Console.WriteLine($"{leftoverVertices.Count} of {numVertices} vertices were unable to be swapped. {(decimal)leftoverVertices.Count / numVertices :p}");

            Console.WriteLine("DoSwap: Succesful.");
            return leftoverVertices;
        }

        // Iterates through the remainder vertices and adjusts their values to the desired values.
        private void DoAdjust(List<Vertex> _leftoverVertices, IProgress<int> _progress, CancellationToken _ct, int _progressWeight)
        {
            Console.WriteLine("Debug DoAdjust:");
            byte bps = (byte)CarrierMedia.BytesPerSample;
            int maxValue = byte.MaxValue;
            int numVertices = _leftoverVertices.Count;
            int sampleIndex = 0, byteIndex = 0;
            byte valueDif;
            byte sampleValue;
            Vertex vertex;
            progressCounter = 1;
            progressUpdateInterval = numVertices / _progressWeight;
            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++, progressCounter++)
            {
                _ct.ThrowIfCancellationRequested();

                vertex = _leftoverVertices[vertexIndex];

                //ensures that it varies which sampe of a vertesx, and which byte of the sample that is adjusted varies.
                sampleIndex = (sampleIndex > 0) ? (sampleIndex - 1) : (SamplesPerVertex - 1);
                byteIndex = (byteIndex > 0) ? (byteIndex - 1) : (bps - 1);

                valueDif = (byte)((modFactor - vertex.Samples[0].ModValue + vertex.Samples[0].TargetModValue) & bitwiseModFactor);
                sampleValue = vertex.Samples[sampleIndex].Values[byteIndex];
                if ((sampleValue + valueDif) <= maxValue)
                {
                    vertex.Samples[sampleIndex].Values[byteIndex] += valueDif;
                }
                else
                {
                    vertex.Samples[sampleIndex].Values[byteIndex] -= (byte)(modFactor - valueDif);
                }

                if (progressCounter == progressUpdateInterval)
                {
                    progressCounter = 1;
                    _progress?.Report(++progress);
                    Console.WriteLine($"... {vertexIndex} of {numVertices} adjusted. {(decimal)vertexIndex / numVertices :p}");
                }
            }
            Console.WriteLine("DoAdjust: Succesful");
        }

        private void DoEncode(List<Sample> _samples)
        {
            int numSamples = _samples.Count;
            int bytesPerSample = CarrierMedia.BytesPerSample;
            int byteIndexOffset;
            for (int sampleIndex = 0; sampleIndex < numSamples; sampleIndex++)
            {
                byteIndexOffset = sampleIndex * bytesPerSample;
                for (int byteIndex = 0; byteIndex < bytesPerSample; byteIndex++)
                {
                    CarrierMedia.ByteArray[byteIndexOffset + byteIndex] = _samples[sampleIndex].Values[byteIndex];
                }
            }
        }
        #endregion

        #region Extract
        public override StegoMessage Extract()
        {
            modFactor = (byte)(1 << messageBitsPerVertex);
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
                    messageValue += messageBitArray[indexOffset + byteIndex] ? (byte) ( 1 << byteIndex ) : (byte) 0;
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
            int bytesPerVertex = bps * SamplesPerVertex;
            int numVertices = (_count * 8) / messageBitsPerVertex;
            int tempValue = 0;
            int byteIndexOffset, bitIndexOffset;

            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                bitIndexOffset = vertexIndex * messageBitsPerVertex;
                tempValue = 0;
                for (int sampleIndex = 0; sampleIndex < samplesPerVertex; sampleIndex++)
                {
                    byteIndexOffset = _numberList.Next * bps;
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

        //Clears the vertices for references to edges.
        private void ClearVertexEdges(List<Vertex> _vertices)
        {
            //Console.WriteLine("Debug ClearVertexEdges:");
            foreach (Vertex vertex in _vertices)
            {
                vertex.Edges.Clear();
            }
            //Console.WriteLine("... vertices cleared for edges.");

            //Console.WriteLine("ClearVertexEdges: Succesful.");
        }


        private void PrintDebug(string _message, List<Sample> _samples, List<byte> _messageBytes)
        {
            Console.WriteLine(_message);
            RandomNumberList rnl = new RandomNumberList(Seed, _samples.Count);
            int temp;
            int numMessageVals = _messageBytes.Count;
            int bps = CarrierMedia.BytesPerSample, spv = SamplesPerVertex;
            int sampleIndex, sampleTval = 0;
            int sampleTotalValue;
            Sample sample;
            for (int i = 0; i < numMessageVals; i++)
            {
                temp = 0;
                for (int j = 0; j < spv; j++)
                {
                    sampleIndex = rnl.Next;
                    sampleTval = _samples[sampleIndex].TargetModValue;
                    for (int k = 0; k < bps; k++)
                    {
                        temp += _samples[sampleIndex].Values[k];
                    }
                }
                sampleTotalValue = temp;
                temp = temp & bitwiseModFactor;

                //Console.Write($"{sampleTotalValue},{temp}|");
                //Console.Write($"{{{_messageBytes[i]};{temp}}}");
            }



        }


    }
}
