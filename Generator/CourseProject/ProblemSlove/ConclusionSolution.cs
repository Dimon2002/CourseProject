using CourseProject.DataStucters.Config;
using DataStucters.Grid;

namespace CourseProject.ProblemSlove;

internal static class ConclusionSolution
{
    internal static void Print(List<double> q, List<Node> nodes)
    {
        double qz, sum1 = 0, sum2 = 0;

        using StreamWriter outWriter = new(Config.Root + Config.Out);

        outWriter.WriteLine("x (узлы)\t\tq = u(x) (численное значение)\t\tq* (точное значение)\t\t||q - q*|| (погрешность)\n");

        for (int i = 0; i < nodes.Count; ++i)
        {
            qz = Function.Func(0, nodes[i].R);
            outWriter.WriteLine("{0:E}", $"{nodes[i].R}\t\t{q[i]}\t\t{qz}\t\t{Math.Abs(q[i] - qz)}");
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            sum1 += Math.Pow(q[i] - Function.Func(0, nodes[i].R), 2);
            sum2 += Math.Pow(Function.Func(0, nodes[i].R), 2);
        }

        outWriter.WriteLine("{0:E}", $"Относительная погрешность ||q* - q|| / ||q*|| = {Math.Sqrt(sum1 / sum2)}");
    }
}
