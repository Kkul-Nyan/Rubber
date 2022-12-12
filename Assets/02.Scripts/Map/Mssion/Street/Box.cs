using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 박스에 나뭇잎 들어오면 나뭇잎을 박스의 자식오브젝트로 바꿔준다.
/// 박스의 나뭇잎 카운터는 ui와 연동
/// </summary>

public class Box : MonoBehaviour
{
    public delegate void BoxElementCountEvent(int value);
    public static BoxElementCountEvent boxElementCountEvent;

    private List<Leaf> leaves;
    public int missionCount = 0;

    public void UpdateBoxElementCount(Leaf leaf)
    {
        leaves.Add(leaf);
        leaf.transform.SetParent(transform);

        if(leaves.Count >= missionCount)
        {
            //박스 인터렉션 활성화
            GetComponent<InteractableObject>().enabled = true;  
        }
    }
}
