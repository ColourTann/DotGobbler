using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Charger : S_Enemy {
	int charging;
	int chargeDX, chargeDY;
	override public void ChooseMove() {
		if (CanSeePlayer()) {
			S_Tile playerTile = Level.Get(gameObject).player.currentTile;
			int newCDX = Util.ProperSign(playerTile.x - currentTile.x);
			int newCDY = Util.ProperSign(playerTile.y - currentTile.y);
			if (playerTile == currentTile) return;
			if (newCDX != chargeDX || newCDY != chargeDY) charging = 0;
			chargeDX = newCDX;
			chargeDY = newCDY;
			if (targetTile!=null) targetTile.UnBlock();
			targetTile = null;
			eyes.SetActive(false);
			charging++;
			if (charging == 3) {
				Fire();
				return;
			}
			else {
				Sounds.PlaySound(Sounds.charge, 1, charging==1?1:1.8f);
			}
		}
		else {
			chargeDX = 0;
			chargeDY = 0;
			charging = 0;
			PathTowardsPlayer();
		}
		UpdateTargeting();
	}

	void Fire() {
		Sounds.PlaySound(Sounds.shoot);
		S_Slider slider = gameObject.AddComponent<S_Slider>();
		Game.Lock();
		ClearTargets();
		slider.SlideTo((int)Level.Get(gameObject).player.transform.position.x, (int)Level.Get(gameObject).player.transform.position.y, .25f, Interpolation.InterpolationType.Pow2In, ()=> { Game.Get().Lose(); Game.Unlock(); });
	}

	List<GameObject> targets = new List<GameObject>();

	void UpdateTargeting() {
		if (Game.Get().state != Game.GameState.Normal) return;
		ClearTargets();
		if (charging > 0) {
			S_Tile playerTile = Level.Get(gameObject).player.currentTile;
			int dx = Util.ProperSign(playerTile.x - currentTile.x);
			int dy = Util.ProperSign(playerTile.y - currentTile.y);
			foreach (S_Tile tile in currentTile.GetTilesInLine(dx, dy)) {
				GameObject go = Primitives.CreateActor(dx == 0 ? Sprites.charge_v : Sprites.charge_h);
				Util.SetLayer(go, Util.LayerName.Tiles, 20);
				go.transform.SetParent(tile.transform, false);
				go.GetComponent<SpriteRenderer>().color = charging == 1 ? Colours.GREEN : Colours.LIGHT;
				targets.Add(go);
			}
		}
	}

	void ClearTargets() {
		foreach (GameObject go in targets) {
			Destroy(go);
		}
		targets.Clear();
	}

	override public void TakeTurn() {
		if (CanSeePlayer()) {
			FinishedMoving();
			return;
		}
		eyes.SetActive(true);
		if (targetTile != null) {
			targetTile.UnBlock();
			if (targetTile.occupier != null && targetTile.occupier != this && targetTile.occupier.Blocks()) {
				//can't move there!
			}
			else {
				if (targetTile.occupier is S_Player) {
					if(Game.IsCurrent(gameObject)) Game.Get().Lose();
				}
				MoveToTile(targetTile, false);
			}
		}
	}

}
