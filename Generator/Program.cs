using CourseProject.DataStucters;
using CourseProject.DataStucters.FileGenerator;
using CourseProject.ProblemSlove;
using CourseProject.ProblemSlove.GlobalComponent;
using CourseProject.ProblemSlove.Matrix;
using DataStucters.Grid;
using Generator.CourseProject.DataStucters.Config;

namespace CourseProject;

internal class Program
{
    static void Main()
    {
        if (Area.rStart > Area.rEnd || Area.Step == 0)
            throw new InvalidDataException("Incorrect calculated area!!!");

        // Генерация входных файлов
        NodeGenerator.Generate(Area.rStart, Area.rEnd, Area.Step);
        ElementGenerator.Generate(Area.rStart, Area.rEnd, Area.Step);
        BoundaryСonditionsGenerator.Generate(InputConditions.ListConditions[0], InputConditions.ListConditions[1]);

        // Считываем сетку и строим портрет
        IGridFactory grid = new GridFactory();
        MatrixPortrait Portrait = new(grid);

        // Собираем глобальные элементы СЛАУ
        GlobalComponents globalComponents = new(Portrait);
        globalComponents.CreateGlobalComponents();

        // Учитываем краевые условия
        AccountingConditions conditions = new(globalComponents);
        conditions.ConsiderAccountingConditions();

        // Решение Слау
        SlaeSolver solver = new(conditions._globalComponents);
        solver.Slove();

        // Вывод решения: u(r)
        ConclusionSolution.Print(solver.WeightsTakes(), Portrait._grid.Nodes);
    }
}