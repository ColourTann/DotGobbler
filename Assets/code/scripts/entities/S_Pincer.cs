using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Pincer : S_Enemy {
	override public void ChooseMove() {
		PathTowardsPlayer();
	}
}
