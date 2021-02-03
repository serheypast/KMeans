using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Model;

namespace Engine
{
    public static class EngineService
    {
        public static Image GetImageByFile(IFormFile file)
        {
            return Image.FromStream(file.OpenReadStream(), true, true);
        }

        public static Bitmap GetBitMap(Image image)
        {
            var bitMap = new Bitmap(image);
            var gsBitMap = MakeGrayscale(bitMap);
            return gsBitMap;
        }

        public static Bitmap MakeGrayscale(this Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            Graphics g = Graphics.FromImage(newBitmap);

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            g.Dispose();
            return newBitmap;
        }

        public static int[] CreateHistogrtam(this Bitmap map)
        {
            var arr = new int[256];
            int max = 0;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    arr[map.GetPixel(i, j).R]++;
                    if (max < arr[map.GetPixel(i, j).R])
                        max = arr[map.GetPixel(i, j).R];
                }
            }

            return arr;
        }

        public static Bitmap Binarization(this Bitmap src, double treshold)
        {
            Bitmap dst = new Bitmap(src.Width, src.Height);

            for (int i = 0; i < src.Width; i++)
            {
                for (int j = 0; j < src.Height; j++)
                {
                    dst.SetPixel(i, j, src.GetPixel(i, j).GetBrightness() < treshold ? Color.Black : Color.White);
                }
            }

            return dst;
        }

        public static Bitmap Preparirovanie(this Bitmap src, int start, int end)
        {
            Bitmap dst = new Bitmap(src.Width, src.Height);

            for (int i = 0; i < src.Width; i++)
            {
                for (int j = 0; j < src.Height; j++)
                {
                    dst.SetPixel(i, j, SectionColor(src.GetPixel(i, j), start, end));
                }
            }


            return dst;
        }

        private static Color SectionColor(Color color, int start, int end)
        {
            return (color.GetBrightness() >= start && color.GetBrightness() <= end) ? color : Color.Black;
        }

        public static int[,] GetArrayOfPixels(this Bitmap map)
        {
            var array = new int[map.Width, map.Height];
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    array[i, j] = map.GetPixel(i, j).GetBrightness() == 1 ? 1 : 0;
                }
            }

            return array;
        }

        public static Bitmap FoursConnection(this Bitmap map)
        {
            Bitmap dst = new Bitmap(map.Width, map.Height);
            var array = map.GetArrayOfPixels();
            var imageObjects = new List<ImageObject>();

            int currentColor = 2;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    dst.SetPixel(i, j, Color.White);
                    if (array[i, j] == 1)
                    {
                        array = ConnectMap(array, i, j, currentColor, map.Width, imageObjects);
                        currentColor++;
                    }
                }
            }

            var imageEngineService = new ImageEngineService();
            imageObjects.ForEach(imageEngineService.SetImageProperties);

            var kMeansProperies = new KMeansProperties(false, false, true, true);
            var clusters = imageEngineService.KMeans(imageObjects, 6, 0.0001, kMeansProperies);

            Random rnd = new Random();
            foreach (var cluster in clusters)
            {
                var RRand = rnd.Next(0, 255);
                var GRand = rnd.Next(0, 255);
                var BRand = rnd.Next(0, 255);
                foreach (var clusterImage in cluster.Images)
                {
                    clusterImage.Pixels.ForEach(x => setRColor(x, RRand, GRand, BRand, dst));
                }
            }

            return dst;
        }

        public static Bitmap ConnectByKMeans(this Bitmap map, List<ImageObject> images, KMeansProperties kMeansProperties, int countOfCluster)
        {
            Bitmap dst = new Bitmap(map.Width, map.Height);
            
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    dst.SetPixel(i, j, Color.White);
                }
            }

            var imageEngineService = new ImageEngineService();
            images.ForEach(imageEngineService.SetImageProperties);
            var clusters = imageEngineService.KMeans(images, countOfCluster, 0.0001, kMeansProperties);
            Random rnd = new Random();
            foreach (var cluster in clusters)
            {
                var RRand = rnd.Next(0, 255);
                var GRand = rnd.Next(0, 255);
                var BRand = rnd.Next(0, 255);
                foreach (var clusterImage in cluster.Images)
                {
                    clusterImage.Pixels.ForEach(x => setRColor(x, RRand, GRand, BRand, dst));
                }
            }

            return dst;
        }

        public static List<ImageObject> GetImageObjects(this Bitmap map)
        {
            var array = map.GetArrayOfPixels();
            var imageObjects = new List<ImageObject>();

            int currentColor = 2;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    //dst.SetPixel(i, j, Color.White);
                    if (array[i, j] == 1)
                    {
                        array = ConnectMap(array, i, j, currentColor, map.Width, imageObjects);
                        currentColor++;
                    }
                }
            }

            return imageObjects;
        }

        private static void setRColor(CustomPixel pixel, int R, int G, int B, Bitmap dst)
        {
            dst.SetPixel(pixel.X, pixel.Y, Color.FromArgb(R, G, B));
            pixel.R = R;
            pixel.G = G;
            pixel.B = B;

        }

        private static int[,] ConnectMap(int[,] map, int i, int j, int color, int width, List<ImageObject> imageObjects)
        {
            var queue = new List<CustomPixel>();
            queue.Add(new CustomPixel
            {
                X = i,
                Y = j
            });
            int currentQState = 0;
            int height = map.Length / width;
            while (currentQState != queue.Count)
            {
                i = queue[currentQState].X;
                j = queue[currentQState].Y;
                map[i, j] = color;
                AddToQueue(map, queue, i + 1, j, width, height);
                AddToQueue(map, queue, i - 1, j, width, height);
                AddToQueue(map, queue, i, j + 1, width, height);
                AddToQueue(map, queue, i, j - 1, width, height);
                currentQState++;
            }
            imageObjects.Add(new ImageObject(queue));
            return map;
        }

        private static void AddToQueue(int[,] map, List<CustomPixel> queue, int i, int j, int width, int height)
        {
            if (IsConnectAndValid(i, j, width, height) && map[i, j] == 1)
            {
                if (!queue.Any(pixel => pixel.X == i && pixel.Y == j))
                {
                    queue.Add(new CustomPixel
                    {
                        X = i,
                        Y = j
                    });
                }
            }
        }

        private static bool IsConnectAndValid(int i, int j, int width, int height)
        {
            return i < width && i >= 0 && j >= 0 && j < height;
        }
    }
}
