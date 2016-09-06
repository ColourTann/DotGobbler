using UnityEngine;
using System.Collections;

public class Sounds  {

    public static AudioClip move = Resources.Load<AudioClip>("sfx/move");
    public static AudioClip pip = Resources.Load<AudioClip>("sfx/pickup");
    public static AudioClip dead = Resources.Load<AudioClip>("sfx/dead");
    public static AudioClip select = Resources.Load<AudioClip>("sfx/select");
	public static AudioClip deselect = Resources.Load<AudioClip>("sfx/deselect");

	public static AudioClip charge = Resources.Load<AudioClip>("sfx/charge");
	public static AudioClip shoot = Resources.Load<AudioClip>("sfx/shoot");

	const int totalSources = 10;
	static int sourceNumber = 0;
	private static AudioSource[] sources = SetupSources();
	private static AudioSource[] SetupSources() {
		AudioSource[] sources = new AudioSource[totalSources];
		for(int i = 0; i < totalSources; i++) {
			sources[i] = Game.GetMisc("sound").AddComponent<AudioSource>();

		}
		return sources;
	}

	public static AudioSource getSource() {
		sourceNumber = (sourceNumber + 1) % totalSources;
		return sources[sourceNumber];		
	}

    public static void PlaySound(AudioClip clip, float volume=1, float pitch=1) {
		Debug.Log(volume + ":" + pitch);
		AudioSource source = getSource();
		source.pitch = pitch;
		source.PlayOneShot(clip, volume);
    }
}
