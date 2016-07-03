﻿using UnityEngine;
using System.Collections.Generic;
using System;


public class S_Tile : MonoBehaviour {
    private S_Tile previous; //for pathfinding
    public int x, y;
    SpriteRenderer spr_renderer;
    public static int width, height;
    public static int b_width, b_height;
    S_Pickup content;
    public void SetPosition(int x, int y) {
       
        this.x = x;
        this.y = y;
        transform.position = new Vector3(width * x + S_Camera.scale, height * y + S_Camera.scale, 0);
    }

    

    void Awake() {
        S_Camera.SetupScale(transform);
        spr_renderer = GetComponent<SpriteRenderer>();
        spr_renderer.sprite = Sprites.tile;
        if (width == 0) {
            width = (int)spr_renderer.bounds.size.x + S_Camera.scale;
            height = (int)spr_renderer.bounds.size.y + S_Camera.scale;
            b_width = width + S_Camera.scale;
            b_height = height + S_Camera.scale;
        }
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

    public int TileDistance(S_Tile to) {
        return Mathf.Abs(to.x - x) + Mathf.Abs(to.y - y);
    }

    public List<S_Tile> GetTilesWithin(int dist, bool includeSelf) {
        List<S_Tile> result = new List<S_Tile>();
        for(int dx = -dist; dx <= dist; dx++) {
            for(int dy = -dist; dy <= dist; dy++) {
                if (dy == x && dx == 0 && !includeSelf) continue;
                S_Tile at = GetTile(dx, dy);
                if (at != null && at.TileDistance(this) <= dist) {
                    result.Add(at);
                }
            }
        }
        return result;
    }

    public S_Tile GetTile(int dx, int dy) {
       return Game.Get().level.GetTile(x+dx, y+dy);
    }

    public S_Tile PathTo(S_Tile target) {
        List<S_Tile> open = new List<S_Tile>();
        List<S_Tile> closed = new List<S_Tile>();
        open.Add(this);
        while (open.Count > 0) {
            S_Tile current = open[0];
            open.RemoveAt(0);
            closed.Add(current);
            if (current == target) {
                while (current.previous != this) {
                    current = current.previous;
                }
                return current;
            }
            foreach(S_Tile potential in current.GetTilesWithin(1, false)) {
                if (closed.Contains(potential)) continue;
                potential.previous = current;
                open.Add(potential);
            }
        }
        return null;
    }
}
