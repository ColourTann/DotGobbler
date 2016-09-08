using UnityEngine;
using System.Collections;

public class S_Pickup : MonoBehaviour {
	public void Pickup() {
		Game.Get().level.Pickup(this);
	}

	public void Magnetise(int index, int x, int y) {
		S_Slider slider = gameObject.AddComponent<S_Slider>();
		int dx = (int)(transform.position.x - transform.position.x);
		int dy = (int)(transform.position.y - transform.position.y);
		int dist = dx * dx + dy * dy;
		Game.Lock();
		slider.SlideTo(x, y, index/10f, Interpolation.InterpolationType.Pow2In, () => { Pickup(); Game.Unlock(); });
	}
}
