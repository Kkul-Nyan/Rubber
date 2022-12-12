using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{
    //���ݽ�ų
    public void MeleeAttack(CommonCharacter player, int canUse)
    {
        //�ٰŸ� ����
        //Į Ȱ��ȭ(Į���� colider�� ����) -> ��ư Ŭ���ÿ��� Ȱ��ȭ
        //
        //Į colider�� ���� ��ü�� ����
        //���� �� 10��? �ȿ� �� ���� �� ����(301(�����) ����)
    }

    public void NeedleGun(CommonCharacter player, int canUse)
    {
        //�߰Ÿ� ����
        //��ų �ߵ��� ray(��Ʈ�ѷ�)�� ���� ��ü�� ����� ���� -> 10�� �� ����
        //���� �� 10��? �ȿ� �� ���� �� ����(301(�����) ����)
    }

    public void Magnum(CommonCharacter player, int canUse)
    {
        //��Ÿ� ����
        //�Ѿ� Ȱ��ȭ(�Ѿ˿��� colider�� ����) -> ��ư Ŭ���� ������ �߻�
        //��ü �浹�� destroy
    }

    public void PresentBomb(CommonCharacter player)
    {
        //Ÿ�� ���������� ���Ƽ Bombed�ο�
        player.isBombed = true;
        player.penalty.Bombed(player);
    }

    //ȸ����ų
    public void Heal(CommonCharacter player, int canUse)
    {
        //���� ���� �츲(��1ȸ) ->
        //�߰Ÿ� ���� ���� �Ҹ���� ġ��
    }

    //�������ͽ� ��ȭ
    public void Energizer(ref float stamina)
    {
        //���¹̳� �ѷ� ����(1.5������)
        stamina *= 1.5f;
    }

    public void Adrenaline(ref float speed, int canUse)
    {
        if (canUse > 0)
        {
            //�̵��ӵ� 1.5��
            speed *= 1.5f;
        }
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

    public void Clocking(int canUse)
    {
        //�����ð�(10��?)���� ������ ���İ�(����) 0 or �Ⱥ��̴� ���̾�� ��ȯ

    }


    //���� ����or�Լ�
    public void ImYourAlly()
    {
        //���۽� ������(���Ǿ�)����Ʈ�� �ö� -> ���������� �Ʊ����� ����
    }

    public void TellMeYourJob(CommonCharacter player, int canUse)
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
        string showText = "ERROR!";
        if (jobName[player.jobCode] is not null)
        {
            showText = jobName[player.jobCode];
        }
        

    }

    public void ShowMeYourItem(CommonCharacter player)
    {
        //����� ������ �ִ� ������(���)�� �� �� ����
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

    public void ShutUp(CommonCharacter player)
    {
        //��ǥ�߿� �Ѹ��� ä�ݽ�ų�� ����
    }

    public void Metamolphosis(CommonCharacter target)
    {
        //Ÿ�� �������� ������ �״�� ������
        Mesh mesh = target.meshFilter.mesh;
        //�� �ϸ��� �ѹ� ��ü����
    }
}
