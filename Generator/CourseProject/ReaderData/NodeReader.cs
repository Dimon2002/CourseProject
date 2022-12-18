using CourseProject.DataStucters.Config;
using DataStucters.Grid;

namespace CourseProject.ReaderData;

internal class NodeReader : IReader<Node>
{
    internal override List<Node> Read()
    {
        using StreamReader NodeReader = new(Config.Root + Config.NodeFile);

        string NodeText = NodeReader.ReadLine();
        
        List<Node> NodeElements = new(int.Parse(NodeText));

        while ((NodeText = NodeReader.ReadLine()) != null)
        { 
            var nodeArray = double.Parse(NodeText);
            NodeElements.Add(new Node(nodeArray));
        }

        return NodeElements;
    }
}