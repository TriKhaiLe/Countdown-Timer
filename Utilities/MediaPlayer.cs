using System.IO;
using System;

namespace Scheduler
{
    public class MediaPlayer
    {
        private readonly System.Windows.Media.MediaPlayer _mediaPlayer;

        public MediaPlayer()
        {
            _mediaPlayer = new System.Windows.Media.MediaPlayer();
        }

        public void InitializeMediaPlayer()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string path = Path.Combine(projectDirectory, "Timer", "Resources", "countdown-sound.wav");
            _mediaPlayer.Open(new Uri(path));
            _mediaPlayer.Volume = 0.1;
        }

        public void Play()
        {
            _mediaPlayer.Play();
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
        }
    }

}