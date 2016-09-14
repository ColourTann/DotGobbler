using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_Particle : MonoBehaviour {

    private static List<GameObject> pool = new List<GameObject>();
    private static GameObject GetPooledParticle() {
        /*if (pool.Count > 0) {
            GameObject pooled = pool[0];
            pool.Remove(pooled);
            return pooled;
        }*/
        GameObject go = Primitives.CreateRectangle(1, 1, Colours.GREEN);
        go.name = "particle";
        go.AddComponent<S_Particle>();
        return go;
    }

	public static GameObject CreateParticle(int x, int y, Color col) {
        GameObject go = GetPooledParticle();
        go.GetComponent<SpriteRenderer>().color = col;
        Util.SetLayer(go, Util.LayerName.Particles, 0);
        S_Particle particle = go.GetComponent<S_Particle>();
        particle.Setup(x, y);
        return go;
    }

    static float MAX_SPEED = 70*S_Camera.scale;
    float speed;
    static float MAX_TIME = .7f;

    void Setup(int x, int y) {
        transform.position = new Vector2(x, y);
        gameObject.SetActive(true);
        speed = Random.Range(MAX_SPEED/3, MAX_SPEED);
    
        dx = Random.Range(-speed, speed);
        dy = Mathf.Sqrt((speed * speed) - (dx * dx));
        if (Random.Range(0, 1f) > .5) {
            dy = -dy;
        }
        initialTime = Random.Range(0,MAX_TIME);
        timeLeft = initialTime;
        S_Camera.SetupScale(transform);
    }
    float initialTime;
    float timeLeft;
    float dx;
    float dy;

	void Update () {
        timeLeft -= Time.deltaTime;
        float factor = timeLeft / initialTime;
        transform.position = new Vector2(transform.position.x + dx * factor * Time.deltaTime, transform.position.y + dy * factor * Time.deltaTime);
        if(timeLeft <= 0) {
            GameObject.Destroy(gameObject);
            //pool.Add(gameObject);
        }           
    }
}
