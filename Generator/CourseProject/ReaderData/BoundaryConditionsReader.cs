using CourseProject.DataStucters.Config;
using DataStucters.Grid;

namespace CourseProject.ReaderData;

internal class BoundaryConditionsReader : IReader<Conditions>
{
    internal override List<Conditions> Read()
    {
        using StreamReader ConditionsReader = new(Config.Root + Config.BoundaryСonditions);

        string conditionText;

        List<Conditions> listConditions = new();

        while ((conditionText = ConditionsReader.ReadLine()) != null)
        {
            var conditionsArray = conditionText.Split(" ").ToArray();
            listConditions.Add(new Conditions(Convert.ToInt32(conditionsArray[0]), Convert.ToBoolean(conditionsArray[1]), Convert.ToDouble(conditionsArray[2])));
        }

        return listConditions;
    }
}
