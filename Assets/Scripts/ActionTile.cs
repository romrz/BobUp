using UnityEngine;
using System.Collections;

public class ActionTile : MonoBehaviour {

    public Sprite actionSprite;
    public bool activateOnCollsion = true;
    public AudioClip sound;

    private bool activated = false;

    public void Activate() {
        SoundManager.instance.PlaySingle(sound);
        activated = true;
        GetComponent<SpriteRenderer>().sprite = actionSprite;
    }

    public bool HasBeenActivated() {
        return activated;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (activateOnCollsion && !activated && collision.gameObject.tag == "Player") {
            Activate();
        }
    }
}
