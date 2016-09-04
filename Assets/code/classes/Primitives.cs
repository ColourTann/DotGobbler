using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Primitives  {

    public static GameObject CreateRectangle(int width, int height, Color tint) {
        GameObject result = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/rectangle")));
        result.transform.localScale = new Vector3(width, height);
        result.GetComponent<SpriteRenderer>().color = tint;
        result.name = "filled_rectangle";
        return result;
    }

    public static GameObject CreateRectangle(int width, int height, int border, Color tint) {
        GameObject result = new GameObject();

        GameObject go = CreateRectangle(width, border, tint);
        go.transform.SetParent(result.transform, false);
        go.transform.position = new Vector2(0, height<0?-border:0);

        go = CreateRectangle(width, border, tint);
        go.transform.SetParent(result.transform, false);
        go.transform.position = new Vector2(0, height-border+ (height < 0 ? border : 0));

        go = CreateRectangle(border, height, tint);
        go.transform.SetParent(result.transform, false);
        go.transform.position = new Vector2(width-border + (width<0?border:0), 0);

        go = CreateRectangle(border, height, tint);
        go.transform.SetParent(result.transform, false);
        go.transform.position = new Vector2(-(width < 0 ? border : 0), 0);
        result.name = "rectangle";
        return result;
    }

    public static GameObject CreateActor(Sprite sprite, int x=0, int y=0) {
        GameObject result = new GameObject();
        SpriteRenderer sr = result.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        result.transform.position = new Vector2(x, y);
        return result;
    }

	private static Canvas canvas;

	public static GameObject CreateText(string text, int x=0, int y = 0) {
		if (canvas == null) canvas = Object.FindObjectOfType<Canvas>();
		GameObject go = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/text")));
		go.GetComponent<Text>().text = text;
		go.GetComponent<Text>().fontSize= 16 * S_Camera.scale;
		go.GetComponent<Text>().color = Colours.LIGHT;
		go.transform.position = new Vector2(x, y);
		go.transform.SetParent(canvas.transform);

		return go;
	}
  
}
