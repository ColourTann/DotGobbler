using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class S_Ability_Move3 : S_Ability {

	public override KeyCode GetKey() {
		return KeyCode.Z;
	}

	public override Sprite GetSprite() {
        return Sprites.ability_dash;
    }

    public override TargetingType GetTargetingType() {
        return TargetingType.SingleTile;
    }

    public override List<S_Tile> GetValidTiles(S_Tile origin) {
        List<S_Tile> result = new List<S_Tile>();
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx != 0 && dy != 0) continue;
                if (dx == 0 && dy == 0) continue;
                bool good = true;
                for (int dist = 1; dist <= 3; dist++) {
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

    public override void Use(S_Player player, S_Tile tile) {
        if (!GetValidTiles(player.currentTile).Contains(tile)) return;
        player.MoveToTile(tile, false);
        SuccessfulUse();
    }

    public override void Use(S_Player player, int dx, int dy) {
        S_Tile target = Game.Get().level.GetTile(player.currentTile.x + dx * 3, player.currentTile.y + dy * 3);
        if (target == null) return;
        player.MoveToTile(target, false);
        SuccessfulUse();
    }
}
