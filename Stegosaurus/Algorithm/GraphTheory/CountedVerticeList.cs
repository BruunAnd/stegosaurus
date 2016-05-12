using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class CountedVerticeList
    {
        public int Count = 0;
        public List<Vertex> vertices = new List<Vertex>();

        public CountedVerticeList()
        {

        }
        public void Add(Vertex _newVertex)
        {

            vertices.Add(_newVertex);
            Count++;
        }

        public Vertex Get(int _index)
        {
            return vertices[_index];
        }
    }
}
