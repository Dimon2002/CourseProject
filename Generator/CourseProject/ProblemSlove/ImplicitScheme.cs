using CourseProject.DataStucters.Config;
using CourseProject.ProblemSlove;
using CourseProject.ProblemSlove.GlobalComponent;
using CourseProject.ProblemSlove.Matrix;
using DataStucters.Grid;
using Generator.CourseProject.DataStucters.Config;
using Generator.CourseProject.DataStucters.Grid;

namespace Generator.CourseProject.ProblemSlove;

internal class ImplicitScheme
{
    private GlobalComponents _components;
    private AccountingConditions _conditions;
    private SlaeSolver _solver;

    private List<List<double>> _qt = new(3);

    private readonly List<TimeGrid> _timeGrid = new();

    internal ImplicitScheme(GlobalComponents components, AccountingConditions conditions, List<TimeGrid> timeGrid)
    {
        _timeGrid = timeGrid;
        _components = components;
        _conditions = conditions;

        for (int i = 0; i < 3; i++)
            _qt.Add(new(new double[components.b.Count]));
    }

    public void Compute()
    {
        _solver = new(_components);

        _components.BuildTwoMatrix();

        InitialTimeLayer();

        // Для трехслойной схемы
        for (int i = 2; i < _timeGrid.Count; ++i)
        {
            var dtA = (1 / (_timeGrid[i].T - _timeGrid[i - 2].T))
                    + (1 / (_timeGrid[i].T - _timeGrid[i - 1].T));

            var dt0 = (_timeGrid[i].T - _timeGrid[i - 1].T)
                    / ((_timeGrid[i - 1].T - _timeGrid[i - 2].T) * (_timeGrid[i].T - _timeGrid[i - 2].T));

            var dt1 = (_timeGrid[i].T - _timeGrid[i - 2].T)
                    / ((_timeGrid[i - 1].T - _timeGrid[i - 2].T) * (_timeGrid[i].T - _timeGrid[i - 1].T));

            _components.BuildA(dtA);
            _components.BuildVector(_qt[0], _qt[1], dt0, dt1, _timeGrid[i].T);
            _conditions.ConsiderAccountingConditions(_timeGrid[i].T);
            _solver.Slove();
            
            _qt[2] = new(_solver.WeightsTakes());

            _components.CleanData();

            ConclusionSolution.Print(_qt[2], _components._matrixPortrait._grid.Nodes, _timeGrid[i].T, Area.NumberFunction);

            SwapStratumSolution();
        }

    }

    private void InitialTimeLayer()
    {
        var nodes = AddInternalPoints(_components._matrixPortrait._grid.Nodes);

        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < _qt[k].Count; i++)
            {
                _qt[k][i] = AnalyticalFunction.Compute(Area.NumberFunction, nodes[i].R , _timeGrid[k].T);
            }
            ConclusionSolution.Print(_qt[k], _components._matrixPortrait._grid.Nodes, _timeGrid[k].T, Area.NumberFunction);
        }
    }

    private static List<Node> AddInternalPoints(List<Node> oldNodes)
    {
        List<Node> newNodes = new();
        Node ri = new(oldNodes[0].R);

        for (int i = 1; i < oldNodes.Count; i++)
        {
            newNodes.Add(ri);
            newNodes.Add((oldNodes[i] - ri) / 2 + ri);
            ri = new(oldNodes[i].R);
        }

        newNodes.Add(oldNodes[^1]);

        return newNodes;
    }

    private void SwapStratumSolution()
    {
        (_qt[0], _qt[1], _qt[2]) = (_qt[1], _qt[2], Enumerable.Repeat(0.0D, _qt[2].Count).ToList());
    }
}
