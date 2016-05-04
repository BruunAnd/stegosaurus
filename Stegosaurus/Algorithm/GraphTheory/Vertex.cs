using System;
using System.Collections.Generic;



namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Vertex
    {
        public List<Sample> Samples { get; set; }
        public int Value { get; set; }

        public Vertex(List<Sample> _samples)
        {
            Samples = _samples;
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
