using UnityEngine;
using System.Collections;

public class TwitterShare : MonoBehaviour {

    private Bounds bounds;
    private string text;
    public string appStoreLink = "https://play.google.com/store/apps/details?id=me.romrz.BobUp"; 

	// Use this for initialization
	void Start () {
        bounds = GetComponent<BoxCollider2D>().bounds;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;

            if (mouse.x > bounds.min.x && mouse.x < bounds.max.x && mouse.y > bounds.min.y && mouse.y < bounds.max.y) { 
                Share();
            }
        }
	}

    void Share()
    {
        string twittershare = "http://twitter.com/home?status=I got " + PlayerPrefs.GetInt("Score") + " points in Bob Up! Can you beat my score? Download it here: " + appStoreLink;
        Application.OpenURL(twittershare);
    }
}
