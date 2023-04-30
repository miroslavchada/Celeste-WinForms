using Celeste_WinForms.Properties;
using NAudio.Wave;
using System.ComponentModel;

namespace Celeste_WinForms;

internal class SoundManager
{
    private static readonly ComponentResourceManager rm = new(typeof(Resources));
    private WaveOutEvent waveOut;
    private readonly string file;
    private WaveFileReader reader;
    public static bool bannedSound = false;
    public static bool bannedMusic = false;

    public SoundManager(string _file, bool variants)
    {
        file = _file;

        reader = new WaveFileReader((Stream)rm.GetObject(file + (variants ? "_01" : "")));
        waveOut = new WaveOutEvent();
        waveOut.Init(reader);
    }

    public void PlaySound(int variant, float volume)
    {
        if (!bannedSound)
        {
            reader.Position = 0;
            waveOut.Volume = volume;
            waveOut.Play();
        }
    }
}