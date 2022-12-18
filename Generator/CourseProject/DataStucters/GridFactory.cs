using DataStucters.Grid;
using CourseProject.ReaderData;

namespace CourseProject.DataStucters;

internal class GridFactory : IGridFactory
{
    private readonly IReader<Node> _nodeReader = new NodeReader();
    private readonly IReader<Element> _elementReader = new ElementReader();

    public Grid CreateGrid()
    {
        var elements = _elementReader.Read();
        var nodes = _nodeReader.Read();

        return elements == null || nodes == null // ?
            ? throw new InvalidDataException()
            : new Grid(elements, nodes);
    }
}
