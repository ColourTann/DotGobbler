using UnityEngine;
using System.Collections.Generic;
using System;

public class Game {

	public int levelNumber = 0;
	public const bool KEYBOARD = true;
	public Level previousLevel;
	private Level level;

	public Game() {
		GameObject background = Primitives.CreateRectangle(Screen.width, Screen.height, Colours.DARK);
		background.name = "backdrop";
		background.transform.SetParent(GetMisc("UI").transform, false);

		int gap = 5 * S_Camera.scale;

        S_Button restartButton = S_Button.CreateButton(Sprites.restart);
        S_Camera.SetupScale(restartButton.transform);
        restartButton.transform.position = new Vector2(gap, Screen.height - 5 * S_Camera.scale - Sprites.GetBounds(Sprites.restart).y * S_Camera.scale);
        restartButton.SetDownAction(InstantRestart);
		restartButton.name = "restart_button";
		restartButton.transform.SetParent(GetMisc("UI").transform, false);
		Util.SetZ(restartButton.gameObject, Util.ZLayer.Buttons );


		S_Button optionsButton = S_Button.CreateButton(Sprites.options);
        S_Camera.SetupScale(optionsButton.transform);
        optionsButton.transform.position = new Vector2(gap*2 + Sprites.GetBounds(Sprites.options).x * S_Camera.scale, Screen.height - 5 * S_Camera.scale - Sprites.GetBounds(Sprites.restart).y * S_Camera.scale);
        optionsButton.SetDownAction(()=> { level.Pause(); } );
		optionsButton.name = "options_button";
		optionsButton.transform.SetParent(GetMisc("UI").transform, false);
		Util.SetZ(optionsButton.gameObject, Util.ZLayer.Buttons);
	}

	public void Init() {
		LoadLevel();
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
		previousLevel = level;
		if (previousLevel != null) {
			previousLevel.SlideAway();
		}
		Texture2D levelData = GetLevelData(levelNumber);
		if (levelData == null) {
			levelNumber--;
			levelData = GetLevelData(levelNumber);
		}
        GameObject go = new GameObject("level parent");

        level = go.AddComponent<Level>();
		level.Init(levelData);
		level.SlideIn();
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
}
