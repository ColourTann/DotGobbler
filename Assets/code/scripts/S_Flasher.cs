using UnityEngine;
using System.Collections;

public class S_Flasher : MonoBehaviour {

    public static GameObject CreateFlasher(int width, int height, Color col, float duration) {
        GameObject go = Primitives.CreateRectangle(width, height, col);
        S_Flasher flasher = go.AddComponent<S_Flasher>();
        flasher.Init(width, height, duration);
        return go;
    }
    int width, height;
    float duration;
    float elapsed;

    void Init(int width, int height, float duration) {
        this.duration = duration;
        this.width = width;
        this.height = height;
    }

	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;
        transform.localScale = new Vector2(width, Mathf.Max(0, height * (1-elapsed/duration)));
        if (elapsed >= duration) {
            GameObject.Destroy(gameObject);
        }    
	}
}
