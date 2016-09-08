using UnityEngine;
using System.Collections.Generic;
using System;

public class Game {
	public const bool KEYBOARD = true;
    private static Game self;
	public int levelNumber = 27;
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

	public static Game Get() {
        if(self == null) {
            self = new Game();
        }
        return self;
    }
    
    public Level previousLevel;
    public Level level;

    public Game() {
        GameObject background = Primitives.CreateRectangle(Screen.width, Screen.height, Colours.DARK);
        background.name = "backdrop";
    }

    public void Init() {
        LoadLevel();
    }

    public void Lose() {
        Restart();
    }


	public enum GameState {
        Normal, Restarting, NextLevel
    }

	public GameState state = GameState.Normal;

    public void NextLevel() {
		if (state == GameState.Normal) {
			state = GameState.NextLevel;
		}
    }

    public void Restart() {
        if(state == GameState.Normal) {
            state = GameState.Restarting;
        }
    }

    public void EndOfTurn() {
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
        level = new Level(levelData);
        level.Init();
        level.SlideIn();
    }

    private Texture2D GetLevelData(int level) {
        string path = "levels/";
        if (level < 10) path += "0";
        path += level;
        return Resources.Load(path) as Texture2D; 
    }

	private static int locks;
	public static void Lock() {
		locks++;
	}

	public static void Unlock() {
		locks--;
		if (!isLocked()) {
			if(Game.Get().state == GameState.Normal) {
				Game.Get().level.Turn();
			}
			else {
				Game.Get().EndOfTurn();
			}
		}
	}

	public static bool isLocked() {
		return locks != 0;
	}
}
