using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class S_Ability_Eye : S_Ability {

	public override KeyCode GetKey() {
		return KeyCode.X;
	}

	public override Sprite GetSprite() {
        return Sprites.ability_eye;
    }

    public override TargetingType GetTargetingType() {
        return TargetingType.Line;
    }

    public override List<S_Tile> GetValidTiles(S_Tile origin) {
        List<S_Tile> result = new List<S_Tile>();
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx != 0 && dy != 0) continue;
                if (dx == 0 && dy == 0) continue;
				List<S_Tile> line = origin.GetTilesInLine(dx, dy);
				if(line.Count>0){
                     result.Add(line[line.Count-1]);
                }
            }
        }
        return result;
    }

    public override void Use(S_Player player, S_Tile tile) {
        int dx = tile.x - player.currentTile.x;
        int dy = tile.y - player.currentTile.y;
        if ((dx == 0) == (dy == 0)) {
            UnsuccessfulUse();
            return;
        }
        Use(player, Util.ProperSign(dx), Util.ProperSign(dy));
    }

    public override void Use(S_Player player, int dx, int dy) {
        if ((dx == 0) == (dy == 0)) {
            UnsuccessfulUse();
            return;
        }

		int index = 0;
		foreach(S_Tile tile in player.currentTile.GetTilesInLine(dx, dy)) {
			S_Pickup pickup = tile.content;
			if (pickup != null) {
				index++;
				pickup.Magnetise(tile.TileDistance(player.currentTile)+index, (int)player.transform.position.x-S_Tile.width/2, (int)player.transform.position.y-S_Tile.height/2);
			}
		}
		if (index > 0) {
			SuccessfulUse();
			Sounds.PlaySound(Sounds.suck);
		}
		else UnsuccessfulUse();
    }
}
