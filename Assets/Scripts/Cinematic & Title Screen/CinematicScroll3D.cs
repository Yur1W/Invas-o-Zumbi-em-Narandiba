using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicScroll3D : MonoBehaviour
{
    public float scrollSpeed = 1f;        // units per second
    public float startY = -5f;           // starting local Y
    public float endY = 20f;            // end local Y

    void Start()
    {
        Vector3 pos = transform.localPosition;
        pos.y = startY;
        transform.localPosition = pos;
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.y += scrollSpeed * Time.deltaTime;
        transform.localPosition = pos;

        if (pos.y > endY)
        {
            // End of crawl (load next scene, fade out, etc.)
        }
    }
}
