using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FoodPhysical deleting = other.GetComponent<FoodPhysical>();
        if (deleting)
        {
            deleting.DestroyFood();
        }
    }
}
