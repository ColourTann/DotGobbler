using UnityEngine;
using System.Collections;
using System;

public class S_Tile : MonoBehaviour {
    int x, y;
    SpriteRenderer spr_renderer;
    public static int width, height;
    public static int b_width, b_height;
    S_Pickup content;
    public void SetPosition(int x, int y) {
        spr_renderer = GetComponent<SpriteRenderer>();
        spr_renderer.sprite = Sprites.tile;
        if (width == 0) {
            width = (int)spr_renderer.bounds.size.x + S_Camera.scale;
            height = (int)spr_renderer.bounds.size.y + S_Camera.scale;
            b_width = width + S_Camera.scale;
            b_height = height + S_Camera.scale;
        }
        this.x = x;
        this.y = y;
        transform.position = new Vector3(width * x + S_Camera.scale, height * y + S_Camera.scale, 0);
    }

    void Awake() {
        S_Camera.SetupScale(transform);
    }

    internal void AddPickup() {
        GameObject pickup = (GameObject)(Instantiate(Resources.Load("prefabs/pickup")));
        content = pickup.GetComponent<S_Pickup>();
        S_Camera.SetupScale(pickup.transform);
        pickup.transform.position = transform.position;
        pickup.transform.parent =transform;
    }

    public void Enter() {
        if (content != null) {
            content.Pickup();
            Destroy(content.gameObject);
            content = null;
        }
    }

    public S_Tile GetTile(int dx, int dy) {
        S_Tile[,] grid = Game.Get().level.grid;
        int newX = x + dx;
        int newY = y + dy;
        if(newX<0 || newX>=grid.GetLength(0) || newY<0 || newY >= grid.GetLength(1)) {
            return null;
        }
        return grid[newX, newY];
    }
}
