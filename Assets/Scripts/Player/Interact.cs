using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private bool holdingItem;
    private float previousMass;

    [Header("Interaction Layer")]
    public LayerMask objectMask;

    [Header("Sound Clips")]
    public AudioSource audioSource;
    public AudioClip pickUpSound;
    public AudioClip dropSound;
    public AudioClip detatchSound;
    public AudioClip attachSound;
    public AudioClip spawnSound;

    [Header("Player UI")]
    public ToolTip leftToolTip;
    public ToolTip rightToolTip;
    public OrdersList orders;

    private bool lookingAtFood;
    private FoodPhysical lookingFoodItem;
    private FoodSpawner lookingSpawnerItem;
    private bool lookingAtSpawner;
    private LevelStart lookingStarterItem;
    private bool lookingAtStarter;
    private Table lookingOrdersItem;
    private bool lookingAtOrder;
    private TutorialStarter lookingTutorialItem;
    private bool lookingAtTutorial;
    private bool loadedOrder;
    private FoodPhysical heldFood;

    private void FixedUpdate()
    {
        // Reset variables for knowing what is being looked at.
        lookingAtFood = false;
        lookingAtSpawner = false;
        lookingAtStarter = false;
        lookingAtOrder = false;
        lookingAtTutorial = false;

        Debug.DrawRay(transform.transform.position, transform.forward, Color.red, 5f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f, objectMask))
        {
            lookingFoodItem = hit.collider.GetComponent<FoodPhysical>();
            lookingSpawnerItem = hit.collider.GetComponent<FoodSpawner>();
            lookingStarterItem = hit.collider.GetComponent<LevelStart>();
            lookingOrdersItem = hit.collider.GetComponent<Table>();
            lookingTutorialItem = hit.collider.GetComponent<TutorialStarter>();

            if (lookingFoodItem)
            {
                // Player is looking at an item of food.
                lookingAtFood = true;
            }
            else if (lookingSpawnerItem)
            {
                // Player is looking at a food spawner.
                lookingAtSpawner = true;
            }
            else if (lookingStarterItem)
            {
                // Player is looking at a level starter object.
                lookingAtStarter = true;
            }
            else if (lookingOrdersItem)
            {
                // Player is looking at a customer's order.
                lookingAtOrder = true;
            }
            else if (lookingTutorialItem)
            {
                // Player is looking at a tutorial starter item.
                lookingAtTutorial = true;
            }
        }
    }

    private void UpdateToolTips()
    {
        if (!holdingItem)
        {
            if (lookingAtFood)
            {
                // Player is looking at an item of food, whilst not holding anything.
                leftToolTip.SetText("Pick up");
                rightToolTip.SetText("Take food off");
                leftToolTip.ShowToolTip();
                rightToolTip.ShowToolTip();
            }
            else if (lookingAtSpawner)
            {
                // Player is looking at a food spawner, whilst not holding anything.
                leftToolTip.SetText("Spawn Food");
                leftToolTip.ShowToolTip();
                rightToolTip.HideToolTip();
            }
            else if (lookingAtStarter)
            {
                // Player is looking at a level starter, whilst not holding anything.
                leftToolTip.SetText("Start Game");
                leftToolTip.ShowToolTip();
                rightToolTip.HideToolTip();
            }
            else if (lookingAtTutorial)
            {
                // Player is looking at a tutorial starter item.
                if (lookingTutorialItem.TutorialActive())
                {
                    // Tutorial is active, so add option of ending it. 
                    leftToolTip.SetText("End Tutorial");
                }
                else
                {
                    // Tutorial has not started, so add option of starting it.
                    leftToolTip.SetText("Start Tutorial");
                }
                leftToolTip.ShowToolTip();
                rightToolTip.HideToolTip();
            }
            else
            {
                leftToolTip.HideToolTip();
                rightToolTip.HideToolTip();
            }
        }
        else
        {
            // Player is holding an item of food, so state their option of droping it.
            leftToolTip.SetText("Drop food");
            leftToolTip.ShowToolTip();

            // Player is looking at an item of food, whilst already holding an item of food.
            if (lookingAtFood)
            {
                rightToolTip.SetText("Attach food");
                rightToolTip.ShowToolTip();
            }
            else
            {
                rightToolTip.HideToolTip();
            }
        }
        
        // Player is looking a customer's order, so load their order if possible.
        if (lookingAtOrder && !loadedOrder)
        {
            if (!lookingOrdersItem.IsTableFree())
            {
                // Avoid doing this every frame so add a variable to keep track of when the order is loaded.
                loadedOrder = true;
                orders.AddRequest(lookingOrdersItem.GetRequestedFood());
            }
        }

        // If the order is loaded, and the player has just moved away from the customer, unload their order.
        else if (!lookingAtOrder && loadedOrder)
        {
            loadedOrder = false;
            orders.ClearList();
        }
    }

    void Update()
    {
        UpdateToolTips();

        // Left Mouse Action
        if (Input.GetMouseButtonDown(0))
        {
            // Pick up item of food.
            if (!holdingItem && lookingAtFood)
            {
                audioSource.clip = pickUpSound;
                audioSource.Play();
                holdingItem = true;
                heldFood = lookingFoodItem;
                heldFood.GetComponent<Rigidbody>().useGravity = false;
                heldFood.gameObject.GetComponent<BoxCollider>().enabled = false;
                heldFood.gameObject.GetComponent<FoodPhysical>().SetOffCook();
            }
            // Spawn an item of food.
            else if (!holdingItem && lookingAtSpawner)
            {
                audioSource.clip = spawnSound;
                audioSource.Play();
                lookingSpawnerItem.SpawnFood();
            }
            // Start a level.
            else if (!holdingItem && lookingAtStarter)
            {
                audioSource.clip = spawnSound;
                audioSource.Play();
                lookingStarterItem.StartGame();
            }
            // Start or end the tutorial.
            else if (!holdingItem && lookingAtTutorial)
            {
                audioSource.clip = spawnSound;
                audioSource.Play();
                lookingTutorialItem.ActivateTutorial();
            }
            // Drop the item of food currently being held.
            else if (holdingItem)
            {
                audioSource.clip = dropSound;
                audioSource.Play();
                holdingItem = false;
                heldFood.GetComponent<Rigidbody>().useGravity = true;
                heldFood.GetComponent<BoxCollider>().enabled = true;
            }
        }

        // Right Mouse Action
        if (Input.GetMouseButtonDown(1))
        {
            if (lookingAtFood)
            {
                // Attach item of food to another.
                if (holdingItem)
                {
                    audioSource.clip = attachSound;
                    audioSource.Play();
                    holdingItem = false;
                    lookingFoodItem.AttachFood(heldFood.gameObject);
                }

                // Detact an item of food from another.
                else
                {
                    audioSource.clip = detatchSound;
                    audioSource.Play();
                    lookingFoodItem.DetatchFood();
                }
            }
        }

        // Move the food item infront of the player's view when being held.
        if (holdingItem)
        {
            heldFood.transform.position = Vector3.Lerp(heldFood.transform.position, transform.position + transform.forward * 1, 10 * Time.deltaTime);
            heldFood.transform.rotation = Quaternion.Lerp(heldFood.transform.rotation, new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), 20 * Time.deltaTime);
        }
    }
}