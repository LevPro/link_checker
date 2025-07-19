using System.Media;

namespace LinkChecker.UI.Services
{
    public class SoundService
    {
        public void PlaySuccess()
        {
            Play("Resources/Sounds/success.wav");
        }

        public void PlayError()
        {
            Play("Resources/Sounds/error.wav");
        }

        private void Play(string file)
        {
            try
            {
                using var player = new SoundPlayer(file);
                player.Play();
            }
            catch { }
        }
    }
}