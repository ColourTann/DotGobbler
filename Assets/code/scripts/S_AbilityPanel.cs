using UnityEngine;
using System.Collections;

public class S_AbilityPanel : MonoBehaviour{
    
    public static int WIDTH = 50 * S_Camera.scale;
    public static int HEIGHT = 150 * S_Camera.scale;
    
    public void Setup() {
        GameObject go =Primitives.CreateRectangle(WIDTH, HEIGHT, Colours.RED);
        go.transform.SetParent(transform, false);
    }

    
}
