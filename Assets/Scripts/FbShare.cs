using UnityEngine;
using System.Collections;

public class FbShare : MonoBehaviour {

    private Bounds bounds;

    public string AppID = "1704225659789173";
    public string Link = "https://goo.gl/cHG75Z";
    // The picture's URL and the picture must be at least 200px by 200px.
    public string Picture = "http://i.imgur.com/P2NPdRq.png";
    // The name of your app or game.
    public string Name = "Bob Up";
    // The caption of your game or app.
    public string Caption = "I got 99 points in Bob Up! Can you beat my score?";
    // The description of your game or app.
    public string Description = "Climb as far as you can!";

    public void FacebookShare()
    {
        string caption = "I got " + PlayerPrefs.GetInt("Score") + " points in Bob Up! Can you beat my score?";

        Application.OpenURL("https://www.facebook.com/dialog/feed?" +
                "app_id=" + AppID +
                "&link=" + Link +
                "&picture=" + Picture +
                "&name=" + SpaceHere(caption) +
                "&caption=" + SpaceHere(caption) +
                "&description=" + SpaceHere(Description) +
                "&redirect_uri=https://facebook.com/");
    }
    string SpaceHere(string val)
    {
        return val.Replace(" ", "%20"); // %20 is only used for space
    }

    // Use this for initialization
    void Start()
    {
        bounds = GetComponent<BoxCollider2D>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;

            if (mouse.x > bounds.min.x && mouse.x < bounds.max.x && mouse.y > bounds.min.y && mouse.y < bounds.max.y)
            {
                FacebookShare();
            }
        }
    }

}
