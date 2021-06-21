using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Table : MonoBehaviour
{
    private Customer customer;
    private bool tableFree = true;
    private bool tableQueued = false;
    public Transform waitingArea;
    public TextMeshPro waitingTimer;
    private FoodOrder[] requestedFood;
    private GameController gameController;
    private bool kill;
    private float tableTimer;

    public bool AddCustomer(Customer other)
    {
        // If the table is free, add the customer to the table to wait for updates.
        if (tableFree)
        {
            // Randomize whether or not the customer wants toasted buns.
            bool toastedBuns = Random.value >= 0.66;

            int extras = Random.Range(0, gameController.GetGameStage() + 1);

            requestedFood = new FoodOrder[extras + 3];

            // Generate bottom buns whether or not they are toasted.
            if (toastedBuns)
            {
                requestedFood[0] = new FoodOrder(FoodType.BottomBun, FoodState.Cooked);
            }
            else
            {
                requestedFood[0] = new FoodOrder(FoodType.BottomBun, FoodState.Raw);
            }

            // Generate Basic Patty.
            requestedFood[1] = new FoodOrder(FoodType.BurgerPatty, FoodState.Cooked);

            // Depending on gamestage, add more patty / other items to the burger.
            for (int i = 0; i <= extras; i++)
            {
                requestedFood[i + 2] = new FoodOrder(FoodType.BurgerPatty, FoodState.Cooked);
            }

            // Generate Top Buns whether or not they are toasted.
            if (toastedBuns)
            {
                requestedFood[requestedFood.Length - 1] = new FoodOrder(FoodType.TopBun, FoodState.Cooked);
            }
            else
            {
                requestedFood[requestedFood.Length - 1] = new FoodOrder(FoodType.TopBun, FoodState.Raw);
            }

            customer = other;
            tableFree = false;

            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {
        if (!gameController.IsGameOver())
        {
            if (!tableFree)
            {
                tableTimer = tableTimer - Time.deltaTime;
                waitingTimer.text = ((int)tableTimer).ToString();
                if (tableTimer <= 0)
                {
                    gameController.CompleteOrder(false);
                    GoHome();
                }
            }
            else
            {
                tableTimer = gameController.GetWaitingTime();
                waitingTimer.text = "";
            }
        }
        else
        {
            waitingTimer.text = "";
        }
    }

    public FoodOrder[] GetRequestedFood()
    {
        return requestedFood;
    }

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the table is not free, this would mean that there is a customer at the table.
        if (!tableFree)
        {
            FoodPhysical droppedFood = other.GetComponent<FoodPhysical>();
            bool orderCorrect = true;
            bool finished = false;
            if (droppedFood)
            {
                int i = 0;
                while (orderCorrect && !finished)
                {
                    if (droppedFood.GetFoodType() == FoodType.BurgerPatty && droppedFood.GetFoodState() == FoodState.Raw)
                    {
                        kill = true;
                    }
                    if (requestedFood[i].GetFoodState() == droppedFood.GetFoodState() && requestedFood[i].GetFoodType() == droppedFood.GetFoodType())
                    {
                        if ((i == requestedFood.Length - 1 && droppedFood.HasChild()) || (i != requestedFood.Length - 1 && !droppedFood.HasChild()))
                        {
                            orderCorrect = false;
                        }
                        else
                        {
                            if (droppedFood.HasChild())
                            {
                                i++;
                                droppedFood = droppedFood.GetChild();
                            }
                            else
                            {
                                finished = true;
                            }
                        }
                    }
                    else
                    {
                        orderCorrect = false;
                    }
                }

                // Reset the table, and tell the customer to go home.
                tableFree = true;
                tableQueued = false;
                customer.TakeFood(other.GetComponent<Transform>());

                if (!kill)
                {
                    // Inform the game controller of the order status.
                    gameController.CompleteOrder(orderCorrect);
                }
                else
                {
                    customer.Die();
                }

                kill = false;
            }
        }
    }

    public bool IsTableFree()
    {
        return tableFree;
    }

    public bool IsTableQueued()
    {
        return tableQueued;
    }

    public void GoHome()
    {
        if (!tableFree)
        {
            tableFree = true;
            tableQueued = false;
            customer.ReturnHome();
        }
    }

    public bool QueueTable()
    {
        // Check if the table is free, and the table is not currently queued by another agent.
        if (tableFree && !tableQueued)
        {
            tableQueued = true;
            return true;
        }
        return false;
    }
}
