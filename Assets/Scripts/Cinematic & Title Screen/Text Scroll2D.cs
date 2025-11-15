using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextScroll2D : MonoBehaviour
{
   
    public float scrollSpeed = 40f;  
    public float startY = -600f;     
    public float endY = 800f;         

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        // Set initial position
        Vector2 pos = rect.anchoredPosition;
        pos.y = startY;
        rect.anchoredPosition = pos;
    }

    void Update()
    {
        Vector2 pos = rect.anchoredPosition;
        pos.y += scrollSpeed * Time.deltaTime;
        rect.anchoredPosition = pos;
        if (pos.y > endY)
        {
            StartCoroutine(LoadNextSceneAfterDelay(1f));
        }
    }
    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
