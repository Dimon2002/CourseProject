namespace CourseProject.DataStucters.Config;

internal static class AnalyticalFunction
{
    internal static double Compute(in int NumberFunction, in double R, in double t) => NumberFunction switch
    {
        0 => t,
        1 => Math.Pow(R, 2) * Math.Pow(t, 2),
        2 => Math.Pow(t, 3),
        3 => Math.Cos(R) * t,
        _ => throw new InvalidDataException("Отсутствует функия для вычисления!"),
    };
}

internal static class NumericalFunction
{
    internal static double Compute(in int NumberFunction, in double R, in double t) => NumberFunction switch
    {
        0 => 2,
        1 => 2 * Math.Pow(R, 2) * t,
        2 => 3 * Math.Pow(t, 2),
        3 => Math.Cos(R) + (Math.Sin(R) * t / R) + t * Math.Cos(R),
        _ => throw new InvalidDataException("Отсутствует функия для вычисления!"),
    };
}