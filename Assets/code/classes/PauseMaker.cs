using UnityEngine;
using System.Collections;

public class PauseMaker : MonoBehaviour {
	static int border = 15 * S_Camera.scale;
	static int pauseWidth = 180 * S_Camera.scale + border * 2;
	static int pauseHeight = 120 * S_Camera.scale + border * 2;
	static int gap = 10 * S_Camera.scale;
	public static GameObject CreatePauseScreen() {

		//all this code is a big pile of rubbish, I need to try unity unity ui next time!

		//background
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
		Util.SetLayer(pauseBG, Util.LayerName.UI, 1);
		pauseScreen.transform.position = new Vector2((int)(Screen.width / 2 - pauseWidth / 2), (int)(Screen.height / 2 - pauseHeight / 2));

		//sliders
		int barWidth = (int)(pauseWidth - border*2 - gap * 3 - Sprites.GetBounds(Sprites.sound).x * S_Camera.scale);
		int barX = (int)(border + gap * 2 + Sprites.GetBounds(Sprites.sound).x * S_Camera.scale);
		int currentY = pauseHeight-border;
		currentY -= gap + S_OptionSlider.TOTAL_HEIGHT;
		GameObject musicSlider = S_OptionSlider.CreateSlider(barWidth);
		musicSlider.transform.position = new Vector2(barX, currentY);
		musicSlider.transform.SetParent(pauseScreen.transform, false);
		Util.SetZ(musicSlider, Util.ZLayer.Sliders);
		GameObject music = Primitives.CreateActor(Sprites.music);
		S_Camera.SetupScale(music.transform);
		music.transform.position = new Vector2(border + gap, currentY + S_OptionSlider.BAR_HEIGHT/2 - Sprites.GetBounds(Sprites.music).y * S_Camera.scale/2);
		music.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(music, Util.LayerName.UI, 50);
		currentY -= gap + S_OptionSlider.TOTAL_HEIGHT;
		GameObject soundSlider = S_OptionSlider.CreateSlider(barWidth);
		soundSlider.transform.position = new Vector2(barX, currentY);
		soundSlider.transform.SetParent(pauseScreen.transform, false);
		Util.SetZ(soundSlider, Util.ZLayer.Sliders);
		GameObject sound = Primitives.CreateActor(Sprites.sound);
		S_Camera.SetupScale(sound.transform);
		sound.transform.position = new Vector2(border + gap, currentY + S_OptionSlider.BAR_HEIGHT / 2 - Sprites.GetBounds(Sprites.music).y * S_Camera.scale / 2);
		sound.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(sound, Util.LayerName.UI, 50);
		currentY -= gap + S_OptionSlider.TOTAL_HEIGHT;


		//icons
		int codeX = (pauseWidth - border * 2) / 3 + border ;
		int musicX = (pauseWidth - border * 2) / 3*2 + border;

		int iconY = currentY;
		int twitterY = (int)(iconY - gap - Sprites.GetBounds(Sprites.twitter).y * S_Camera.scale/2f);
		iconY += gap / 2;

		GameObject code = Primitives.CreateActor(Sprites.code);
		S_Camera.SetupScale(code.transform);
		code.transform.position = new Vector2(codeX - Sprites.GetBounds(Sprites.code).x * S_Camera.scale / 2f, iconY  -Sprites.GetBounds(Sprites.code).y * S_Camera.scale / 2f);
		code.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(code, Util.LayerName.UI, 50);

		music = Primitives.CreateActor(Sprites.music);
		S_Camera.SetupScale(music.transform);
		music.transform.position = new Vector2(musicX - Sprites.GetBounds(Sprites.music).x * S_Camera.scale / 2f, iconY - Sprites.GetBounds(Sprites.music).x * S_Camera.scale / 2f);
		music.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(music, Util.LayerName.UI, 50);


		//bottom info
		S_Button myTwitter = S_Button.CreateButton(Sprites.twitter);
		S_Camera.SetupScale(myTwitter.transform);
		myTwitter.transform.position = new Vector2(codeX - Sprites.GetBounds(Sprites.twitter).x * S_Camera.scale / 2f, twitterY - Sprites.GetBounds(Sprites.twitter).y * S_Camera.scale / 2f);
		myTwitter.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(myTwitter.gameObject, Util.LayerName.UI, 50);
		myTwitter.SetDownAction(() => Application.OpenURL("https://twitter.com/ColourTann"));

		S_Button website = S_Button.CreateButton(Sprites.website);
		S_Camera.SetupScale(website.transform);
		website.transform.position = new Vector2(musicX - Sprites.GetBounds(Sprites.website).x * S_Camera.scale / 2f, twitterY - Sprites.GetBounds(Sprites.website).x * S_Camera.scale / 2f);
		website.transform.SetParent(pauseScreen.transform, false);
		Util.SetLayer(website.gameObject, Util.LayerName.UI, 50);
		website.SetDownAction(() => Application.OpenURL("https://twitter.com/ColourTann"));

		return pauseScreen;
	}
}
