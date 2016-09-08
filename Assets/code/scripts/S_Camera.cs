using UnityEngine;
using System.Collections;

public class S_Camera : MonoBehaviour {
    Camera myCam;
    public static int scale = Screen.height / 200;

	void Start () {
		myCam = GetComponent<Camera>();
        myCam.orthographicSize = Screen.height / 2;
        myCam.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, -1);
    }
	private static S_Camera self;

    public static void SetupScale(Transform obj) {
        obj.localScale = new Vector3(scale, scale, 1);
    }
}
