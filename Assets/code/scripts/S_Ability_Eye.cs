using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class S_Ability_Eye : S_Ability {
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
                S_Tile tile = origin;
                while (true) {
                    S_Tile newTile = Game.Get().level.GetTile(tile.x + dx, tile.y + dy);
                    if (newTile == null) {
                        if (tile != origin) {
                            result.Add(tile);
                        }
                        break;
                    }
                    tile = newTile;
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
        S_Tile currentTile = player.currentTile;
        while (currentTile != null) {
			S_Pickup pickup = currentTile.content;
			if (pickup != null) {
				Game.Lock();
				currentTile.Enter(player, false);
			}
            currentTile = currentTile.GetTile(dx, dy);
        }
        SuccessfulUse();
    }
}
