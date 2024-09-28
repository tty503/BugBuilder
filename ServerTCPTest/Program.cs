using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPtest
{
    public class Program
    {
        static void Main(string[] args)
        {
             Server.Init();
        }
    }
    public class Server
    {
        public static void Init()
        {
            // Crear un objeto TcpListener que escuche en el puerto 8080
            var listener = new TcpListener(IPAddress.Any, 8080);

            // Iniciar las escuchas
            listener.Start();

            // Bucle infinito para aceptar conexiones entrantes
            while (true)
            {
                // Aceptar una conexión entrante
                var client = listener.AcceptTcpClient();

                // Crear un objeto NetworkStream para leer y escribir datos en la conexión
                var stream = client.GetStream();

                // Leer un mensaje del cliente
                byte[] message = new byte[1024];
                int bytesRead = stream.Read(message, 0, 256);

                var show = Encoding.UTF8.GetString(message);
                Console.WriteLine($"Client say : {show}");

                // Enviar un mensaje de respuesta al cliente
                message = Encoding.UTF8.GetBytes("Hola, mundo!");
                stream.Write(message, 0, message.Length);


                // Cerrar la conexión con el cliente
                client.Close();
            }
        }
    }
}