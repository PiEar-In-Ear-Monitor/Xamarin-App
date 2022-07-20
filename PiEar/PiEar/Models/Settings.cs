using System.Collections;
using System.Collections.Generic;
using Plugin.Settings;

namespace PiEar.Models
{
    public static class Settings
    {
        public const string ChannelFile = "channelSettings.set";
        public const string ClickFile = "channelSettings.set";
        public static string ClickFilename
        {
            get => CrossSettings.Current.GetValueOrDefault("clickSound", "PiEar.Click.Beep.ogg", ClickFile);
            set => CrossSettings.Current.AddOrUpdateValue("clickSound", $"PiEar.Click.{value}.ogg", ClickFile);
        }
        public static readonly IList ClickOptions = new List<string>()
            { "Beep", "Click", "Clink", "Clonk", "Cluck", "Metal", "Plastic", "Sticks", "Tap", "Wood" };
    }
}