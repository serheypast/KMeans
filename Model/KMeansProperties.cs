using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class KMeansProperties
    {
        public KMeansProperties(bool square, bool perimeter, bool elongation, bool compactness)
        {
            IsSquareKMeans = square;
            IsPerimeterKMeans = perimeter;
            IsElongationKMeans = elongation;
            IsCompactnessKMeans = compactness;
        }

        public bool IsSquareKMeans { get; set; }
        public bool IsPerimeterKMeans { get; set; }
        public bool IsElongationKMeans { get; set; }
        public bool IsCompactnessKMeans { get; set; }

        public double EvalByProperites(ImageCluster firstCluster, ImageObject image)
        {
            double ans = 0;
            if (IsSquareKMeans)
            {
                ans += Math.Pow(firstCluster.Square - image.Square, 2);
            }
            if (IsPerimeterKMeans)
            {
                ans += Math.Pow(firstCluster.Perimeter - image.Perimeter, 2);
            }
            if (IsElongationKMeans)
            {
                ans += Math.Pow(firstCluster.Elongation - image.Elongation, 2);
            }
            if (IsCompactnessKMeans)
            {
                ans += Math.Pow(firstCluster.Compactness - image.Compactness, 2);
            }

            return Math.Sqrt(ans);
        }
    }
}
