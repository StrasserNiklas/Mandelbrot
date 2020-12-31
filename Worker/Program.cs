using MandelbrotLibrary;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== WORKER ======");

            var sinkPort = ConfigurationManager.AppSettings.Get("sinkPort") ?? "8088";
            var ventilatorPort = ConfigurationManager.AppSettings.Get("ventilatorPort") ?? "400";

            using (var receiver = new PullSocket($">tcp://localhost:{sinkPort}"))
            using (var sender = new PushSocket($">tcp://localhost:{ventilatorPort}"))
            {
                while (true)
                {
                    string workload = receiver.ReceiveFrameString();

                    // the protocoll is as follows: [0] -> lower, [1] -> upper,  [2] -> height
                    string[] workLoadArray = workload.Split(',');

                    var calculator = new MandelbrotCalculator();
                    var result = calculator.Calculate(Convert.ToInt32(workLoadArray[2]), 400, Convert.ToInt32(workLoadArray[0]), Convert.ToInt32(workLoadArray[1]));

                    byte[] data;
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    using (var memoryStream = new MemoryStream())
                    {
                        binaryFormatter.Serialize(memoryStream, result);
                        data = memoryStream.ToArray();
                    }

                    Console.WriteLine("Sending");
                    sender.SendFrame(data);
                }
            }

            
        }
    }
}
