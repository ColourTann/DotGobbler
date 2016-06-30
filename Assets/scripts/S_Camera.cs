using UnityEngine;
using System.Collections;

public class S_Camera : MonoBehaviour {
    Camera myCam;
    static int scale = Screen.height / 100;
	// Use this for initialization
	void Start () {
        myCam = GetComponent<Camera>();
        myCam.orthographicSize = Screen.height / 2;
        myCam.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, -1);
    }

    private static S_Camera self;

    public static void setupScale(Transform obj) {
        obj.localScale = new Vector3(scale, scale, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
