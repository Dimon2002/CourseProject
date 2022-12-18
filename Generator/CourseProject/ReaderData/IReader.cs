namespace CourseProject;

internal abstract class IReader<T>
{
    internal abstract List<T> Read();
}