namespace DataStucters.Grid;
public readonly record struct Node(double R)
{
    public static Node operator +(Node nodeLeft, Node nodeRight)
    {
        return new Node(nodeLeft.R +  nodeRight.R);
    }

    public static Node operator -(Node nodeLeft, Node nodeRight)
    {
        return new Node(nodeLeft.R - nodeRight.R);
    }

    public static Node operator /(Node valueLeft, double valueRight)
    {
        return new Node(valueLeft.R / valueRight);
    }

    public static Node operator *(Node valueLeft, double valueRight)
    {
        return new Node(valueLeft.R * valueRight);
    }
}
