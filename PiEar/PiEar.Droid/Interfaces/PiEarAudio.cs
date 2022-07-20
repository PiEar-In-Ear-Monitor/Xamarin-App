using System;
using Android.Media;
using Xamarin.Forms;

[assembly: Dependency(typeof(PiEar.Droid.Interfaces.PiearAudio))]
namespace PiEar.Droid.Interfaces
{
    [Android.Runtime.Register("java/io/ByteArrayInputStream", DoNotGenerateAcw=true)]
    public sealed class PiearAudio : PiEar.Interfaces.IPiearAudio
    {
        private MediaPlayer _mediaPlayer;
        private float _volume;
        private float _pan;
        public bool Load(string fileName)
        {
            try
            {
                if (_mediaPlayer == null) {
                    _mediaPlayer = new MediaPlayer();
                } else {
                    _mediaPlayer.Completion -= OnPlaybackEnded;
                    _mediaPlayer.Stop();
                    _mediaPlayer.Reset();
                }
                _mediaPlayer.SetDataSource(fileName);
                _mediaPlayer.Prepare();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Play(byte[] data)
        {
            try {
                if (_mediaPlayer == null) {
                    _mediaPlayer = new MediaPlayer();
                } else {
                    _mediaPlayer.Completion -= OnPlaybackEnded;
                    _mediaPlayer.Stop();
                    _mediaPlayer.Reset();
                }
                _mediaPlayer.SetDataSource(new CustomAudioSource(data));
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
                _mediaPlayer.Completion += OnPlaybackEnded;
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool Play(string file)
        {
            try {
                if (_mediaPlayer == null)
                {
                    _mediaPlayer = new MediaPlayer();
                } else {
                    _mediaPlayer.Completion -= OnPlaybackEnded;
                    _mediaPlayer.Stop();
                    _mediaPlayer.Reset();
                }
                _mediaPlayer.SetDataSource(file);
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
                _mediaPlayer.Completion += OnPlaybackEnded;
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool Play()
        {
            try
            {
                _mediaPlayer.Start();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool Pause()
        {
            try {
                _mediaPlayer.Pause();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool Resume()
        {
            try {
                _mediaPlayer.Start();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool Reset()
        {
            try {
                _mediaPlayer.Reset();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool Stop()
        {
            try {
                _mediaPlayer.Stop();
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }

        public bool IsPlaying()
        {
            try {
                return _mediaPlayer.IsPlaying;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool SetVolume(float volume)
        {
            try {
                _volume = volume;
                float leftVolume = volume + (_pan < 0 ? _pan : 0);
                float rightVolume = volume - (_pan > 0 ? _pan : 0);
                _mediaPlayer.SetVolume(leftVolume, rightVolume);
                return true;
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public bool SetPan(float pan)
        {
            try {
                _pan = pan;
                return SetVolume(_volume);
            } catch (Exception e) {
                MainActivity.AppLog.ErrorWrite(e.Message);
                return false;
            }
        }
        public event EventHandler<EventArgs> OnFinish;
        private void OnPlaybackEnded(object sender, EventArgs e)
        {
            OnFinish?.Invoke(this, e);
        }
    }
}