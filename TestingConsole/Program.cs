using System;
using System.Drawing;
using System.IO;
using Engine;
using Model;

namespace TestingConsole
{
    class Program
    {

        static int a1 = 1; 
        static int a2 = 0;
        static int a3 = 0;
        static int a4 = 0;
        
        static void Main(string[] args)
        {
            string path = "E:\\easy.jpg";
            double value = 0.6;
            Bitmap bitmap = new Bitmap(path);
            var map = bitmap.MakeGrayscale().Binarization(value);
            var images = map.GetImageObjects();
            var clustersCount = 5;
            for (int i = 2; i < 8; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                   // кластеризует по всем возможным вариантам (т.е. совокупность определеных принципов вычисления кластера)
                   var image = map.ConnectByKMeans(images, new KMeansProperties(check(a1), check(a2), check(a3), check(a4)), clustersCount);
                   string pathToCatalog = $"E:\\clusters\\name{ConcatePath()}\\";
                   Directory.CreateDirectory(pathToCatalog);
                   image.Save($"{pathToCatalog}clu{6}.jpg");
                   MySum();
                }
            }

            Console.WriteLine("zdartava");
        }

        private static bool check(int value) => value == 1;

        private static void MySum()
        {
            a1++;
            if (a1 > 1)
            {
                a1 = 0;
                a2++;
                if (a2 > 1)
                {
                    a2 = 0;
                    a3++;
                    if (a3 > 1)
                    {
                        a3 = 0;
                        a4++;
                    }
                }
            }
        }

        private static string ConcatePath()
        {
            return string.Concat(
                check(a1) ? $"sq" : string.Empty,
                check(a2) ? $"pe" : string.Empty,
                check(a3) ? $"el" : string.Empty,
                check(a4) ? $"co" : string.Empty);
        }
    }
}
