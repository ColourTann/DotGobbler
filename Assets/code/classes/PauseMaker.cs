using UnityEngine;
using System.Collections;

public class PauseMaker : MonoBehaviour {
	static int pauseWidth = 180 * S_Camera.scale;
	static int pauseHeight = 120 * S_Camera.scale;
	static int border = 4 * S_Camera.scale;
	public static GameObject CreatePauseScreen() {
		GameObject pauseScreen = new GameObject("pause_screen");
		GameObject pauseBG = Primitives.CreateRectangle(pauseWidth, pauseHeight, Colours.RED);
		
		pauseBG.transform.SetParent(pauseScreen.transform, false);
		BoxCollider2D pausebgcol = pauseBG.AddComponent<BoxCollider2D>();
		pausebgcol.size = new Vector2(1, 1);
		pausebgcol.offset = new Vector2(.5f, .5f);

		Util.SetLayer(pauseBG, Util.LayerName.UI, 0);
		Util.SetZ(pauseBG, Util.ZLayer.PauseLayer);
		GameObject pauseBGInner = Primitives.CreateRectangle(pauseWidth-border*2, pauseHeight - border*2, Colours.DARK);
		pauseBGInner.transform.position = new Vector2(border, border);
		pauseBGInner.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(pauseBGInner, Util.LayerName.UI, 1);
		pauseScreen.transform.position = new Vector2((int)(Screen.width / 2 - pauseWidth / 2), (int)(Screen.height / 2 - pauseHeight / 2));


		GameObject musicSlider = S_OptionSlider.CreateSlider();
		musicSlider.transform.position = new Vector2(50, 50);
		musicSlider.transform.SetParent(pauseScreen.transform, false);
		Util.SetZ(musicSlider, Util.ZLayer.Sliders);

		GameObject soundSlider = S_OptionSlider.CreateSlider();
		soundSlider.transform.position = new Vector2(50, 100);
		soundSlider.transform.SetParent(pauseScreen.transform, false);
		Util.SetZ(soundSlider, Util.ZLayer.Sliders);

		return pauseScreen;
	}
}
