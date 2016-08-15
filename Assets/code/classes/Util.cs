using UnityEngine;
using System.Collections;

public class Util  {
    public enum LayerName { UI}
	public static void SetLayer(GameObject go, LayerName layer, int position) {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = layer.ToString();
        sr.sortingOrder = position;
    }

    public static void SetColour(GameObject go, Color col) {
        go.GetComponent<SpriteRenderer>().color = col;
    }
}
