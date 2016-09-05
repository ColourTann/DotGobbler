using UnityEngine;
using System.Collections;

public class S_Animation : MonoBehaviour {

	public static GameObject CreateAnimation(Sprite[] frames, float speed, int x, int y) {
		GameObject go = Primitives.CreateActor(frames[0], x, y);
		go.name = "animation";
		S_Animation anim = go.AddComponent<S_Animation>();
		anim.Init(frames, speed);
		return go;
	}

	Sprite[] frames;
	float animationSpeed;
	SpriteRenderer animRenderer;
	float time;
	int frameIndex;

	public void Init(Sprite[] frames, float speed) {
		this.frames = frames;
		this.animationSpeed = speed;
		animRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

	void Update () {
		time += Time.deltaTime / animationSpeed;
		int newIndex = (int)(time % frames.Length);
		if (newIndex != frameIndex) {
			frameIndex = newIndex;
			animRenderer.sprite = frames[frameIndex];
		}
	}
}
