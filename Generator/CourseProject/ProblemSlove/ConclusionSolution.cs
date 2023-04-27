﻿using CourseProject.DataStucters.Config;
using DataStucters.Grid;

namespace CourseProject.ProblemSlove;

internal static class ConclusionSolution
{
    internal static List<Node> AddInternalPoints(List<Node> oldNodes)
    {
        List<Node> newNodes = new();
        Node ri = new(oldNodes[0].R);

        for (int i = 1; i < oldNodes.Count; i++)
        {
            newNodes.Add(ri);
            newNodes.Add((oldNodes[i] - ri) / 2 + ri);
            ri = new(oldNodes[i].R);
        }

        newNodes.Add(oldNodes[^1]);

        return newNodes;
    }
    internal static void Print(List<double> q, List<Node> nodes, int NumberFunction = 0)
    {
        double sum1 = 0, sum2 = 0;

        var newNodes = AddInternalPoints(nodes);
        
        using StreamWriter outWriter = new(Config.Root + Config.Out);

        Console.WriteLine("r (узлы)\tq = u(r)\t\tq*\t\t\t||q - q*||");
        Console.WriteLine(new string('=', 75));
        
        outWriter.WriteLine("r (узлы)\tq = u(r)\t\t\tq*(Аналитическое)\t||q - q*||");
        outWriter.WriteLine(new string('=', 75));
        
        for (int i = 0; i < q.Count; ++i)
        {
            if (i % 2 == 0)  
                Console.ForegroundColor = ConsoleColor.Green;

            var qz = Function.AnalyticalFunc(NumberFunction, newNodes[i].R);

            Console.WriteLine($"" +
                $"{newNodes[i].R:F5}\t\t" +
                $"{q[i]:E}\t\t" +
                $"{qz:E}\t\t" +
                $"{Math.Abs(q[i] - qz):E}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('=', 75));

            outWriter.WriteLine($"" +
                $"{newNodes[i].R:F5}\t\t" +
                $"{q[i]:E}\t\t" +
                $"{qz:E}\t\t" +
                $"{Math.Abs(q[i] - qz):E}");

            outWriter.WriteLine(new string('=', 75));

            sum1 += Math.Pow(q[i] - Function.AnalyticalFunc(NumberFunction, newNodes[i].R), 2);
            sum2 += Math.Pow(Function.AnalyticalFunc(NumberFunction, newNodes[i].R), 2);
        }

        Console.WriteLine($"Относительная погрешность ||q* - q|| / ||q*|| = {Math.Sqrt(sum1 / sum2):E15}");

        outWriter.WriteLine($"Относительная погрешность ||q* - q|| / ||q*|| = {Math.Sqrt(sum1 / sum2):E15}");
    }
}