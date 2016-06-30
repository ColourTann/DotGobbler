using UnityEngine;
using System.Collections;

public class S_Statics : MonoBehaviour {
    public Sprite[] tiles;
	// Use this for initialization

    private static S_Statics self;
    public static S_Statics get() {
        if (self == null) {
            self = FindObjectOfType<S_Statics>();
        }
        return self;
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
