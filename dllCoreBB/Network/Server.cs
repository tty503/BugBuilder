using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Timers;
using Newtonsoft.Json;

namespace dllCoreBB.Server
{
    public class ModelInfo
    {
        public string ip { get; set; }
        public string country { get; set; }
        public string user_pc { get; set; }
        public bool status { get; set; }
        public string Status { get => status == true ? "CONNECT" : "DISCONNECT"; }
        public string operatingSystem { get; set; }
        public int ping { get; set; }

    }

    public class Server
    {
        public ModelInfo modelInfo;
        private TcpListener listener;

        public Server(string host, int port)
        {
            IPAddress ip = IPAddress.Parse(host); // Convierte la cadena a IPAddress
            listener = new TcpListener(ip, port);
            listing();
        }

        public Server(IPAddress ip, int port)
        {
            listener = new TcpListener(ip, port);
            listing();
        }

        private void listing()
        {
            // Iniciar escucha
            listener.Start();

            // Programar el temporizador para comprobar conexiones
            var timer = new Timer();
            timer.Interval = 50;
            timer.Elapsed += (sender, e) => CheckForConnections();
            timer.Start();
        }

        private void CheckForConnections()
        {
            // Comprobar si hay conexiones entrantes
            if (listener.Pending())
            {
                // Aceptar la conexión entrante en un hilo separado
                Task.Run(() =>
                {
                    try
                    {
                        var client = listener.AcceptTcpClient();
                        HandleClient(client); // Maneja la conexión en una función separada
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error handling client: " + ex.Message);
                    }
                });
            }
        }

        private async void HandleClient(TcpClient client)
        {
            // Crear un objeto NetworkStream para leer y escribir datos en la conexión
            var stream = client.GetStream();

            // Leer un mensaje del cliente
            using (var reader = new StreamReader(stream))
            {
                string message = await reader.ReadToEndAsync();
                modelInfo = JsonConvert.DeserializeObject<ModelInfo>(message);
            }

            // Cerrar la conexión con el cliente
            client.Close();
        }
    }
}
