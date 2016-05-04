namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Edge
    {
        public Vertex First, Second;

        public Edge(Vertex _firstVertex, Vertex _secondVertex)
        {
            First = _firstVertex;
            Second = _secondVertex;

            First.IsInEdge = true;
            Second.IsInEdge = true;
        }

    }
}
