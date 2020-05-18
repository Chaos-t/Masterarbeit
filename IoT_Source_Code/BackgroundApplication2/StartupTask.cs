using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using BackgroundApplication2.Services;
using BackgroundApplication2.Controller;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BackgroundApplication2
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral backgroundTaskDeferral;
        

        public void Run(IBackgroundTaskInstance taskInstance)
        {

            // Get the deferral and save it to local variable so that the app stays alive.
            this.backgroundTaskDeferral = taskInstance.GetDeferral();



            IAsyncAction asyncAction = ThreadPool.RunAsync(async (handler) =>
            {
                await Server.Task_Run();
                MessageController.OpenConnection();
            });
        }



    }
}
