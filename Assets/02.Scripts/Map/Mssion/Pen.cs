using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public StuffsList StuffsList;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Stuff"))
        {
            Stuff stuff = other.GetComponent<Stuff>();
            StuffsList.AddStuff(stuff);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stuff"))
        {
            Stuff stuff = other.GetComponent<Stuff>();
            StuffsList.RemoveStuff(stuff);
        }
    }
}
