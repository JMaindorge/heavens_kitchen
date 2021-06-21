using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private Vector3 homeLocation;
    private Table table;
    private NavMeshAgent agent;
    public Transform foodHolder;
    private bool holdingItem;
    private Transform itemPosition;
    private bool movingToTable;
    private bool returning;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        homeLocation = transform.position;
        FindTable();
    }

    // Find a table an available table in the scene.
    private bool FindTable()
    {
        // Get a list of all the tables in the scene.
        Table[] tables = GameObject.FindObjectsOfType<Table>();
        for (int i = 0; i < tables.Length; i++)
        {
            // Find and queue a free table.
            if (tables[i].QueueTable())
            {
                table = tables[i];
                agent.destination = table.transform.position;
                movingToTable = true;
                return true;
            }
        }
        return false;
    }

    private void FixedUpdate()
    {
        // While the customer is moving to their table. 
        if (movingToTable)
        {
            // if the customer is within a certain distance, attempt to add themselves to the table.
            if (Vector3.Distance(transform.position, table.waitingArea.transform.position) <= 0.5f)
            {
                if (table.AddCustomer(this))
                {
                    movingToTable = false;
                }
                else
                {
                    movingToTable = false;
                    FindTable();
                }
            }
        }
        // While the customer is returning.
        else if (returning)
        {
            // If the customer is within a certain distance, delete everything related to them, even the food.
            if (Vector3.Distance(transform.position, homeLocation) <= 2f)
            {
                if (holdingItem)
                {
                    itemPosition.gameObject.GetComponent<FoodPhysical>().DestroyFood();
                }
                Destroy(gameObject);
            }
        }
    }

    // Holding items is done in the Update function because it is smoother. 
    private void Update()
    {
        if (holdingItem)
        {
            itemPosition.position = Vector3.Lerp(itemPosition.position, foodHolder.position, 10 * Time.deltaTime);
            itemPosition.rotation = Quaternion.Lerp(itemPosition.rotation, new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), 20 * Time.deltaTime);
        }
    }

    // Allow the customer to grab their food and exit the restaurant.
    public void TakeFood(Transform other)
    {
        itemPosition = other;
        itemPosition.gameObject.GetComponent<Rigidbody>().useGravity = false;
        itemPosition.gameObject.GetComponent<BoxCollider>().enabled = false;
        holdingItem = true;
        ReturnHome();
    }

    // Set the customer's destination from where they spawned, thus leaving the restaurant.
    public void ReturnHome()
    {
        returning = true;
        GetComponent<NavMeshAgent>().destination = homeLocation;
    }

    public void Die()
    {
        itemPosition.gameObject.GetComponent<FoodPhysical>().DestroyFood();
        // Play a death animation here.
        Destroy(gameObject);
    }
}
