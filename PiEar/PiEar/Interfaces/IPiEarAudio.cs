using System;

namespace PiEar.Interfaces
{
    public interface IPiearAudio
    {
        bool Init();
        bool Buffer(short[] data);
        bool Play();
        bool Pause();
        bool Resume();
        bool IsPlaying();
        bool SetVolume(float volume);
        bool SetPan(float pan);
    }
}