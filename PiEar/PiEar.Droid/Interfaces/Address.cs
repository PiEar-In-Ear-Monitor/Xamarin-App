using System;
using System.Net;
using Android.Content;
using Android.Net.Wifi;

[assembly: Xamarin.Forms.Dependency(typeof(PiEar.Droid.Interfaces.Address))]
namespace PiEar.Droid.Interfaces
{
    public class Address : PiEar.Interfaces.IAddress
    {
        public string StringIpAddress()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            IPAddress ip = new IPAddress(BitConverter.GetBytes(wifiManager.DhcpInfo.IpAddress));
            return ip.ToString();
        }
        public string StringIpGateway()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            IPAddress ip = new IPAddress(BitConverter.GetBytes(wifiManager.DhcpInfo.Gateway)); 
            return ip.ToString();
        }
        public byte[] ByteIpGateway()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            return BitConverter.GetBytes(wifiManager.DhcpInfo.Gateway); 
        }
        public byte[] ByteIpAddress()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            return BitConverter.GetBytes(wifiManager.DhcpInfo.IpAddress); 
            
        }
    }
}