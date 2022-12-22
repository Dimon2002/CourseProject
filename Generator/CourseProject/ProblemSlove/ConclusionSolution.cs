using CourseProject.DataStucters.Config;
using DataStucters.Grid;

namespace CourseProject.ProblemSlove;

internal static class ConclusionSolution
{
    internal static void Print(List<double> q, List<Node> nodes, int NumberFunction = 1)
    {
        double sum1 = 0, sum2 = 0;

        using StreamWriter outWriter = new(Config.Root + Config.Out);

        outWriter.WriteLine("r (узлы)\t\t\tq = u(r)\t\t\tq*\t\t\t\t\t||q - q*||");
        outWriter.WriteLine(new string('=',75));
        for (int i = 0; i < nodes.Count; ++i)
        {
            var qz = Function.Func(NumberFunction, nodes[i].R);
            outWriter.WriteLine($"{nodes[i].R:E}\t\t{q[i]:E}\t\t{qz:E}\t\t{Math.Abs(q[i] - qz):E}");
            outWriter.WriteLine(new string('=', 75));

            sum1 += Math.Pow(q[i] - Function.Func(NumberFunction, nodes[i].R), 2);
            sum2 += Math.Pow(Function.Func(NumberFunction, nodes[i].R), 2);
        }
        outWriter.WriteLine("{0:E}", $"Относительная погрешность ||q* - q|| / ||q*|| = {Math.Sqrt(sum1 / sum2):E15}");
    }
}