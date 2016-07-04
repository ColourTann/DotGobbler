using UnityEngine;
using System.Collections;

public class S_Pincer : S_Entity {
    override public void TakeTurn() {
        S_Tile newTarget = currentTile.PathTo(Game.Get().level.player.currentTile);
        if (newTarget != null) {
            int dx = newTarget.x - currentTile.x;
            int dy = newTarget.y - currentTile.y;
            int rotation = 0;
            if (dx == -1) rotation = 180;
            else if (dy == 1) rotation = 90;
            else if (dy == -1) rotation = 270;
            transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
            MoveToTile(newTarget, false);
            
        }
    }
}
