using CourseProject.DataStucters.Config;
using DataStucters.Grid;

namespace CourseProject.ReaderData;

internal class ElementReader : IReader<Element>
{
    internal override List<Element> Read()
    {
        using StreamReader ElementReader = new(Config.Root + Config.ElemFile);

        string elementText = ElementReader.ReadLine();

        List<Element> listElement = new(int.Parse(elementText));

        while ((elementText = ElementReader.ReadLine()) != null)
        {
            var elemArray = elementText.Split(" ").Select(item => double.Parse(item)).ToArray();
            listElement.Add(new Element(new int[] { Convert.ToInt32(elemArray[0]), Convert.ToInt32(elemArray[1]) }, elemArray[2], elemArray[3], Convert.ToInt32(elemArray[4])));
        }

        return listElement;
    }
}