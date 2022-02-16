namespace PiEar.Models
{
    public abstract class Click : Stream
    {
        public string AudioFile { get; set; }
        public Click() { Label = "Click"; }
    }
}