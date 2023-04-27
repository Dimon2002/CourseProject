namespace CourseProject.DataStucters.FileGenerator;
using static Math;
internal static class ElementGenerator
{
    internal static void Generate(double rStart, double rEnd, double h, int NumberFucntion = 0)
    {
        using StreamWriter ElementWriter = new(Config.Config.Root + Config.Config.ElemFile);

        var difference = rEnd - rStart;
        var CountElements = difference % h == 0
                 ? Truncate(difference / h)
                 : Truncate(difference / h) + 1;

        ElementWriter.WriteLine(CountElements);

        for (int i = 1; i <= CountElements; i++)
            ElementWriter.WriteLine(i - 1 + " "
                                    + i + " " 
                                    + Convert.ToDouble(Config.Materials.Gamma) + " " 
                                    + Convert.ToDouble(Config.Materials.Diffusion) + " " 
                                    + NumberFucntion);
    }
}