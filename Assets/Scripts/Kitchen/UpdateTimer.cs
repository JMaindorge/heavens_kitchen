using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTimer : MonoBehaviour
{
    private GameController gameController;
    private TextMeshProUGUI timerText;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsGameOver())
        {
            float timeLeft = gameController.GetTime();
            if (timeLeft <= 60)
            {
                timerText.text = (int) timeLeft + " Second(s) Remaining";
            }
            else
            {
                timerText.text = Mathf.Round(timeLeft / 60) + " Minute(s) Remaining";
            }
        }
        else
        {
            timerText.text = "";
        }
    }
}
