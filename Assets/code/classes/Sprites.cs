using UnityEngine;
using System.Collections;

public class Sprites {
    public static Sprite tile = Resources.Load<Sprite>("images/tile_0");
    public static Sprite tile_highlight = Resources.Load<Sprite>("images/tile_highlight");
    public static Sprite pixel = Resources.Load<Sprite>("images/pixel");
	
    public static Sprite ability_border = Resources.Load<Sprite>("images/abilities/border");
    public static Sprite ability_dash = Resources.Load<Sprite>("images/abilities/dash");
    public static Sprite ability_eye = Resources.Load<Sprite>("images/abilities/eye");
	public static Sprite ability_swap = Resources.Load<Sprite>("images/abilities/swap");
	public static Sprite ability_pip = Resources.Load<Sprite>("images/abilities/pip");

	public static Sprite charge_h = Resources.Load<Sprite>("images/charge_h");
	public static Sprite charge_v = Resources.Load<Sprite>("images/charge_v");

	public static Sprite[] tutorial_0_keyboard = new Sprite[] { Resources.Load<Sprite>("images/tutorial/push00"), Resources.Load<Sprite>("images/tutorial/push01") };
	public static Sprite[] tutorial_0_touch = new Sprite[] { Resources.Load<Sprite>("images/tutorial/swipe00"), Resources.Load<Sprite>("images/tutorial/swipe01") };

	public static Sprite[] tutorial_1_keyboard = new Sprite[] { Resources.Load<Sprite>("images/tutorial/push10"), Resources.Load<Sprite>("images/tutorial/push11") };
	public static Sprite[] tutorial_1_touch = new Sprite[] { Resources.Load<Sprite>("images/tutorial/swipe10"), Resources.Load<Sprite>("images/tutorial/swipe11") };

	public static Sprite eye = Resources.Load<Sprite>("images/enemy_eye");

    public static Vector2 GetBounds(Sprite s) {
        return new Vector2(s.bounds.size.x , s.bounds.size.y);
    }

}
