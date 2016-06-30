using UnityEngine;
using System.Collections;

public class S_Tile : MonoBehaviour {
    int x, y;
    SpriteRenderer spr_renderer;
    static int width, height;
    public void SetPosition(int x, int y) {
        spr_renderer = GetComponent<SpriteRenderer>();
        spr_renderer.sprite = S_Statics.get().tiles[(x+y)%2];
        if (width == 0) {

            width = (int)spr_renderer.bounds.size.x;
            height = (int)spr_renderer.bounds.size.y;
        }
        this.x = x;
        this.y = y;
        print(S_Statics.get().tiles[(x + y) % 2]);
        transform.position = new Vector3(width * x, height * y, 0);
    }

    void Awake() {
        S_Camera.setupScale(transform);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
