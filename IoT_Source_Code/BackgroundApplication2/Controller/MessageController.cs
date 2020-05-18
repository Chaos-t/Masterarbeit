using BackgroundApplication2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;

namespace BackgroundApplication2.Controller
{
    public sealed class MessageController
    {

        private static readonly MessageRelayService _connection = MessageRelayService.Instance;
        static readonly object locker = new object();

        public static void SendMessage(string key, string json)
        {
                  IAsyncAction asyncAction = ThreadPool.RunAsync(async (handler) =>
                  {
                      await EnsureConnected();
                      SendJson(key, json);
                  });
        }

        public static void EnsureConnect()
        {
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (handler) =>
            {
                await EnsureConnected();              
            });
        }
        private static async Task EnsureConnected()
        {
            var retryDelay = 10000;
            await Task.Delay(retryDelay);
            while (!_connection.IsConnected)
            {
                try
                {
                    _connection.Open();
                }
                catch (Exception)
                {
                    // note ensure MessageRelay is deployed by right clicking on UwpMessageRelay.MessageRelay and selecting "Deploy"
                    await Task.Delay(retryDelay);
                }
            }
        }

        public static void OpenConnection()
        {

            _connection.Open();

        }

        private static void SendJson(string key, string json)
        {
            _connection.SendMessageAsync(key, json);
        }
    }
}
