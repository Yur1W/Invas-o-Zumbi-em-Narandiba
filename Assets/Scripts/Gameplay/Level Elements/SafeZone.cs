using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{   
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           gameController.LevelComplete();
        }
    }
}

