using System;

namespace PiEar.Models
{
    public class Stream
    {
        // Internal ID, Useful???
        private static int _count = 0;
        
        // Public values
        public string Label { get; set; }
        public bool Mute { get; set; } = false;
        public double Pan { get; set; } = 0;
        public double VolumeMultiplier { get; set; } = 0;

        // Alternate Expressions of Public values
        public string ImageSource => (Mute) ? "mute" : "unmute";
        public bool NotMute => !Mute;
        public double VolumeDouble => Math.Pow(2, (3 * VolumeMultiplier)) + 1;
        public string Id { get; } = (_count++).ToString();

        // Initializer
        public Stream(string label)
        {
            Label = label;
        }
    }
}