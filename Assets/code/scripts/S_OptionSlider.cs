using UnityEngine;
using System.Collections;

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
		pip.transform.position = new Vector2(width/5f*3, -Sprites.GetBounds(Sprites.pip).y  / 2f);
		S_OptionSlider slider = go.AddComponent<S_OptionSlider>();
		slider.width = width;
		bar.transform.SetParent(go.transform, false);
		Util.SetLayer(bar.gameObject, Util.LayerName.UI, 5);
		slider.pipObject = pip.gameObject;
		bar.SetDownAction(() => slider.down = true);
		bar.SetUpAction(() => slider.down = false);

		S_Camera.SetupScale(pip.transform);


		return slider;
	}



	void Start() {

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
			GetValue();
		}
	}
}
