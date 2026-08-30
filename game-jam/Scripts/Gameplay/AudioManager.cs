using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public partial class AudioManager : Node
	{
		public static AudioManager Instance { get; private set; }

		private AudioStreamPlayer _musicPlayer;
		[Export] public AudioStream MenuMusic;
		[Export] public AudioStream GambleBarMusic;
		[Export] public AudioStream GameplayMusic;

		public override void _Ready()
		{
			if (Instance == null)
			{
				Instance = this;
				_musicPlayer = new AudioStreamPlayer();
				_musicPlayer.Bus = "Master";
				AddChild(_musicPlayer);
			}
			else
			{
				QueueFree();
			}
		}

		public void PlayMusic(AudioStream stream)
		{
			if (stream == null) return;

			// Se a música solicitada já estiver tocando, não reinicia do zero
			if (_musicPlayer.Stream == stream && _musicPlayer.Playing)
				return;

			_musicPlayer.Stream = stream;
			_musicPlayer.Play();
		}

		public void StopMusic()
		{
			_musicPlayer.Stop();
		}
	}
}
