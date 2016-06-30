using UnityEngine;
using System.Collections;

public class Interpolation  {

    public static float Pow2Out(float initial, float end, float value) {
        return PowNOut(initial, end, value, 2);
    }

    public static float PowNOut(float initial, float end, float value, float power) {
        
        return (float)Mathf.Pow(value - 1, power) * (power % 2 == 0 ? -1 : 1) + 1;
    }
}
