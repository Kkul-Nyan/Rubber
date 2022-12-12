using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;

public class CommonCharacter : MonoBehaviour
{
    public float speed = 1;
    public float stamina = 10;
    public int jobCode;
    public Jobs jobs = new Jobs();
    public Skills skill = new Skills();
    public Penaltys penalty = new Penaltys();
    //Buff,Debuff
    public bool isComa = false;
    public bool isHeal = false;
    public bool isBombed = false;
    public bool iCanFly = false;

    //
    public bool cantVote = false;


    public MeshFilter meshFilter;
    public Mesh[] bluemesh;

    public void ChangeColor(int selectColor)
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = bluemesh[selectColor];
    }


    private void Start()
    {
        jobCode = 100;
        JobPreset();
    }

    private void Update()
    {

    }

    public void JobChange(int codeForChange)
    {
        jobCode = codeForChange;
        JobPreset();
    }

    public void JobPreset()
    {
        switch (jobCode)
        {
            //러버덕진영
            case 102:
                jobs.Doctor_Set(penalty);
                break;
            case 105:
                jobs.Betlayer_Set(skill, penalty);
                break;
            case 106:
                jobs.Marathoner_Set(skill, penalty, ref stamina);
                break;
            case 108:
                jobs.Mechanic_Set(skill, penalty);
                break;
            case 109:
                jobs.Wing_Set(skill, penalty, ref stamina);
                break;
            //생체덕진영
            case 201:
                jobs.Blind_Set(penalty);
                break;
            case 206:
                jobs.Disguiser_Set(skill, penalty);
                break;

            //DeadDuck
            case 301:
                jobs.DeadDuck_Set(this);
                break;

            //프리세팅이 불필요한 직업군
            default:
                break;
        }
    }

    public void JobsDo()
    {
        if (isComa)
        {
            return;
        }

        switch (jobCode)
        {
            //러버덕 진영
            case 101:
                jobs.Sherrif_Do(skill, penalty, this);
                break;
            case 102:
                jobs.Doctor_Do(skill, penalty);
                break;
            case 103:
                jobs.Shaman_Do(skill, penalty);
                break;
            case 104:
                jobs.GoodNose_Do(skill, penalty);
                break;
            case 106:
                jobs.Marathoner_Do(skill, penalty);
                break;
            case 107:
                jobs.Detective_Do(skill, penalty);
                break;
            case 108:
                jobs.Mechanic_Do(skill, penalty);
                break;
            case 109:
                jobs.Wing_Do(skill, penalty, ref stamina);
                break;
            case 110:
                jobs.SpeedRacer_Do(skill, penalty, ref speed);
                break;

            //오리 진영
            case 201:
                jobs.Blind_Do(skill, penalty);
                break;
            case 204:
                jobs.Spy_Do(skill, penalty);
                break;
            case 205:
                jobs.Bully_Do(skill, penalty);
                break;
            case 206:
                jobs.Disguiser_Do(skill, penalty);
                break;

            //직업스킬이 불필요한 직업군
            default:
                break;
        }
    }

    public void Attack()
    {
        if (isComa)
        {
            return;
        }

        switch (jobCode)
        {
            //생체덕진영
            case 201:
                jobs.Blind_Attack(skill, penalty);
                break;
            case 202:
                jobs.Cowboy_Attack(skill, penalty);
                break;
            case 203:
                jobs.NeedleSpitter_Attack(skill, penalty);
                break;
            case 204:
                jobs.Spy_Attack(skill, penalty);
                break;
            case 205:
                jobs.Bully_Attack(skill, penalty);
                break;
            case 206:
                jobs.Disguiser_Attack(skill, penalty);
                break;
            case 207:
                jobs.Bomber_Attack(skill, penalty);
                break;
            default:
                break;
        }
    }

    public void RubberDuckRun(bool isRun)
    {
        if (isRun)
        {
            stamina -= Time.deltaTime;
            if (stamina > 0)
            {
                speed = 2;
            }
            if (iCanFly)
            {

            }
        }
        else
        {
            speed = 1;
        }
    }
}