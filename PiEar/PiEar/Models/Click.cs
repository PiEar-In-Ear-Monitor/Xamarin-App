namespace PiEar.Models
{
    public class Click : Stream
    {
        public string AudioFile { get; set; }
        public int Bpm { get; set; } = 100;
        public bool Enabled { get; set; } = false;
        public int StepCount { get; set; } = 10;
        public Click() { Label = "Click"; }
    }
}