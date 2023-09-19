using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    private float maxTimeAllowed;
    private float minTimeRequired;
    private float timeTaken;
    private float scorePercent;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button tryAgainBtn;

    public bool gameFinished = false;
    private bool scoreSaved = false;

    void Start()
    {
        gameOverText.enabled = false;
        tryAgainBtn.gameObject.SetActive(false);
        gameFinished = false;
    }

    void Update()
    {
        if (!gameFinished && maxTimeAllowed > 0)
        {
            timeTaken += Time.deltaTime; // update the time taken while the game is running

            if (timeTaken >= maxTimeAllowed)
            {
                gameFinished = true;
                timeTaken = maxTimeAllowed; // limit the time taken to the max time allowed
            }
        }

        countdownText.text = "Time: " + timeTaken.ToString("F2"); // update the countdown text
        if (timeTaken >= minTimeRequired)
        {
            CalculateScore(); // calculate the score
        }
        else if (timeTaken < minTimeRequired)
        {
            scoreText.text = "Score: 100%"; // update the score text
        }

        if (gameFinished && !scoreSaved)
        {
            EndGame();
        }

    }

    private void CalculateScore()
    {
        // Calculate the score as a percentage using the formula
        scorePercent = ((maxTimeAllowed - timeTaken) / (maxTimeAllowed - minTimeRequired)) * 100f;
        scoreText.text = "Score: " + scorePercent.ToString("F2") + "%"; // update the score text
    }

    private void EndGame()
    {
        float score = 100f;
        gameOverText.enabled = true;
        tryAgainBtn.gameObject.SetActive(true);
        Debug.Log("Game Over. Score: " + scorePercent + "%");
        if (timeTaken >= minTimeRequired)
        {
            score = scorePercent;
        }
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
