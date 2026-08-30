using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public partial class AudioManager : Node
	{
		public static AudioManager Instance { get; private set; }

		private AudioStreamPlayer _musicPlayer;
		private AudioStreamPlayer _sfxPlayer; // Novo player para efeitos sonoros

		[Export] public AudioStream MenuMusic;
		[Export] public AudioStream GambleBarMusic;
		[Export] public AudioStream GameplayMusic;
		
		// Efeito sonoro do baralho
		[Export] public AudioStream ShuffleSfx;

		public override void _Ready()
		{
			if (Instance == null)
			{
				Instance = this;
				
				_musicPlayer = new AudioStreamPlayer { Bus = "Master" };
				_sfxPlayer = new AudioStreamPlayer { Bus = "Master" };

				AddChild(_musicPlayer);
				AddChild(_sfxPlayer);
			}
			else
			{
				QueueFree();
			}
		}

		public void PlayMusic(AudioStream stream)
		{
			if (stream == null || (_musicPlayer.Stream == stream && _musicPlayer.Playing)) return;
			_musicPlayer.Stream = stream;
			_musicPlayer.Play();
		}

		// Método para tocar efeitos sonoros pontuais
		public void PlaySfx(AudioStream stream)
		{
			if (stream == null) return;
			
			// Instancia um player rápido para permitir sons sobrepostos sem cortar
			AudioStreamPlayer sfxOneShot = new AudioStreamPlayer();
			sfxOneShot.Stream = stream;
			sfxOneShot.Bus = "Master";
			AddChild(sfxOneShot);
			
			sfxOneShot.Play();
			sfxOneShot.Finished += () => sfxOneShot.QueueFree(); // Deleta da memória ao terminar
		}
	}
}
