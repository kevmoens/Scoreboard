using Scoreboard.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Scoreboard.UWP
{
    public class HttpServer : IDisposable
    {
        private const uint BufferSize = 8192;
        private static readonly StorageFolder LocalFolder
                     = Windows.ApplicationModel.Package.Current.InstalledLocation;

        private readonly StreamSocketListener listener;

        private MainViewModel mainViewModel;

        public MainViewModel ViewModel
        {
            get { return mainViewModel; }
            set { mainViewModel = value; }
        }


        public HttpServer(int port)
        {
            this.listener = new StreamSocketListener();
            this.listener.ConnectionReceived += (s, e) => ProcessRequestAsync(e.Socket);
            Await(this.listener.BindServiceNameAsync(port.ToString()).AsTask());
        }
        public static async void Await(Task task)
        {
            try
            {
               await task;
            } catch { }
        }
        public void Dispose()
        {
            this.listener.Dispose();
        }

        private async void ProcessRequestAsync(StreamSocket socket)
        {
            // this works for text only
            StringBuilder request = new StringBuilder();
            using (IInputStream input = socket.InputStream)
            {
                byte[] data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }

            using (IOutputStream output = socket.OutputStream)
            {
                string requestMethod = request.ToString().Split('\n')[0];
                string[] requestParts = requestMethod.Split(' ');
                string[] requestParmsParts = request.ToString().Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string requestParms = request.ToString().Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] urlParms = requestParms.Split(new char[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries);

                string actionTeam = string.Empty;
                string actionAwayTeam = string.Empty;
                string actionHomeTeam = string.Empty;
                string actionScore = string.Empty;
                int score = 0;
                bool reset = false;

                ParseParameters(urlParms, ref actionTeam, ref actionAwayTeam, ref actionHomeTeam, ref actionScore, ref score, ref reset);

                //Run on UI Thread
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await ExecuteAction(actionTeam, actionAwayTeam, actionHomeTeam, score, reset);
                });

                if (requestParts[0] == "GET")
                    await WriteResponseAsync(requestParts[1], output);
                else
                    throw new InvalidDataException("HTTP method not supported: "
                                                   + requestParts[0]);
            }
        }

        private async Task ExecuteAction(string actionTeam, string actionAwayTeam, string actionHomeTeam, int score, bool reset)
        {
            if (reset)
            {
                ExecuteReset(actionTeam);
            }
            else
            {
                if (score > 0)
                {
                    await ExecuteScore(actionTeam, score);
                }
                else
                {
                    ExecuteSetTeam(actionAwayTeam, actionHomeTeam);
                }
            }
        }

        private void ExecuteSetTeam(string actionAwayTeam, string actionHomeTeam)
        {
            if (actionAwayTeam != string.Empty)
            {
                ViewModel.away = ViewModel.teams.Values.FirstOrDefault((t) => t.Name == actionAwayTeam.Replace('+', ' '));
            }
            if (actionHomeTeam != string.Empty)
            {
                ViewModel.home = ViewModel.teams.Values.FirstOrDefault((t) => t.Name == actionHomeTeam.Replace('+', ' '));
            }
        }

        private async Task ExecuteScore(string actionTeam, int score)
        {
            if (actionTeam == "AWAY")
            {
                await ViewModel.AwayScore(score);
            }
            else
            {
                await ViewModel.HomeScore(score);
            }
        }

        private void ExecuteReset(string actionTeam)
        {
            if (actionTeam == "AWAY")
            {
                ViewModel.AwayResetCommand.Execute(true);
            }
            else
            {
                ViewModel.HomeResetCommand.Execute(true);
            }
        }

        private static void ParseParameters(string[] urlParms, ref string actionTeam, ref string actionAwayTeam, ref string actionHomeTeam, ref string actionScore, ref int score, ref bool reset)
        {
            foreach (string parm in urlParms)
            {
                string[] parmSplit = parm.Split('=');
                switch (parmSplit[0])
                {
                    case "AWAYTEAMNAME":
                        actionAwayTeam = parmSplit[1];
                        break;
                    case "HOMETEAMNAME":
                        actionHomeTeam = parmSplit[1];
                        break;
                    case "TEAM":
                        actionTeam = parmSplit[1];
                        break;
                    case "SCORE":
                        actionScore = parmSplit[1];
                        int.TryParse(actionScore, out score);
                        break;
                    case "RESET":
                        reset = true;
                        break;
                }
            }
        }

        private async Task WriteResponseAsync(string path, IOutputStream os)
        {
            using (Stream resp = os.AsStreamForWrite())
            {
                bool exists = true;
                try
                {
                    string body = await readWebClientHtml();
                    string header = String.Format("HTTP/1.1 200 OK\r\n" +
                                    "Content-Length: {0}\r\n" +
                                    "Connection: close\r\n\r\n"
                                    , body.Length);
                    byte[] headerArray = Encoding.UTF8.GetBytes(header);
                    await resp.WriteAsync(headerArray, 0, headerArray.Length);
                    byte[] bodyArray = Encoding.UTF8.GetBytes(body);
                    await resp.WriteAsync(bodyArray, 0, bodyArray.Length);
                }
                catch (FileNotFoundException)
                {
                    exists = false;
                }

                if (!exists)
                {
                    byte[] headerArray = Encoding.UTF8.GetBytes(
                                          "HTTP/1.1 404 Not Found\r\n" +
                                          "Content-Length:0\r\n" +
                                          "Connection: close\r\n\r\n");
                    await resp.WriteAsync(headerArray, 0, headerArray.Length);
                }

                await resp.FlushAsync();
            }
        }
        async Task<string> readWebClientHtml()
        {
            var uri = new System.Uri("ms-appx:///webclient.html");
            var sampleFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            return await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
        }
    }
}
