using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
