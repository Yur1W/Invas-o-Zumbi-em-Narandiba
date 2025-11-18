using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{   [Header("References")]
    [SerializeField]
    public PlayerControllerIso playerController;
    [SerializeField]
    GameObject bossZombie;
    public UIController ui;
     [SerializeField]
    GameObject Blockage;
    [Header("Game Stats")]
    [SerializeField]
    public static int lifes = 250;
    [SerializeField]
    public static int killCount = 0;
    public static int zombiesAlive = 0;
    public static bool isGameOver = false;

    



    void Start()
    {
        killCount = 0;
        lifes = 250;
        isGameOver = false;
        Blockage.SetActive(true);
        RestartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        ui.UpdateLives(lifes);
        ui.UpdateKillCount(killCount);
        ui.UpdateUpgradeSlider(killCount);
        if (bossZombie != null)
        {
            ui.UpdateBoddHP(bossZombie.GetComponent<BossZombie>().hp);
        }
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
    public void BossDefeated()
    {
        ui.ActivateBossDefeatedScreen();
        Debug.Log("Boss Defeated!");
    }
    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void KilledEnemys()
    {
       if (killCount >= 100)
        {
            Blockage.SetActive(false);
        }
    }
}
