using Foreground.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreground.Controller
{
    public class MessageController
    {
        internal static async Task EnsureConnected(MessageRelayService _connection)
        {
            var retryDelay = 10000;
            await Task.Delay(retryDelay);
            while (!_connection.IsConnected)
            {
                try
                {
                    await _connection.Open();
                }
                catch (Exception)
                {

                    await Task.Delay(retryDelay);
                }
            }
        }

        public async static void OpenConnection(MessageRelayService _connection)
        {

            await _connection.Open();

        }

        private async static void SendJson(MessageRelayService _connection,string key, string json)
        {
            await _connection.SendMessageAsync(key, json);
        }

    }
}
