using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<FoodPhysical>())
        {
            other.GetComponent<FoodPhysical>().SetToCook();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<FoodPhysical>()) 
        {
            other.GetComponent<FoodPhysical>().SetOffCook();
        }
    }
}
