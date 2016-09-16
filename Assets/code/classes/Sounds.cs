using UnityEngine;
using System.Collections;

public class Sounds  {
	private static float GlobalVolumeMultiplier = 2.5f;
	public static AudioClip move = Resources.Load<AudioClip>("sfx/move");
    public static AudioClip pip = Resources.Load<AudioClip>("sfx/pickup");
    public static AudioClip spike = Resources.Load<AudioClip>("sfx/spike");
    public static AudioClip select = Resources.Load<AudioClip>("sfx/select");
	public static AudioClip deselect = Resources.Load<AudioClip>("sfx/deselect");

	public static AudioClip charge = Resources.Load<AudioClip>("sfx/charge");
	public static AudioClip shoot = Resources.Load<AudioClip>("sfx/shoot");
	public static AudioClip suck = Resources.Load<AudioClip>("sfx/suck");
	public static AudioClip teleport = Resources.Load<AudioClip>("sfx/teleport");

	public static AudioClip mystery = Resources.Load<AudioClip>("sfx/mystery");

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
		AudioSource source = getSource();
		source.pitch = pitch;
		source.PlayOneShot(clip, volume * S_OptionSlider.sfx.GetValue()*GlobalVolumeMultiplier);
    }

	public static int[][] nicePitches =
		{
		/*new int[] {12},
		new int[] {0,12},
		new int[] {0,7,12},
		new int[] {0,4,7,12},
		new int[] {0,2,4,7,12},
		new int[] {0,2,4,7,10,12},
		new int[] {0,2,4,7,9,11,12},
		new int[] {0,2,4,5,7,9,11,12},
		new int[] {0,2,4,5,7,8,9,11,12},
		new int[] {0,2,4,5,7,8,9,10,11,12},
		new int[] {0,2,4,5,6,7,8,9,10,11,12},
		new int[] {0,2,3,4,5,6,7,8,9,10,11,12},
		new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12},*/
		};
}
