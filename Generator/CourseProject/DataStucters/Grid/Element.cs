namespace DataStucters.Grid;

public readonly record struct Element(int[] GlobalNodeIndexs, double Gamma, double Diffusion, int NumberFunction);