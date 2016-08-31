using UnityEngine;
using System.Collections;

public class Sounds  {

    public static AudioClip move = Resources.Load<AudioClip>("sfx/move");
    public static AudioClip pip = Resources.Load<AudioClip>("sfx/pickup");
    public static AudioClip dead = Resources.Load<AudioClip>("sfx/dead");
    public static AudioClip select = Resources.Load<AudioClip>("sfx/select");
    public static AudioClip deselect = Resources.Load<AudioClip>("sfx/deselect");
    

    private static AudioSource source= Game.GetMisc("sound").AddComponent<AudioSource>();

	

    public static void PlaySound(AudioClip clip, float volume=1, float pitch=1) {
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
    }
}
