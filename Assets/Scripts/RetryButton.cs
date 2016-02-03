using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RetryButton : MonoBehaviour {
    private Bounds bounds;

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
                SceneManager.LoadScene("Game");
            }
        }
    }

}
