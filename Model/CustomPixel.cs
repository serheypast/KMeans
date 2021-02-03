using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class CustomPixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Color { get; set; }

        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public int GrayScaleValue { get; set; }

        public CustomPixel()
        {
            
        }

        public CustomPixel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public CustomPixel(int x, int y, int color): this(x ,y)
        {
            Color = color;
        }
    }
}
