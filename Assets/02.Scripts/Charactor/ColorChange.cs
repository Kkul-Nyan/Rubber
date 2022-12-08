using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    MeshFilter meshFilter;
    public Mesh bluemesh;
    
    public void ChangeColor()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = bluemesh;
    }

}
