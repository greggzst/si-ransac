using RANSAC.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC.Utilities
{
    class RANSAC
    {
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
    }
}
