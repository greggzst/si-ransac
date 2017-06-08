﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC.Structures
{
    class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float[] Features { get; set; }

        public Point(float x, float y, float[] features)
        {
            X = x;
            Y = y;
            Features = features;
        }

        public double distance(Point p)
        {
            return Math.Sqrt(Math.Pow(X - p.X,2) + Math.Pow(Y - p.Y,2));
        }
    }
}
