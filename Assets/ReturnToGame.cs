using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ReturnToGame : MonoBehaviour
{
    private float lastDisplayTime;
    private float textSpeed = 1;
    private int textIndex = 0;
    public int numTextToDisplay = 0;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        lastDisplayTime = Time.time;

        // Set stats
        livesText.text = "Lives Left: " + PlayerStatistics.Instance.lives.ToString();
        countText.text = "Items Gathered: " + PlayerStatistics.Instance.count.ToString();
        scoreText.text = "Score: " + PlayerStatistics.Instance.score.ToString();
        
        // Hide all text initially
        for (int i = 0; i < numTextToDisplay; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    void Update()
    {
        // Display stats one by one
        if (Time.time - lastDisplayTime >= textSpeed && textIndex < numTextToDisplay)
        {
            lastDisplayTime = Time.time;
            transform.GetChild(textIndex).gameObject.SetActive(true);
            textIndex++;
        }
    }

    void OnKeyPress()
    {
        // If all text has been displayed, allow user to return to game
        if (textIndex == numTextToDisplay)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
