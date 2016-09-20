using UnityEngine;
using System.Collections.Generic;

public class Game {

	public int levelNumber = 0;
	public const bool KEYBOARD = true;
	public Level previousLevel;
	private Level level;
	S_Button mysteryButton;
	GameObject innerMystery;
	int mysteryCol = 0;
	public static bool paused;
	public Game() {
		levelNumber = PlayerPrefs.GetInt("level", 0);
		GameObject background = Primitives.CreateRectangle(Screen.width, Screen.height, Colours.DARK);
		background.name = "backdrop";
		background.transform.SetParent(GetMisc("UI").transform, false);

		int gap = 5 * S_Camera.scale;
		mysteryButton = S_Button.CreateButton(Sprites.outline);
		S_Camera.SetupScale(mysteryButton.transform);
		mysteryButton.transform.position = new Vector2(gap, Screen.height - 5 * S_Camera.scale - Sprites.GetBounds(Sprites.restart).y * S_Camera.scale);
		mysteryButton.SetDownAction(() => { MysteryButton(); });
		mysteryButton.name = "mystery";
		mysteryButton.transform.SetParent(GetMisc("UI").transform, false);
		Util.SetZ(mysteryButton.gameObject, Util.ZLayer.Buttons);

		S_Button optionsButton = S_Button.CreateButton(Sprites.options);
        S_Camera.SetupScale(optionsButton.transform);
        optionsButton.transform.position = new Vector2(gap*2 + Sprites.GetBounds(Sprites.options).x * S_Camera.scale, Screen.height - 5 * S_Camera.scale - Sprites.GetBounds(Sprites.restart).y * S_Camera.scale);
        optionsButton.SetDownAction(()=> { PauseButton(); } );
		optionsButton.name = "options_button";
		optionsButton.transform.SetParent(GetMisc("UI").transform, false);
		Util.SetZ(optionsButton.gameObject, Util.ZLayer.Buttons);

		S_Button restartButton = S_Button.CreateButton(Sprites.restart);
		S_Camera.SetupScale(restartButton.transform);
		restartButton.transform.position = new Vector2(gap*3 + Sprites.GetBounds(Sprites.options).x * S_Camera.scale * 2, Screen.height - 5 * S_Camera.scale - Sprites.GetBounds(Sprites.restart).y * S_Camera.scale);
		restartButton.SetDownAction(()=> { RestartButton(); });
		restartButton.name = "restart_button";
		restartButton.transform.SetParent(GetMisc("UI").transform, false);
		Util.SetZ(restartButton.gameObject, Util.ZLayer.Buttons);
	}

	public void MysteryButton() {
		if (IsPaused()) return;
		Sounds.PlaySound(Sounds.mystery, 1, Random.Range(.8f, 1.2f));
		ToggleColour();
	}

	public void PauseButton() {
		if (IsPaused()) return;
		Sounds.PlaySound(Sounds.select);
		Pause();
	}

	public void RestartButton() {
		if (IsPaused()) return;
		InstantRestart();
		Sounds.PlaySound(Sounds.select);
	}
	

	public void Init() {
		LoadLevel();
		Pause();
		Unpause();
	}

	public enum GameState {
		Normal, Restarting, NextLevel
	}
	public GameState state = GameState.Normal;

	public void Lose() {
		Restart();
	}

	public void Victory() {
		if (state == GameState.Normal) state = GameState.NextLevel;
	}

    public void InstantRestart() {
        LoadLevel();    
    }

    public void Restart(bool instant = false) {
		if (state == GameState.Normal) state = GameState.Restarting;
	}

	public void CheckForEndOfLevel() {
		switch (state) {
			case GameState.NextLevel:
				levelNumber++;
				LoadLevel();
				state = GameState.Normal;
				break;
			case GameState.Restarting:
				LoadLevel();
				state = GameState.Normal;
				break;
		}
	}

	private void LoadLevel() {

		UpdateMystery();
		previousLevel = level;
		if (previousLevel != null) {
			previousLevel.SlideAway();
		}
		Texture2D levelData = GetLevelData(levelNumber);
		if (levelData == null) {
			EndScreen();
			return;
		}
        GameObject go = new GameObject("level parent");
		PlayerPrefs.SetInt("level", levelNumber);
        level = go.AddComponent<Level>();
		level.Init(levelData);
		level.SlideIn();
	}

	static bool end;

	private void EndScreen() {
		end = true;
		GameObject rect = Primitives.CreateRectangle(Screen.width, Screen.height, Colours.DARK);
		Util.SetLayer(rect, Util.LayerName.Particles, 400);
		GameObject go = Primitives.CreateActor(Sprites.end);
		int xScale = (int)(Screen.width / Sprites.GetBounds(Sprites.end).x);
		int yScale = (int)(Screen.height/ Sprites.GetBounds(Sprites.end).y);
		int scale = Mathf.Min(xScale, yScale);
		go.transform.localScale = new Vector2(scale, scale);
		go.transform.position = new Vector2((int)(Screen.width - Sprites.GetBounds(Sprites.end).x * scale)/2, (int)(Screen.height - Sprites.GetBounds(Sprites.end).y * scale)/2);
		Util.SetLayer(go, Util.LayerName.Particles, 500);
		PlayerPrefs.SetInt("level", 0);
	}

	private void UpdateMystery() {
		if (levelNumber >= Sprites.mysteries.Length) return;
		if (innerMystery != null) GameObject.Destroy(innerMystery);
		innerMystery = Primitives.CreateActor(Sprites.mysteries[levelNumber]);
		innerMystery.transform.SetParent(mysteryButton.transform, false);
		innerMystery.transform.localPosition = new Vector2(1, 1);
		innerMystery.transform.localScale = new Vector2(2, 2);
		Util.SetLayer(innerMystery, Util.LayerName.UI, 50);
		innerMystery.GetComponent<SpriteRenderer>().color = Colours.ALL[mysteryCol];
	}

	private void ToggleColour() {
		mysteryCol = (mysteryCol + 1) % 3;
		innerMystery.GetComponent<SpriteRenderer>().color = Colours.ALL[mysteryCol];
	}

	private Texture2D GetLevelData(int level) {
		string path = "levels/";
		if (level < 10) path += "0";
		path += level;
		return Resources.Load(path) as Texture2D;
	}

	//kinda dodgy system for dealing with abilities that take time
	private static int locks;
	public static void Lock() {
		locks++;
	}

	public static void Unlock() {
		locks--;
		if (!isLocked()) {
			if (Game.Get().state == GameState.Normal) {
				Game.Get().level.EnemyTurn();
			}
			else {
				Game.Get().CheckForEndOfLevel();
			}
		}
	}

	public static bool isLocked() {
		return locks != 0 && Get().state == GameState.Normal;
	}

	private static Game self;
	public static Game Get() {
		if (self == null) self = new Game();
		return self;
	}

	// for parenting miscellaneous gameobjects so they show up nicely in the hierarchy
	private static GameObject misc;
	public static GameObject GetMisc() {
		if (misc == null) misc = new GameObject("misc");
		return misc;
	}
	public static Dictionary<string, GameObject> objectTable = new Dictionary<string, GameObject>();
	public static GameObject GetMisc(string name) {
		if (objectTable.ContainsKey(name)) {
			return objectTable[name];
		}
		GameObject go = new GameObject(name);
		go.transform.SetParent(GetMisc().transform);
		objectTable.Add(name, go);
		return go;
	}

	S_Button inputBlocker;

	GameObject pauseScreen;

	public void Pause() {
		if (pauseScreen == null) {
			inputBlocker = Primitives.CreateInputBlocker();
			inputBlocker.SetDownAction(() => { Unpause(); Sounds.PlaySound(Sounds.deselect); });
			Util.SetZ(inputBlocker.gameObject, Util.ZLayer.Blocker);
			pauseScreen = PauseMaker.CreatePauseScreen();
			Time.timeScale = 0;
		}
		inputBlocker.gameObject.SetActive(true);
		pauseScreen.SetActive(true);
		Game.paused = true;
	}

	public void Unpause() {
		Game.paused = false;
		inputBlocker.gameObject.SetActive(false);
		pauseScreen.SetActive(false);
		Time.timeScale = 1;
	}

	internal static bool IsPaused() {
		return paused || end;
	}

	public static bool IsCurrent(GameObject go) {
		return Level.Get(go) == Get().level;
	}
}
