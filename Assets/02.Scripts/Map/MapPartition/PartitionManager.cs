using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionManager : MonoBehaviour
{
    //플레이어가 위치한 장소의 이름을 바꿔주는 함수
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
