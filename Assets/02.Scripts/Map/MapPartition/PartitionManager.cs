using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionManager : MonoBehaviour
{
    //�÷��̾ ��ġ�� ����� �̸��� �ٲ��ִ� �Լ�
    public delegate void SectorChangeEvent(string value);
    public SectorChangeEvent OnSectorChanged;

    private PartitionCollider currentPartition;
    public PartitionCollider CurrentPartition
    {
        get
        {
            return currentPartition;
        }
        set
        {
            if(value != null)
            {
                currentPartition = value;
                OnSectorChanged?.Invoke(currentPartition.name);
            }
        }
    }

    public string localOwner;

    public List<PartitionCollider> partitionColliders;

    private void Start()
    {
        foreach(var partition in partitionColliders)
        {
            partition.manager = this;
        }
    }
}
