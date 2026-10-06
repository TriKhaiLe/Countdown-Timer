using System;
using System.IO;
using System.Media;
using System.Reflection;

namespace ChroniTimer
{
    /// <summary>
    /// Plays the embedded timeout sound when a timer session ends.
    /// </summary>
    public class SoundService : IDisposable
    {
        private SoundPlayer? _soundPlayer;

        public SoundService()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("ChroniTimer.Resources.timeout-sound.wav");
                if (stream != null)
                    _soundPlayer = new SoundPlayer(stream);
            }
            catch
            {
                // Sound is optional – degrade gracefully
            }
        }

        public void Play()
        {
            _soundPlayer?.Play();
        }

        public void Stop()
        {
            _soundPlayer?.Stop();
        }

        public void Dispose()
        {
            _soundPlayer?.Dispose();
        }
    }
}
