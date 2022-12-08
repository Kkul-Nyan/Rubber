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
        bool cleanFountain = false;

        // 식당 - 설거지
        if (collision.gameObject.tag == "SOAP" && changeColor <= 1 && timer > 2)
        {
            changeColor -= 0.3f;
            Debug.Log("Enter");
            render.material.color = Color.LerpUnclamped(Color.black, Color.white, changeColor);
            timer = 0;
        }
        else if (changeColor <= 0)
        {
            cleanPlate = true;
            Debug.Log("cleanPlate = " + cleanPlate);
        }

        // 대공원 - 분수닦기
        if (collision.gameObject.tag == "FOUNTAIN" && changeColor <= 1 && timer > 2)
        {
                changeColor -= 0.3f;
                Debug.Log("Enter");
                render.material.color = Color.LerpUnclamped(Color.black, Color.gray, changeColor);
                timer = 0;
        }
        else if (changeColor <= 0)
        {
            cleanFountain = true;
            Debug.Log("cleanFountain = " + cleanFountain);
        }

    }
}
