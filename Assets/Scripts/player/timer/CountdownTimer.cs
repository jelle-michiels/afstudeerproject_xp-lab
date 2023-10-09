using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System;
using UnityEngine.EventSystems;

public class CountdownTimer : MonoBehaviour
{
    private float maxTimeAllowed;
    private float minTimeRequired;
    private float timeTaken;
    private float score;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;

    public TextMeshProUGUI youWinText;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI checkpointText;

    public GameObject checkpointFlash;

    public GameObject dmgScreen;

    public GameObject UI;

    public Button tryAgainBtn, homeButton;

    public static bool gameFinished = false;
    private bool scoreSaved = false;
    private bool timeStarted = false;

    public PlayerControl playerControl;

    void Start()
    {
        scoreText.gameObject.SetActive(false);
        gameOverText.enabled = false;
        youWinText.enabled = false;
        tryAgainBtn.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        gameFinished = false;
        timeTaken = int.Parse(SettingsMenu.maxTimeText);
        checkpointText.enabled = false;
        checkpointFlash.SetActive(false);
        dmgScreen.SetActive(false);
        UI.SetActive(false);
        //GameObject.Find("Player").GetComponent<HealthState>().Health = 3;
    }

    void Update()
    {
        if (!gameFinished )
        {
            if (timeStarted)
            {
                timeTaken -= Time.deltaTime; // update the time taken while the game is running
            }
            /*timeTaken -= Time.deltaTime;*/ // update the time taken while the game is running

            if (timeTaken <= int.Parse(SettingsMenu.minTimeText))
            {
                /*CalculateScore();*/
                EndGame(false);
            }
            countdownText.text = "Time: " + timeTaken.ToString("F2"); // update the countdown text
        }

    }

    private void CalculateScore()
    {
        // Calculate the score as a percentage using the formula
        score = (int.Parse(SettingsMenu.maxTimeText) + timeTaken) / 5;
        if (score < 0)
        {
            score = 0;
        }
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score: " + score.ToString("F2"); // update the score text
    }

    public void EndGame(Boolean result)
    {
        GameResult(result);
        gameFinished = true; // Check gameResult() before touching this
        tryAgainBtn.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        Debug.Log("Game Over. Score: " + score);
        GameObject.Find("LoadCanvas").GetComponent<EditorDatabase>().SetScore(score);
        scoreSaved = true;
        timeStarted = false;
    }

    // If the game is finished the win - lose screen won't change to the other
    public void GameResult(Boolean result){
        Debug.Log("Game Result: " + result);
        if (gameFinished == false) {
            (result ? youWinText : gameOverText).enabled = true;
            countdownText.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(result ? homeButton.gameObject : tryAgainBtn.gameObject);
            playerControl.enabled = false;
            playerControl.rb.velocity = Vector3.zero;

            if (result)
            {
                CalculateScore();
            }
        }
            
    }

    public void checkpointReached()
    {
        checkpointText.enabled = true;
        checkpointFlash.SetActive(true);
        StartCoroutine(checkpointDelay());
    }

    private IEnumerator checkpointDelay()
    {
        yield return new WaitForSeconds(0.5f);
        checkpointFlash.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        checkpointText.enabled = false;
    }

        private IEnumerator damageDelay()
    {
        yield return new WaitForSeconds(0.5f);
        dmgScreen.SetActive(false);
    }

    public void damageTaken(){
        dmgScreen.SetActive(true);
        StartCoroutine(damageDelay());
    }

    public void died(){
        EndGame(false);
    }



    /*public void GameOver()
    {
        gameFinished = true;
        scoreSaved = true;
        gameOverText.enabled = true;
        tryAgainBtn.gameObject.SetActive(true);
        scoreText.text = "Score: 0%";
        GameObject.Find("LoadCanvas").GetComponent<EditorDatabase>().SetScore(0f);
    }*/

    public void resetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset the game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Load Main Menu");
    }

    public void setTimers(int maxTime, int minTime)
    {
        maxTimeAllowed = maxTime;
        minTimeRequired = minTime;
    }

    public void startTimer()
    {
        timeStarted = true;
        UI.SetActive(true);
    }

}
