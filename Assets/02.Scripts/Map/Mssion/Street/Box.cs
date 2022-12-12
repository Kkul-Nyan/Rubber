using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڽ��� ������ ������ �������� �ڽ��� �ڽĿ�����Ʈ�� �ٲ��ش�.
/// �ڽ��� ������ ī���ʹ� ui�� ����
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
            //�ڽ� ���ͷ��� Ȱ��ȭ
            GetComponent<InteractableObject>().enabled = true;  
        }
    }
}
