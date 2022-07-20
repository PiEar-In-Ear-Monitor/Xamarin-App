using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Helpers;
using PiEar.Models;

namespace PiEar.ViewModels
{
    public class MainViewViewModel
    {
        public MainViewViewModel()
        {
            Task.Run(_setup);
            Task.Run(SseLoop);
        }
        public IList<StreamViewModel> Streams { get; } = new List<StreamViewModel>();
        public ClickViewModel ClickViewModel { get; } = new ClickViewModel();
        public GlobalMuteViewModel GlobalMute { get; } = new GlobalMuteViewModel();
        private void HandleStreamReceived(object sender, BackgroundTasks.StreamEvent e) {
            Task.Run(() => {
                if (Streams.Count - 1 <= e.Channel || GlobalMuteViewModel.GlobalMuteStatus.Mute || Streams[e.Channel].Stream.Mute)
                {
                    return;
                }
                // Streams[e.Channel].Stream.Buffer.Write(e.Data, 0, e.Data.Length);
                // Streams[e.Channel].Stream.Buffer.Seek(0, SeekOrigin.Begin);
                // Streams[e.Channel].Stream.Player.Load(_streams[e.Channel].Stream.Buffer);
                // Streams[e.Channel].Stream.Player.Play();
                Streams[e.Channel].Stream.Player.Play(e.Data);
            });
        }
        private void LoadClickFile(object sender, EventArgs e)
        {
            App.Logger.InfoWrite($"Loading click: {Settings.ClickFilename}");
            // _clickViewModel.Click.Player.Load(typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream(Settings.ClickFilename)?.ToString());
        }
        private async void _setup()
        {
            LoadClickFile(null, null);
            BackgroundTasks.StreamEventReceived += HandleStreamReceived;
            SettingsViewModel.ClickFileChanged += LoadClickFile;
            while(Networking.ServerIp == null)
            {
                await Task.Delay(500);
            }
            string resp = await Networking.GetRequest("/channel-name");
            JsonData json;
            if (resp != null)
            {
                json = JsonConvert.DeserializeObject<JsonData>(resp);
                if (json != null)
                {
                    if (json.Error != null)
                    {
                        if (json.ChannelCount != -1)
                        {
                            for (int i = 0; i < json.ChannelCount; i++)
                            {
                                Streams.Add(new StreamViewModel());
                            }
                        }   
                    }
                    else
                    {
                        App.Logger.ErrorWrite($"Could not get channel count: {json.Error}");
                    }
                }
            }
            resp = await Networking.GetRequest("/bpm");
            if (resp != null)
            {
                json = JsonConvert.DeserializeObject<JsonData>(resp);
                if (json != null)
                {
                    if (json.Error == null)
                    {
                        if (json.Bpm != -1)
                        {
                            ClickViewModel.Click.ChangeBpm(json.Bpm);
                            ClickViewModel.Click.ChangeToggle(json.BpmEnabled);
                        }
                    }
                    else
                    {
                        App.Logger.ErrorWrite($"Could not get bpm: {json.Error}");
                    }
                }
            }
        }
        private void SseLoop()
        {
            while (Networking.ServerIp == null)
            {
                Task.Delay(1000);
            }
            while (true)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        using (var stream = client.GetStreamAsync($"http://{Networking.ServerIp}:{Networking.Port}/channel-name/listen").Result)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                while (true)
                                { 
                                    string line = reader.ReadLine();
                                    if (line == null || line.Length < 6)
                                    {
                                        continue;
                                    }
                                    line = line.Substring(6);
                                    var json = JsonConvert.DeserializeObject<JsonData>(line);
                                    if (json == null)
                                    {
                                        continue;
                                    }
                                    if (line.Contains("channel_name")) {
                                        Streams[json.Id].Stream.ChangeLabel(json.ChannelName);
                                    }
                                    if (line.Contains("bpm_enabled"))
                                    {
                                        ClickViewModel.Click.ChangeToggle(json.BpmEnabled);
                                    }
                                    if (line.Contains("bpm") && !line.Contains("bpm_enabled"))
                                    {
                                        ClickViewModel.Click.ChangeBpm(json.Bpm);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    App.Logger.WarnWrite($"SSE Error occured: {e.Message}");
                }
            }
        }
    }
}