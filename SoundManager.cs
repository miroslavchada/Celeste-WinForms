using Celeste_WinForms.Properties;
using NAudio.Wave;
using System.ComponentModel;

namespace Celeste_WinForms;

internal class SoundManager {
    private static readonly ComponentResourceManager rm = new(typeof(Resources));
    private WaveOutEvent waveOut;
    private readonly string file;
    private WaveFileReader reader;
    public static bool bannedSound = false;
    public static float volume = 0.5f;

    public SoundManager(string _file) {
        file = _file;

        reader = new WaveFileReader((Stream)rm.GetObject(file));
        waveOut = new WaveOutEvent();
        waveOut.Init(reader);
    }

    public void PlaySound() {
        if (!bannedSound) {
            reader.Position = 0;
            waveOut.Volume = volume;
            waveOut.Play();
        }
    }

    public void StopSound() {
        if (waveOut.PlaybackState == PlaybackState.Playing) {
            waveOut.Stop();
        }
    }
}