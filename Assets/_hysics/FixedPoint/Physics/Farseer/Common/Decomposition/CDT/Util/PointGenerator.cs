using System;
using System.Collections.Generic;
using FP = Spax.FP;
using FPRandom = Spax.FPRandom;

namespace Spax.Physics2D
{
    internal class PointGenerator
    {
        private static readonly FPRandom RNG = FPRandom.New(0);

        public static List<TriangulationPoint> UniformDistribution(int n, FP scale)
        {
            List<TriangulationPoint> points = new List<TriangulationPoint>();
            for (int i = 0; i < n; i++)
            {
                points.Add(new TriangulationPoint(scale*(0.5 - RNG.NextFP()), scale*(0.5 - RNG.NextFP())));
            }
            return points;
        }

        public static List<TriangulationPoint> UniformGrid(int n, FP scale)
        {
            FP x = 0;
            FP size = scale/n;
            FP halfScale = 0.5*scale;

            List<TriangulationPoint> points = new List<TriangulationPoint>();
            for (int i = 0; i < n + 1; i++)
            {
                x = halfScale - i*size;
                for (int j = 0; j < n + 1; j++)
                {
                    points.Add(new TriangulationPoint(x, halfScale - j*size));
                }
            }
            return points;
        }
    }
}