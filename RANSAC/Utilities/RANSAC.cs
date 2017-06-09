using MathNet.Numerics.LinearAlgebra;
using RANSAC.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC.Utilities
{
    public abstract class RANSAC
    {
        protected abstract Matrix<double> evaluate(List<Tuple<FPoint, FPoint>> samplePairs);
        protected abstract double evaluateError(Matrix<double> model, FPoint keyPoint1, FPoint keyPoint2);


        private List<Tuple<FPoint,FPoint>> getSamples(List<Tuple<FPoint,FPoint>> keyPointsPairs, int amountOfSamples)
        {
            var copy = keyPointsPairs.ToList();
            var result = new List<Tuple<FPoint, FPoint>>();
            Random random = new Random();

            for (int i = 0; i < amountOfSamples; i++)
            {
                int index = random.Next(copy.Count);
                result.Add(copy[index]);
                copy.RemoveAt(index);
            }

            return result;
        }

        public List<Tuple<FPoint, FPoint>> transform(List<Tuple<FPoint, FPoint>> pairs, double imageSize, int iterations, int maxError)
        {
            int bestScore = 0;
            var bestFilteredPairs = new List<Tuple<FPoint, FPoint>>();
            List<Tuple<FPoint, FPoint>> filteredPairs;
            var samplePairs = new List<Tuple<FPoint, FPoint>>();
            Matrix<double> model;
            FPoint kp1;
            FPoint kp2;
            double error;
            for (int i = 0; i < iterations; i++)
            {
                model = null;
                var score = 0;
                filteredPairs = new List<Tuple<FPoint, FPoint>>();
                while (model == null)
                {
                    samplePairs = getSamples(pairs,3);
                    model = evaluate(samplePairs);
                }
                List<Tuple<FPoint, FPoint>> pairsList = pairs.ToList();
                for (int j = 0; j < pairs.Count(); j++)
                {
                    kp1 = pairsList[j].Item1;
                    kp2 = pairsList[j].Item2;
                    error = evaluateError(model, kp1, kp2);
                    if (error < maxError)
                    {
                        score++;
                        filteredPairs.Add(pairsList[j]);
                    }
                }
                if (score > bestScore)
                {
                    bestScore = score;
                    bestFilteredPairs = filteredPairs;
                }
            }
            return bestFilteredPairs;
        }
    }
}
