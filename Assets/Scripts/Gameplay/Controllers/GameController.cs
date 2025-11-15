using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{   [Header("References")]
    [SerializeField]
    public PlayerControllerIso playerController;
    [SerializeField]
    public UIController ui;
     [SerializeField]
    GameObject SafeZone;
    [Header("Game Stats")]
    [SerializeField]
    public static int lifes = 100;
    [SerializeField]
    public static int killCount = 0;
   


    void Start()
    {
        killCount = 0;
        lifes = 100;
        SafeZone.SetActive(false);
        RestartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        ui.UpdateLives(lifes);
        ui.UpdateKillCount(killCount);
        ui.UpdateUpgradeSlider(killCount);
        KilledEnemys();
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
    public void KilledEnemys()
    {
       if (killCount >= 150)
        {
            SafeZone.SetActive(true);
            SafeZone.GetComponent<BoxCollider2D>().enabled =  false;
        }
    }
}
