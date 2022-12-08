using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager s_Instance;
    public GameObject localTank;
    public Button BoostButton;
    public Button ShootButton;
    public bool IsGameInputEnabled = false;
    public int RedTeamScore = 0;
    public int BlueTeamScore = 0;

    void Awake()
    {
        s_Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartBuffer());
    }

    IEnumerator StartBuffer()
    {
        yield return new WaitForSeconds(5);
        IsGameInputEnabled = true;
    }

    public void RemoveTank(GameObject tank)
    {
        Destroy(tank);
    }
}
