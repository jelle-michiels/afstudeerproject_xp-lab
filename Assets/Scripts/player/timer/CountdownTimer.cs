using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class CountdownTimer : MonoBehaviour
{
    private float maxTimeAllowed;
    private float minTimeRequired;
    private float timeTaken;
    private float score;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button tryAgainBtn;

    public bool gameFinished = false;
    private bool scoreSaved = false;

    void Start()
    {
        scoreText.gameObject.SetActive(false);
        gameOverText.enabled = false;
        tryAgainBtn.gameObject.SetActive(false);
        gameFinished = false;
    }

    void Update()
    {
        if (!gameFinished )
        {
            timeTaken += Time.deltaTime; // update the time taken while the game is running

            if (timeTaken >= int.Parse(OptionMenu.maxTimeText))
            {
                gameFinished = true;
                CalculateScore();
                EndGame();
            }
            countdownText.text = "Time: " + timeTaken.ToString("F2"); // update the countdown text
        }

    }

    private void CalculateScore()
    {
        // Calculate the score as a percentage using the formula
        score = (int.Parse(OptionMenu.maxTimeText) - timeTaken) * 5;
        if (score < 0)
        {
            score = 0;
        }
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score: " + score.ToString("F2"); // update the score text
    }

    private void EndGame()
    {
        gameOverText.enabled = true;
        tryAgainBtn.gameObject.SetActive(true);
        Debug.Log("Game Over. Score: " + score);
        GameObject.Find("LoadCanvas").GetComponent<EditorDatabase>().SetScore(score);
        scoreSaved = true;
    }

    public void GameOver()
    {
        gameFinished = true;
        scoreSaved = true;
        gameOverText.enabled = true;
        tryAgainBtn.gameObject.SetActive(true);
        scoreText.text = "Score: 0%";
        GameObject.Find("LoadCanvas").GetComponent<EditorDatabase>().SetScore(0f);
    }

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

}
