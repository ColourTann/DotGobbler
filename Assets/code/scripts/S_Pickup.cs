using UnityEngine;
using System.Collections;

public class S_Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Pickup() {
        Game.Get().level.Pickup(this);
    }
}
