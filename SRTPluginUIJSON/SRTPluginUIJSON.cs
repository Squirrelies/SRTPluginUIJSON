using SRTPluginBase;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SRTPluginUIJSON
{
    public class SRTPluginUIJSON : IPluginUI
    {
        public IPluginInfo Info => new PluginInfo();
        public static object gameMemory = null;
        public static MethodInfo serializer = null;
        private static IPluginHostDelegates hostDelegates;

        public CancellationTokenSource cancellationTokenSource;
        public JSONServer jsonServer;
        public Task jsonServerTask;

        public int Startup(IPluginHostDelegates hostDelegates)
        {
            SRTPluginUIJSON.hostDelegates = hostDelegates;

            // Start the JSON server for listening for new reqeusts.
            cancellationTokenSource = new CancellationTokenSource();
            jsonServer = new JSONServer();
            jsonServerTask = jsonServer?.Start(cancellationTokenSource.Token);

            // Return success.
            return 0;
        }

        public int Shutdown()
        {
            // Shutdown the JSON server to stop listening for new requests.
            cancellationTokenSource?.Cancel();

            // Dispose and cleanup.
            try
            {
                jsonServer?.Dispose();
                jsonServer = null;
            }
            catch { return 1; }

            try
            {
                if (jsonServerTask != null && !jsonServerTask.IsCompleted)
                    jsonServerTask?.Wait(1000); // Sometimes the Task has not finished closing by the time we reach this point. Give it a second to wrap up.

                jsonServerTask?.Dispose();
                jsonServerTask = null;
            }
            catch { return 1; }

            try
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
            catch { return 1; }

            // Return success if we reached this point without issue.
            return 0;
        }

        public int ReceiveData(object gameMemory)
        {
            // Update the GameMemory variable with the latest data from the game's memory. The JSON Web Server will use GameMemory to serialize it for output.
            SRTPluginUIJSON.gameMemory = gameMemory;

            if (serializer == null)
                serializer = typeof(JsonSerializer).GetMethods(BindingFlags.Public | BindingFlags.Static).First(a => a.Name == "SerializeAsync" && a.IsGenericMethod == true).MakeGenericMethod(SRTPluginUIJSON.gameMemory.GetType());

            // Return success.
            return 0;
        }
    }
}
