using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerControllerIso playerController;
    public UIController ui;
    public static int lifes = 5;
    public static int killCount = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ui.UpdateLives(lifes);
        ui.UpdateKillCount(killCount);
    }
    public void RestartLevel()
    {
        playerController.enabled = true;
        playerController.Respawn();
    }
    public void GameOver()
    {
        ui.ActivateGameOverScreen();
        Debug.Log("Game Over!");
    }

    public void LevelComplete()
    {
        ui.ActivateLevelCompleteScreen();
        Debug.Log("Level Complete!");
    }
    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
