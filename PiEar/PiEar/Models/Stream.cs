namespace PiEar.Models
{
    public class Stream
    {
        public static int Count = 0;
        public string Label { get; set; } = $"Channel {Count + 1}";
        public bool Mute { get; set; } = false;
        public double Pan { get; set; } = 0;
        public double VolumeMultiplier { get; set; } = 0;
        public Stream(string label) { Label = label; }
        public Stream() {}
    }
}