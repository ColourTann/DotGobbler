using UnityEngine;
using System.Collections;
using System;

public class S_Button : MonoBehaviour {

      public static GameObject CreateButton(Sprite sprite, Action onClick) {
        
        GameObject result = new GameObject();
        SpriteRenderer sr = result.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "UI";
        sr.sprite = sprite;
        S_Button buttonScript = result.AddComponent<S_Button>();
        buttonScript.action = onClick;
        BoxCollider2D collider = result.AddComponent<BoxCollider2D>();
        return result;
    }

    Action action;

    void OnMouseDown() {
        action.Invoke();
        Debug.Log("iudgfiudhgui");
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
