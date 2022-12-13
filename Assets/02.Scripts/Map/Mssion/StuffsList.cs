using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StuffsList : MonoBehaviour
{
    // public TMP_Text stuffText;

    public Stuff[] stuffs;
    public TMP_Text[] stuffsText;
    public List<int> randomNumbers = new List<int>();

    [SerializeField] bool isSuccessList = false;

    public bool isWrong = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stuffs.Length; i++)
        {
            Debug.Log(i);
            Debug.Log(stuffs[i]);
            stuffs[i].ID = i;

        }
        StuffList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StuffList()
    {
        // string[] stuffsName = new string[] { "corn", "tomato", "pumpkin", "sausage", "bread" };
        // string randomName = stuffs[Random.Range(0, stuffs.Length)];
        // stuffText.text = randomName;
        bool[] randomFlages = new bool[stuffs.Length];

        while (randomNumbers.Count < stuffsText.Length)
        {
            int random = Random.Range(0, stuffs.Length);
            if (randomFlages[random] == true)
            {
                continue;
            }
            else
            {
                randomFlages[random] = true;
                randomNumbers.Add(random);
            }
        }
        for (int i = 0; i < stuffsText.Length; i++)
        {
            stuffsText[i].text = stuffs[randomNumbers[i]].stuffName;
        }


    }
    public int count = 0;

    public void AddStuff(Stuff stuff)
    {

        if (randomNumbers.Contains(stuff.ID))
        {
            Debug.Log($"{stuff.stuffName}");
            int textIndex = randomNumbers.IndexOf(stuff.ID);


            //들어왔을때 처리
            stuff.gameObject.SetActive(false);

            count++;
            if (count >= stuffsText.Length && isWrong == false)
            {
                isSuccessList = true;
            }
            
        }
        else
        {
            isWrong = true;
            
            Debug.Log("no");
        }
    }

    public void RemoveStuff(Stuff stuff)
    {

        if (randomNumbers.Contains(stuff.ID))
        {
           
        }
        else
        {
            isWrong = false;
            
            Debug.Log("no");
        }
    }
}
