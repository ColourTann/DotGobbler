using UnityEngine;
using System.Collections;

public class Primitives  {

    public static GameObject CreateRectangle(int width, int height, Color tint) {
        GameObject result = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/rectangle")));
        result.transform.localScale = new Vector3(width, height);
        result.GetComponent<SpriteRenderer>().color = tint;
        return result;
    }

    public static GameObject CreateActor(Sprite sprite, int x=0, int y=0) {
        GameObject result = new GameObject();
        SpriteRenderer sr = result.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        result.transform.position = new Vector2(x, y);
        return result;
    }
  
}
