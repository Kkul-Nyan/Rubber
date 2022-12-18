using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_PartA : MonoBehaviour
{
    float changeColor = 1;
    float timer = 0;

    bool cleanConnect = false;
    bool cleanRinger = false;
    bool cleanPlate = false;
    bool cleanFountain = false;

    private void OnCollisionStay(Collision collision)
    {
        timer += Time.deltaTime;
        Renderer render;
        render = gameObject.GetComponent<Renderer>();

        // 식당 - 설거지
        if (collision.gameObject.tag == "SOAP" && changeColor <= 1 && timer > 2)
        {
            changeColor -= 0.3f;
            Debug.Log("Stay");
            render.material.color = Color.LerpUnclamped(Color.black, Color.white, changeColor);
            timer = 0;
        }
        else if (changeColor <= 0)
        {
            cleanPlate = true;
            Debug.Log("cleanPlate = " + cleanPlate);
        }

        // 대공원 - 분수닦기
        if (collision.gameObject.tag == "DUSTCLOTH" && changeColor <= 1 && timer > 2)
        {
            changeColor -= 0.3f;
            Debug.Log("Stay");
            render.material.color = Color.LerpUnclamped(Color.black, Color.gray, changeColor);
            timer = 0;
        }
        else if (changeColor <= 0)
        {
            cleanFountain = true;
            Debug.Log("cleanFountain = " + cleanFountain);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;

        // 의료실 - 오리와 링거 연결
        if (other.gameObject.tag == "DUCK" && timer >= 10)
        {
            cleanRinger = true;
            Debug.Log("cleanRinger = " + cleanRinger);
        }
        else if (timer < 10)
        {
            Debug.Log("Stay");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // 배전함 - 전선 연결
        if (collision.gameObject.tag == "SPHERERED" && collision.gameObject.tag == "SPHEREYELLOW" && collision.gameObject.tag == "SPHEREBLUE")
        {
            cleanConnect = true;
            Debug.Log("cleanConnect = " + cleanConnect);
        }

        
    }


}
