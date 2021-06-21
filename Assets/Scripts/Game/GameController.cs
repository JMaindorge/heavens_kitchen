using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Customer AI Prefab")]
    public GameObject customerPrefab;
    [Header("Possible AI Spawns")]
    public Transform[] spawnLocations;
    private int currentCustomers;
    private Table[] tables;
    private float timeElapsed;
    private int gameStage = 1;
    private bool gameOver = true;
    private int numberOfTables;
    private LevelStart[] levelStarts;
    private TutorialStarter tutorialStarter;
    private int moneyEarned;
    private int badReviews = 0;
    private bool endlessMode;
    private float timeWaiting;
    private float timeRemaining;
    private LevelStart levelStartRef;

    void Start()
    {
        tables = GameObject.FindObjectsOfType<Table>();
        levelStarts = FindObjectsOfType<LevelStart>();
        tutorialStarter = FindObjectOfType<TutorialStarter>();
    }

    void Update()
    {
        if (!gameOver)
        {
            ProgressGame();
            UpdateCustomers();
        }
        else
        {
            ForceHome();
        }
    }

    public void SetGame(int timeRemaining, int timeWaiting, int gameStage, int numberOfTables, bool endlessMode, LevelStart levelStartRef)
    {
        // Set nessesary variables for the new level. 
        this.timeWaiting = timeWaiting;
        this.gameStage = gameStage;
        this.numberOfTables = numberOfTables;
        this.endlessMode = endlessMode;
        this.levelStartRef = levelStartRef;
        this.timeRemaining = timeRemaining;

        // Reset the money for the new level.
        moneyEarned = 0;

        // Set game over to false which means the if statement in the update function will now start running.
        gameOver = false;

        // Deactivate all of the level starters as the player will now be put into a new level.
        for (int i = 0; i < levelStarts.Length; i++)
        {
            levelStarts[i].gameObject.SetActive(false);
        }

        // Deactivate the tutorial starter as well.
        tutorialStarter.gameObject.SetActive(false);
    }

    private void ProgressGame()
    {
        // Keep track of how long segments of the level have been going on for.
        // This is for knowing when to change the game stage, and the amount of free tables.
        timeElapsed += Time.deltaTime;

        // If the player is not in endless mode, then the time needs to decrease. 
        if (!endlessMode)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                GameOver();
            }
        }

        // If the player has made three or more mistakes in the level, then end it now. 
        if (badReviews >= 3)
        {
            GameOver();
        }

        // If an interval of 60 seconds has passed, activate the next stage of the game.
        else if (timeElapsed >= 60)
        {
            timeElapsed = 0;
            gameStage++;
            numberOfTables++;
            numberOfTables = Mathf.Clamp(numberOfTables, 1, tables.Length);
        }
    }

    // This method is nessessary to force customers to go home when the level has ended. 
    private void ForceHome()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            tables[i].GoHome();
        }
    }

    // Keep track of current customers, and attempt to spawn more in if possible.
    private void UpdateCustomers()
    {
        currentCustomers = 0;
        for (int i = 0; i < numberOfTables; i++)
        {
            // If the table is not free. Add a tally to the customer counter.
            if (!tables[i].IsTableFree())
            {
                currentCustomers++;
            }

            // If the table is not queued by a customer. Spawn a new customer.
            if (!tables[i].IsTableQueued())
            {
                // Choose a random spawn location for the customer from a list of points.
                int randAmount = spawnLocations.Length;
                int randNum = Random.Range(0, randAmount);

                // Instantiate a new customer at the random index.
                Instantiate(customerPrefab, spawnLocations[randNum]);
            }
        }
    }

    // Take note of whether an order was correct. 
    public void CompleteOrder(bool orderCorrect)
    {
        if (orderCorrect)
        {
            moneyEarned += 50 + (25 * gameStage);
        }
        else
        {
            badReviews++;
            moneyEarned += 5;
        }
    }

    // End the level. Reset all of the variables for the level.
    // Avoid resetting the money here though as it would show up as zero on the UI.
    private void GameOver()
    {
        gameOver = true;
        badReviews = 0;
        timeElapsed = 0;
        for (int i = 0; i < levelStarts.Length; i++)
        {
            levelStarts[i].gameObject.SetActive(true);
        }
        levelStartRef.SetBest(moneyEarned);
        tutorialStarter.gameObject.SetActive(true);
    }

    /*
    Getters
    */
    public float GetWaitingTime()
    {
        return timeWaiting;
    }

    // Returns the time remaining in the level.
    public float GetTime()
    {
        return timeRemaining;
    }

    // Returns a bool stating if the level is over or not.
    public bool IsGameOver()
    {
        return gameOver;
    }

    // Returns the current amount of customers waiting at tables.
    public int GetCurrentCustomers()
    {
        return currentCustomers;
    }

    // Returns the amount of money the player has made in a given level.
    public int GetMoney()
    {
        return moneyEarned;
    }

    // Returns whether or not the game is endless.
    public bool IsEndless()
    {
        return endlessMode;
    }

    // Returns the current gamestage of the game.
    public int GetGameStage()
    {
        return gameStage;
    }

    public int GetBadReviews()
    {
        return badReviews;
    }

}
