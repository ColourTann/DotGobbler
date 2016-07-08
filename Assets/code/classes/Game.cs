using UnityEngine;
using System.Collections;

public class Game {
    private static Game self;
    int levelNumber = 3;
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
    }

    public void Init() {
        LoadLevel();
    }

    public void Lose() {
        Restart();
    }

    public void NextLevel() {
        levelNumber++;
        LoadLevel();
    }

    public void Restart() {
        LoadLevel();
    }

    private void LoadLevel() {
        previousLevel = level;
        if (previousLevel != null) {
            previousLevel.SlideAway();
        }
        Texture2D levelData = Resources.Load("levels/" + levelNumber) as Texture2D;
        if (levelData == null) {
           levelNumber--;
           levelData = Resources.Load("levels/" + levelNumber) as Texture2D;
        }
        level = new Level(levelData);
        level.Init();
        level.SlideIn();
    }
}
