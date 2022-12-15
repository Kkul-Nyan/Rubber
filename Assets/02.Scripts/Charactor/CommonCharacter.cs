using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;

public class CommonCharacter : MonoBehaviour
{
    //Character Parameter
    private string _character_ID;
    public Camera playerCamera;
    public float speed = 1;
    public float stamina = 10;
    public int jobCode;
    public Jobs jobs = new Jobs();
    public Skills skill = new Skills();
    public Penaltys penalty = new Penaltys();
    public GameObject knife;

    //Environment
    public CommonCharacter target;
    public bool isDiscuss = false;

    //Buff,Debuff
    public bool isComa = false;
    public float comaTimer = 10f;
    public bool isWounded = false;
    public float woundTimer = 10f;
    public bool isBombed = false;
    public float bombedTimer = 10f;
    public bool isSilence = false;

    //Job Set
    public bool cantVote = false;
    public bool iCanFly = false;
    public bool iAmBetlayer = false;

    //Item Pick
    public bool haveTape = false;
    public bool haveItem = false;

    //Color Pick
    public MeshFilter meshFilter;
    public Mesh[] bluemesh;
    int selectColor;

    public void ChangeColor(int selectColor)
    {
        this.selectColor = selectColor;
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = bluemesh[this.selectColor];
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
        //isDiscuss = GameManager.instance.isDiscuss;

        if (!isDiscuss)
        {
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
        }

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
    /// This 
    /// </summary>
    public void DuckInit(bool allClear)
    {
        if (allClear)
        {
            JobChange(100);
        }
        //Character Parameter
        speed = 1;
        stamina = 10;

        //Buff,Debuff
        isComa = false;
        comaTimer = 10f;
        isWounded = false;
        woundTimer = 10f;
        isBombed = false;
        bombedTimer = 10f;
        isSilence = false;
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
                jobs.Doctor_Set();
                break;
            case 105:
                jobs.Betlayer_Set();
                break;
            case 106:
                jobs.Marathoner_Set();
                break;
            case 108:
                jobs.Mechanic_Set();
                break;
            case 109:
                jobs.Wing_Set();
                break;
            //��ü������
            case 201:
                jobs.Blind_Set();
                break;
            case 206:
                jobs.Disguiser_Set();
                break;

            //DeadDuck
            case 301:
                jobs.DeadDuck_Set();
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

        //��ź�� �޾����� Ÿ�ٿ��� ��ź�� �ǳ���
        if (isBombed)
        {
            //�ǳ��ִ� ��ź�� Ÿ�̸Ӵ� ��� �帧.
            //��ź�� �ǳ��ִµ� ���������� ��ź ���� bool���� false��.
            isBombed = skill.PresentBomb(target, bombedTimer);
            return;
        }

        //������ ��뽺ų ����
        switch (jobCode)
        {
            //������ ����
            case 101:
                jobs.Sherrif_Do();
                break;
            case 102:
                jobs.Doctor_Do();
                break;
            case 103:
                jobs.Shaman_Do();
                break;
            case 104:
                jobs.GoodNose_Do();
                break;
            case 107:
                jobs.Detective_Do();
                break;
            case 108:
                jobs.Mechanic_Do();
                break;
            case 110:
                jobs.SpeedRacer_Do();
                break;

            //���� ����
            case 201:
                jobs.Blind_Do();
                break;
            case 204:
                jobs.Spy_Do();
                break;
            case 205:
                jobs.Bully_Do();
                break;
            case 206:
                jobs.Disguiser_Do();
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
                jobs.Blind_Attack();
                break;
            case 202:
                jobs.Cowboy_Attack();
                break;
            case 203:
                jobs.NeedleSpitter_Attack();
                break;
            case 204:
                jobs.Spy_Attack();
                break;
            case 205:
                jobs.Bully_Attack();
                break;
            case 206:
                jobs.Disguiser_Attack();
                break;
            case 207:
                jobs.Bomber_Attack();
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
        if (haveTape && target.isWounded == true)
        {
            target.isWounded = false;
            target.woundTimer = 10f;
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
            if (jobCode == 106)
            {
                //ĳ���� ���� �Ұ�
                //penalty.IDontKnowWhoYouAre(true);
            }
            if (jobCode == 109)
            {
                //�ݶ��̴� �Ͻ�����(�� �� ĳ���Ͱ� ������ ����)
                GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            speed = 1;
            if (jobCode == 106)
            {
                //ĳ���� ���� �Ұ�
                //penalty.IDontKnowWhoYouAre(false);
            }
            if (jobCode == 109)
            {
                //�ݶ��̴� ���(�� �� ĳ���Ͱ� ������ ���)
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}