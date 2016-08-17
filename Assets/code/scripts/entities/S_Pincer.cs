using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Pincer : S_Entity {

    override public void ChooseMove() {
        S_Tile playerTile = Game.Get().level.player.currentTile;
        targetTile = currentTile.PathTo(playerTile);

        Level level = Game.Get().level;


        //if there's no path, pick a tile that's vaguely in the right direction
        if (targetTile == null) {
            List<S_Tile> potentials = currentTile.GetTilesWithin(1, false);
            int bestWeirdDist = 999;
            foreach (S_Tile t in potentials) {
                if (t.IsBlocked()) continue;
                int xDist = t.x - playerTile.x;
                int yDist = t.y - playerTile.y;
                int calcDist = xDist * xDist + yDist * yDist;
                if (calcDist < bestWeirdDist) {
                    bestWeirdDist = calcDist;
                    targetTile = t;
                }
            }

        }

        if (targetTile != null) {
            int dx = targetTile.x - currentTile.x;
            int dy = targetTile.y - currentTile.y;
            int rotation = 0;
            if (dx == -1) rotation = 180;
            else if (dy == 1) rotation = 90;
            else if (dy == -1) rotation = 270;
            if (rotation - startRotation > 180) {
                rotation -= 360;
            }
            if (rotation - startRotation < -180) {
                rotation += 360;
            }
            targetRotation = rotation;
            targetTile.Block();
        }
        else {

            Debug.Log("no path!!");
        }
    }

    override public void TakeTurn() {
        startRotation = targetRotation;
        if (targetTile != null) {
            targetTile.UnBlock();
            if (targetTile.occupier != null && targetTile.occupier != this && targetTile.occupier.Blocks()) {
                //can't move there!
            }
            else {
                if (targetTile.occupier is S_Player) {
                    Game.Get().Lose();
                }
                MoveToTile(targetTile, false);
            }
        }
    }

    public override bool Blocks() {
        return true;
    }

    public override void Init() {
        base.Setup("pincer");
    }
}
