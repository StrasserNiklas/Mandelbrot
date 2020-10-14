using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer
{
    public class MandelbrotCalculator
    {
        private object lockObject = new object();

        public List<(int, int, int)> Calculate(int height, int width)
        {
            //var bitmap = new Bitmap(width, height);

            var valueList = new List<(int, int, int)>();

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    double a = (double)(x - (width / 2)) / (double)(width / 4);
                    double b = (double)(y - (height / 2)) / (double)(height / 4);

                    var c = new Complex(a, b);
                    var z = new Complex(0, 0);

                    var iteration = 0;

                    do
                    {
                        iteration++;
                        z.Square();
                        z.Add(c);

                        if (z.Magnitude() > 2.0)
                        {
                            break;
                        }
                    }
                    while (iteration < 300);

                    lock (lockObject)
                    {
                        valueList.Add((x, y, iteration));
                        //bitmap.SetPixel(x, y, iteration < 100 ? Color.Black : Color.White);
                    }
                }
            });

            return valueList;
        }
    }
}
