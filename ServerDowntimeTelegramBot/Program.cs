using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ServerDowntimeTelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
                return;

            Directory.CreateDirectory("Saves");

            string token = args[0];
            string serverName = args[1];
            string ip = args[2];
            if (ip.Contains("//"))
                ip = ip.Split(new string[] { "//" }, 2, StringSplitOptions.None)[1];

            string[] targets = args.Skip(3).ToArray();

            string savePath = "Saves/" + ReplaceInvalidChars(serverName) + ".save";
            bool saveExists = File.Exists(savePath);

            //IF NO SERVICE ANSWERS ITS DOWN!
            if (!isReachablePing(ip) && !isPortOpen(ip, 1433) && !isReachableHttp(ip))
            {
                if (!saveExists)
                {
                    var botClient = new TelegramBotClient(token);
                    foreach (string target in targets)
                    {
                        Task.Run(async () =>
                        {
                            await botClient.SendTextMessageAsync(
                              chatId: target,
                              text: "⚠️ THE SERVER \"" + serverName + "\" IS NOT REACHABLE! ⚠️"
                            );
                        }).GetAwaiter().GetResult();
                    }

                    File.Create(savePath);
                }
            }
            else if (saveExists)
            {
                File.Delete(savePath);
            }
        }

        static bool isReachableHttp(string ip)
        {
            if (!ip.ToLower().StartsWith("http"))
                ip = "http://" + ip;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ip);
            request.Timeout = 10000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
        static bool isReachablePing(string ip)
        {
            Ping ping = new Ping();
            return ping.Send(ip).Status == IPStatus.Success;
        }

        static bool isPortOpen(string ip, int port)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(ip, port);
                    return true;
                }
                catch (Exception)
                {

                }
            }
            return false;
        }

        static string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
