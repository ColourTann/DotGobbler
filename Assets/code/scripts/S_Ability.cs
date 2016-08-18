using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Ability : MonoBehaviour {
    int pips;
    bool active;
    AbilityType type;
    public enum AbilityType {
        Move3
    }
    public void Toggle(bool sound) {
        if (!active && !IsAvailable()) {
            if(sound) Sounds.PlaySound(Sounds.deselect, .3f, Random.Range(.7f, .8f));
            return;
        }
        
        active = !active;
        if (sound) Sounds.PlaySound(active ?Sounds.select: Sounds.deselect, .3f, Random.Range(.7f, .8f));
        GetComponent<SpriteRenderer>().color = active ? Colours.LIGHT : Colours.RED;
        Game.Get().level.ActivateAbility(this, active);
    }

    public void Use(S_Player player, int dx, int dy) {
        S_Tile target = Game.Get().level.GetTile(player.currentTile.x + dx * 3, player.currentTile.y + dy * 3);
        if (target == null) return;
        player.MoveToTile(target, false);
        SuccessfulUse();
    }

    public void Use(S_Player player, S_Tile tile) {
        if (!GetValidTiles(player.currentTile).Contains(tile)) return;
        player.MoveToTile(tile, false);
        SuccessfulUse();
    }

    internal void SuccessfulUse() {
        pips--;
        UpdatePips();
        Toggle(false);
    }


    public List<S_Tile> GetValidTiles(S_Tile origin) {
        List<S_Tile> result = new List<S_Tile>();
        for (int dx = -1; dx <= 1; dx ++) {
            for (int dy = -1; dy <= 1; dy ++) {
                if (dx != 0 && dy != 0) continue;
                if (dx == 0 && dy == 0) continue;
                bool good = true;
                for(int dist = 1; dist <= 3; dist++) {
                    S_Tile t = Game.Get().level.GetTile(origin.x + dx * dist, origin.y + dy * dist);
                    if (t == null) {
                        good = false;
                        break;
                    }
                }
                if (good) {
                    result.Add(Game.Get().level.GetTile(origin.x + 3 * dx, origin.y + 3 * dy));
                }
            }
        }

        return result;
    }

    public void Deplete() {
        pips--;
        UpdatePips();
    }

    public bool IsAvailable() {
        return pips > 0;
    }

    public void init(int pips, AbilityType type) {
        this.type = type;
        this.pips = pips;
        UpdatePips();
    }


    List<GameObject> pipObjects = new List<GameObject>();
    internal void UpdatePips() {
        foreach(GameObject p in pipObjects) {
            Destroy(p);
        }
        pipObjects.Clear();
        int pipGap = (int)((Sprites.GetBounds(Sprites.ability_border).x - 2 - Sprites.GetBounds(Sprites.ability_pip).x * pips) / (pips + 1));
        int currentX = S_Camera.scale + pipGap;
        for (int i = 0; i < pips; i++) {
            GameObject pip = Primitives.CreateActor(Sprites.ability_pip, currentX, S_Camera.scale);
            pipObjects.Add(pip);
            Util.SetLayer(pip, Util.LayerName.UI, 5);
            currentX += (int)(Sprites.GetBounds(Sprites.ability_pip).x);
            currentX += pipGap;
            pip.transform.SetParent(transform, false);
            pip.name = "pip";
        }
    }

}
