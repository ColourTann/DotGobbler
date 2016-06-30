using UnityEngine;
using System.Collections;

public class S_Player : MonoBehaviour {
    const float MOVE_SPEED = .1f;
    public S_Tile previousTile;
    public S_Tile currentTile;
    bool moving;
    float moveTicker=0;
	void Start () {
        S_Camera.SetupScale(transform);
    }
	
	void Update () {
        int dx = 0;
        int dy = 0;
        if (Input.GetKeyDown("left"))   dx =-1;
        if (Input.GetKeyDown("right"))  dx = 1;
        if (Input.GetKeyDown("down"))   dy =-1;
        if (Input.GetKeyDown("up"))     dy = 1;
        if(dx!=0 || dy!=0) {
            S_Tile newTile = currentTile.GetTile(dx, dy);
            if(newTile != null) {
                MoveToTile(newTile, false);
                
            }
        }
        if (moving) {
            moveTicker += Time.deltaTime / MOVE_SPEED;
            if (moveTicker >= 1) {
                moveTicker = 1;
                moving = false;
            }
            
            transform.position = Vector3.Lerp(previousTile.transform.position, currentTile.transform.position, Interpolation.Pow2Out(0,1,moveTicker));

        }
    }

    public void MoveToTile(S_Tile tile, bool instant) {
        previousTile = currentTile;
        SetTile(tile);
        tile.Enter();
        if (instant) {
            transform.position = currentTile.transform.position;
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
}

