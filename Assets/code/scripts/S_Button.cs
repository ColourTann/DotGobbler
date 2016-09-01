using UnityEngine;
using System.Collections;
using System;

public class S_Button : MonoBehaviour {

      public static S_Button CreateButton(Sprite sprite) {
        
        GameObject result = new GameObject();
        SpriteRenderer sr = result.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "UI";
        sr.sprite = sprite;
        S_Button buttonScript = result.AddComponent<S_Button>();
        result.AddComponent<BoxCollider2D>();
        return buttonScript;
    }

    public void SetAction(Action onClick) {
        action = onClick;
    }

    Action action;

    void OnMouseDown() {
		if (Game.isLocked()) return;
		action.Invoke();
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Bounds GetBounds() {
        return GetComponent<SpriteRenderer>().bounds;
    }
}
