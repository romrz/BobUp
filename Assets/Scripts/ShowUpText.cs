using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowUpText : MonoBehaviour {

    public Text textObject;
    public float time;

    private float alpha = 0;

    public void ShowText(string text)
    {
        StopFadeIn();
        StopFadeOut();

        textObject.text = text;
        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, 0);
        InvokeRepeating("FadeIn", 0, 0.05f);
        InvokeRepeating("FadeOut", 1, 0.05f);
        Invoke("StopFadeIn", 1);
        Invoke("StopFadeOut", 2);
    }

    void FadeIn()
    {
        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, textObject.color.a + 0.05f);
    }

    void FadeOut()
    {
        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, textObject.color.a - 0.05f);
    }

    void StopFadeIn()
    {
        CancelInvoke("FadeIn");
        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, 1);
    }

    void StopFadeOut()
    {
        CancelInvoke("FadeOut");
        textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, 0);
    }
}
