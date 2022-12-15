using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{
    string showText;

    //���ݽ�ų
    public int MeleeAttack(CommonCharacter player, int canUse)
    {
        //�ٰŸ� ����
        //Į Ȱ��ȭ(Į���� colider�� ����) -> ��ư Ŭ���ÿ��� Ȱ��ȭ
        if (canUse != 0)
        {
            player.knife.SetActive(true);
            canUse--;
        }
        //Į colider�� ���� ��ü�� ����
        //���� �� 10��? �ȿ� �� ���� �� ����(301(�����) ����)
        return canUse;
    }

    public int NeedleGun(CommonCharacter target, int canUse)
    {
        //�߰Ÿ� ����
        //��ų �ߵ��� ray(��Ʈ�ѷ�)�� ���� ��ü�� ����� ���� -> 10�� �� ����
        //���� �� 10��? �ȿ� �� ���� �� ����(301(�����) ����)
        canUse--;
        return canUse;
    }

    public int Magnum(CommonCharacter player, int canAttack)
    {
        //��Ÿ� ����
        if (canAttack != 0)
        {
            //�Ѿ� Ȱ��ȭ(�Ѿ˿��� colider�� ����) -> ��ư Ŭ���� ������ �߻�

            //��ü �浹�� destroy

            canAttack--;
        }

        
        return canAttack;
    }

    public void PresentBomb(CommonCharacter target)
    {
        //Ÿ�� ���������� ���Ƽ Bombed�ο�
        target.isBombed = true;
    }

    public bool PresentBomb(CommonCharacter target, float timeLeft)
    {
        //Ÿ�� ���������� ���Ƽ Bombed�ο�
        if (target is not null)
        {
            target.isBombed = true;
            target.bombedTimer = timeLeft;
            return false;
        }
        return true;
    }

    //ȸ����ų
    public int Heal(CommonCharacter target, int canUse)
    {
        //���� ���� �츲(��1ȸ) ->
        if (canUse > 0 && target.isComa == true)
        {
            target.isComa = false;
            target.comaTimer = 10f;
            canUse--;
        }
        //�߰Ÿ� ���� ���� �Ҹ���� ġ��
        else if (target.isWounded == true)
        {
            target.isWounded = false;
            target.woundTimer = 10f;
        }


        return canUse;
    }

    //�������ͽ� ��ȭ
    public void Energizer(ref float stamina)
    {
        //���¹̳� �ѷ� ����(1.5������)
        stamina *= 1.5f;
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

    //ī�޶� ��ȭ
    public int TopView(int canUse)
    {
        //ž��������� �� Ȯ�� ����, �� �Ͽ� ����� ��ü ��ġ Ȯ��
        canUse--;
        return canUse;
    }

    public void DogNose()
    {
        //����� �̵����� ����(��ó3���?)
        //����ڰ� �������� ���̸� ���������� ����� �������� ȭ��ǥ ����(LookAt�Լ��̿�, 10���� ��ü Destroy)
        //�� �޼���� �� ȭ��ǥ�� �� �� �ְ� ������ִ� �޼���
    }

    public void Clocking(int canUse)
    {
        //�����ð�(10��?)���� ������ ���İ�(����) 0 or �Ⱥ��̴� ���̾�� ��ȯ

    }


    //���� ����or�Լ�
    public void ImYourAlly(CommonCharacter player)
    {
        //���۽� ������(���Ǿ�)����Ʈ�� �ö� -> ���������� �Ʊ����� ����
        player.iAmBetlayer = true;
    }

    public int TellMeYourJob(CommonCharacter target, int canUse)
    {
        //��ǥ�� ������ ������ �������� �˼�����
        //�ڵ庰 ������ ����
        Dictionary<int, string> jobName = new Dictionary<int, string>();
        jobName[100] = "RubberDuck";
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

        jobName[200] = "Duck";
        jobName[201] = "Blind";
        jobName[202] = "Cowboy";
        jobName[203] = "NeedleSpitter";
        jobName[204] = "Spy";
        jobName[205] = "Bully";
        jobName[206] = "Disguiser";
        jobName[207] = "Bomber";

        jobName[301] = "DeadDuck";

        //ǥ�õ� �������� ǥ�ÿ� ������ �Է�
        showText = "ERROR!";
        if (jobName[target.jobCode] is not null && canUse != 0)
        {
            showText = jobName[target.jobCode];
            canUse--;
        }
        //�޽����ڽ��� ���� �̸�(showText) ���

        //��� �� ���� �ʱ�ȭ
        showText = "";
        return canUse;
    }

    public void ShowMeYourItem(CommonCharacter target)
    {
        //����� ������ �ִ� ������(���)�� �� �� ����
        showText = "Nothing";
        if (target.haveTape)
        {
            showText = "Tape";
        }
        if (target.haveItem)
        {
            showText = "Item";
        }
        //�޽����ڽ��� ������ ���� ������(showText) ���

        //��� �� ���� �ʱ�ȭ
        showText = "";
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

    public int ShutUp(CommonCharacter target, int canUse)
    {
        if (canUse != 0)
        {
            //��ǥ�߿� �Ѹ��� ä�ݽ�ų�� ����
            target.isSilence = true;
            canUse--;
        }
        return canUse;
    }

    public int Metamolphosis(CommonCharacter player, CommonCharacter target, int canUse)
    {
        
        //�� �ϸ��� �ѹ� ��ü����
        if (canUse != 0)
        {
            //Ÿ�� �������� ������ �״�� ������
            Mesh mesh = target.meshFilter.mesh;
            player.meshFilter.mesh = mesh;
            canUse--;
        }

        return canUse;
        

    }
}
