using UnityEngine;
using System.Collections.Generic;

public class Music{

	private static float GlobalVolumeMultiplier = 1f;

	private static AudioClip[] allMusic = Resources.LoadAll<AudioClip>("music");
	private static AudioSource source = Game.GetMisc("music").AddComponent<AudioSource>();
	static List<AudioClip> m = new List<AudioClip>(allMusic);
	static int index = m.Count;
	public static void PlayNext() {
		UpdateVolume();
		index++;
		if (index >= m.Count) {
			var rnd = new Random();
			for (int i = 0; i < m.Count; i++) {
				AudioClip temp = m[i];
				int randomIndex = Random.Range(i, m.Count);
				m[i] = m[randomIndex];
				m[randomIndex] = temp;
			}
			index = 0;
		}
		source.clip = m[index];
		source.Play();
	}

	public static void Update() {
		if (!source.isPlaying) PlayNext();
	}

	public static void UpdateVolume() {
		source.volume = S_OptionSlider.music.GetValue() * GlobalVolumeMultiplier;
	}

}
