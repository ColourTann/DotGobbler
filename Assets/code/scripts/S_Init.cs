using UnityEngine;
using System.Collections;

public class S_Init : MonoBehaviour {

    void Start() {
        PlayerPrefs.DeleteAll();
        Game.Get().Init();
    }
}
