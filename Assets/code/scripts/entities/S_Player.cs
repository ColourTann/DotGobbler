using UnityEngine;
using System.Collections;

public class S_Player : S_Entity {




    int nextDx = -5;
    int nextDy = -5;

   

    override protected void Update () {
        base.Update();
        
    }

    override protected void CheckInput() {
        int dx = 0;
        int dy = 0;
        
        if (Input.GetKeyDown("left")) dx = -1;
        else if (Input.GetKeyDown("right")) dx = 1;
        else if (Input.GetKeyDown("down")) dy = -1;
        else if (Input.GetKeyDown("up")) dy = 1;
        if (moving && dx != 0 || dy != 0) {
            nextDx = dx;
            nextDy = dy;
        }
        if (!moving) {
            if (nextDx != -5) {
                dx = nextDx;
                dy = nextDy;
            }
            if (dx != 0 || dy != 0) {
                nextDx = -5;
                nextDy = -5;
                S_Tile newTile = currentTile.GetTile(dx, dy);
                if (newTile != null) {
                    MoveToTile(newTile, false);
                }
            }
        }
    }

    protected override void FinishedMoving() {
        Game.Get().level.Turn();
    }

}

