using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private object lockObject = new object();

        private void Form1_Shown(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Parallel.For(0, pictureBox1.Width, x =>
            {
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    double a = (double)(x - (pictureBox1.Width / 2)) / (double)(pictureBox1.Width / 4);
                    double b = (double)(y - (pictureBox1.Height / 2)) / (double)(pictureBox1.Height / 4);

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
                        bitmap.SetPixel(x, y, iteration < 100 ? Color.Black : Color.White);
                    }
                }
            });

            

            pictureBox1.Image = bitmap;

            // old code

            //for (int x = 0; x < pictureBox1.Width; x++)
            //{
            //    for (int y = 0; y < pictureBox1.Height; y++)
            //    {
            //        double a = (double)(x - (pictureBox1.Width / 2)) / (double)(pictureBox1.Width / 4);
            //        double b = (double)(y - (pictureBox1.Height / 2)) / (double)(pictureBox1.Height / 4);

            //        var c = new Complex(a, b);
            //        var z = new Complex(0, 0);

            //        var iteration = 0;

            //        do
            //        {
            //            iteration++;
            //            z.Square();
            //            z.Add(c);

            //            if (z.Magnitude() > 2.0)
            //            {
            //                break;
            //            }
            //        } 
            //        while (iteration < 300);

            //        // 
            //        bitmap.SetPixel(x, y, iteration < 100 ? Color.Black : Color.White);

            //    }
            //}

            //pictureBox1.Image = bitmap;
        }
    }
}
