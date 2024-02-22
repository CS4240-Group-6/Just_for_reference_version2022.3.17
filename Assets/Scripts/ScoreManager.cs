using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score = 0; // Player's current score
    public TextMeshProUGUI scoreText; // Reference to UI Text element that displays the score and change from Text to TextMeshProUGUI

    // void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
        
    //     // Automatically find the TextMeshProUGUI component in child GameObjects
    //     scoreText = GetComponentInChildren<TextMeshProUGUI>();
    // }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScoreText(); // Update the score display
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
