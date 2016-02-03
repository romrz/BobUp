using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowScore : MonoBehaviour {

    public Text outlineText;
    public Text fillText;

    public Text outlineHighscoreText;
    public Text fillHighscoreText;

    // Use this for initialization
    void Start () {
        //GameObject scoreObject = GameObject.FindGameObjectWithTag("Score");
        //int score = scoreObject.GetComponent<Score>().getScore();
        int score = PlayerPrefs.GetInt("Score");
        outlineText.text = score + "";
        fillText.text = score + "";

        int highscore = PlayerPrefs.GetInt("Highscore") >= score ? PlayerPrefs.GetInt("Highscore") : score;
        PlayerPrefs.SetInt("Highscore", highscore);

        outlineHighscoreText.text = highscore + "";
        fillHighscoreText.text = highscore + "";

        //scoreObject.GetComponent<Score>().Reset();
       //Destroy(scoreObject.gameObject);
    }
}
