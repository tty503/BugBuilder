using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

using Newtonsoft.Json;

namespace ClientTCPTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Client2.Init();
            }
        }
    }

    public class Client
    {
        public static void Init()
        {
            // Crear un objeto TcpClient para conectarse al servidor
            var client = new TcpClient();

            // Iniciar la conexión con el servidor
            client.Connect("localhost", 8080);

            // Enviar un mensaje al servidor
            byte[] message = Encoding.UTF8.GetBytes("Hola, servidor!");
            client.GetStream().Write(message, 0, message.Length);

            // Recibir la respuesta del servidor
            byte[] response = new byte[1024];
            int bytesRead = client.GetStream().Read(response, 0, response.Length);

            // Imprimir la respuesta del servidor
            Console.WriteLine(Encoding.UTF8.GetString(response));

            // Cerrar la conexión con el servidor
            client.Close();
        }
    }

    public class ModelInfo
    {
        public string ip { get; set; }
        public string country { get; set; }
        public string user_pc { get; set; }
        public bool status { get; set; }
        public string operatingSystem { get; set; }
        public int ping { get; set; }
    }
    class Client2
    {
        public static void Init()
        {
            Random random = new Random();
            // Crear un objeto TcpClient para conectarse al servidor
            TcpClient client = new TcpClient("localhost", 3323);

            // Crear un objeto NetworkStream para leer y escribir datos en la conexión
            var stream = client.GetStream();

            // Crear un objeto ModelInfo con los datos de prueba
            ModelInfo modelInfo = new ModelInfo
            {
                ip = "192.168.1.10",
                country = "Venezuela",
                user_pc = "DESKTOP-ABCDE",
                status = true,
                operatingSystem = "Windows 10",
                ping = random.Next(100, 251)
        };

            // Convertir el objeto ModelInfo a JSON
            string json = JsonConvert.SerializeObject(modelInfo);

            // Enviar el JSON al servidor
            stream.Write(Encoding.UTF8.GetBytes(json), 0, json.Length);

            Console.WriteLine(json);

            // Cerrar la conexión con el servidor
            client.Close();
        }

    }


}