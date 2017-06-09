using MathNet.Numerics.LinearAlgebra;
using RANSAC.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC.Utilities
{
    class RANSACAffine : RANSAC
    {
        protected override int amountOfPoints()
        {
            return 3;
        }

        protected override Matrix<double> evaluate(List<Tuple<FPoint, FPoint>> samplePairs)
        {
            List<Tuple<FPoint, FPoint>> sampleList = samplePairs.ToList();
            Matrix<double> model = null;
            FPoint kpX1 = sampleList[0].Item1;
            FPoint kpU1 = sampleList[0].Item2;

            FPoint kpX2 = sampleList[1].Item1;
            FPoint kpU2 = sampleList[1].Item2;

            FPoint kpX3 = sampleList[2].Item1;
            FPoint kpU3 = sampleList[2].Item2;

            var matrix1 = CreateMatrix.DenseOfArray<double>(new double[,] {
                {kpX1.X, kpX1.Y, 1, 0, 0, 0},
                {kpX2.X, kpX2.Y, 1, 0, 0, 0},
                {kpX3.X, kpX3.Y, 1, 0, 0, 0},
                {0, 0, 0, kpX1.X, kpX1.Y, 1},
                {0, 0, 0, kpX2.X, kpX2.Y, 1},
                {0, 0, 0, kpX3.X, kpX3.Y, 1}
            });

            var matrix2 = CreateMatrix.DenseOfArray<double>(new double[,] {
                {kpU1.X},
                {kpU2.X},
                {kpU3.X},
                {kpU1.Y},
                {kpU2.Y},
                {kpU3.Y}
            });

            if (matrix1.Determinant() != 0)
            {
                var equationResult = matrix1.Inverse().Multiply(matrix2);
                model = CreateMatrix.DenseOfArray<double>(new double[,] {
                    {equationResult[0,0], equationResult[1,0], equationResult[2, 0]},
                    {equationResult[3,0], equationResult[4,0], equationResult[5,0]},
                    {0,0,1}
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
