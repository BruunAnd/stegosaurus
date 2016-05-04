using System;
using System.Collections.Generic;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm.GraphTheory;
using System.Collections;
using System.Linq;
using Stegosaurus.Utility;
using Stegosaurus.Exceptions;
using System.Windows.Forms;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : IStegoAlgorithm
    {
        public ICarrierMedia CarrierMedia
        {
            get; set;
        }

        public ICryptoProvider CryptoProvider
        {
            get; set;
        }

        public string Name => "GTA";

        public long ComputeBandwidth()
        {
            return 100000;
        }

        public void Embed(StegoMessage message)
        {
            var messageBits = new BitArray(message.ToByteArray(CryptoProvider));
            Console.WriteLine("{0} bits", messageBits.Length);

            // TODO fix
            if (messageBits.Length % 3 != 0)
            {
                throw new StegoAlgorithmException("Invalid size.");
            }

            // Generate pixels


            // Generate vertices
            var vertices = new List<Vertex>();




            var position = 0;
            for (int i = 0; i < messageBits.Length; i++)
            {
                Vertex newVertex = new Vertex(position);
                newVertex.Samples = new byte[] { CarrierMedia.ByteArray[position++], CarrierMedia.ByteArray[position++], CarrierMedia.ByteArray[position++] };
                newVertex.TargetValue = new bool[] { messageBits[i], messageBits[i++], messageBits[i++] };

                if (newVertex.HasMatchingBits(newVertex))
                {
                    //Console.WriteLine("{0}, {1}, {2} matches with {3}, {4}, {5}", newVertex.Samples[0], newVertex.Samples[1], newVertex.Samples[2], newVertex.TargetValue[0], newVertex.TargetValue[1], newVertex.TargetValue[2]);
                }
                else
                    vertices.Add(newVertex);
            }
            //MessageBox.Show($"{vertices.Count} vertices");

            // Generate edges
            List<Edge> edges = new List<Edge>();
            for (int i = vertices.Count - 1; i >= 0; i--)
            {
                Vertex outer = vertices[i];

                for (int j = vertices.Count - 1; j >= 0; j--)
                {
                    Vertex inner = vertices[j];

                    if (i == j)
                        continue;


                    if (inner.HasMatchingBits(outer) && outer.HasMatchingBits(inner))
                    {
                        edges.Add(new Edge(inner, outer));
                        vertices.RemoveAt(i--);
                        vertices.RemoveAt(j);
                        Console.WriteLine("{0} matches {1} ({2} left)", i, j, vertices.Count);
                        break;
                    }
                }
            }

            
        }

        public StegoMessage Extract()
        {
            throw new NotImplementedException();
        }
    }

    class Pixel
    {
        public byte[] Samples;
    }
}
