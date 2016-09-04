using UnityEngine;
using System.Collections;

public class S_Follower : MonoBehaviour {
	//annoying thing to make text follow a game object
	Transform transformToFollow;
	int xOffset, yOffset;
	public void Follow(GameObject obj, int xOffset=0, int yOffset=0) {
		transformToFollow = obj.transform;
		this.xOffset = xOffset;
		this.yOffset = yOffset;
	}
	
	void Update () {
		Vector3 mine = transform.position;
		Vector3 theirs = transformToFollow.position;
		if (mine.x +xOffset== theirs.x && mine.y+yOffset == theirs.y) return;
		if(xOffset==0 && yOffset == 0) {
		transform.position = transformToFollow.position;
		}
		else {
			transform.position = new Vector2(transformToFollow.position.x + xOffset, transformToFollow.position.y + yOffset);
		}
	}
}
