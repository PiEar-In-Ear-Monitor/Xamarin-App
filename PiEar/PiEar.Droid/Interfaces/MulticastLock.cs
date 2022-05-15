using System.Diagnostics;
using Android.Content;
using Android.App;
using Android.Net.Wifi;

[assembly: Xamarin.Forms.Dependency(typeof(PiEar.Droid.Interfaces.MulticastLock))]
namespace PiEar.Droid.Interfaces
{
    public class MulticastLock : PiEar.Interfaces.IMulticastLock
    {
        private WifiManager.MulticastLock _lock;
        public void Dispose()
        {
            _lock.Release();
        }
        public void Acquire()
        {
            var wifiManager = (WifiManager)Application.Context.GetSystemService(Context.WifiService);
            // Acquire the lock
            if (wifiManager != null)
            {
                _lock = wifiManager.CreateMulticastLock("PiEar");
                if (_lock != null)
                {
                    _lock.Acquire();
                } else
                {
                    Debug.WriteLine("Could not acquire multicast lock");
                }
            } else {
                Debug.WriteLine("WifiManager is null");
            }
        }
        public bool IsHeld { get => _lock.IsHeld; }
        public void Release()
        {
            _lock.Release();
        }
    }
}