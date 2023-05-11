using CourseProject.DataStucters.Config;
using CourseProject.ProblemSlove.GlobalComponent;
using CourseProject.ReaderData;
using DataStucters.Grid;
using Generator.CourseProject.DataStucters.Config;

namespace CourseProject.ProblemSlove.Matrix;

internal class AccountingConditions
{
    public readonly GlobalComponents _globalComponents;
 
    private readonly IReader<Conditions> _reader = new BoundaryConditionsReader();
    private readonly List<Conditions> _conditions = new();
    private readonly List<Node> _nodes;

    public AccountingConditions(GlobalComponents globalComponents)
    {
        _conditions = _reader.Read();
        _globalComponents = globalComponents;
        _nodes = _globalComponents._matrixPortrait._grid.Nodes;
    }

    public void ConsiderAccountingConditions(in double t)
    {
        foreach (var condition in _conditions.OrderByDescending(item => item.TypeConditions))
        {
            switch (condition.TypeConditions)
            {
                case 1:
                    AccountingFirstConditions(condition.Side, t);
                    break;
                case 2:
                    AccountingSecondConditions(condition.Side, t);
                    break;
                default:
                    break;
            }
        }
    }

    // SideConditions: if true then
    //                      right boundary conditions
    //                 else
    //                      left  boundary conditions
    public void AccountingFirstConditions(bool SideConditions, double t)
    {
        if (SideConditions)
        {
            _globalComponents.di[^1] = 1e+60;
            _globalComponents.b[^1] = 1e+60 * AnalyticalFunction.Compute(Area.NumberFunction, _nodes[^1].R, t);
        }
        else
        {
            _globalComponents.di[0] = 1e+60;
            _globalComponents.b[0] = 1e+60 * AnalyticalFunction.Compute(Area.NumberFunction, _nodes[0].R, t);
        }
    }
    
    // SideConditions: if "true" then
    //                      right boundary conditions
    //                 else
    //                      left  boundary conditions

    // TODO: Переписать вторые краевые
    public void AccountingSecondConditions(bool SideConditions, double t)
    {
        if (SideConditions)
            _globalComponents.b[^1] += t * _globalComponents._matrixPortrait.LastNode;
        else
            _globalComponents.b[0] -= t * _globalComponents._matrixPortrait.FirstNode;
    }
}