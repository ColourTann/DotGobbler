using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Ability_Swap : S_Ability {
	public override KeyCode GetKey() {
		return KeyCode.C;
	}

	public override Sprite GetSprite() {
		return Sprites.ability_swap;
	}

	public override TargetingType GetTargetingType() {
		return TargetingType.SingleTile;
	}

	public override List<S_Tile> GetValidTiles(S_Tile origin) {
		List<S_Tile> result = new List<S_Tile>();
		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				if ((dx == 0) == (dy == 0)) continue;
				List<S_Tile> possibles = origin.GetTilesInLine(dx, dy);
				foreach(S_Tile poss in possibles) {
					if (poss.occupier != null) {
						result.Add(poss);
						break;
					}
				}
			}
		}
		return result;
	}

	public override void Use(S_Player player, S_Tile tile) {
		S_Enemy swappee = (S_Enemy)tile.occupier;

        float time = 0.2f;
        Color col = Colours.GREEN;
        int width = (S_Tile.width-S_Camera.scale)/S_Camera.scale;
        int height = S_Tile.height / S_Camera.scale;

        GameObject flasher = S_Flasher.CreateFlasher(width, height, col, time);
        Util.SetLayer(flasher, Util.LayerName.UI, 0);
        flasher.transform.SetParent(tile.transform, false);

        flasher = S_Flasher.CreateFlasher(width, height, col, time);
        Util.SetLayer(flasher, Util.LayerName.UI, 0);
        flasher.transform.SetParent(player.currentTile.transform, false);

        swappee.CancelMove();
		swappee.MoveToTile(player.currentTile, true);
		player.MoveToTile(tile, true);
		swappee.Stun(1);
		Sounds.PlaySound(Sounds.teleport);
		SuccessfulUse();
        Level.Get(gameObject).EnemyTurn();
	}

	public override void Use(S_Player player, int dx, int dy) {
		if((dx == 0) == (dy == 0)) {
			UnsuccessfulUse();
			return;
		}
		List<S_Tile> line = player.currentTile.GetTilesInLine(dx, dy);
		foreach (S_Tile potential in line) {
			if (potential.occupier != null && !(potential.occupier is S_Player)) {
				Use(player, potential);
				return;
			}
		}
		UnsuccessfulUse();
	}
}
