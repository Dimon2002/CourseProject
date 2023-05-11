using CourseProject.DataStucters.Config;
using Generator.CourseProject.DataStucters.Grid;

namespace Generator.CourseProject.ReaderData;

internal class TimeGridReader
{ 
    internal static List<TimeGrid> Read()
    {
        using StreamReader TimeReader = new(Config.Root + Config.TimeFile);
        
        string TimeText = TimeReader.ReadLine();

        List<TimeGrid> TimeElements = new(int.Parse(TimeText));

        while ((TimeText = TimeReader.ReadLine()) != null)
        {
            var nodeArray = double.Parse(TimeText);
            TimeElements.Add(new TimeGrid(nodeArray));
        }

        return TimeElements;
    }
}