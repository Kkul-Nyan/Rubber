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

   

    [SerializeField] bool isSuccessCan = false;
    [SerializeField] bool isSuccessLoading = false;
    


   

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
        // ���� - ���Ǳ�
        if(collision.gameObject.tag == "machine")
        {
            Destroy(collision.gameObject);
            can.SetActive(true);
            isSuccessCan = true;
            
            // can.GetComponent<Collider>().enabled = false; << ���� ������Ʈ ��ü�� ���°� �ƴ϶� �����ִ� �ٸ��� ��������� 
        }
        // ���� - ������
        else if(collision.gameObject.tag == "game")
        {
            Destroy(collision.gameObject);
            gamePanel.SetActive(true);

        }
        // ����ī�� - ����������
        else if(collision.gameObject.tag == "water_management")
        {
            loading.gameObject.SetActive(true);
            StartCoroutine(Timer(30));

        }

      
    }
    IEnumerator Timer(int time)
    {
        while (isSuccessLoading == false)
        {
            // int min = time / 60;
            int sec = time % 60;
            loading.text = ("Loading..."  + sec);
            yield return new WaitForSecondsRealtime(1.0f);
            time--;

            if(time <= 0)
            {
                loading.gameObject.SetActive(false);
                isSuccessLoading = true;
            }
        }
        
    }
     // �Ĵ� ��� ����Ʈ�� �ؽ�Ʈ �������� ����.
   


    


}
