﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class S_Tile : MonoBehaviour {
	private S_Tile previous; //for pathfinding
	public int x, y;
	SpriteRenderer spr_renderer;
	public static int BASE_TILE_SIZE = 15;
	public static int width = S_Camera.scale * (BASE_TILE_SIZE + 1), height = S_Camera.scale * (BASE_TILE_SIZE + 1);
	public static int width_including_full_border = width + S_Camera.scale, height_including_full_border = height + S_Camera.scale;
	public S_Pickup content;
	public S_Entity occupier;
	SpriteRenderer highlight_renderer;
	public void SetPosition(int x, int y) {
		this.x = x;
		this.y = y;
		transform.position = new Vector3(width * x + S_Camera.scale, height * y + S_Camera.scale, 0);
		GameObject highlight = Primitives.CreateActor(Sprites.tile_highlight, 0, 0);
		highlight.name = "highlight";
		highlight.transform.SetParent(transform, false);
		highlight_renderer = highlight.GetComponent<SpriteRenderer>();
		Util.SetLayer(highlight, Util.LayerName.Tiles, 5);
		highlight_renderer.color = Colours.LIGHT;
		highlight_renderer.enabled = false;
		gameObject.AddComponent<BoxCollider2D>();
		Util.SetZ(gameObject, Util.ZLayer.Gameplay);
	}

	public void SetHighlight(bool lit) {
		highlight_renderer.enabled = lit;
	}

	public bool IsBlocked() {
		return blocked || occupier != null;
	}

	void Awake() {
		S_Camera.SetupScale(transform);
		spr_renderer = GetComponent<SpriteRenderer>();
		spr_renderer.sprite = Sprites.tile;
	}

	internal void AddPickup() {
		GameObject pickup = (GameObject)(Instantiate(Resources.Load("prefabs/pickup")));
		content = pickup.GetComponent<S_Pickup>();
		pickup.name = "pickup";
		S_Camera.SetupScale(pickup.transform);
		pickup.transform.position = transform.position;
		pickup.transform.parent = transform;
	}

	public void Enter(S_Entity enterer) {
		if (enterer is S_Player && content != null) {
			content.Pickup();
		}
	}

	public int TileDistance(S_Tile to) {
		return Mathf.Abs(to.x - x) + Mathf.Abs(to.y - y);
	}

	public List<S_Tile> GetTilesWithin(int dist, bool includeSelf) {
		List<S_Tile> result = new List<S_Tile>();
		for (int dx = -dist; dx <= dist; dx++) {
			for (int dy = -dist; dy <= dist; dy++) {
				if (dy == 0 && dx == 0 && !includeSelf) continue;
				S_Tile at = GetTile(dx, dy);
				if (at != null && at.TileDistance(this) <= dist) {
					result.Add(at);
				}
			}
		}
		return result;
	}

	public List<S_Tile> GetTilesInLine(int dx, int dy, bool includeSelf = false) {
		List<S_Tile> result = new List<S_Tile>();
		if (includeSelf) result.Add(this);
		S_Tile tile = GetTile(dx, dy);
		while (tile != null) {
			result.Add(tile);
			tile = tile.GetTile(dx, dy);
		}
		return result;
	}

	public S_Tile GetTile(int dx, int dy) {
		return Level.Get(gameObject).GetTile(x + dx, y + dy);
	}


	public S_Tile PathTo(List<S_Tile> targets) {
		List<S_Tile> open = new List<S_Tile>();
		List<S_Tile> closed = new List<S_Tile>();
		open.Add(this);
		while (open.Count > 0) {
			S_Tile current = open[0];
			open.RemoveAt(0);
			closed.Add(current);
			foreach (S_Tile potential in current.GetTilesWithin(1, false)) {
				if (closed.Contains(potential)) continue;
				potential.previous = current;
				if (targets.Contains(potential)) {
					S_Tile result = potential;
					while (result.previous != this) {
						result = result.previous;
					}
					return result;
				}
				if (potential.IsBlocked()) continue;
				open.Add(potential);
			}
		}
		if (targets.Count == 1) {
			//if the path is blocked, also check nearby tiles
			return PathTo(targets[0].GetTilesWithin(1, false));
		}
		return null;
	}

	public S_Tile PathTo(S_Tile target) {
		if (target == this) return null;
		return PathTo(new List<S_Tile>() { target });
	}

	bool blocked;

	public void Block() {
		blocked = true;
	}

	public void UnBlock() {
		blocked = false;
	}

	public int GetDistance(S_Tile other) {
		return Math.Abs(other.x - x) + Mathf.Abs(other.y - y);
	}
}
