namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Edge
    {
        public Vertex[] Vertices = new Vertex[2];
        int weight;

        public Edge(Vertex _firstVertex, Vertex _secondVertex)
        {
            Vertices[0] = _firstVertex;
            Vertices[1] = _secondVertex;
        }

        public override string ToString()
        {
            return $"{Vertices[0]};{Vertices[1]}";
        }
    }
}
