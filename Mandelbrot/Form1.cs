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
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using MandelbrotLibrary;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private object lockObject = new object();

        private static readonly HttpClient httpClient = new HttpClient();

        private async void Form1_Shown(object sender, EventArgs e)
        {
            var mainServer = ConfigurationManager.AppSettings["mainServer"];

            //var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = 10, Width = 10 });
            var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = pictureBox1.Height, Width = pictureBox1.Width });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(mainServer, data);
            var responseString = await response.Content.ReadAsStringAsync();

            var valueList = JsonConvert.DeserializeObject<List<TripleResult>>(responseString);
            //var valueList = JsonConvert.DeserializeObject<List<TripleResultNew>>(responseString); // with triple


            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            //dto
            foreach (var value in valueList)
            {
                bitmap.SetPixel(value.X, value.Y, value.Iteration < 100 ? Color.Black : Color.White);
            }

            // try with triple
            //foreach (var value in valueList)
            //{
            //    bitmap.SetPixel(value.Result.Item1, value.Result.Item2, value.Result.Item3 < 100 ? Color.Black : Color.White);
            //}



            //Parallel.For(0, pictureBox1.Width, x =>
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

            //        lock (lockObject)
            //        {
            //            bitmap.SetPixel(x, y, iteration < 100 ? Color.Black : Color.White);
            //        }
            //    }
            //});

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
