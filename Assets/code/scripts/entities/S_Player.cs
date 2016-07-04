using UnityEngine;
using System.Collections;

public class S_Player : S_Entity {




    int nextDx = -5;
    int nextDy = -5;

    static readonly int minSwipe = Screen.width / 15;
    bool touching;
    Vector2 touchStart;
    override protected void Update() {
        base.Update();
    }

    override protected void CheckInput() {
        int dx = 0;
        int dy = 0;
        
        if (Input.GetKeyDown("left")) dx = -1;
        else if (Input.GetKeyDown("right")) dx = 1;
        else if (Input.GetKeyDown("down")) dy = -1;
        else if (Input.GetKeyDown("up")) dy = 1;

        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                touching = true;
                touchStart = touch.position;
            }


            else if (touching){
                if (touch.phase == TouchPhase.Ended) {
                    touching = false;
                }
                Vector2 newTouch = touch.position;
                float xDiff = newTouch.x - touchStart.x;
                float yDiff = newTouch.y - touchStart.y;
                float absX = Mathf.Abs(xDiff);
                float absY = Mathf.Abs(yDiff);
                if(Mathf.Max(absX, absY) > minSwipe) {
                    if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff)) {
                        dx = (int)Mathf.Sign(xDiff);
                    }
                    else {
                        dy = (int)Mathf.Sign(yDiff);
                    }
                    touching = false;
                }
            }
        }





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

