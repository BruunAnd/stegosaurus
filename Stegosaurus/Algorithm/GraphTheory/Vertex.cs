using System;
using System.Collections.Generic;



namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Vertex
    {
        public Sample[] Samples;
        public List<Edge> Edges = new List<Edge>();
        public byte Value;
        public bool IsValid;

        public Vertex(Sample[] _samples)
        {
            Samples = _samples;
            IsValid = true;
        }

        public override string ToString()
        {
            string message = $"E:{Edges.Count ,4}";
            foreach (Sample sample in Samples)
            {
                message += $"V:{sample.ModValue}, T:{sample.TargetValue}|";
            }

            return message;
        }
    }
}
