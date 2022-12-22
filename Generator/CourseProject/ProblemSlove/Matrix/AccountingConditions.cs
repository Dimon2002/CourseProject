using CourseProject.ProblemSlove.GlobalComponent;
using CourseProject.ReaderData;
using DataStucters.Grid;

namespace CourseProject.ProblemSlove.Matrix;

internal class AccountingConditions
{
    public readonly GlobalComponents _globalComponents;
 
    private readonly IReader<Conditions> _reader = new BoundaryConditionsReader();
    private readonly List<Conditions> _conditions = new();

    public AccountingConditions(GlobalComponents globalComponents)
    {
        _conditions = _reader.Read();
        _globalComponents = globalComponents;
    }

    public void ConsiderAccountingConditions()
    {
        foreach (var condition in _conditions.OrderByDescending(item => item.TypeConditions))
        {
            switch (condition.TypeConditions)
            {
                case 1:
                    AccountingFirstConditions(condition.Side, condition.Value);
                    break;
                case 2:
                    AccountingSecondConditions(condition.Side,condition.Value);
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
    public void AccountingFirstConditions(bool SideConditions, double value)
    {
        if (SideConditions)
        {
            _globalComponents.di[_globalComponents._matrixPortrait.NodesCount - 1] = 1e+60;
            _globalComponents.b[_globalComponents._matrixPortrait.NodesCount - 1] = 1e+60 * value;
        }
        else
        {
            _globalComponents.di[0] = 1e+60;
            _globalComponents.b[0] = 1e+60 * value;
        }
    }

    // SideConditions: if "true" then
    //                      right boundary conditions
    //                 else
    //                      left  boundary conditions
    public void AccountingSecondConditions(bool SideConditions, double value)
    {
        if (SideConditions)
            _globalComponents.b[^1] += value * _globalComponents._matrixPortrait.LastNode;
        else
            _globalComponents.b[0] -= value * _globalComponents._matrixPortrait.FirstNode;
    }
}