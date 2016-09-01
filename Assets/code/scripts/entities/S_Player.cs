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

    public override bool Blocks() {
        return false;
    }

    override protected void CheckInput() {
		if (Game.isLocked()) return;
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
            else if (touching) {
                if (touch.phase == TouchPhase.Ended) {
                    touching = false;
                }
                Vector2 newTouch = touch.position;
                float xDiff = newTouch.x - touchStart.x;
                float yDiff = newTouch.y - touchStart.y;
                float absX = Mathf.Abs(xDiff);
                float absY = Mathf.Abs(yDiff);
                if (Mathf.Max(absX, absY) > minSwipe) {
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
            ActivateDirection(dx, dy);
        }
    }

    void ActivateDirection(int dx, int dy) {
        if (!(dx != 0 || dy != 0)) return;
        nextDx = -5;
        nextDy = -5;

        if (Game.Get().level.activeAbility != null) {
            Sounds.PlaySound(Sounds.move, .75f, Random.Range(1.2f, 1.4f));
            Game.Get().level.activeAbility.Use(this, dx, dy);
        }

        else {
            S_Tile newTile = currentTile.GetTile(dx, dy);
            if (newTile != null) {
                Sounds.PlaySound(Sounds.move, .75f, Random.Range(.7f, .9f));
                ActivateTile(newTile);
            }
        }
    }

    public void ActivateTile(S_Tile tile) {
        //check for validity
        if (moving || tile==null) return;
        if (Game.Get().level.activeAbility != null) {
            Game.Get().level.activeAbility.Use(this, tile);
        }
        else {
            if (tile.GetDistance(currentTile) != 1) return;
            MoveToTile(tile, false);
        }
    }

    protected override void FinishedMoving() {

        Game.Get().level.Turn();
    }

    public override void MoveToTile(S_Tile tile, bool instant) {
        if (!instant) {
            
        }
        bool lost = tile.occupier != null;
        if (lost) Game.Get().Lose();
        base.MoveToTile(tile, instant);
    }


    public override void Init() {
        base.Setup("player");
    }

}

