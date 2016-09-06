using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Charger : S_Enemy {
	int charging;
	override public void ChooseMove() {
		if (CanSeePlayer()) {
			if(targetTile!=null) targetTile.UnBlock();
			targetTile = null;
			eyes.SetActive(false);
			charging++;
			Debug.Log("sounds");
			if (charging == 3) {
				Fire();
				return;
			}
			else {
				Sounds.PlaySound(Sounds.charge, 1, charging==1?1:1.8f);
			}
			
		}
		else {
			if(charging > 0) {
				charging = 0;
			}
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
		slider.SlideTo((int)Game.Get().level.player.transform.position.x, (int)Game.Get().level.player.transform.position.y, .25f, ()=> { Game.Get().Lose(); Game.Unlock(); } , Interpolation.InterpolationType.Pow2In);
	}

	List<GameObject> targets = new List<GameObject>();

	void UpdateTargeting() {
		ClearTargets();
		if (charging > 0) {
			S_Tile playerTile = Game.Get().level.player.currentTile;
			int dx = Util.ProperSign(playerTile.x - currentTile.x);
			int dy = Util.ProperSign(playerTile.y - currentTile.y);
			S_Tile tile = currentTile.GetTile(dx, dy);

			while(tile!=null && !(tile.occupier is S_Player)) {
				GameObject go = Primitives.CreateActor(dx == 0 ? Sprites.charge_v : Sprites.charge_h);
				Util.SetLayer(go, Util.LayerName.Tiles, 20);
				go.transform.SetParent(tile.transform, false);
				go.GetComponent<SpriteRenderer>().color = charging == 1 ? Colours.GREEN : Colours.LIGHT;
				tile = tile.GetTile(dx, dy);
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
					Game.Get().Lose();
				}

				MoveToTile(targetTile, false);
			}
		}
	}

}
