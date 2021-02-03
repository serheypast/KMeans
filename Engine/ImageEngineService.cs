using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Model;
using ZedGraph;

namespace Engine
{
    public class ImageEngineService
    {
        public void SetImageProperties(ImageObject image)
        {
            image.Perimeter = Perimeter(image);
            image.Square = Square(image);
            image.Compactness = Compactness(image.Perimeter, image.Square);
            image.Elongation = Elongation(image);
        }

        public int Square(ImageObject image) => image.Pixels.Count;

        public int Perimeter(ImageObject image)
        {
            int count = 0;
            foreach (var customPixel in image.Pixels)
            {
                if (
                    image.Pixels.Any(x => x.X == customPixel.X && x.Y == customPixel.Y + 1)
                    && image.Pixels.Any(x => x.X == customPixel.X && x.Y == customPixel.Y - 1)
                    && image.Pixels.Any(x => x.X == customPixel.X - 1 && x.Y == customPixel.Y)
                    && image.Pixels.Any(x => x.X == customPixel.X + 1 && x.Y == customPixel.Y))
                {
                    count++;
                }
            }

            return image.Pixels.Count - count;
        }

        public double Compactness(int square, int perimeter) => Math.Pow(perimeter, 2) / (square * 1.0);

        public double CenterMass(ImageObject image, bool isX)
        {
            return image.Pixels.Sum(x => isX ? x.X : x.Y) / (1.0 * image.Pixels.Count);
        }

        public double Elongation(ImageObject image)
        {
            var yMass = CenterMass(image, false);
            var xMass = CenterMass(image, true);
            var m02 = image.Pixels.Sum(x => Math.Pow(x.Y - yMass, 2));
            var m20 = image.Pixels.Sum(x => Math.Pow(x.X - xMass, 2));
            var m11 = image.Pixels.Sum(x => x.X - xMass + x.Y - yMass);
            var sqrtFunc = Math.Sqrt(Math.Pow(m20 - m02, 2) + 4 * Math.Pow(m11, 2));
            var elongation = (m20 + m02 + sqrtFunc) / (m20 + m02 - sqrtFunc);
            return (Double.IsNaN(elongation) || Double.IsInfinity(elongation)) ? 0 : elongation;
        }

        public double AverageBrightness(Bitmap originalImage, ImageObject image)
        {
            double sumBrightness = 0;
            foreach (var customPixel in image.Pixels)
            {
                sumBrightness += originalImage.GetPixel(customPixel.X, customPixel.Y).GetBrightness();
            }

            return sumBrightness / (image.Pixels.Count * 1.0);
        }

        public CustomColor AverageColor(Bitmap originalImage, ImageObject image)
        {
            int R = 0;
            int G = 0;
            int B = 0;
            foreach (var customPixel in image.Pixels)
            {
                R += originalImage.GetPixel(customPixel.X, customPixel.Y).R;
                G += originalImage.GetPixel(customPixel.X, customPixel.Y).G;
                B += originalImage.GetPixel(customPixel.X, customPixel.Y).B;
            }

            return new CustomColor(R / image.Pixels.Count, G / image.Pixels.Count, B / image.Pixels.Count);
        }

        public List<ImageCluster> KMeans(List<ImageObject> images, int countClusters, double stopValue, KMeansProperties properties)
        {
            var clusters = new List<ImageCluster>(countClusters);
            for (int i = 0; i < countClusters; i++)
            {
                clusters.Add(new ImageCluster());
            }

            InitializeCluster(clusters, images);
            

            while (clusters.Any(x => x.IsNeedToRecalculate))
            {
                CalculateClusters(clusters, images, properties);
                clusters.ForEach(x => x.ReCalculateCenterOfCluster(stopValue));
            } 

            return clusters;
        }

        private void CalculateClusters(List<ImageCluster> clusters, List<ImageObject> images, KMeansProperties properties)
        {
            clusters.ForEach(x => x.ClearImageItems());
            images.ForEach(image => clusters.Aggregate((x, y) => FindClusterWithMinDistance(x, y, image, properties)).Images.Add(image));
        }

        private ImageCluster FindClusterWithMinDistance(ImageCluster firstCluster, ImageCluster secondCluster, ImageObject image, KMeansProperties properties)
        {
            var firstDistance = properties.EvalByProperites(firstCluster, image);
            var secondDistance = properties.EvalByProperites(secondCluster, image);
            return (firstDistance < secondDistance) ? firstCluster : secondCluster;
        }

        private void InitializeCluster(List<ImageCluster> clusters, IList<ImageObject> images)
        {
            var rnd = new Random();
            var useImages = new List<int>();
            int id = 0;
            foreach (var cluster in clusters)
            {
                if(id == images.Count - 1) break;
                int useImageIterator;
                do
                {
                    useImageIterator = rnd.Next(0, images.Count - 1);
                } while (useImages.Any(x => x == useImageIterator));

                useImages.Add(useImageIterator);
                cluster.Id = id++;
                cluster.Images = new List<ImageObject>{images[useImageIterator]};
            }
            clusters.ForEach(x => x.ReCalculateCenterOfCluster(0));
        }
    }
}
