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
    GameObject bossDefeatedScreen;
    [SerializeField]
    GameObject tutorielScreen;
    [SerializeField]
    public GameObject HUD;
    [SerializeField]
    GameObject KillCountText;
    [SerializeField]
    GameObject UpgradeSlider;
    [SerializeField]
    GameObject LivesSlider;
    [SerializeField]
    GameObject BossHPSlider;
    [SerializeField]
    TitleScreen titleScreen;
    [SerializeField]
    GameObject Warning;
    void Start()
    {
        gameOverScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        tutorielScreen.SetActive(true);
        HUD.SetActive(true);
        StartCoroutine(CloseTutorielScreen());
    }
    public void ActivateGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        GameController.isGameOver = true;
        Debug.Log("Game Over ");
    }
    public void ActivateLevelCompleteScreen()
    {
        levelCompleteScreen.SetActive(true);
        Debug.Log("Level Complete ");
        StartCoroutine(NextLevelDelay());
    }
    IEnumerator NextLevelDelay()
    {
        yield return new WaitForSeconds(3f);
        titleScreen.NextLevel();
    }
    public void ActivateBossDefeatedScreen()
    {
        bossDefeatedScreen.SetActive(true);
        Debug.Log("Boss Defeated ");
    }
    public void UpdateKillCount(int killCount)
    {
        KillCountText.GetComponent<TextMeshProUGUI>().text =  killCount.ToString();
    }
    public void UpdateUpgradeSlider(float value)
    {
        UpgradeSlider.GetComponent<Slider>().value = value;
    }
    public void UpdateLives(int lives)
    {
        LivesSlider.GetComponent<Slider>().value = lives;
    }
    public void UpdateBoddHP(float value)
    {
        BossHPSlider.GetComponent<Slider>().value = value;
    }
    public void ActivateWarning()
    {
        Warning.SetActive(true);
        StartCoroutine(DeactivateWarning());
    }
    IEnumerator DeactivateWarning()
    {
        yield return new WaitForSeconds(3f);
        Warning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CloseTutorielScreen()
    {
        yield return new WaitForSeconds(10f);
        tutorielScreen.SetActive(false);
    }
}
