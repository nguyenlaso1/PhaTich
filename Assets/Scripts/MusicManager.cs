// @sonhg: class: MusicManager
using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance
	{
		get
		{
			if (MusicManager._instance == null)
			{
				MusicManager._instance = UnityEngine.Object.FindObjectOfType<MusicManager>();
				UnityEngine.Object.DontDestroyOnLoad(MusicManager._instance.gameObject);
			}
			return MusicManager._instance;
		}
	}

	public float MasterVolume
	{
		get
		{
			return PlayerPrefs.GetFloat("MASTER_KEY", 0f);
		}
		set
		{
			this.SetMasterVolume(value);
		}
	}

	public float MusicVolume
	{
		get
		{
			return PlayerPrefs.GetFloat("MUSIC_KEY", 0f);
		}
		set
		{
			this.SetMusicVolume(value);
		}
	}

	public float SoundVolume
	{
		get
		{
			return PlayerPrefs.GetFloat("SOUND_KEY", 0f);
		}
		set
		{
			this.SetSoundVolume(value);
		}
	}

	private void Awake()
	{
		if (MusicManager._instance == null)
		{
			MusicManager._instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else if (this != MusicManager._instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void PlaySingle(AudioClip clip, float volume)
	{
		this.effectSource.clip = clip;
		this.effectSource.volume = volume;
		this.effectSource.Play();
	}

	public void PlayOneShot(AudioClip clip, float volume)
	{
		this.effectSource.volume = volume;
		this.effectSource.PlayOneShot(clip);
	}

	public void RandomizeSfx(params AudioClip[] clips)
	{
		int num = UnityEngine.Random.Range(0, clips.Length);
		float pitch = UnityEngine.Random.Range(this.lowPitchRange, this.highPitchRange);
		this.effectSource.pitch = pitch;
		this.effectSource.clip = clips[num];
		this.effectSource.Play();
	}

	public void PlayMusic(AudioClip clip)
	{
		this.SetMasterVolume(this.MasterVolume);
		this.SetMusicVolume(this.MusicVolume);
		this.SetSoundVolume(this.SoundVolume);
		this.StopMusic();
		this.musicSource.clip = clip;
		this.musicSource.PlayDelayed(this.delayTime);
	}

	public void RandomizeMusic(params AudioClip[] clips)
	{
		this.StopMusic();
		int num = UnityEngine.Random.Range(0, clips.Length);
		this.musicSource.clip = clips[num];
		this.musicSource.PlayDelayed(this.delayTime);
	}

	public void StopMusic()
	{
		this.musicSource.Stop();
	}

	private void SetMasterVolume(float volume)
	{
		this.mixer.SetFloat("MasterVolume", volume);
		PlayerPrefs.SetFloat("MASTER_KEY", volume);
	}

	private void SetMusicVolume(float volume)
	{
		this.mixer.SetFloat("MusicVolume", volume);
		PlayerPrefs.SetFloat("MUSIC_KEY", volume);
	}

	private void SetSoundVolume(float volume)
	{
		this.mixer.SetFloat("SoundVolume", volume);
		PlayerPrefs.SetFloat("SOUND_KEY", volume);
	}

	public const string MASTER_KEY = "MASTER_KEY";

	public const string MUSIC_KEY = "MUSIC_KEY";

	public const string SOUND_KEY = "SOUND_KEY";

	[SerializeField]
	private AudioMixer mixer;

	public AudioSource effectSource;

	public AudioSource musicSource;

	public float lowPitchRange = 0.95f;

	public float highPitchRange = 1.05f;

	public float delayTime = 0.5f;

	private static MusicManager _instance;
}
