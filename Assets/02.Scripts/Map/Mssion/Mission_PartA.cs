using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_PartA : MonoBehaviour
{
    float changeColor = 1;
    float timer = 0;
    private void OnCollisionStay(Collision collision)
    {
        timer += Time.deltaTime;
        Renderer render;
        render = gameObject.GetComponent<Renderer>();
        bool cleanPlate = false;

        if (collision.gameObject.name == "SM_Item_Soap_01" && changeColor <= 1 && timer > 2)
        {
            changeColor -= 0.3f;
            Debug.Log("Enter");
            render.material.color = Color.LerpUnclamped(Color.white, Color.black, changeColor);
            timer = 0;
        }
        else if(changeColor <= 0)
        {
            cleanPlate = true;
            Debug.Log("cleanPlate = " + cleanPlate);
        }
    }
}
