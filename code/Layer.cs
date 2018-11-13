public class Layer
{
    public List<Node> nodes = new List<Node>();

    public Layer(int numNodes)
    {
        for( int i = 0; i < numNodes; i++)
        {
            Node node = new Node();
            nodes.Add(node);
        }
    }
}