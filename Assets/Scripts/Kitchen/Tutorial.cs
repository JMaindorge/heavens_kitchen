using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private bool tutorialStarted;
    private TextMeshProUGUI text;
    private FoodSpawner[] spawners;
    private LevelStart[] levelStarters;

    [Header("Food References")]
    public FoodPhysical burgerTutorial;
    public FoodPhysical topTutorial;
    public FoodPhysical bottomTutorial;

    [Header("Food Prefabs")]
    public FoodPhysical burgerPatty;
    public FoodPhysical topBun;
    public FoodPhysical bottomBun;

    [Header("Spawn Areas")]
    public Transform pattySpawn;
    public Transform topSpawn;
    public Transform bottomSpawn;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        spawners = FindObjectsOfType<FoodSpawner>();
        levelStarters = FindObjectsOfType<LevelStart>();
    }

    public void SetTutorial(bool start)
    {
        tutorialStarted = start;
        text.gameObject.SetActive(start);
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].gameObject.SetActive(!start);
        }
        for (int i = 0; i < levelStarters.Length; i++)
        {
            levelStarters[i].gameObject.SetActive(!start);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialStarted)
        {
            if (burgerTutorial == null)
                burgerTutorial = Instantiate(burgerPatty, pattySpawn);
                
            if (topTutorial == null)
                topTutorial = Instantiate(topBun, topSpawn);

            if (bottomTutorial == null)
                bottomTutorial = Instantiate(bottomBun, bottomSpawn);

            if (burgerTutorial.GetFoodState() == FoodState.Raw)
            {
                text.text = "Pick up a burger patty, and chuck it on the stove, make sure not to burn it!";
            }
            else if (burgerTutorial.GetFoodState() == FoodState.Burnt)
            {
                text.text = "You've burnt that one, throw that in the bin and try again!";
            }
            else if (bottomTutorial.HasChild() &&
            bottomTutorial.GetChild().GetFoodType() == FoodType.BurgerPatty &&
            bottomTutorial.GetChild().GetFoodState() == FoodState.Cooked)
            {
                if (burgerTutorial.HasChild() && burgerTutorial.GetChild().GetFoodType() == FoodType.TopBun)
                {
                    SetTutorial(false);
                    text.text = "";
                }
                else
                {
                    text.text = "Great, now place the top bun on the patty to complete the burger!";
                }
            }
            else if (burgerTutorial.GetFoodState() == FoodState.Cooked && !burgerTutorial.IsCooking())
            {
                text.text = "Now place that cooked burger on top of the buttom bun!";
            }
        }
    }


}
