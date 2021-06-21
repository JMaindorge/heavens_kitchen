using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPhysical : MonoBehaviour
{
    /*
    Editor Settings
    */
    [Header("Food Properties")]
    public float timeToCook;
    public float overCook;
    public FoodType foodType;

    [Header("Food Models")]
    public GameObject rawModel;
    public GameObject cookedModel;
    public GameObject burntModel;

    [Header("Particle System")]
    public ParticleSystem particleSystem;

    [Header("Child Anchor")]
    public Transform TopAnchor;

    /*
    Class Settings
    */
    private float currentlyCooked;
    private bool isCooking;
    private FoodState foodState = FoodState.Raw;
    private bool hasChild = false;
    private FoodPhysical childFoodObject;

    private void Start()
    {
        if (childFoodObject != null)
        {
            childFoodObject.GetComponent<BoxCollider>().enabled = false;
            childFoodObject.GetComponent<Rigidbody>().useGravity = false;
            hasChild = true;
        }
    }

    private void FixedUpdate()
    {
        // Play a nice particle animation when cooking and keep track of how long the food is on the stove for.
        if (isCooking)
        {
            particleSystem.enableEmission = true;
            currentlyCooked += Time.deltaTime;
            // Item has been burnt, change model from cooked to burnt.
            if (currentlyCooked >= overCook && foodState == FoodState.Cooked)
            {
                foodState = FoodState.Burnt;
                burntModel.SetActive(true);
                cookedModel.SetActive(false);
                rawModel.SetActive(false);
            }
            // Item has been cooked, change model from raw to cooked.
            else if (currentlyCooked >= timeToCook && foodState == FoodState.Raw)
            {
                foodState = FoodState.Cooked;
                cookedModel.SetActive(true);
                rawModel.SetActive(false);
                burntModel.SetActive(false);
            }
        }
        else
        {
            // Item isn't cooking, so no effects.
            particleSystem.enableEmission = false;
        }
    }

    private void Update()
    {
        if (hasChild)
        {
            childFoodObject.transform.position = TopAnchor.transform.position;
            childFoodObject.transform.rotation = TopAnchor.transform.rotation;
        }
    }

    // Attach the item of food to the top most item of food.
    public void AttachFood(GameObject otherFood)
    {
        if (!hasChild)
        {
            otherFood.GetComponent<BoxCollider>().enabled = false;
            otherFood.GetComponent<Rigidbody>().useGravity = false;
            childFoodObject = otherFood.GetComponent<FoodPhysical>();
            hasChild = true;
        }
        else
        {
            childFoodObject.AttachFood(otherFood);
        }
    }

    // Detact the top most food item.
    public bool DetatchFood()
    {
        if (hasChild && !childFoodObject.DetatchFood())
        {
            childFoodObject.GetComponent<BoxCollider>().enabled = true;
            childFoodObject.GetComponent<Rigidbody>().useGravity = true;
            hasChild = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Check if the current item of food is cooking.
    public bool IsCooking()
    {
        return isCooking;
    }

    // Set the item of food to cook mode.
    public void SetToCook()
    {
        isCooking = true;
    }

    // Get the type of food being used.
    public FoodType GetFoodType()
    {
        return foodType;
    }

    // Set the food off of the cook mode.
    public void SetOffCook()
    {
        isCooking = false;
    }

    // Get the current state of the food.
    public FoodState GetFoodState()
    {
        return foodState;
    }

    // Get the current child object.
    public FoodPhysical GetChild()
    {
        return childFoodObject;
    }

    // Set the current child to something external.
    public void SetChild(FoodPhysical child)
    {
        this.childFoodObject = child;
        this.hasChild = true;
    }

    // Return the existance of its child.
    public bool HasChild()
    {
        return hasChild;
    }

    // Debugging Method for printing food list.
    public void PrintSystem()
    {
        print(foodState.ToString() + " : " + foodType.ToString());
        if (hasChild)
        {
            childFoodObject.PrintSystem();
        }
    }

    // Destroy all, and child if possible.
    public void DestroyFood()
    {
        if (hasChild)
        {
            childFoodObject.DestroyFood();
        }
        Destroy(gameObject);
    }
}
