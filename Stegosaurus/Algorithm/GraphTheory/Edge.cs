namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Edge
    {
        public Vertex[] Vertices = new Vertex[2];
        public short Weight;
        public byte[] BestSwaps;
        public Edge(Vertex _firstVertex, Vertex _secondVertex, short _weight, byte[] _bestSwaps)
        {
            Vertices[0] = _firstVertex;
            Vertices[1] = _secondVertex;
            Weight = _weight;
            BestSwaps = (byte[])_bestSwaps.Clone();
        }

        public override string ToString()
        {
            return $"W:{Weight}||{Vertices[0]};{Vertices[1]}";
        }
    }
}
