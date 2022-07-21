using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Helpers;
using PiEar.Models;

namespace PiEar.ViewModels
{
    public class MainViewViewModel
    {
        public MainViewViewModel() => Task.Run(Setup).Wait();
        public IList<StreamViewModel> Streams { get; } = new List<StreamViewModel>();
        public ClickViewModel Click { get; } = new ClickViewModel();
        public GlobalMuteViewModel GlobalMute { get; } = new GlobalMuteViewModel();
        public bool SetupComplete { get; private set; }
        private Task _sseLoopTask;
        private void HandleStreamReceived(object sender, BackgroundTasks.StreamEvent e)
        {
            if (!App.GlobalMuteStatusValid)
            {
                return;
            }
            Task.Run(() =>
            {
                if (e.Channel >= Streams.Count || App.GlobalMuteStatus || Streams[e.Channel].Stream.Mute)
                {
                    return;
                }
                Streams[e.Channel].Stream.Player.Buffer(e.Data);
            });
        }
        private void LoadClickFile(object sender, EventArgs e)
        {
            App.Logger.InfoWrite($"Loading click: {Settings.ClickFilename}");
            Click.Click.Player.Load(typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream(Settings.ClickFilename));
        }
        private async void Setup()
        {
            LoadClickFile(this, null);
            BackgroundTasks.StreamEventReceived += HandleStreamReceived;
            SettingsViewModel.ClickFileChanged += LoadClickFile;
            App.Logger.InfoWrite("Waiting for server to be found");
            while (Networking.ServerIp == null)
            {
                await Task.Delay(500);
            }
            App.Logger.InfoWrite($"Found server at {Networking.ServerIp}");
            string resp = await Networking.GetRequest("/channel-name");
            JsonData json;
            if (resp != null)
            {
                json = JsonConvert.DeserializeObject<JsonData>(resp);
                if (json != null)
                {
                    if (json.Error != "")
                    {
                        App.Logger.InfoWrite($"Setting up {json.ChannelCount} channels");
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
                    if (json.Error != "")
                    {
                        if (json.Bpm != -1)
                        {
                            Click.Click.ChangeBpm(json.Bpm);
                            Click.Click.ChangeToggle(json.BpmEnabled);
                        }
                    }
                    else
                    {
                        App.Logger.ErrorWrite($"Could not get bpm: {json.Error}");
                    }
                }
            }
            _sseLoopTask = Task.Run(SseLoop);
            SetupComplete = true;
        }
        private void SseLoop()
        {
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
                                        App.Logger.VerboseWrite("Testing");
                                        continue;
                                    }
                                    if (line.Contains("channel_name")) {
                                        App.Logger.InfoWrite($"Changing channel {json.Id}'s name to {json.ChannelName}");
                                        Streams[json.Id].Stream.ChangeLabel(json.ChannelName);
                                    } else if (line.Contains("bpm_enabled"))
                                    {
                                        App.Logger.InfoWrite($"BpmEnabled = {json.BpmEnabled}");
                                        Click.Click.ChangeToggle(json.BpmEnabled);
                                    } else if (line.Contains("bpm"))
                                    {
                                        App.Logger.InfoWrite($"Changing BPM to {json.Bpm}");
                                        Click.Click.ChangeBpm(json.Bpm);
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