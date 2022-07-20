using System;

namespace PiEar.Interfaces
{
    public interface IPiearAudio
    {
        bool Load(string fileName);
        bool Play(byte[] data);
        bool Play(string file);
        bool Play();
        bool Pause();
        bool Resume();
        bool Reset();
        bool Stop();
        bool IsPlaying();
        bool SetVolume(float volume);
        bool SetPan(float pan);
        event EventHandler<EventArgs> OnFinish;
    }
}