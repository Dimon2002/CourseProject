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

    private List<double> Mal { get; set; }
    private List<double> Mdi { get; set; }
    private List<double> Gal { get; set; }
    private List<double> Gdi { get; set; }

    public GlobalComponents(MatrixPortrait matrixPortrait)
    {
        _matrixPortrait = matrixPortrait;
        ia = matrixPortrait.ia;

        al = new(new double[3 * _matrixPortrait.ElementsCount]);
        Mal = new(new double[3 * _matrixPortrait.ElementsCount]);
        Gal = new(new double[3 * _matrixPortrait.ElementsCount]);

        di = new(new double[2 * _matrixPortrait.ElementsCount + 1]);
        Mdi = new(new double[2 * _matrixPortrait.ElementsCount + 1]);
        Gdi = new(new double[2 * _matrixPortrait.ElementsCount + 1]);

        b = new(new double[2 * _matrixPortrait.ElementsCount + 1]);
        _countElements = matrixPortrait.ElementsCount;
    }

    public void BuildTwoMatrix()
    {
        MatrixStiffness();
        MatrixMass();
    }

    private void MatrixStiffness()
    {
        for (int i = 0; i < _countElements; i++)
        {
            var rk = _matrixPortrait.R(i);
            var hk = _matrixPortrait.R(i + 1) - rk;

            var c = CoefficientFrontMatrixStiffness(_matrixPortrait.Diffusion(i), hk);

            Gal[i * 3] += c * (-8 * rk - 2 * hk);
            Gal[i * 3 + 1] += c * (rk + hk / 2);
            Gal[i * 3 + 2] += c * (-8 * rk - 6 * hk);

            Gdi[i * 2] += c * ((14 * rk + 3 * hk) / 2);
            Gdi[i * 2 + 1] += c * (16 * rk + 8 * hk);
            Gdi[i * 2 + 2] += c * (7 * rk + 5.5D * hk);
        }
    }
    private void MatrixMass()
    {
        for (int i = 0; i < _countElements; i++)
        {
            var rk = _matrixPortrait.R(i);
            var hk = _matrixPortrait.R(i + 1) - rk;

            var c = CoefficientFrontMatrixMass(_matrixPortrait.Gamma(i), hk);

            Mal[i * 3] += c * 2 * rk;
            Mal[i * 3 + 1] += c * (-rk - hk / 2);
            Mal[i * 3 + 2] += c * 2 * (rk + hk);

            Mdi[i * 2] += c * (4 * rk + hk / 2);
            Mdi[i * 2 + 1] += c * (16 * rk + 8 * hk);
            Mdi[i * 2 + 2] += c * (4 * rk + 3.5D * hk);
        }
    }

    public void BuildA(double dt)
    {
        for (int i = 0; i < Mdi.Count; i++)
        {
            di[i] += Mdi[i] * dt + Gdi[i];
        }

        for (int i = 0; i < Mal.Count; i++)
        {
            al[i] += Mal[i] * dt + Gal[i];
        }
    }

    public void BuildVector(List<double> q0, List<double> q1, double dt1, double dt2, in double t)
    {
        for (int i = 0; i < _countElements; i++)
        {
            var rk = _matrixPortrait.R(i);
            var hk = _matrixPortrait.R(i + 1) - rk;

            LocalVector(i, rk, hk, t);
        }

        var d1 = MultiplyMatrixOnOldVector(q0).Select(value => value * dt1).ToList();
        var d2 = MultiplyMatrixOnOldVector(q1).Select(value => value * dt2).ToList();

        for (int i = 0; i < b.Count; i++)
            b[i] += d2[i] - d1[i];
    }

    public void CleanData()
    {
        for (int i = 0; i < al.Count; i++) al[i] = 0.0D;
        for (int i = 0; i < di.Count; i++) di[i] = 0.0D;
        for (int i = 0; i < b.Count; i++) b[i] = 0.0D;
    }

    private List<double> MultiplyMatrixOnOldVector(List<double> q)
    {
        List<double> resultMultiply = new(new double[q.Count]);

        resultMultiply[0] += Mdi[0] * q[0] + Mal[0] * q[1] + Mal[1] * q[2];

        int c = 0;

        Enumerable.Range(1, resultMultiply.Count - 2).ToList().ForEach(i =>
        {
            resultMultiply[i] += Mdi[i] * q[i];

            resultMultiply[i] += i % 2 == 1
            ? Mal[i - 1 + c] * q[i - 1]
             + Mal[i + 1 + c] * q[i + 1]
            : Mal[i - 1 + c] * q[i - 2]
             + Mal[i + c] * q[i - 1]
             + Mal[i + 1 + c] * q[i + 1]
             + Mal[i + 2 + c] * q[i + 2];

            if (i % 2 == 0)
                c++;
        });

        resultMultiply[^1] += Mdi.Last() * q.Last() + Mal.Last() * q[^2] + Mal[^2] * q[^3];

        return resultMultiply;
    }

    private void LocalVector(
                         in int i,
                         in double rk,
                         in double hk,
                         in double t)
    {
        var fNumber = _matrixPortrait.NumberFunction(i);

        var Middle = (2 * rk + hk) / 2;
        var End = rk + hk;

        var c = CoefficientFrontVector(hk);

        b[i * 2] += c * ((NumericalFunction.Compute(fNumber, rk, t) * (8 * rk + hk) / 2)
                    + (NumericalFunction.Compute(fNumber, Middle, t) * 2 * rk)
                    + (NumericalFunction.Compute(fNumber, End, t) * ((-2 * rk - hk) / 2)));

        b[i * 2 + 1] += c * ((NumericalFunction.Compute(fNumber, rk, t) * 2 * rk)
                        + (NumericalFunction.Compute(fNumber, Middle, t) * 8 * (2 * rk + hk))
                        + (NumericalFunction.Compute(fNumber, End, t) * 2 * (rk + hk)));

        b[i * 2 + 2] += c * ((-NumericalFunction.Compute(fNumber, rk, t) * ((2 * rk + hk) / 2))
                        + (NumericalFunction.Compute(fNumber, Middle, t) * 2 * (rk + hk))
                        + (NumericalFunction.Compute(fNumber, End, t) * (4 * rk + 3.5D * hk)));
    }

    private static double CoefficientFrontMatrixStiffness(double upperValue, double lowerValue) =>
        upperValue / (3 * lowerValue);
    private static double CoefficientFrontMatrixMass(double upperValue1, double upperValue2) =>
        (upperValue1 * upperValue2) / 30;
    private static double CoefficientFrontVector(double value) =>
        value / 30;
}