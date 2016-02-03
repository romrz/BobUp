using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    public GameObject explosion;
    public AudioClip explosionSound;

	public void explode() {
        SoundManager.instance.PlaySingle(explosionSound);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
    