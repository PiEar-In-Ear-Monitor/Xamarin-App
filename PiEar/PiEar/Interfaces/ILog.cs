namespace PiEar.Interfaces
{
    public interface ILog
    {
        void DebugWrite(string message);
        void InfoWrite(string message);
        void WarnWrite(string message);
        void ErrorWrite(string message);
        void VerboseWrite(string message);
    }
}