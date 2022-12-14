using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen2 : MonoBehaviour
{
    public MiniFarm MiniFarm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vegetable"))
        {
            Vegetable Vegetable = other.GetComponent<Vegetable>();
            MiniFarm.AddVegetable(Vegetable);
        }
    }
   
}
