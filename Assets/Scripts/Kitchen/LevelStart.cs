using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelStart : MonoBehaviour
{
    [Range(0, 3)]
    public int gameStageStart;
    [Range(1, 3)]
    public int numberOfTables;
    public int customerWaitingTime;
    public bool setEndless;
    public TextMeshPro scoreText;
    public TextMeshPro completionText;
    public int remainingTime;
    public int scoreTarget;
    private int bestScore;

    private void Start() {
        scoreText.text = "Best Earnings: £" + 0 + " / £" + scoreTarget;
    }

    // Given input from the interact script, start a new level from the public variables within this script.
    public void StartGame()
    {
        // Get a reference to the game controller script.
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // Check if the game controller exists.
        if (gameController)
        {
            // Initialise game from variables.
            gameController.SetGame(remainingTime, customerWaitingTime, gameStageStart, numberOfTables, setEndless, this);
        }
        else
        {
            Debug.LogError("GameController Not In Scene");
        }
    }

    // Attempt to set a new record for the level if possible. 
    // This method is accessed from the game controller after the level has finished.
    public void SetBest(int moneyEarned)
    {
        if (moneyEarned > bestScore)
        {
            bestScore = moneyEarned;
            scoreText.text = "Best Earnings: £" + bestScore + " / £" + scoreTarget;
        }
        if (bestScore >= scoreTarget)
        {
            completionText.text = "Complete!";
        }
        else
        {
            completionText.text = "Failed!";
        }
    }
}
