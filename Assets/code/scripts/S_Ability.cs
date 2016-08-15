using UnityEngine;
using System.Collections;

public class S_Ability : MonoBehaviour {
    int pips;
    bool active;
    AbilityType type;
    public enum AbilityType {
        Move3
    }
    public void Toggle() {
        active = !active;
        GetComponent<SpriteRenderer>().color = active ? Colours.LIGHT : Colours.RED;
    }

    public void Activate() {
      //  Game.Get().level.SetState();
    }

    public void Deplete() {
        pips--;
    }

    public bool IsAvailable() {
        return pips > 0;
    }

    public void init(int pips, AbilityType type) {
        this.type = type;
        this.pips = pips;
    }


}
