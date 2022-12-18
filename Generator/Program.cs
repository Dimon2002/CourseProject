using CourseProject.DataStucters;
using CourseProject.DataStucters.FileGenerator;
using CourseProject.ProblemSlove;
using CourseProject.ProblemSlove.GlobalComponent;
using CourseProject.ProblemSlove.Matrix;
using DataStucters.Grid;

namespace CourseProject;

internal class Program
{
    const double rStart = 1D;
    const double rEnd = 3D;
    const double Step = 1D;

    static void Main()
    {
        // Генерация входных файлов
        NodeGenerator.Generate(rStart, rEnd, Step);
        ElementGenerator.Generate(rStart, rEnd, Step);
        BoundaryСonditionsGenerator.Generate(new Conditions(1, false, 1), new Conditions(1, true, 6));

        // Считываем сетку и строим портрет
        IGridFactory grid = new GridFactory();
        MatrixPortrait Portrait = new(grid);

        // Собираем глобальные элементы СЛАУ
        GlobalComponents globalComponents = new(Portrait);
        globalComponents.CreateGlobalComponents();

        // Учитываем краевые условия
//        AccountingConditions conditions = new(globalComponents);
//        conditions.ConsiderAccountingConditions();

        // Решение Слау
        SlaeSolver solver = new(globalComponents);
        if (solver.Slove() != "OK")
            throw new InvalidDataException("Decomposition error!");

        // Вывод решения: u(r)
        // ConclusionSolution.Print(solver.WeightsTakes(), Portrait._grid.Nodes);
    }
}