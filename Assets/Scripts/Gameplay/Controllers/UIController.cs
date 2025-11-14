using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverScreen;
    [SerializeField]
    GameObject levelCompleteScreen;
    [SerializeField]
    TextMeshPro KillCountText;
    [SerializeField]
    TextMeshPro LivesText;
    void Start()
    {
        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
    }
    public void ActivateGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Debug.Log("Activating Game Over Screen");
    }
    public void ActivateLevelCompleteScreen()
    {
        levelCompleteScreen.SetActive(true);
        Debug.Log("Activating Level Complete Screen");
    }
    public void UpdateKillCount(int killCount)
    {
        KillCountText.text = "Kills: " + killCount.ToString();
    }
    public void UpdateLives(int lives)
    {
        LivesText.text = "Lives: " + lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
