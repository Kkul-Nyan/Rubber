using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using Unity.Services.Lobbies.Models;

public class RubberDuck : MonoBehaviour 
{
    public float speed = 1;
    public float stamina = 10;
    public Animator animator;
    public int jobCode;
    public Jobs jobs = new Jobs();
    public Skills skills = new Skills();
    public Penaltys penalty = new Penaltys();
    public bool isComa = false;
    public bool isHeal = false;
    public bool isBombed = false;

    public bool cantVote = false;


    public Dictionary<int, string> jobName;

    MeshFilter meshFilter;
    public Mesh [] bluemesh;
    
    public void ChangeColor(int selectColor)
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = bluemesh[selectColor];
    }

    public void DeadTimer(float time, string method)
    {

        
        while (true)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                
                return;
            }
        }
        
    }
    public void Dead()
    {
        //DeadDuck���� ��ü����
    }

    public Dictionary<int, string> jobSetting()
    {
        jobName = new Dictionary<int, string>();
        jobName[101] = "Sherrif";
        jobName[102] = "Doctor";
        jobName[103] = "Shaman";
        jobName[104] = "GoodNose";
        jobName[105] = "Betlayer";
        jobName[106] = "Marathoner";
        jobName[107] = "Detective";
        jobName[108] = "Mechanic";
        jobName[109] = "Wing";
        jobName[110] = "SpeedRacer";

        jobName[201] = "Blind";
        jobName[202] = "Cowboy";
        jobName[203] = "NeedleSpitter";
        jobName[204] = "Spy";
        jobName[205] = "Bully";
        jobName[206] = "Disguiser";
        jobName[207] = "Bomber";
        
        jobName[301] = "DeadDuck";

        return jobName;
    }

    public void JobsDo()
    {
        if(isComa)
        {
            return;
        }

        switch (jobCode)
        {
            //����������
            case 101:
                jobs.Sherrif_Do(skills, penalty);
                break;
            case 102:
                jobs.Doctor_Do(skills, penalty);
                break;
            case 103:
                jobs.Shaman_Do(skills, penalty);
                break;
            case 104:
                jobs.GoodNose_Do(skills, penalty);
                break;
            case 105:
                break;
            case 106:
                jobs.Marathoner_Do(skills, penalty);
                break;
            case 107:
                jobs.Detective_Do(skills, penalty);
                break;
            case 108:
                jobs.Mechanic_Do(skills, penalty);
                break;
            case 109:
                jobs.Wing_Do(skills, penalty, ref stamina);
                break;
            case 110:
                jobs.SpeedRacer_Do(skills, penalty, ref speed);
                break;

            //��ü������
            case 201:
                jobs.Blind_Do(skills, penalty);
                break;
            case 202:
                break;
            case 203:
                break;
            case 204:
                jobs.Spy_Do(skills, penalty);
                break;
            case 205:
                jobs.Bully_Do(skills, penalty);
                break;
            case 206:
                jobs.Disguiser_Do(skills, penalty);
                break;
            case 207:
                break;
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
            //��ü������
            case 201:
                jobs.Blind_Attack(skills, penalty);
                break;
            case 202:
                jobs.Cowboy_Attack(skills, penalty);
                break;
            case 203:
                jobs.NeedleSpitter_Attack(skills, penalty);
                break;
            case 204:
                jobs.Spy_Attack(skills, penalty);
                break;
            case 205:
                jobs.Bully_Attack(skills, penalty);
                break;
            case 206:
                jobs.Disguiser_Attack(skills, penalty);
                break;
            case 207:
                jobs.Bomber_Attack(skills, penalty);
                break;
            default:
                break;
        }
    }
}

public class Jobs
{
    private RubberDuck target;
    private GameObject target_Gimmick;
    private int canUse;

    public RubberDuck Target { get => target; set => target = value; }
    public GameObject Target_Gimmick { get => target_Gimmick; set => target_Gimmick = value; }
    public int CanUse { get => canUse; set => canUse = value; }

    //code 101
    public void Sherrif_Do(Skills skills, Penaltys penalty)
    {
        //��ǥ �÷��̾ ����
        skills.MeleeAttack(Target, CanUse);

        //���� �÷��̾ ���������� ����
        //�������̸� ���Ȱ� ����
        if (Target.jobCode < 200)
        {
            //penalty.Dead();
        }
    }

    //code 102
    public void Doctor_Set(Penaltys penalty)
    {
        //������ ���̵�� ���� ȥ�� ���Ƽ(����? ���̵� �����? ���� ����?)
        penalty.IDontKnowWhoYouAre(true);
    }

    public void Doctor_Do(Skills skills, Penaltys penalty)
    {
        //��ǥ �÷��̾��� ���� ����(����->����)
        //���� ���� ������ ��� �Ͻ�����(�����ϱ���)
        skills.Heal(Target, CanUse);
    }

    //code 103
    public void Shaman_Do(Skills skills, Penaltys penalty)
    {
        //ž��, ��ü��ġȮ��
        //skills.TopView(CanUse);
    }

    //code 104
    public void GoodNose_Do(Skills skills, Penaltys penalty)
    {
        //���� ��ü ��ó���� ���� ����� �̵����� Ȯ��
        //skills.DogNose();
    }

    //code 105
    public void Betlayer_Set(Skills skills, Penaltys penalty)
    {
        //���۽� ���Ǿ� ����Ʈ(���ǾƳ��� Ȯ�� ����)�� ����.
        //skills.ImYourAlly();
    }

    //code 106
    public void Marathoner_Set(Skills skills, Penaltys penalty, ref float stamina)
    {
        //���¹̳� ���� 1.5��� ����.
        //stamina *= 1.5f;
    }

    public void Marathoner_Do(Skills skills, Penaltys penalty)
    {
        //�޸��� �����߿� �ǾƱ��� �ȵ�
        /*
        if (isRunning)
        {
            penalty.IDontKnowWhoYouAre(true);
        }
        else
        {
            penalty.IDontKnowWhoYouAre(false);
        }
        */
    }

    //code 107
    public void Detective_Do(Skills skills, Penaltys penalty)
    {
        //�� ���� ȸ���߿� ���� �Ѹ��� ���������� �� �� ����
        /*
            if (isDiscussing)
            {
                skills.TellMeYourJob(Target, CanUse);
            }
        */
    }

    //code 108
    public void Mechanic_Set(Skills skills, Penaltys penalty)
    {
        //�������� ���� ���гʸ� �ϳ� ǥ���Ѵ�.
        //penalty.IamEngineer();
    }

    public void Mechanic_Do(Skills skills, Penaltys penalty)
    {
        //����� �ı��Ǿ������� ����, �ı����� �ʾ����� ��ȣ������
        //skills.ImGenius(Target_Gimmick, CanUse);
    }

    //code 109
    public void Wing_Set(Skills skills, Penaltys penalty, ref float stamina)
    {
        //���¹̳� �� 1/2��
        //stamina *= 0.5f;
    }

    public void Wing_Do(Skills skills, Penaltys penalty, ref float stamina)
    {
        //�޸����� ��ֹ��� ���ظ� ��������
        //skills.ICanFly(stamina);
    }

    //code 110
    public void SpeedRacer_Do(Skills skills, Penaltys penalty, ref float speed)
    {
        //1���� �̵��ӵ��� 1.5��(�ȴ¼ӵ� 1.5�� �ٴ¼ӵ�3��(1.5*2��))
        //1���Ӵ� 1ȸ ��밡����
        //skills.Adrenaline(ref speed, CanUse);
    }


    //code 201
    public void Blind_Set(Penaltys penalty)
    {
        //�÷����� ���Ƽ(�������� �ش����� ����?)
        penalty.ICantSee(true);
    }

    public void Blind_Do(Skills skills, Penaltys penalty)
    {
        //���� �� ����ȭ
        skills.Clocking(CanUse);
        //���� �÷� ����ȭ
        penalty.ICantSee(false);
    }

    public void Blind_Attack(Skills skills, Penaltys penalty)
    {
        //�������� ����(������)
        skills.MeleeAttack(Target, -1);
    }


    //code 202
    public void Cowboy_Attack(Skills skills, Penaltys penalty)
    {
        //���Ÿ� ���� ����
        //�ϴ� 1�� ����
        skills.Magnum(Target, CanUse);
        //���� �ѼҸ� ���� �鸲
        penalty.GunSound();
        //�ڵ�ȸ�� ����?
        //�ٰŸ� �÷��̾� ��ġ�ľ�?
    }

    //code 203
    public void NeedleSpitter_Attack(Skills skills, Penaltys penalty)
    {
        //�߰Ÿ� ���ݰ���
        //�ϴ� 1�� ����
        skills.NeedleGun(Target, CanUse);
    }

    //code 204
    public void Spy_Do(Skills skills, Penaltys penalty)
    {
        //������ ������ Ȯ��
        skills.ShowMeYourItem(Target);
    }

    public void Spy_Attack(Skills skills, Penaltys penalty)
    {
        //�������� ����(������)
        skills.MeleeAttack(Target, -1);
    }


    //code 205
    public void Bully_Do(Skills skills, Penaltys penalty)
    {
        //������ ä�ñ���
        skills.ShutUp(Target);
    }

    public void Bully_Attack(Skills skills, Penaltys penalty)
    {
        //�������� ����(������)
        skills.MeleeAttack(Target, -1);
    }

    //code 206
    public void Disguiser_Set(Skills skills, Penaltys penalty)
    {
        //�ǾƱ��� ����
        penalty.IDontKnowWhoYouAre(true);
    }

    public void Disguiser_Do(Skills skills, Penaltys penalty)
    {
        //������ ������
        skills.Metamolphosis(Target);
    }

    public void Disguiser_Attack(Skills skills, Penaltys penalty)
    {
        //�������� ����(������)
        skills.MeleeAttack(Target, -1);
    }

    //code 207
    public void Bomber_Attack(Skills skills, Penaltys penalty)
    {
        //��ź�� ���������� ��ġ
        skills.PresentBomb(Target);
    }

    //code 301
    public void DeadDuck_Set(RubberDuck player)
    {
        //�ݶ��̴� ����(�� �� ĳ���Ͱ� ������ ����)

        //ä�� ����(����� ä�� ����) ���� �� �� ä��(�� ���� ä�� ����) ���� ä��

        //���̾� ����(�����) -> (feat ������ ���� �ʿ�)

        //ī�޶� �������� ���̾�(�����) �߰� -> (feat ������ ���� �ʿ�)

        //isComa Ȱ��ȭ(��ų �� �޸���, Attack ����)
        player.isComa = true;
        //
    }
}

public class Skills
{
    public void MeleeAttack(RubberDuck player, int canUse)
    {
        //�ٰŸ� ����
        //Į Ȱ��ȭ(Į���� colider�� ����) -> ��ư Ŭ���ÿ��� Ȱ��ȭ
        //Į colider�� ���� ��ü�� ����
        //���� �� 10��? �ȿ� �� ���� �� ����(301(�����) ����)
    }

    public void NeedleGun(RubberDuck player, int canUse)
    {
        //�߰Ÿ� ����
        //��ų �ߵ��� ray(��Ʈ�ѷ�)�� ���� ��ü�� ����� ���� -> 10�� �� ����
        //���� �� 10��? �ȿ� �� ���� �� ����(301(�����) ����)
    }

    public void Magnum(RubberDuck player, int canUse)
    {
        //��Ÿ� ����
        //�Ѿ� Ȱ��ȭ(�Ѿ˿��� colider�� ����) -> ��ư Ŭ���� ������ �߻�
        //��ü �浹�� destroy
    }

    public void Heal(RubberDuck player, int canUse)
    {
        //���� ���� �츲(��1ȸ) ->
        //�߰Ÿ� ���� ���� �Ҹ���� ġ��
    }

    public void Run(float stamina)
    {
        //���¹̳� �Ҹ��ϸ� �����̵�(�ȴ� �ӵ� 2��)
    }

    public void TopView(int canUse)
    {
        //ž��������� �� Ȯ�� ����, �� �Ͽ� ����� ��ü ��ġ Ȯ��
    }

    public void DogNose()
    {
        //����� �̵����� ����(��ó3���?)
        //����ڰ� �������� ���̸� ���������� ����� �������� ȭ��ǥ ����(LookAt�Լ��̿�, 10���� ��ü Destroy)
        //�� �޼���� �� ȭ��ǥ�� �� �� �ְ� ������ִ� �޼���
    }

    public void ImYourAlly()
    {
        //���۽� ������(���Ǿ�)����Ʈ�� �ö� -> ���������� �Ʊ����� ����
    }

    public void Energizer(ref float stamina)
    {
        //���¹̳� �ѷ� ����(1.5������)
        stamina *= 1.5f;
    }

    public void TellMeYourJob(RubberDuck player, int canUse)
    {
        //��ǥ�� ������ ������ �������� �˼�����
        /*
        if (canUse > 0)
        {
            string showText = jobName[player.jobCode];
        }
        */
    }

    public void ImGenius(GameObject gimmick, int canUse)
    {
        /*
        if (gimmick.CompareTag("Gimmick"))
        {
            if (gimmick.destroyed)
            {
                //�ı��� ��� ����
                gimmick.destroyed = false;
            }
            else
            {
                //�ı���ȣ��(�� �Ͽ� ����)
                gimmick.cantDestroy = true;
            }
        }
        */
        //����� �̼ǽð� 50%����(�߿䵵 B)
    }

    public void ICanFly(float stamina)
    {
        if (stamina > 0)
        {
            //���¹̳� �����ñ��� �ݶ��̴� ����
        }
        else
        {
            //���ݶ��̴� �ӿ��� ���¹̳� ������ DeadDuck����
        }
    }

    public void Adrenaline(ref float speed, int canUse)
    {
        if (canUse > 0)
        {
            //�̵��ӵ� 1.5��
            speed *= 1.5f;
        }
    }

    public void Clocking(int canUse)
    {
        //�����ð�(10��?)���� ������ ���İ�(������) 0 or �Ⱥ��̴� ���̾�� ��ȯ

    }

    public void ShowMeYourItem(RubberDuck player)
    {
        //����� ������ �ִ� ������(���)�� �� �� ����
    }

    public void ShutUp(RubberDuck player)
    {
        //��ǥ�߿� �Ѹ��� ä�ݽ�ų�� ����
    }

    public void Metamolphosis(RubberDuck player)
    {
        //Ÿ�� �������� ������ �״�� ������
        //�� �ϸ��� �ѹ� ��ü����
    }

    public void PresentBomb(RubberDuck player)
    {
        //Ÿ�� ���������� ���Ƽ Bombed�ο�
        player.isBombed = true;
        player.penalty.Bombed(player);
    }
}

public class Penaltys
{
    public void Coma(RubberDuck player)
    {
        //�̵� �� �ൿ ����(ĳ���� ������)
        player.isComa = true;
        player.speed = 0;
        //���� ���Ѹ� ���� 10�� �� Dead
        float time = 0;

        while (!player.isHeal)
        {
            time += Time.deltaTime;
            if(time >= 10 )
            {
                Dead(player);
                break;
            }
        }
    }

    public void Dead(RubberDuck player)
    {
        //DeadDuck���� ��ü����
        player.jobCode = 301;
        //DeadDuck Setting
        player.jobs.DeadDuck_Set(player);
    }

    public void CantVote(RubberDuck player)
    {
        //��ǥ�ð��� ��ǥ�� ������ ������� �ڵ���ǥ
        player.cantVote = true;
    }

    public void IDontKnowWhoYouAre(bool onOff)
    {
        //�ٸ� �÷��̾� ���� �ȵ�(��� �÷��̾���� �����?����)

    }

    public void ICantSee(bool onOff)
    {
        //��� ����� -> ī�޶� ����
    }

    public void Bombed(RubberDuck player)
    {
        //��ź Ÿ�̸Ӱ� �ٵɶ� �� ������� ���� �÷��̾�� ���
        float time = 0;

        while (player.isBombed)
        {
            time += Time.deltaTime;
            if (time >= 10)
            {
                Coma(player);
                break;
            }
        }
    }

    public void GunSound()
    {
        //ū�Ҹ�(��� �� �������� �鸲, ��� ȸ�� ����)

        //�ٰŸ��� �ִ� �����鿡�� �뷫���� �ٿ��� ǥ��(��)
    }

    public void IamEngineer()
    {
        //���� ����� �ϳ��� �ް� �ٴ�
    }
}

public class CommonCharacter : MonoBehaviour
{

    public RubberDuck players;
    public Jobs jobs = new Jobs();
    public int jobcode;
    public Skills skill;
    public Penaltys penalty;

    private void Start()
    {
        players = new RubberDuck();

        jobLoad(jobcode);
    }

    private void Update()
    {

    }

    public void jobLoad(int jobCode)
    {
        players.jobCode = jobCode;

        switch (jobCode)
        {
            //����������
            case 101:
                break;
            case 102:
                jobs.Doctor_Set(penalty);
                break;
            case 103:
                break;
            case 104:
                break;
            case 105:
                jobs.Betlayer_Set(skill, penalty);
                break;
            case 106:
                jobs.Marathoner_Set(skill, penalty, ref players.stamina);
                break;
            case 107:
                break;
            case 108:
                jobs.Mechanic_Set(skill, penalty);
                break;
            case 109:
                jobs.Wing_Set(skill, penalty, ref players.stamina);
                break;
            case 110:
                break;
            //��ü������
            case 201:
                jobs.Blind_Set(penalty);
                break;
            case 202:
                break;
            case 203:
                break;
            case 204:
                break;
            case 205:
                break;
            case 206:
                jobs.Disguiser_Set(skill, penalty);
                break;
            case 207:
                break;
        }
    }
}