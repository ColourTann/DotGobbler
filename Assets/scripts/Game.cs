using UnityEngine;
using System.Collections;

public class Game {
    private static Game self;
    public static Game Get() {
        if(self == null) {
            self = new Game();
        }
        return self;
    }

    public Level level;

    public Game() {
        GameObject background = Primitives.CreateRectangle(Screen.width, Screen.height, Colours.DARK);
        level = new Level();
    }
}
