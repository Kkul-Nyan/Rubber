using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;

public class CommonCharacter : MonoBehaviour
{
    private string _character_ID;
    public float speed = 1;
    public float stamina = 10;
    public int jobCode;
    public Jobs jobs = new Jobs();
    public Skills skill = new Skills();
    public Penaltys penalty = new Penaltys();
    public GameObject knife;

    public CommonCharacter target;

    //Buff,Debuff
    public bool isComa = false;
    public float comaTimer = 10f;
    public bool isWounded = false;
    public float woundTimer = 10f;
    public bool isBombed = false;
    public float bombedTimer = 10f;

    //Job Set
    public bool cantVote = false;
    public bool iCanFly = false;

    //Item Pick
    public bool haveTape = false;
    public bool haveItem = false;

    //Color Pick
    public MeshFilter meshFilter;
    public Mesh[] bluemesh;

    public void ChangeColor(int selectColor)
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = bluemesh[selectColor];
    }


    private void Start()
    {
        _character_ID = "";
        jobs.Player = this;
        meshFilter = GetComponent<MeshFilter>();

        jobCode = 100;
        JobPreset();
    }

    private void Update()
    {
        //GameManager.instance.isDiscuss => ȸ�����̶�� bool��
        //���ӸŴ������� ��� �������̶�� �����Ͽ� ����.
        //if (!GameManager.instance.isDiscuss)
        //{
            //��ó ������ 10�� �� ����
            if (isWounded)
            {
                woundTimer -= Time.deltaTime;
                if (woundTimer <= 0)
                {
                    isComa = true;
                }
            }
            //���� ���ϸ� 10�� �� ����
            if (isBombed)
            {
                bombedTimer -= Time.deltaTime;
                if (bombedTimer <= 0)
                {
                    isComa = true;
                }
            }
            //���� ���ϸ� 10�� �� Dead
            if (isComa)
            {
                comaTimer -= Time.deltaTime;
                if (comaTimer <= 0)
                {
                    penalty.Dead(this);
                }
            }
        //}

    }
    /// <summary>
    /// Can change job of this Duck to 'jobCodeForChange'`s job.
    /// </summary>
    /// <param name="jobCodeForChange"></param>
    public void JobChange(int jobCodeForChange)
    {
        jobCode = jobCodeForChange;
        JobPreset();
    }

    /// <summary>
    /// Can preset this Duck`s Param for these job.
    /// </summary>
    public void JobPreset()
    {
        switch (jobCode)
        {
            //����������
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
                jobs.Wing_Set(ref stamina);
                break;
            //��ü������
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

            //���������� ���ʿ��� ������
            default:
                break;
        }
    }

    /// <summary>
    /// For use the Skills of jobs.
    /// </summary>
    public void JobsDo()
    {
        if (isComa)
        {
            return;
        }

        jobs.Target = target;

        switch (jobCode)
        {
            //������ ����
            case 101:
                jobs.Sherrif_Do(skill, penalty);
                break;
            case 102:
                jobs.Doctor_Do(skill);
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
            case 110:
                jobs.SpeedRacer_Do(skill, penalty, ref speed);
                break;

            //���� ����
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

            //������ų�� ���ʿ��� ������
            default:
                break;
        }
    }

    /// <summary>
    /// For set this Duck`s attack variation.
    /// </summary>
    public void Attack()
    {
        if (isComa)
        {
            return;
        }

        switch (jobCode)
        {
            //��ü������
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

    /// <summary>
    /// If you have a Item, you can Use it.
    /// </summary>
    public void UseItem()
    {
        if (haveTape)
        {
            skill.Heal(target, 1);
            haveTape = false;
        }
        /*
        if (haveItem)
        {
            skill.Item(target, 1);
            haveItem = false;
        }
        */
    }

    public void GetItem()
    {
        if (haveTape && haveItem)
        {
            return;
        }
        if (target.CompareTag("Tape"))
        {
            haveTape = true;
        }
        if (target.CompareTag("Item"))
        {
            haveItem = true;
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
            if (jobCode == 109)
            {
                //�ݶ��̴� �Ͻ�����(�� �� ĳ���Ͱ� ������ ����)
                this.GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            speed = 1;
            if (jobCode == 109)
            {
                //�ݶ��̴� ���(�� �� ĳ���Ͱ� ������ ���)
                this.GetComponent<Collider>().enabled = true;
            }
        }
    }
}