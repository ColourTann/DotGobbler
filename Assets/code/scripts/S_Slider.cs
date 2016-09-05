using UnityEngine;
using System.Collections;
using System;
public class S_Slider : MonoBehaviour {
    Vector3 startPosition;
    Vector3 targetPosition;
    float time;
    float counter;
    bool sliding;
    Action function;
	Interpolation.InterpolationType type;
    public void SlideTo(int x, int y, float speed, Action function = null, Interpolation.InterpolationType terpType = Interpolation.InterpolationType.Pow2Out) {
        this.function = function;
        counter = 0;
        time = speed;
        startPosition = transform.position;
        targetPosition = new Vector2(x, y);
        sliding = true;
		this.type = terpType;
    }

	void Update () {
        if (!sliding) return;
        counter += Time.deltaTime / time;
        if (counter >= 1) {
            counter = 1;
            sliding = false;
            if (function != null) {
                function();
            }
        }
        transform.position = Vector3.Lerp(startPosition, targetPosition, Interpolation.Terp(type, 0, 1, counter));
    }
}
