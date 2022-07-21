using System;
using Android.Media;
using Xamarin.Forms;

[assembly: Dependency(typeof(PiEar.Droid.Interfaces.PiearAudio))]
namespace PiEar.Droid.Interfaces
{
    [Android.Runtime.Register("java/io/ByteArrayInputStream", DoNotGenerateAcw=true)]
    public sealed class PiearAudio : PiEar.Interfaces.IPiearAudio
    {
        private AudioTrack _audioTrack;
        private float _volume;
        private float _pan;
        public bool Init()
        {
            try
            {
                var attributes = new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.Media)?
                    .SetContentType(AudioContentType.Music)?
                    .SetFlags(AudioFlags.LowLatency)?
                    .SetLegacyStreamType(Stream.Music)?
                    .SetAllowedCapturePolicy(CapturePolicies.BySystem)
                    .SetHapticChannelsMuted(true)
                    .Build();
                var format = new AudioFormat.Builder()
                    .SetEncoding(Encoding.Pcm16bit)?
                    .SetSampleRate(48000)?
                    .SetChannelMask(ChannelOut.Mono)
                    .Build();
                if (attributes == null)
                {
                    App.Logger.ErrorWrite($"Could not create AudioAttributes");
                    return false;
                }
                if (format == null)
                {
                    App.Logger.ErrorWrite($"Could not create AudioFormat");
                    return false;
                }
                _audioTrack = new AudioTrack.Builder()
                    .SetAudioAttributes(attributes)
                    .SetAudioFormat(format)
                    .SetBufferSizeInBytes(128 * 2 * 25) // 128 samples per buffer, 2 bytes per buffer, buffer size capable of holding 15 times that. 
                    .SetPerformanceMode(AudioTrackPerformanceMode.LowLatency)
                    .Build();
                return true;
            }
            catch (Exception e)
            {
                MainActivity.AppLog.ErrorWrite($"Failed to create AudioTrack: \n {e.Message}");
                return false;
            }
        }
        public bool Buffer(byte[] data)
        {
            try
            {
                _audioTrack.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception e)
            {
                MainActivity.AppLog.ErrorWrite($"Failed to add {data.Length} bytes to the buffer: \n {e.Message}");
                return false;
            }
        }
        public bool Play()
        {
            try
            {
                _audioTrack.Play();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite($"Failed to play: \n {e.Message}");
                return false;
            }
        }
        public bool Pause()
        {
            try {
                _audioTrack.Pause();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite($"Failed to pause: \n {e.Message}");
                return false;
            }
        }
        public bool Resume()
        {
            try {
                _audioTrack.Play();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite($"Failed to play (resume): \n {e.Message}");
                return false;
            }
        }
        public bool IsPlaying()
        {
            try {
                return _audioTrack.PlayState == PlayState.Playing;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite($"Failed to get PlayState: \n {e.Message}");
                return false;
            }
        }
        public bool SetVolume(float volume)
        {
            try {
                if (volume > 1 || volume < 0 || Math.Abs(volume - _volume) < 0.000001)
                {
                    return false;
                }
                _volume = volume;
                float leftVolume = Math.Max(volume + (_pan < 0 ? _pan : 0), 0);
                float rightVolume = Math.Max(volume - (_pan > 0 ? _pan : 0), 0);
                _audioTrack.SetStereoVolume(leftVolume, rightVolume); // TODO: This is a deprecated call
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite($"Failed to set volume: \n {e.Message}");
                return false;
            }
        }
        public bool SetPan(float pan)
        {
            try {
                if (pan < -1 || pan > 1 || Math.Abs(pan - _pan) < 0.000001)
                {
                    return false;
                }
                _pan = pan;
                if (!SetVolume(_volume))
                {
                    throw new Exception("Failed to set volume after panning");
                }
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite($"Failed panning: \n {e.Message}");
                return false;
            }
        }
    }
}