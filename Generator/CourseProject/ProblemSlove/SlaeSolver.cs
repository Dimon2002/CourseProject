﻿using CourseProject.ProblemSlove.GlobalComponent;

namespace CourseProject.ProblemSlove;

internal class SlaeSolver
{
    private readonly List<int> _ia;
    private readonly List<double> _al;
    private readonly List<double> _di;
    private readonly List<double> _b;
    private readonly List<double> _q;

    public SlaeSolver(GlobalComponents globalComponent)
    {
        _ia = globalComponent.ia;
        _al = globalComponent.al;
        _di = globalComponent.di;
        _b = globalComponent.b;

        _q = new(new double[_b.Count]);
    }

    public List<double> WeightsTakes() => _q;

    public void Slove()
    {
        CholeskyDecomposition();
        ForwardReverse();
    }

    private void CholeskyDecomposition()
    {
        double totalSum;

        _di[0] = Math.Sqrt(_di[0]);                           // l11 = sqrt(a11)

        for (int i = 1; i < _q.Count; i++)                 // пробегаем по строкам матрицы
        {
            int a = i - (_ia[i + 1] - _ia[i]);                  // начало i-й строки в абсолютной нумерации

            for (int j = 0; j < _ia[i + 1] - _ia[i]; j++)     // пробегаем по столбцам до диагонали
            {
                int b = a + j - _ia[a + j + 1] + _ia[a + j];  // начало j-й строки в абсолютной нумерации
                totalSum = _al[_ia[i] + j - 1];

                if (a < b)
                    for (int k = _ia[a + j + 1] - _ia[a + j] - 1; k >= 0; k--)
                        totalSum -= _al[_ia[a + j] + k - 1] * _al[_ia[i] + b - a + k - 1];    // если i-я строка началась раньше j-й строки
                else
                    for (int k = a - b; k < _ia[a + j + 1] - _ia[a + j]; k++)
                        totalSum -= _al[_ia[i] + k - 1 - (a - b)] * _al[_ia[a + j] + k - 1]; // если j-я строка началась раньше i-й строки
                _al[_ia[i] + j - 1] = totalSum / _di[a + j];
            }

            totalSum = _di[i] - Enumerable.Range(_ia[i], _ia[i + 1] - _ia[i]).SelectMany(k => new[] { _al[k - 1] * _al[k - 1] }).Sum();

            _di[i] = Math.Sqrt(totalSum);

            if (_di[i] < 1e-14)
                throw new InvalidOperationException("Decomposition error!");
        }
    }

    private void ForwardReverse()
    {
        _q[0] = _b[0] / _di[0];

        for (int i = 1; i < _q.Count; ++i)
        {
            _q[i] = _b[i];
            for (int j = 0; j < _ia[i + 1] - _ia[i]; ++j)
                _q[i] -= _al[_ia[i] + j - 1] * _q[i - _ia[i + 1] + _ia[i] + j];
            _q[i] /= _di[i];
        }

        for (int i = 0; i < _q.Count; ++i)
            _b[i] = _q[i];

        for (int i = _q.Count - 1; i >= 0; --i)
        {
            _q[i] = _b[i] / _di[i];
            for (int j = 0; j < _ia[i + 1] - _ia[i]; ++j)
                _b[i - _ia[i + 1] + _ia[i] + j] -= _q[i] * _al[_ia[i] + j - 1];
        }
    }

    private void PrintComponentsCholesky()
    {
        Console.WriteLine("al:");
        _al.ForEach(Console.WriteLine);
        Console.WriteLine($"{new string('=', 20)}\ndi:");
        _di.ForEach(Console.WriteLine);
        Console.WriteLine($"{new string('=', 20)}\nia:");
        _ia.ForEach(Console.WriteLine);
        Console.WriteLine($"{new string('=', 20)}\nb:");
        _b.ForEach(Console.WriteLine);
        Console.WriteLine($"{new string('=', 20)}\nq:");
        _q.ForEach(Console.WriteLine);
    }
}