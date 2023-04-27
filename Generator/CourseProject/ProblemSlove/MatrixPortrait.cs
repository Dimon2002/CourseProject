using CourseProject.DataStucters;
using DataStucters.Grid;

namespace CourseProject.ProblemSlove;

internal class MatrixPortrait
{
    public readonly Grid _grid;

    public int ElementsCount { get => _grid.Elements.Count; private set => _ = _grid.Elements.Count; }
    public int NodesCount { get => _grid.Nodes.Count; private set => _ = _grid.Nodes.Count; }
    public double LastNode { get => _grid.Nodes.LastOrDefault().R; set => _ = _grid.Nodes.LastOrDefault().R; }
    public double FirstNode { get => _grid.Nodes.FirstOrDefault().R; set => _ = _grid.Nodes.FirstOrDefault().R; }

    public List<int> ia;

    public MatrixPortrait(IGridFactory gridFactory)
    {
        _grid = gridFactory.CreateGrid();
        CreateMatrixPortrait();
    }

    public int NumberFunction(in int ElementIndex) =>
        _grid.Elements[ElementIndex].NumberFunction;
    public double Diffusion(in int ElementIndex) =>
       _grid.Elements[ElementIndex].Diffusion;
    public double Gamma(in int ElementIndex) =>
        _grid.Elements[ElementIndex].Gamma;
    public double R(in int ElementIndex) =>
        _grid.Nodes[ElementIndex].R;

    private void CreateMatrixPortrait()
    {
        var AdjacencyList = new List<List<int>>();

        for (int i = 0; i < _grid.Nodes.Count; ++i)
            AdjacencyList.Add(new List<int>());

        foreach (var element in _grid.Elements)
            for (int i = 0; i < element.GlobalNodeIndexs.Length; ++i)
                for (int j = 0; j < i; ++j)
                    AdjacencyList[element.GlobalNodeIndexs[i]].Add(element.GlobalNodeIndexs[j]);

        SetIa(AdjacencyList);
        AdjacencyList.Clear();
    }

    private void SetIa(in List<List<int>> AdjacencyList)
    {
        ia = new(new int[2 * ElementsCount + 2])
        {
            [0] = 1,
            [1] = 1
        };

        for (int i = 2; i < ia.Count; i++)
        {
            ia[i] = ia[i - 1];

            if (i % 2 == 0)
            {
                ia[i] += 1;
                continue;
            }
            ia[i] += 2;
        }
    }

    private static void PrintAdjacencyList(List<List<int>> AdjacencyList)
    {
        for (int i = 0; i < AdjacencyList.Count; i++)
        {
            foreach (var item in AdjacencyList[i])
            {
                Console.Write(i + ":" + item + " ");
            }
            Console.WriteLine();
        }
    }
}