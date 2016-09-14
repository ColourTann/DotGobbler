using UnityEngine;
using System.Collections;

public class S_Pickup : MonoBehaviour {
	public void Pickup() {
        Level.Get(gameObject).Pickup(this);
        Vector2 ppos = Level.Get(gameObject).player.transform.position;
        for(int i = 0; i < 13; i++) {
            GameObject go = S_Particle.CreateParticle((int)ppos.x, (int)ppos.y, i<8?Colours.LIGHT:Colours.GREEN);
            go.transform.SetParent(Level.Get(gameObject).slider.transform);
        }
	}

	public void Magnetise(int index, int x, int y) {
		S_Slider slider = gameObject.AddComponent<S_Slider>();
		int dx = (int)(transform.position.x - transform.position.x);
		int dy = (int)(transform.position.y - transform.position.y);
		int dist = dx * dx + dy * dy;
		Game.Lock();
		slider.SlideTo(x, y, index/20f, Interpolation.InterpolationType.Pow2In, () => { Pickup(); Game.Unlock(); });
	}
}
