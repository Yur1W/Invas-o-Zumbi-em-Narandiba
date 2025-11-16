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
    PlayerControllerIso playerController;
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
