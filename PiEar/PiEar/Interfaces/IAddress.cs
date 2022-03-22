namespace PiEar.Interfaces
{
    public interface IAddress
    {
        string IpAddress();
        string IpNetmask();
        string IpGateway();
    }
}