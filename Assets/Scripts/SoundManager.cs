using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource musicSource;
    public AudioSource[] efxSources;
    public static SoundManager instance = null;

	void Awake () {
	    if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle(AudioClip clip) {
        if (clip == null) return;

        for(int i = 0; i < efxSources.Length; i++) {
            if(!efxSources[i].isPlaying) {
                efxSources[i].clip = clip;
                efxSources[i].Play();
                break;
            }
        }
    }

    public void SetMusicVolume(float volume) {
        musicSource.volume = volume;
    }

    public void StopMusic() {
        musicSource.Stop();
    }

    public void PlayMusic() {
        musicSource.Play();
    }

}
