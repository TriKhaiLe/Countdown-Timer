using System.IO;
using System;
using System.Windows.Media;
using System.Media;

namespace ChroniTask
{
    public class MediaPlayerService
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly SoundPlayer _soundPlayer;

        public MediaPlayerService()
        {

            _mediaPlayer = new MediaPlayer(); // tạo biến này để app không bị phóng to

            Stream audioStream = Timer.Properties.Resources.timeout_sound1;
            _soundPlayer = new SoundPlayer(audioStream);
        }

        public void Play()
        {
            _soundPlayer.Play();
        }

        public void Stop()
        {
            _soundPlayer.Stop();
        }
    }

}