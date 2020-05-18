using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;

namespace BackgroundApplication2.Services
{
    public sealed class MessageRelayService
    {
        const string AppServiceName = "UwpMessageRelayService";
        private AppServiceConnection _connection;

        public static MessageRelayService Instance { get; } = new MessageRelayService();
        public bool IsConnected => _connection != null;

        private async Task<AppServiceConnection> CachedConnection()
        {
            if (_connection != null) return _connection;
            _connection = await MakeConnection();
            _connection.ServiceClosed += ConnectionOnServiceClosed;
            return _connection;
        }

        public void Open()
        {
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (handler) =>
            {
                await CachedConnection();
            });         
        }

        private async Task<AppServiceConnection> MakeConnection()
        {
            var listing = await AppServiceCatalog.FindAppServiceProvidersAsync(AppServiceName);

            if (listing.Count == 0)
            {
                throw new Exception("Unable to find app service '" + AppServiceName + "'");
            }
            var packageName = listing[0].PackageFamilyName;

            var connection = new AppServiceConnection
            {
                AppServiceName = AppServiceName,
                PackageFamilyName = packageName
            };

            var status = await connection.OpenAsync();

            if (status != AppServiceConnectionStatus.Success)
            {
                throw new Exception("Could not connect to MessageRelay, status: " + status);
            }

            return connection;
        }


        private void ConnectionOnServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            DisposeConnection();
        }

        private void DisposeConnection()
        {
            try
            {
                if (_connection == null) return;

                _connection.ServiceClosed -= ConnectionOnServiceClosed;
                _connection.Dispose();
                _connection = null;
            }
            catch(NullReferenceException e)
            {
                return;
            }
        }



        public void CloseConnection()
        {
            DisposeConnection();
        }

        private async void ReTry(KeyValuePair<string, object> keyValuePair)
        {
            await Task.Delay(1000);
            await SendMessageAsync(keyValuePair);
        }

        private async Task SendMessageAsync(KeyValuePair<string, object> keyValuePair)
        {
            var connection = await CachedConnection();
            var result = await connection.SendMessageAsync(new ValueSet { keyValuePair });

            if (result.Status == AppServiceResponseStatus.Success)
            {
                return;
            }
            throw new Exception("Error sending " + result.Status);
        }

        public void SendMessageAsync(string key, string value)
        {
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (handler) =>
            {
                await SendMessageAsync(new KeyValuePair<string, object>(key, value));
            });          
        }
    }
}