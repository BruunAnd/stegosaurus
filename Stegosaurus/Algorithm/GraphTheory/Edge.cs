namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Edge
    {
        public int[] Vertices; 
        public short Weight;
        public byte[] BestSwaps;
        public Edge(int _firstVertex, int _secondVertex, short _weight, byte[] _bestSwaps)
        {
            Vertices = new int[] { _firstVertex, _secondVertex };
            BestSwaps = new byte[] { _bestSwaps[0], _bestSwaps[1] };
            Weight = _weight;
        }

        public override string ToString()
        {
            return $"W:{Weight}|{Vertices[0]},{BestSwaps[0]};{Vertices[1]},{BestSwaps[1]}";
        }
    }
}
