using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
        if(Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene("Game");
            SoundManager.instance.SetMusicVolume(0.3f);
        }

	}
}
