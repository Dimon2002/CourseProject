using DataStucters.Grid;

namespace CourseProject.DataStucters.FileGenerator;

internal class BoundaryСonditionsGenerator
{
    internal static void Generate(Conditions Condition1, Conditions Condition2)
    {
        using StreamWriter ElementWriter = new(Config.Config.Root + Config.Config.BoundaryСonditions);

        ElementWriter.WriteLine(Condition1.TypeConditions + " " + Condition1.Side + " " + Condition1.Value);
        ElementWriter.WriteLine(Condition2.TypeConditions + " " + Condition2.Side + " " + Condition2.Value);
    }
}