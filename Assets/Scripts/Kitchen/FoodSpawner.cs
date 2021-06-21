using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform spawnLocation;

    public void SpawnFood()
    {
        // Spawn Food at Location.
        Instantiate(foodPrefab, spawnLocation);
    }
}
