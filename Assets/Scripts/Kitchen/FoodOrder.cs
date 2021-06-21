using System.Collections;
using System.Collections.Generic;

public class FoodOrder
{
    private FoodType foodType;
    private FoodState foodState;

    // Comparison script for creating orders within a table.
    public FoodOrder(FoodType foodType, FoodState foodState)
    {
        this.foodState = foodState;
        this.foodType = foodType;
    }

    public FoodType GetFoodType()
    {
        return foodType;
    }

    public FoodState GetFoodState()
    {
        return foodState;
    }
}
