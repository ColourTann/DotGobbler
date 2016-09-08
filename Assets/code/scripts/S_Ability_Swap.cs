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
		swappee.CancelMove();
		swappee.MoveToTile(player.currentTile, true);
		player.MoveToTile(tile, true);
		swappee.Stun(1);
		SuccessfulUse();
		Game.Get().level.EnemyTurn();
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
