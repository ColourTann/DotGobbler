using UnityEngine;
using System.Collections;

public class S_Init : MonoBehaviour { 
    int tilesAcross = 5;
    int tilesDown = 5;
    // Use this for initialization
    void Start () {
        PlayerPrefs.DeleteAll();
	    for(int x = 0; x < tilesAcross; x++) {
            for(int y = 0; y < tilesDown; y++) {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Tile"));
                obj.GetComponent<S_Tile>().SetPosition(x, y);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
