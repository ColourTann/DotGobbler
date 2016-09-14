using UnityEngine;
using System.Collections;

public class S_Entity : MonoBehaviour {
    protected const float MOVE_SPEED = .1f;
    protected S_Tile previousTile;
    public S_Tile currentTile;
    protected bool moving;
    protected float moveTicker = 0;
    public GameObject PositionSetter;
    bool active = true;
    protected S_Tile targetTile;

    protected void Setup(string name) {
        PositionSetter = new GameObject(name);
        transform.parent = PositionSetter.transform;
        S_Camera.SetupScale(transform);
        transform.position = new Vector3(7.5f * S_Camera.scale, 7.5f * S_Camera.scale, 0);
    }

    public virtual void Init()
    {

    }

    protected virtual void Update () {
        if (!active) return;
        CheckInput();
        Move();
    }

    protected virtual void CheckInput() {
       
    }

    public virtual void TakeTurn() {

    }

    protected virtual void Move() {
        if (moving) {
            moveTicker += Time.deltaTime / MOVE_SPEED;
            if (moveTicker >= 1) {
                moveTicker = 1;
                currentTile.Enter(this);
                moving = false;
                FinishedMoving();
            }
        }
        if(previousTile!=null) PositionSetter.transform.position = Vector3.Lerp(previousTile.transform.position, currentTile.transform.position, Interpolation.Pow2Out(0, 1, moveTicker));
    }

    protected int currentDX, currentDY;

    public virtual void MoveToTile(S_Tile tile, bool instant) {
        if (tile == null || tile==currentTile) return;
        previousTile = currentTile;
        SetTile(tile);
        if (previousTile != null){
            currentDX = currentTile.x - previousTile.x;
            currentDY = currentTile.y - previousTile.y;
        }
        if (instant) {
            PositionSetter.transform.localPosition = currentTile.transform.localPosition;
            tile.Enter(this);
        }
        else {
            StartMoving();
        }
    }

    void StartMoving() {
        moving = true;
        moveTicker = 0;
    }

    public void SetTile(S_Tile tile) {
        if (previousTile != null && previousTile.occupier==this) {
            previousTile.occupier = null;
        }
        currentTile = tile;
        currentTile.occupier = this;
    }

    protected virtual void FinishedMoving() { }

    public virtual bool Blocks() { return false; }

    public void Deactivate() {
        active = false;
    }

    public virtual void ChooseMove() {
    }

}
