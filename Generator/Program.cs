using CourseProject.DataStucters;
using CourseProject.DataStucters.Config;
using CourseProject.DataStucters.FileGenerator;
using CourseProject.ProblemSlove;
using CourseProject.ProblemSlove.GlobalComponent;
using CourseProject.ProblemSlove.Matrix;
using Generator.CourseProject.DataStucters.Config;
using Generator.CourseProject.ProblemSlove;
using Generator.CourseProject.ReaderData;

namespace CourseProject;

internal class Program
{
    static void Main()
    {
        File.Delete(Config.Root + Config.Out);

        var timeGrid = TimeGridReader.Read();

        if (timeGrid.Count < 3)
            throw new InvalidDataException("Incorrect time grid!!!");

        if (Area.rStart > Area.rEnd || Area.Step == 0)
            throw new InvalidDataException("Incorrect calculated area!!!");

        // Генерация входных файлов
        NodeGenerator.Generate(Area.rStart, Area.rEnd, Area.Step);
        ElementGenerator.Generate(Area.rStart, Area.rEnd, Area.Step, Area.NumberFunction);
        BoundaryСonditionsGenerator.Generate(InputConditions.ListConditions[0], InputConditions.ListConditions[1]);
         
        // Считываем сетку и строим портрет
        IGridFactory grid = new GridFactory();
        MatrixPortrait Portrait = new(grid);

        // Создаем экземляры классов
        GlobalComponents globalComponents = new(Portrait);
        AccountingConditions conditions = new(globalComponents);

        ImplicitScheme myScheme = new(globalComponents, conditions, timeGrid);
        myScheme.Compute();

        Console.WriteLine("Completed");
    }
}