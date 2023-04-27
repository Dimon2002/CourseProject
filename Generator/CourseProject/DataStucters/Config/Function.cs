namespace CourseProject.DataStucters.Config;

internal static class Function
{
    internal static double Func(in int NumberFunction, in double R) => NumberFunction switch
    {
        0 => -4,
        1 => 5,
        2 => 1 / R + R,
        3 => R,
        _ => 0,
    };

    internal static double AnalyticalFunc(in int NumberFunction, in double R) => NumberFunction switch
    {
        0 => R * R,
        1 => 5,
        2 => R,
        3 => R,
        _ => 0,
    };
}