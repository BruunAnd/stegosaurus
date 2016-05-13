﻿using System;
using System.Collections.Generic;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm.GraphTheory;
using System.Collections;
using System.Linq;
using System.Text;
using Stegosaurus.Utility;
using Stegosaurus.Forms;
using System.ComponentModel;
using System.Threading;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : StegoAlgorithmBase
    {

        private static readonly byte[] GraphTheorySignature = { 0x12, 0x34, 0x56, 0x78 };
        private byte samplesPerVertex = 2;
        private byte messageBitsPerVertex = 2;
        private ushort discriminationFactor = 1024;

        [Category("Algorithm"), Description("The number of samples collected in each vertex. Higher numbers means less bandwidth but more imperceptibility.(Default = 2, Max = 4.)")]
        public byte SamplesPerVertex
        {
            get { return samplesPerVertex; }
            set { samplesPerVertex = (value <= 4)? value:(byte)4;  }
        }
        
        [Category("Algorithm"), Description("The number of bits hidden in each vertex. Higher numbers means more bandwidth but less imperceptibility.(Default = 2, Max = 4.)")]
        public byte MessageBitsPerVertex
        {
            get { return messageBitsPerVertex; }
            set { messageBitsPerVertex = (value <= 4) ? value : (byte)4; }
        }

        [Category("Algorithm"), Description("The maximum distance^2 between sample values for an edge to be valid. Higher numbers means less visual imperceptibility but more statistical imperceptibility. (Default = 1024, Max = 8192.)")]
        public ushort DiscriminationFactor
        {
            get { return discriminationFactor; }
            set { discriminationFactor = (value <= 8192) ? value : (ushort)8192; }
        }

        private short modFactor;
        private short bitwiseModFactor;

        private List<byte> messageHunks;
        private List<Sample> samples = new List<Sample>();
        private List<Vertex> vertices = new List<Vertex>();
        private List<Vertex> reserveVertices = new List<Vertex>();
        private List<Edge> edges = new List<Edge>();
        
        public override string Name => "Graph Theoretic Algorithm";

        // todo: implement
        protected override byte[] Signature
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long ComputeBandwidth()
        {
            return ((((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / samplesPerVertex) * messageBitsPerVertex ) / 8) - GraphTheorySignature.Length;
        }

        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            modFactor = (short)(1 << messageBitsPerVertex);
            bitwiseModFactor = (byte)(modFactor - 1);

            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(GraphTheorySignature.Concat(messageArray).ToArray());
            messageHunks = new List<byte>();
            int len = messageInBits.Length / messageBitsPerVertex;
            int index = 0, a = 0;
            byte messageHunk = 0;
            GetMessageHunks(_message);
            _progress.Report(5);
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
            _progress.Report(10);

            GetSamples();
            _progress.Report(20);

            // Generate random sequence of integers
            //IEnumerable<int> numberList = new RandomNumberList(Seed, samples.Count);

            int dimSize = byte.MaxValue + 1;
            
            // TODO: make dynamic. Currently only compatible with 3 bytes per sample.
            CountedVerticeList[,,,,] verticeArray = new CountedVerticeList[dimSize, dimSize, dimSize, modFactor, bitwiseModFactor];
            _progress.Report(25);

            GetVertices(messageInBits, verticeArray);
            _progress.Report(45);

            GetEdges(verticeArray);
            _progress.Report(75);

            Swap();
            _progress.Report(95);

            vertices.Clear();
            reserveVertices.Clear();
            edges.Clear();
            _progress.Report(100);
        }

        public override StegoMessage Extract()
        {
            throw new NotImplementedException();
        }

        // Gets the encrypted message and seperates the bit pattern into chunks of size messageBitsPerVertex which are added to the messageHunk list.
        private void GetMessageHunks(StegoMessage _message)
        {
            messageHunks = new List<byte>();
            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(GraphTheorySignature.Concat(messageArray).ToArray());
            int len = messageInBits.Length / messageBitsPerVertex;
            int index = 0, indexOffset = 0;
            byte messageHunk = 0;
            while (index < len)
            {
                messageHunk = new byte();
                indexOffset = index++ * messageBitsPerVertex;
                for (int i = 0; i < messageBitsPerVertex; i++)
                {
                    messageHunk += messageInBits[indexOffset + i] ? (byte)Math.Pow(2, i) : (byte)0;
                }
                messageHunks.Add(messageHunk);
            }
        }

        private void GetSamples()
        {
            byte[] tempSampleBytes = new byte[CarrierMedia.BytesPerSample];
            long numSamples = CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample;
            long len = CarrierMedia.ByteArray.Length;

            Sample tempSample;
            for (long i = 0; i < len; i += CarrierMedia.BytesPerSample)
            {
                for (int j = 0; j < CarrierMedia.BytesPerSample; j++)
                {
                    tempSampleBytes[j] = CarrierMedia.ByteArray[i + j];
                }
                tempSample = new Sample(tempSampleBytes);
                short value = 0;
                foreach (byte item in tempSampleBytes)
                {
                    value += (short)item;
                }
                tempSample.Value = (short)(value & bitwiseModFactor);
                samples.Add(tempSample);
            }
        }

        private void GetVertices(BitArray _messageInBits, CountedVerticeList[,,,,] _verticeArray)
        {

            int len = samples.Count / samplesPerVertex;
            int mlen = messageHunks.Count;

            Vertex temp;
            Sample[] vertexSamples;
            short value = 0;
            short deltaValue;
            for (int i = 0; i < len; i++)
            {
                vertexSamples = new Sample[samplesPerVertex];
                value = 0;

                for (int index = 0; index < samplesPerVertex; index++)
                {
                    // linear, non-random sample pairing.
                    vertexSamples[index] = samples[(i * 2) + index];

                    // TODO: find better performing method of pseudo-random sample pairing.
                    //vertexSamples.Add(samples[_numberList.First()]);

                    value += vertexSamples[index].Value;
                }

                value = (short)(value & bitwiseModFactor);

                temp = new Vertex(vertexSamples);
                temp.Value = value;

                
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
                    deltaValue = (short)((modFactor + messageHunks[i] - temp.Value) & bitwiseModFactor);
                    temp.ValueDif = deltaValue;
                    foreach (Sample item in temp.Samples)
                    {
                        item.TargetValue = (short)((item.Value + deltaValue) & bitwiseModFactor);
                        if (_verticeArray[item.Bytes[0], item.Bytes[1], item.Bytes[2], item.Value, deltaValue - 1] == null)
                        {
                            _verticeArray[item.Bytes[0], item.Bytes[1], item.Bytes[2], item.Value, deltaValue - 1] = new CountedVerticeList();
                        }
                        _verticeArray[item.Bytes[0], item.Bytes[1], item.Bytes[2], item.Value, deltaValue - 1].Add(temp);
                    }
                    vertices.Add(temp);
                }
            }

            
        }

        private void GetEdges(CountedVerticeList[,,,,] _verticeArray)
        {

            int bps = CarrierMedia.BytesPerSample;
            int numVertices = vertices.Count;
            
            short distance;
            byte[] bestSwaps = new byte[2];
            
            Vertex outerVertex;
            Sample sampleX;
            Edge newEdge;
            int dimSize = byte.MaxValue;
            int[] sampleValues = new int[bps];
            int[] maxValues = new int[bps];
            int maxDelta = (int)Math.Log(discriminationFactor, 2);
            short sampleModValue, sampleTargetValue, sampleValueDelta;
            int numEdgeless = 0;
            
            for (int numVertex = 0; numVertex < numVertices; numVertex++)
            {
                outerVertex = vertices[numVertex];
                for (int i = 0; i < samplesPerVertex; i++)
                {
                    sampleX = outerVertex.Samples[i];
                    sampleModValue = sampleX.Value;
                    sampleTargetValue = sampleX.TargetValue;
                    sampleValueDelta = (short)(modFactor - outerVertex.ValueDif - 1);
                    bestSwaps[0] = (byte)i;
                    for (int j = 0; j < bps; j++)
                    {
                        sampleValues[j] = sampleX.Bytes[j];
                        maxValues[j] = sampleValues[j] + maxDelta;
                        maxValues[j] = maxValues[j] > dimSize ? dimSize : maxValues[j];
                    }
                    
                    for (int x = sampleValues[0], xDelta = 0 ;x < maxValues[0]; x++, xDelta++)
                    {
                        for (int y = sampleValues[1], yDelta = 0; y < maxValues[1]; y++, yDelta++)
                        {
                            for (int z = sampleValues[2], zDelta = 0; z < maxValues[2]; z++, zDelta++)
                            {
                                if (_verticeArray[x,y,z,sampleTargetValue,sampleValueDelta] != null)
                                {
                                    distance = (short)((xDelta * xDelta) + (yDelta * yDelta) + (zDelta * zDelta));
                                    foreach (Vertex innerVertex in _verticeArray[x, y, z, sampleTargetValue, sampleValueDelta].vertices)
                                    {
                                        bestSwaps[1] = (innerVertex.Samples[0].Bytes ==  new byte[]{ (byte)x, (byte)y, (byte)z }) ? (byte)0 : (byte)1;
                                        newEdge = new Edge(outerVertex, innerVertex, distance, bestSwaps);
                                        foreach (Vertex vertex in newEdge.Vertices)
                                        {
                                            vertex.Edges.Add(newEdge);
                                            vertex.numEdges++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (outerVertex.numEdges == 0)
                {
                    numEdgeless++;
                }
                if ((numVertex & 255) == 0)
                {
                    Console.WriteLine($"{numVertex} of {numVertices} handled. {(decimal)numVertex / numVertices :p}");
                }
            }
            Console.WriteLine($"{numEdgeless} of {numVertices} vertices had 0 edges. {(decimal)numEdgeless / numVertices:p}");

            //for (int i = 0; i < (numVertices - 1); i++)
            //{
            //    vertexA = vertices[i];
            //    valueDifA = vertexA.ValueDif;
            //    for (int j = i + 1 ; j < numVertices; j++)
            //    {

            //        vertexB = vertices[j];
            //        if (valueDifA != modFactor - vertexB.ValueDif)
            //        {
            //            debug++;
            //            continue;
            //        }
            //        minDistance = (short)(discriminationFactor + 1);
            //        distance = 1;
            //        isValidEdge = false;
            //        for (int k = 0; (k < samplesPerVertex) && (distance != 0) ; k++)
            //        {
            //            sampleA = vertexA.Samples[k];
            //            for (int l = 0; l < samplesPerVertex; l++)
            //            {
            //                sampleB = vertexB.Samples[l];
            //                if ((sampleA.TargetValue == sampleB.Value) && (sampleA.Value == sampleB.TargetValue))
            //                {
            //                    distance = 0;
            //                    for (int m = 0; m < bps; m++)
            //                    {
            //                        tempDistance = Math.Abs(((int)sampleA.Bytes[m] - (int)sampleB.Bytes[m]));
            //                        distance += tempDistance * tempDistance;
            //                    }
            //                    if (distance < minDistance)
            //                    {
            //                        minDistance = (short)distance;
            //                        isValidEdge = true;
            //                        bestSwaps[0] = (byte)k;
            //                        bestSwaps[1] = (byte)l;
            //                        if (distance == 1)
            //                        {
            //                            debug2++;
            //                            break;
            //                        }

            //                    }
            //                }
            //            }
            //        }
            //        if (isValidEdge)
            //        {
            //            newEdge = new Edge(vertexA, vertexB, minDistance, bestSwaps);

            //            vertexA.Edges.Add(newEdge);
            //            vertexB.Edges.Add(newEdge);
            //            vertexA.numEdges++;
            //            vertexB.numEdges++;
            //        }
            //    }

            //}

        }
        
        private void Swap()
        {
            Console.WriteLine("Sorting.");
            vertices.Sort((v1, v2) => v1.numEdges - v2.numEdges);

            int numVertices = vertices.Count;
            byte[] tempSampleBytes;
            Vertex vertex;
            List<Vertex> lsbVertices = new List<Vertex>();
            bool swapped;
            int progressMessageInterval = 8191;
            int i = 0, j = 0;
            do
            {
                vertex = vertices[j++];
                if (vertex.IsValid)
                {
                    swapped = false;
                    vertex.Edges.Sort((e1, e2) => e1.Weight - e2.Weight);
                    foreach (Edge edge in vertex.Edges)
                    {
                        if (edge.Vertices[0].IsValid && edge.Vertices[1].IsValid)
                        {
                            //swap sample bytes.
                            tempSampleBytes = edge.Vertices[0].Samples[edge.BestSwaps[0]].Bytes;
                            edge.Vertices[0].Samples[edge.BestSwaps[0]].Bytes = edge.Vertices[1].Samples[edge.BestSwaps[1]].Bytes;
                            edge.Vertices[1].Samples[edge.BestSwaps[1]].Bytes = tempSampleBytes;
                            foreach (Vertex vertice in edge.Vertices)
                            {
                                vertice.IsValid = false;
                                i++;
                            }
                            swapped = true;
                            break;
                        }                
                    }
                    if (!swapped)
                    {
                        lsbVertices.Add(vertex);
                        i++;
                    }
                }
                if ((j & progressMessageInterval) == 0)
                {
                    Console.WriteLine($"{i} of {numVertices} vertices vertices handled. {(decimal)i / numVertices:p}");
                }
            } while (i < numVertices);
            
            
            Console.WriteLine($"{lsbVertices.Count} of {numVertices} vertices needed LSB alteration. {(decimal)lsbVertices.Count / numVertices :p}");

            System.Windows.Forms.MessageBox.Show("LSBSwap() not implemented yet.");
        }
    }
}
