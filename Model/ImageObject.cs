using System.Collections.Generic;

namespace Model
{
    public class ImageObject
    {
        public ImageObject(List<CustomPixel> pixels)
        {
            Pixels = pixels;
        }

        public List<CustomPixel> Pixels { get; set; }

        public int Square { get; set; }
        public int Perimeter { get; set; }
        public double Elongation { get; set; }
        public double Compactness { get; set; }
    }
}
