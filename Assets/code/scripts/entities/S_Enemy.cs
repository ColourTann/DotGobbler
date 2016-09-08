using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class S_Enemy : S_Entity {

	public GameObject eyes;

	public void PathTowardsPlayer() {
		S_Tile playerTile = Game.Get().level.player.currentTile;
		targetTile = currentTile.PathTo(playerTile);

		//if there's no path, pick a tile that's vaguely in the right direction
		if (targetTile == null) {
			List<S_Tile> potentials = currentTile.GetTilesWithin(1, false);
			int bestWeirdDist = 999;
			foreach (S_Tile t in potentials) {
				if (t.IsBlocked()) continue;
				int xDist = t.x - playerTile.x;
				int yDist = t.y - playerTile.y;
				int calcDist = xDist * xDist + yDist * yDist;
				if (calcDist < bestWeirdDist) {
					bestWeirdDist = calcDist;
					targetTile = t;
				}
			}
		}

		if (targetTile != null) {
			int dx = targetTile.x - currentTile.x;
			int dy = targetTile.y - currentTile.y;
			int rotation = 0;
			if (dx == -1) rotation = 180;
			else if (dy == 1) rotation = 90;
			else if (dy == -1) rotation = 270;
			eyes.transform.rotation = Quaternion.AngleAxis(rotation + 180, Vector3.forward);
			targetTile.Block();
		}
	}

	int stun;
	internal void Stun(int i) {
		stun += i;
	}

	internal void CancelMove() {
		if (targetTile != null) targetTile.UnBlock();
	}

	protected override void Update() {
		eyes.GetComponent<SpriteRenderer>().enabled = !moving && targetTile != null;
		base.Update();
	}

	public override void Init() {
		base.Setup("pincer");
		eyes = Primitives.CreateActor(Sprites.eye);
		Util.SetLayer(eyes, Util.LayerName.Entities, 5);
		eyes.transform.SetParent(transform, false);
	}

	override public void TakeTurn() {
		if (stun > 0) {
			stun--;
			return;
		}
		if (targetTile != null) {
			targetTile.UnBlock();
			if (targetTile.occupier != null && targetTile.occupier != this && targetTile.occupier.Blocks()) {
				//can't move there!
			}
			else {
				if (targetTile.occupier is S_Player) {
					Game.Get().Lose();
				}

				MoveToTile(targetTile, false);
			}
		}
	}

	public override bool Blocks() {
		return true;
	}

	public bool CanSeePlayer() {
		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				if ((dx == 0) == (dy == 0)) continue;
				S_Tile tile = currentTile;
				while (tile != null) {
					if (tile.occupier is S_Player) return true;
					tile = tile.GetTile(dx, dy);
				}
			}
		}
		return false;
	}
}
