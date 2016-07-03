using UnityEngine;
using System.Collections;

public class Primitives  {

    public static GameObject CreateRectangle(int width, int height, Color tint) {
        GameObject result = (GameObject)(GameObject.Instantiate(Resources.Load("prefabs/rectangle")));
        result.transform.localScale = new Vector3(width, height);
        result.GetComponent<SpriteRenderer>().color = tint;
        return result;
    }

}
