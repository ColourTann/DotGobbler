using UnityEngine;
using System.Collections;

public class S_Pincer : S_Entity {

    override public void ChooseMove() {
        targetTile = currentTile.PathTo(Game.Get().level.player.currentTile);
        if (targetTile != null) {
            int dx = targetTile.x - currentTile.x;
            int dy = targetTile.y - currentTile.y;
            int rotation = 0;
            if (dx == -1) rotation = 180;
            else if (dy == 1) rotation = 90;
            else if (dy == -1) rotation = 270;
            transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
            targetTile.Block();
        }
    }

    override public void TakeTurn() {
        if (targetTile != null) {
            targetTile.UnBlock();
            if (targetTile.occupier != null && targetTile.occupier != this && targetTile.occupier.Blocks()) {
            }
            else { 
            MoveToTile(targetTile, false);
         }
        }
        ChooseMove();
    }

    public override bool Blocks() {
        return true;
    }
}
