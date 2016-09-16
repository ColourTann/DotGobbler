using UnityEngine;
using System.Collections;

public class S_Player : S_Entity {
    static readonly int minSwipe = Screen.width / 15;

    int nextDx = -5;
    int nextDy = -5;
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

		//abilities
		if (Input.GetKeyDown(KeyCode.Z)) AbilityKeyboardPress(KeyCode.Z);
		if (Input.GetKeyDown(KeyCode.X)) AbilityKeyboardPress(KeyCode.X);
		if (Input.GetKeyDown(KeyCode.C)) AbilityKeyboardPress(KeyCode.C);

		//directions
		int dx = 0;
        int dy = 0;

		//put key input into directions
        if (Input.GetKeyDown(KeyCode.LeftArrow)) dx = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) dx = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) dy = -1;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) dy = 1;

		//put swipe input into directions
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

		//buffer input for next turn if two directions are pressed
        if (moving && (dx != 0 || dy != 0)) {
            nextDx = dx;
            nextDy = dy;
        }

        if (!moving && !Game.isLocked()) {
            if (nextDx != -5) {
                dx = nextDx;
                dy = nextDy;
            }
            ActivateDirection(dx, dy);
        }
    }

	void AbilityKeyboardPress(KeyCode key) {
        Level.Get(gameObject).abilityPanel.ActivateAbilityFromKeypress(key);
	}

    void ActivateDirection(int dx, int dy) {
        if (!(dx != 0 || dy != 0)) return;
        nextDx = -5;
        nextDy = -5;


		if (Level.Get(gameObject).abilityPanel.activeAbility != null) {
            Level.Get(gameObject).abilityPanel.activeAbility.Use(this, dx, dy);
        }

        else {
            S_Tile newTile = currentTile.GetTile(dx, dy);
            if (newTile != null) {
                ActivateTile(newTile);
            }
        }
    }

    public void ActivateTile(S_Tile tile) {
        //check for validity
        if (moving || tile==null) return;
		Sounds.PlaySound(Sounds.move, .75f, Random.Range(.9f, 1.1f));
		if (Level.Get(gameObject).abilityPanel.activeAbility != null) {
            Level.Get(gameObject).abilityPanel.activeAbility.Use(this, tile);
        }
        else {
            if (tile.GetDistance(currentTile) != 1) return;
            MoveToTile(tile, false);
        }
    }

    protected override void FinishedMoving() {
        Level.Get(gameObject).EnemyTurn();
    }

    public override void MoveToTile(S_Tile tile, bool instant) {
        bool lost = tile.occupier != null;
        if (lost) Game.Get().Lose();
        base.MoveToTile(tile, instant);
    }


    public override void Init() {
        base.Setup("player");
    }

    protected override void Move() {
        base.Move();
        float thing = Mathf.Sin(moveTicker * Mathf.PI) * .2f;
        transform.localScale = new Vector2(S_Camera.scale * (1 + (currentDX == 0 ? -thing : thing)), S_Camera.scale * (1 + (currentDY == 0 ? -thing : thing)));
    }
    }



