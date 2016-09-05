using UnityEngine;
using System.Collections;

public class Interpolation  {
	public enum InterpolationType { Pow2In, Pow2Out, Pow3In}
	public static float Pow2Out(float initial, float end, float value) {
		return PowNOut(initial, end, value, 2);
	}

	public static float PowNIn(float initial, float end, float value, float pow) {
		return (end-initial) * Mathf.Pow(value, pow) + initial;
	}

	public static float PowNOut(float initial, float end, float value, float power) {
        
        return (float)Mathf.Pow(value - 1, power) * (power % 2 == 0 ? -1 : 1) + 1;
    }

	public static float Terp(InterpolationType type, float initial, float end, float value) {
		switch (type) {
			case InterpolationType.Pow2In: return PowNIn(initial, end, value, 2);
			case InterpolationType.Pow3In: return PowNIn(initial, end, value, 3);
			case InterpolationType.Pow2Out: return Pow2Out(initial, end, value);
			default: Debug.Log("unimplemented interpolation type????");  return 0;
		}
	}
}
