using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCustomers : MonoBehaviour
{
    private GameController controller;
    private TextMeshProUGUI waitingText;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        waitingText = GetComponent<TextMeshProUGUI>();
        controller.GetCurrentCustomers();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current customer's in the restarant.
        if (!controller.IsGameOver())
        {
            waitingText.text = "Customers Waiting: " + controller.GetCurrentCustomers();
        }
        else
        {
            waitingText.text = "";
        }
    }
}
