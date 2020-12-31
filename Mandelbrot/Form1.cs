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
using NetMQ.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NetMQ;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        private object lockObject = new object();
        private Bitmap bitmap;

        private static readonly HttpClient httpClient = new HttpClient();

        public Form1()
        {
            InitializeComponent();

            this.bitmap = new Bitmap(400, 400);
            //this.StartSink();
            //this.StartCalculation();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            this.StartCalculation();
            this.StartSink();
        }

        private void StartSink()
        {
            var sinkPort = ConfigurationManager.AppSettings.Get("sinkPort") ?? "80";

            Task.Run(() =>
            {
                using (var sender = new PushSocket($"@tcp://*:{sinkPort}"))
                using (var sink = new PullSocket(">tcp://localhost:8080"))
                {
                    Thread.Sleep(1000);

                    int upper = 10;
                    int height = 400;
                    for (int lower = 0; lower < height; lower += 10)
                    {

                        sender.SendFrame(lower + "," + upper + "," + height);
                        upper += 10;

                    }
                }
            });
        }

        private async void StartCalculation()
        {
            /*
            var mainServer = ConfigurationManager.AppSettings["mainServer"];
            var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = pictureBox1.Height, Width = pictureBox1.Width });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            button1.Text = "LOADING";
            var response = await httpClient.PostAsync(mainServer, data);
            var responseString = await response.Content.ReadAsStringAsync();
            var valueList = JsonConvert.DeserializeObject<List<TripleResult>>(responseString);
            */

            await Task.Run(() =>
            {
                var ventilatorPort = ConfigurationManager.AppSettings.Get("ventilatorPort") ?? "4400";

                List<(int, int, int)> resultList = new List<(int, int, int)>();

                //socket to receive messages on
                using (var receiver = new PullSocket($"@tcp://localhost:{ventilatorPort}"))
                {
                    for (int taskNumber = 0; taskNumber < 400; taskNumber = taskNumber + 10)
                    {
                        var workerDoneTrigger = receiver.ReceiveFrameBytes();
                        List<(int, int, int)> gameField = null;
                        BinaryFormatter binaryFormatter2 = new BinaryFormatter();

                        using (var memoryStream2 = new MemoryStream(workerDoneTrigger))
                        {
                            gameField = (List<(int, int, int)>)binaryFormatter2.Deserialize(memoryStream2);
                            resultList.AddRange(gameField);
                        }

                    }
                }

                var valueList = resultList.Select(item => new TripleResult()
                {
                    X = item.Item1,
                    Y = item.Item2,
                    Iteration = item.Item3
                }).ToList();

                //dto
                foreach (var value in valueList)
                {
                    this.bitmap.SetPixel(value.X, value.Y, value.Iteration < 100 ? Color.Black : Color.White);
                }

                pictureBox1.Image = bitmap;
            });


        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            /*

            var mainServer = ConfigurationManager.AppSettings["mainServer"];

            //var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = 10, Width = 10 });
            var json = JsonConvert.SerializeObject(new CalculationRequest() { Height = pictureBox1.Height, Width = pictureBox1.Width });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            button1.Text = "LOADING";

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

            pictureBox1.Image = bitmap;

            button1.Text = "Reload";
            */

        }
    }
}
