namespace CourseProject.DataStucters.FileGenerator;

using static Math;
internal static class NodeGenerator
{
    internal static void Generate(double rStart, double rEnd, double h)
    {
        using StreamWriter Nodewriter = new(Config.Config.Root + Config.Config.NodeFile);

        var difference = rEnd - rStart;

        var CountNode = difference % h == 0
            ? Truncate(difference / h) + 1
            : Truncate(difference / h) + 2;

        Nodewriter.WriteLine(CountNode);

        Nodewriter.WriteLine(rStart);
        while ((rStart += h) < rEnd)
            Nodewriter.WriteLine(rStart);
        Nodewriter.WriteLine(rEnd);
   }
}