using CourseProject.DataStucters.Config;

namespace CourseProject.ProblemSlove.GlobalComponent;

internal class GlobalComponents
{
    public readonly MatrixPortrait _matrixPortrait;

    public readonly int _countElements;
    public List<int> ia { get; private set; }
    public List<double> di { get; private set; }
    public List<double> al { get; private set; }
    public List<double> b { get; private set; }

    public GlobalComponents(MatrixPortrait matrixPortrait)
    {
        _matrixPortrait = matrixPortrait;
        ia = matrixPortrait.ia;

        di = new(new double[2 * matrixPortrait.ElementsCount + 1]);
        al = new(new double[matrixPortrait.ia.Last() + 1]);
        b = new(new double[2 * matrixPortrait.ElementsCount + 1]);
        _countElements = matrixPortrait.ElementsCount;
    }

    public void CreateGlobalComponents()
    {
        for (int i = 0; i < _countElements; i++)
        {
            var DiffusionOnElement = _matrixPortrait.Diffusion(i);
            var GammaOnElement = _matrixPortrait.Gamma(i);
            var rk = _matrixPortrait.R(i);
            var hk = _matrixPortrait.R(i + 1) - rk;

            LocalMatrixStiffness(i, rk, hk, DiffusionOnElement);
            LocalMatrixMass(i, rk, hk, GammaOnElement);
            LocalVector(i, rk, hk);
        }
    }

    private void LocalMatrixStiffness(
                                 in int i,
                                 in double rk,
                                 in double hk,
                                 in double DiffusionOnElem)
    {
        var c = CoefficientFrontMatrixStiffness(DiffusionOnElem, hk);

        al[i * 3] += c * (-8 * rk - 2 * hk);
        al[i * 3 + 1] += c * (rk + hk / 2);
        al[i * 3 + 2] += c * (-8 * rk - 6 * hk);

        di[i * 2] += c * ((14 * rk + 3 * hk) / 2);
        di[i * 2 + 1] += c * (16 * rk + 8 * hk);
        di[i * 2 + 2] += c * (7 * rk + 5.5D * hk);
    }

    private void LocalMatrixMass(
                                 in int i,
                                 in double rk,
                                 in double hk,
                                 in double GammaOnElement)
    {
        var c = CoefficientFrontMatrixMass(GammaOnElement, hk);

        al[i * 3] += c * 2 * rk;
        al[i * 3 + 1] += c * (-rk - hk / 2);
        al[i * 3 + 2] += c * 2 * (rk + hk);

        di[i * 2] += c * (4 * rk + hk / 2);
        di[i * 2 + 1] += c * (16 * rk + 8 * hk);
        di[i * 2 + 2] += c * (4 * rk + 3.5D * hk);
    }

    private void LocalVector(
                             in int i,
                             in double rk,
                             in double hk)
    {
        var fNumber = _matrixPortrait.NumberFunction(i);

        var Middle = (2 * rk + hk) / 2;
        var End = rk + hk;

        var c = CoefficientFrontVector(hk);

        b[i * 2] += c * ((Function.Func(fNumber, rk) * (8 * rk + hk) / 2)
                    + (Function.Func(fNumber, Middle) * 2 * rk)
                    + (Function.Func(fNumber, End) * ((-2 * rk - hk) / 2)));

        b[i * 2 + 1] += c * ((Function.Func(fNumber, rk) * 2 * rk)
                        + (Function.Func(fNumber, Middle) * 8 * (2 * rk + hk))
                        + (Function.Func(fNumber, End) * 2 * (rk + hk)));

        b[i * 2 + 2] += c * ((-Function.Func(fNumber, rk) * ((2 * rk + hk) / 2))
                        + (Function.Func(fNumber, Middle) * 2 * (rk + hk))
                        + (Function.Func(fNumber, End) * (4 * rk + 3.5D * hk)));

    }

    private static double CoefficientFrontMatrixStiffness(double upperValue, double lowerValue) =>
        upperValue / (3 * lowerValue);
    private static double CoefficientFrontMatrixMass(double upperValue1, double upperValue2) =>
        (upperValue1 * upperValue2) / 30;
    private static double CoefficientFrontVector(double value) =>
        value / 30;
}