using UnityEngine;
using System.Collections;

public class S_Entity : MonoBehaviour {
    protected const float MOVE_SPEED = .1f;
    protected S_Tile previousTile;
    public S_Tile currentTile;
    protected bool moving;
    protected float moveTicker = 0;
    public GameObject PositionSetter;

    public virtual void Init() {
        PositionSetter = new GameObject("pos set");
        transform.parent = PositionSetter.transform;
        S_Camera.SetupScale(transform);
        transform.position = new Vector3(7.5f * S_Camera.scale, 7.5f * S_Camera.scale, 0);
    }

    protected virtual void Update () {
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
                currentTile.Enter();
                moving = false;
                FinishedMoving();
            }
            PositionSetter.transform.position = Vector3.Lerp(previousTile.transform.position, currentTile.transform.position, Interpolation.Pow2Out(0, 1, moveTicker));
        }
    }

    public void MoveToTile(S_Tile tile, bool instant) {
        if (tile == null) return;
        previousTile = currentTile;
        SetTile(tile);
        if (instant) {
            PositionSetter.transform.position = currentTile.transform.position;
            tile.Enter();
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
        currentTile = tile;
    }

    protected virtual void FinishedMoving() { }

}
