using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    public GameObject explosion;

	public void explode() {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
    