using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Mission_PartB : MonoBehaviour
{
    
    public GameObject can;
    public GameObject gameCard;

    public TMP_Text loading;

   

    // Start is called before the first frame update
    void Start()
    {
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
            gameCard.SetActive(true);
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
    public GameObject[] game = new GameObject[5];
    private void StuffList()
    {
       

        
    }

}
