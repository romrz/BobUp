using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    public GameObject player;
    public int tilesWide;
    private int score;
    private float tileSize;
    private float initialPosition;

    public Text outlineText;
    public Text fillText;

    // Use this for initialization
    void Start () {
        //player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = player.transform.position.y;
        outlineText.text = "0";
        fillText.text = "0";
        float cameraWidth = Camera.main.aspect * Camera.main.orthographicSize * 2.0f;
        tileSize = cameraWidth / tilesWide;

        //DontDestroyOnLoad(gameObject);
        PlayerPrefs.SetInt("Score", 0);
        score = 0;
    }
	
	// Update is called once per frame
	void Update () {
        int newScore = (int) ((player.transform.position.y - initialPosition) / (tileSize * 3));
	    if(newScore > score)
        {
            score = newScore;
            PlayerPrefs.SetInt("Score", score);
            outlineText.text = score + "";
            fillText.text = score + "";
        }
    }

    public void Reset()
    {
        score = 0;
    }

    public int getScore()
    {
        return score;
    }

}
