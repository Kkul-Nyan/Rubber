using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxElementCollider : MonoBehaviour
{
    private Box box;

    private void Start()
    {
        box = GetComponentInParent<Box>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        Leaf leaf = other.GetComponent<Leaf>();
        box.UpdateBoxElementCount(leaf);
    }
}
