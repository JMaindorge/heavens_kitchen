using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyEarned : MonoBehaviour
{
    private GameController controller;
    private TextMeshProUGUI moneyText;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        moneyText = GetComponent<TextMeshProUGUI>();
        controller.GetCurrentCustomers();
    }

    void Update()
    {
        // Update the player's money from the round.
        if (!controller.IsGameOver())
        {
            moneyText.text = "Money Earned: £" + controller.GetMoney().ToString();
        }
        else
        {
            moneyText.text = "";
        }
    }
}
