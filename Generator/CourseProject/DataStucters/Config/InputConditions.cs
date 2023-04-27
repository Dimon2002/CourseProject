using DataStucters.Grid;

namespace Generator.CourseProject.DataStucters.Config;

internal static class InputConditions
{
    internal static List<Conditions> ListConditions = new()
    {
        new Conditions(1,false,1),
        new Conditions(1,true,4),
    };
}