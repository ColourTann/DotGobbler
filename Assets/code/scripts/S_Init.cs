using UnityEngine;
using System.Collections;

public class S_Init : MonoBehaviour {

    void Start() {
        Game.Get().Init();
    }

	void Update() {
		Music.Update();
	}
}
