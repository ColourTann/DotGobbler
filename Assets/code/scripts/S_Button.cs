using UnityEngine;
using System.Collections;
using System;

public class S_Button : MonoBehaviour {

	public static S_Button CreateButton(Sprite sprite) {
		GameObject result = new GameObject();
		SpriteRenderer sr = result.AddComponent<SpriteRenderer>();
		sr.sortingLayerName = "UI";
		sr.sprite = sprite;
		S_Button buttonScript = result.AddComponent<S_Button>();
		result.AddComponent<BoxCollider2D>();
		return buttonScript;
	}

	public void SetDownAction(Action down) {
		this.down = down;
	}

	public void SetUpAction(Action up) {
		this.up = up;
	}
	Action up;
	Action down;

	void OnMouseDown() {
		if (Game.isLocked()) return;
        if(down!=null)	down.Invoke();
	}


	void OnMouseUp() {
		if (Game.isLocked()) return;
		if (up != null) up.Invoke();
	}

	public Bounds GetBounds() {
		return GetComponent<SpriteRenderer>().bounds;
	}
}
