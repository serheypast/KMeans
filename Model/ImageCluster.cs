using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ImageCluster
    {
        public int Id { get; set; }
        public double Square { get; set; }
        public double Perimeter { get; set; }
        public double Elongation { get; set; }
        public double Compactness { get; set; }
        public IList<ImageObject> Images { get; set; }

        public bool IsNeedToRecalculate { get; private set; }

        public void ReCalculateCenterOfCluster(double stopValue)
        {
            if (Images.Any())
            {

                var newSquare = Images.Average(x => x.Square);
                var newPerimeter = Images.Average(x => x.Perimeter);
                var newElongation = Images.Average(x => x.Elongation);
                var newCompactness = Images.Average(x => x.Compactness);
                IsNeedToRecalculate = Math.Abs(newSquare - Square) >= stopValue
                                      || Math.Abs(newPerimeter - Perimeter) >= stopValue
                                      || Math.Abs(newElongation - Elongation) >= stopValue
                                      || Math.Abs(newCompactness - Compactness) >= stopValue;
                Square = newSquare;
                Perimeter = newPerimeter;
                Elongation = newElongation;
                Compactness = newCompactness;
            }
            else
            {
                IsNeedToRecalculate = false;
            }
        }

        public void ClearImageItems()
        {
            Images = new List<ImageObject>();
        }
    }
}
