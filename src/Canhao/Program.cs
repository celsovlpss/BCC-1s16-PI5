﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Canhao
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "0.0.0.0";
            int porta = 10800;
            string mensagem = "AVIAO A VISTA";
            byte[] pacote = new byte[64];

            TcpListener servidor = new TcpListener(IPAddress.Parse(ip), porta);
            servidor.Start();

            TcpClient clienteRadar = servidor.AcceptTcpClient();
            servidor.Stop();

            Stopwatch sw = Stopwatch.StartNew();

            byte[] dados = Encoding.ASCII.GetBytes(mensagem);
            pacote[0] = (byte)dados.Length;
            dados.CopyTo(pacote, 1);
            clienteRadar.Client.Send(pacote);

            int offset = 0;
            while (offset < pacote.Length)
                offset += clienteRadar.Client.Receive(pacote, offset, pacote.Length - offset, SocketFlags.None);

            sw.Stop();

            Console.WriteLine("Tamanho: {0}", pacote[0]);
            Console.WriteLine("Texto: {0}", Encoding.ASCII.GetString(pacote, 1, pacote[0]));
            Console.WriteLine("Tempo: {0}", sw.Elapsed);

            clienteRadar.Close();

            Console.ReadKey();
        }
    }
}