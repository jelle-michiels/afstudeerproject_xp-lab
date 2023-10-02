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

    public Button tryAgainBtn, homeButton;

    public bool gameFinished = false;
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
        if (this.gameFinished == false) {
            (result ? youWinText : gameOverText).enabled = true;
            countdownText.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(result ? homeButton.gameObject : tryAgainBtn.gameObject);
            playerControl.enabled = false;


            if (result)
            {
                CalculateScore();
            }
        }
            
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
    }

}
