using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersList : MonoBehaviour
{
    [Header("Raw Prefabs")]
    public GameObject topBun;
    public GameObject bottomBun;

    [Header("Cooked Prefabs")]
    public GameObject toastedTopBun;
    public GameObject toastedBottomBun;
    public GameObject burgerPatty;

    public void AddRequest(FoodOrder[] food)
    {
        if (food.Length > 1)
        {
            gameObject.SetActive(true);
            int i = 0;
            
            while (i < food.Length)
            {
                // Top Bun
                if (food[i].GetFoodType() == FoodType.TopBun)
                {
                    // If the food type is a top bun, and it's raw, instantiate a raw top bun.
                    if (food[i].GetFoodState() == FoodState.Raw)
                    {
                        Instantiate(topBun, transform);
                    }
                    // Otherwise, instantiate a cooked top bun.
                    else
                    {
                        Instantiate(toastedTopBun, transform);
                    }
                }
                // Cooked Patty. Customer will never ask for a raw patty, so no need to check for that.
                else if (food[i].GetFoodType() == FoodType.BurgerPatty)
                {
                    Instantiate(burgerPatty, transform);
                }
                // Bottom bun.
                else if (food[i].GetFoodType() == FoodType.BottomBun)
                {
                    if (food[i].GetFoodState() == FoodState.Raw)
                    {
                        Instantiate(bottomBun, transform);
                    }
                    else
                    {
                        Instantiate(toastedBottomBun, transform);
                    }
                }
                i++;
            }
        }
    }

    public void ClearList()
    {
        // Destroy all prefab children when the player isn't looking at the customer's order.
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        gameObject.SetActive(false);
    }
}
