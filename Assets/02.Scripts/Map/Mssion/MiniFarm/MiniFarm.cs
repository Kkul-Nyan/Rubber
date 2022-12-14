using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniFarm : MonoBehaviour
{
    public Vegetable[] vegetables;
    

    [SerializeField] bool isSuccessFarm = false;

    public bool isWrong = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < vegetables.Length; i++)
        {
            Debug.Log(i);
            Debug.Log(vegetables[i]);
            vegetables[i].vegetableID = i;

        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    
    public int count = 0;

    public void AddVegetable(Vegetable vegetable)
    {
        vegetable.gameObject.SetActive(false);
        count++;
        
        if(count == vegetables.Length)
        {
            isSuccessFarm = true;
        }
        

    }

   
}
