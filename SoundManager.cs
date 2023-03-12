using Celeste_WinForms.Properties;
using NAudio.Wave;
using System.ComponentModel;

namespace Celeste_WinForms
{
    internal class SoundManager
    {
        ComponentResourceManager rm = new(typeof(Resources));
        Stream stream;
        WaveOut waveOut;
        string file;
        readonly float volume;
        public static bool bannedSound = true;

        public SoundManager(string _file, float _volume, bool variants)
        {
            file = _file;
            volume = _volume;

            stream = (Stream)rm.GetObject(file + (variants ? "_01" : ""));
            waveOut = new WaveOut();
            waveOut.Init(new WaveFileReader(stream));
        }

        public void PlaySound(int variant)
        {
            waveOut.Stop();
            waveOut.Dispose();

            if (!bannedSound)
            {
                stream = (Stream)rm.GetObject(file + (variant != 0 ? $"_0{variant}" : ""));
                waveOut = new();
                waveOut.Init(new WaveFileReader(stream));
                waveOut.Volume = volume;

                waveOut.Play();
            }
        }
    }
}
