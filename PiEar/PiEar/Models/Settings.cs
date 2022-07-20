using System.Collections;
using System.Collections.Generic;
using Plugin.Settings;

namespace PiEar.Models
{
    public static class Settings
    {
        public const string File = "Settings.set";
        public static string ClickFilename
        {
            get => CrossSettings.Current.GetValueOrDefault("clickSound", "Beep", File);
            set => CrossSettings.Current.AddOrUpdateValue("clickSound", $"PiEar.Click.{value}.ogg", File);
        }
        public static readonly IList ClickOptions = new List<string>()
            { "Beep", "Click", "Clink", "Clonk", "Cluck", "Metal", "Plastic", "Sticks", "Tap", "Wood" };
    }
}