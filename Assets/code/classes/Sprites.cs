using UnityEngine;
using System.Collections;

public class Sprites {
    public static Sprite tile = Resources.Load<Sprite>("images/tile_0");
    public static Sprite tile_highlight = Resources.Load<Sprite>("images/tile_highlight");
    public static Sprite pixel = Resources.Load<Sprite>("images/pixel");


    public static Sprite ability_border = Resources.Load<Sprite>("images/abilities/border");
    public static Sprite ability_dash = Resources.Load<Sprite>("images/abilities/dash");
    public static Sprite ability_pip = Resources.Load<Sprite>("images/abilities/pip");

    public static Vector2 GetBounds(Sprite s) {
        return new Vector2(s.bounds.size.x , s.bounds.size.y);
    }

}
