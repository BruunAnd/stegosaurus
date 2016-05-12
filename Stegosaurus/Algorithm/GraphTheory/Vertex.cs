using System;
using System.Collections.Generic;



namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Vertex
    {
        public Sample[] Samples;
        public List<Edge> Edges = new List<Edge>();
        public short Value;
        public short ValueDif;
        public int numEdges = 0;
        public bool IsValid;

        public Vertex(Sample[] _samples)
        {
            Samples = _samples;
            IsValid = true;
        }
        public override string ToString()
        {
            string message = "";
            foreach (Sample sample in Samples)
            {
                message += $"V:{sample.Value}, T:{sample.TargetValue}|";
            }

            return message;
        }
    }
}
