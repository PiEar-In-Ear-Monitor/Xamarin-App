namespace PiEar.Interfaces
{
    public interface IAddress
    {
        string StringIpAddress();
        string StringIpGateway();
        byte[] ByteIpGateway();
        byte[] ByteIpAddress();
    }
}