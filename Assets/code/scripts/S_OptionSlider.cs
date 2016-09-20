using UnityEngine;
using System.Collections;
using System;
public class S_OptionSlider : MonoBehaviour {
	bool down;
	GameObject pipObject;
	public static int BAR_HEIGHT = 5 * S_Camera.scale;
	public static int TOTAL_HEIGHT = (int)(Sprites.GetBounds(Sprites.pip).y * S_Camera.scale);
	int width;

	public static S_OptionSlider sfx;
	public static S_OptionSlider music;
	public static S_OptionSlider CreateSlider(int width) {
		GameObject go = new GameObject("Slider");
		GameObject pip = Primitives.CreateActor(Sprites.pip);
		S_Button bar = S_Button.CreateButton(Sprites.pixel);
		bar.transform.localScale = new Vector2(width, BAR_HEIGHT);
		bar.GetComponent<BoxCollider2D>().size = new Vector2(1f, 3.5f);
		bar.GetComponent<SpriteRenderer>().color = Colours.GREEN;


		Util.SetLayer(pip, Util.LayerName.UI, 5);
		pip.transform.SetParent(go.transform, false);
		pip.transform.position = new Vector2((int)(width/5f*3), (int)(-Sprites.GetBounds(Sprites.pip).y * S_Camera.scale / 2f + BAR_HEIGHT/2));
		S_OptionSlider slider = go.AddComponent<S_OptionSlider>();
		slider.width = width;
		bar.transform.SetParent(go.transform, false);
		Util.SetLayer(bar.gameObject, Util.LayerName.UI, 5);
		slider.pipObject = pip.gameObject;
		bar.SetDownAction(() => slider.down = true);
		bar.SetUpAction(() => slider.OnUp());

		S_Camera.SetupScale(pip.transform);
		return slider;
	}

	Action onUp;
	public void SetUpAction(Action action) {
		this.onUp= action;
	}


	public void OnUp() {
		if (onUp != null) onUp.Invoke();
		down = false;
	}

	void Start() {

	}

	public void SetRatio(float value) {
		pipObject.transform.localPosition = new Vector2 (value * (width - Sprites.GetBounds(Sprites.pip).x * S_Camera.scale), pipObject.transform.localPosition.y);
	}

	public float GetValue() {
		float ratio = (pipObject.transform.position.x - transform.position.x ) / (width - Sprites.GetBounds(Sprites.pip).x * S_Camera.scale);
		return ratio;
	}

	void Update() {
		if (down) {
			int x = (int)(Input.mousePosition.x - Sprites.GetBounds(Sprites.pip).x * S_Camera.scale / 2f);
			x = (int)Mathf.Min(transform.position.x + width- Sprites.GetBounds(Sprites.pip).x * S_Camera.scale, Mathf.Max(x, transform.position.x));
			pipObject.transform.position = new Vector2(x, pipObject.transform.position.y);
			if (this == music) Music.UpdateVolume();
		}
	}
}
