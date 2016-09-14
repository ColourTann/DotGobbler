using UnityEngine;
using System.Collections;
using System;

public class Util  {

	public enum ZLayer { Gameplay, Buttons, Blocker, PauseLayer, Sliders}

    public enum LayerName {UI, Tiles, Entities, Particles}

    public static void SetLayer(GameObject go, LayerName layer, int position) {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = layer.ToString();
        sr.sortingOrder = position;

	}

    public static void SetLayerContainer(GameObject go, LayerName layer, int position) {
        foreach(SpriteRenderer sr in go.GetComponentsInChildren<SpriteRenderer>()) {
            sr.sortingLayerName = layer.ToString();
            sr.sortingOrder = position;
        }
    }

    public static void SetColour(GameObject go, Color col) {
        go.GetComponent<SpriteRenderer>().color = col;
    }

    public static int ProperSign(int num) {
        return num == 0 ? 0 : num > 0 ? 1 : -1;
    }

	internal static void SetZ(GameObject go, ZLayer layer ) {
		go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 50-(int)layer);
	}
}

