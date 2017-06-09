using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using RANSAC.Structures;

namespace RANSAC.Utilities
{
    class RANSACPerspective : RANSAC
    {
        protected override int amountOfPoints()
        {
            return 4;
        }

        protected override Matrix<double> evaluate(List<Tuple<FPoint, FPoint>> samplePairs)
        {
            List<Tuple<FPoint, FPoint>> sampleList = samplePairs.ToList();
            Matrix<double> model = null;
            FPoint x1 = sampleList[0].Item1;
            FPoint u1 = sampleList[0].Item2;

            FPoint x2 = sampleList[1].Item1;
            FPoint u2 = sampleList[1].Item2;

            FPoint x3 = sampleList[2].Item1;
            FPoint u3 = sampleList[2].Item2;

            FPoint x4 = sampleList[3].Item1;
            FPoint u4 = sampleList[3].Item2;

            var matrix1 = CreateMatrix.DenseOfArray<double>(new double[,] {
                {x1.X, x1.Y, 1, 0, 0, 0, -1 * u1.X * x1.X, -1 * u1.X * x1.Y},
                {x2.X, x2.Y, 1, 0, 0, 0, -1 * u2.X * x2.X, -1 * u2.X * x2.Y},
                {x3.X, x3.Y, 1, 0, 0, 0, -1 * u3.X * x3.X, -1 * u3.X * x3.Y},
                {x4.X, x4.Y, 1, 0, 0, 0, -1 * u4.X * x4.X, -1 * u4.X * x4.Y},
                {0, 0, 0, x1.X, x1.Y, 1, -1 * u1.Y * x1.X, -1 * u1.Y * x1.Y},
                {0, 0, 0, x2.X, x2.Y, 1, -1 * u2.Y * x2.X, -1 * u3.Y * x1.Y},
                {0, 0, 0, x3.X, x3.Y, 1, -1 * u3.Y * x3.X, -1 * u2.Y * x1.Y},
                {0, 0, 0, x4.X, x4.Y, 1, -1 * u4.Y * x4.X, -1 * u4.Y * x1.Y}
            });

            var matrix2 = CreateMatrix.DenseOfArray<double>(new double[,] {
                {u1.X},
                {u2.X},
                {u3.X},
                {u4.X},
                {u1.Y},
                {u2.Y},
                {u3.Y},
                {u4.Y}
            });

            if (matrix1.Determinant() != 0)
            {
                var equationResult = matrix1.Inverse().Multiply(matrix2);
                model = CreateMatrix.DenseOfArray<double>(new double[,] {
                    {equationResult[0,0], equationResult[1,0], equationResult[2, 0]},
                    {equationResult[3,0], equationResult[4,0], equationResult[5,0]},
                    {equationResult[6,0], equationResult[7,0], 1}
                });
            }

            return model;
        }

        protected override double evaluateError(Matrix<double> model, FPoint keyPoint1, FPoint keyPoint2)
        {
            Matrix<double> secondMatrix = CreateMatrix.DenseOfArray<double>(
                new double[,] {
                    {(double)keyPoint1.X},
                    {(double)keyPoint2.Y},
                    {1.0}
                }
            );
            var timedMatrix = model.Multiply(secondMatrix);
            double estimatedX = timedMatrix[0, 0];
            double estimatedY = timedMatrix[1, 0];

            return Math.Sqrt(Math.Pow(keyPoint2.X - estimatedX, 2) + Math.Pow(keyPoint2.Y - estimatedY, 2));
        }
    }
}
