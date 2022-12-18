namespace CourseProject.DataStucters.Config;

internal static class Function
{
    internal static double Func(in int NumberFunction, in double R) => NumberFunction switch
    {
        0 => 5,
        1 => R,
        _ => 0,
    };
}