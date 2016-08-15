using UnityEngine;
using System.Collections;

public class Game {
    private static Game self;
    int levelNumber = 5;
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

    enum GameState {
        Normal, Restarting, NextLevel
    }

    GameState state = GameState.Normal;

    public void NextLevel() {
        state = GameState.NextLevel;
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
