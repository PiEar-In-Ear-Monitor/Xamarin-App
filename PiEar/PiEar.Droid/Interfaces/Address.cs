using System;
using System.Diagnostics;
using System.Net;
using Android.Content;
using Android.Net.Wifi;

[assembly: Xamarin.Forms.Dependency(typeof(PiEar.Droid.Interfaces.Address))]
namespace PiEar.Droid.Interfaces
{
    public class Address : PiEar.Interfaces.IAddress
    {
        public string IpAddress()
        {            
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            IPAddress ip = new IPAddress(BitConverter.GetBytes(wifiManager.DhcpInfo.IpAddress));
            return ip.ToString();
        }
        public string IpNetmask()
        {            
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            IPAddress ip = new IPAddress(BitConverter.GetBytes(wifiManager.DhcpInfo.Netmask)); 
            return ip.ToString();
        }
        public string IpGateway()
        {            
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            IPAddress ip = new IPAddress(BitConverter.GetBytes(wifiManager.DhcpInfo.Gateway)); 
            return ip.ToString();
        }
    }
}