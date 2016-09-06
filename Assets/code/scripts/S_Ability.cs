using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public abstract class S_Ability : MonoBehaviour {
    int pips;
    bool active;

    public abstract Sprite GetSprite();

	public abstract KeyCode GetKey();

    public void Toggle(bool sound) {

		if(sound) Game.Get().level.ActionCompleted(active ? Level.ActionType.DeselectAbility : Level.ActionType.SelectAbility);

        if (!active && !IsAvailable()) {
            if(sound) Sounds.PlaySound(Sounds.deselect, .3f, Random.Range(.7f, .8f));
            return;
        }
        
        active = !active;
        if (sound) Sounds.PlaySound(active ?Sounds.select: Sounds.deselect, .3f, Random.Range(.7f, .8f));
        GetComponent<SpriteRenderer>().color = active ? Colours.LIGHT : Colours.RED;
        Game.Get().level.ActivateAbility(this, active);
    }

    public enum TargetingType { SingleTile, Line}

    public abstract TargetingType GetTargetingType();

    public abstract void Use(S_Player player, int dx, int dy);

    public abstract void Use(S_Player player, S_Tile tile);

    internal void SuccessfulUse() {
        pips--;
        UpdatePips();
        Toggle(false);
		Game.Get().level.ActionCompleted(Level.ActionType.UseAbility);
	}

	internal void Click() {
		Game.Get().level.DeselectAbility(this);
		Toggle(true);
	}

	internal void UnsuccessfulUse() {
        Toggle(true);
    }

    public abstract List<S_Tile> GetValidTiles(S_Tile origin);

    public void Deplete() {
        pips--;
        UpdatePips();
    }

    public bool IsAvailable() {
        return pips > 0;
    }

    public void init(int pips) {
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
        int currentX = 2 + pipGap;
        for (int i = 0; i < pips; i++) {
            GameObject pip = Primitives.CreateActor(Sprites.ability_pip, currentX, 2);
            pipObjects.Add(pip);
            Util.SetLayer(pip, Util.LayerName.UI, 5);
            currentX += (int)(Sprites.GetBounds(Sprites.ability_pip).x);
            currentX += pipGap;
            pip.transform.SetParent(transform, false);
            pip.name = "pip";
        }
    }

	GameObject text;

	internal void SetText(GameObject text) {
		this.text = text;
	}

	internal void ClearText() {
		GameObject.Destroy(text);
	}
}
