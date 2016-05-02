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
            weight = GetLowestWeight();
        }

        private int GetLowestWeight()
        {
            foreach (var item in Vertices[0].)
            {

            }

            return 0;
        }
    }
}
