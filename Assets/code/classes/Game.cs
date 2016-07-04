using UnityEngine;
using System.Collections;

public class Game {
    private static Game self;
    int levelNumber = 0;
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
        NextLevel();
    }

    public void NextLevel() {
        previousLevel = level;
        if (previousLevel != null) {
            previousLevel.SlideAway();
        }
        level = new Level(levelNumber++);
        level.Init();
        level.SlideIn();
    }
}
