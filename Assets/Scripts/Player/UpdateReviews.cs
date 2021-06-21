using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateReviews : MonoBehaviour
{
    private GameController gameController;
    private TextMeshProUGUI text;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player's review score in the round.
        if (!gameController.IsGameOver())
        {
            text.text = "Bad Reviews: " + gameController.GetBadReviews() + " / 3"; 
        }
        else 
        {
            text.text = "";
        }
    }
}
