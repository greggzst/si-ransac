using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC.Structures
{
    public class FPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float[] Features { get; set; }

        public FPoint(float x, float y, float[] features)
        {
            X = x;
            Y = y;
            Features = features;
        }

        public double distance(FPoint p)
        {
            return Math.Sqrt(Math.Pow(X - p.X,2) + Math.Pow(Y - p.Y,2));
        }

        public double featuresDistance(FPoint p)
        {
            double distance = 0;
            for(int i = 0; i < Features.Length; i++)
            {
                distance += Math.Pow(Features[i] - p.Features[i], 2);
            }

            return Math.Sqrt(distance);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FPoint p = (FPoint)obj;
            return (X == p.X) && (Y == p.Y) && Features.SequenceEqual(p.Features);
        }
    }
}
