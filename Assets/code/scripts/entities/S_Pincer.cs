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
                if(targetTile.occupier is S_Player) {
                    Game.Get().Lose();
                }
                MoveToTile(targetTile, false);
            }
        }
    }

    public override bool Blocks() {
        return true;
    }

    public override void Init()
    {
        base.Setup("pincer");
    }
}
