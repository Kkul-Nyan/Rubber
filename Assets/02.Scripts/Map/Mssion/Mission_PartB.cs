using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Mission_PartB : MonoBehaviour
{
    
    public GameObject can;
    public GameObject gamePanel;

    public TMP_Text loading;

    public TMP_Text stuffText;

   

    // Start is called before the first frame update
    void Start()
    {
        StuffList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 코인 - 자판기
        if(collision.gameObject.tag == "machine")
        {
            Destroy(collision.gameObject);
            can.SetActive(true);
            
            // can.GetComponent<Collider>().enabled = false; << 게임 오브젝트 자체를 끄는게 아니라 속해있는 다른걸 끄고싶을떄 
        }
        // 코인 - 오락기
        else if(collision.gameObject.tag == "game")
        {
            Destroy(collision.gameObject);
            gamePanel.SetActive(true);

        }
        // 보안카드 - 수질관리기
        else if(collision.gameObject.tag == "water_management")
        {
            loading.gameObject.SetActive(true);
            StartCoroutine(Timer(30));
        }
    }
    IEnumerator Timer(int time)
    {
        while (time > 0)
        {
            // int min = time / 60;
            int sec = time % 60;
            loading.text = ("Loading..."  + sec);
            yield return new WaitForSecondsRealtime(1.0f);
            time--;
        }
    }
     // 식당 재료 리스트에 텍스트 랜덤으로 생성.
    public Stuff[] stuffs = new Stuff[5];
    public TMP_Text[] stuffsText;


    private void StuffList()
    {
        // string[] stuffsName = new string[] { "corn", "tomato", "pumpkin", "sausage", "bread" };
        // string randomName = stuffs[Random.Range(0, stuffs.Length)];
        // stuffText.text = randomName;
        bool[] randomFlages = new bool[stuffs.Length];
        List<int> randomNumbers = new List<int>();
       

        while(randomNumbers.Count < stuffs.Length)
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
        for(int i = 0; i < stuffsText.Length; i++)
        {
            stuffsText[i].text = stuffs[randomNumbers[i]].stuffName;
        }

        
    }


}
