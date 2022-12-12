using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jobs
{
    private CommonCharacter target;
    private GameObject target_Gimmick;
    private int canUse;

    public CommonCharacter Target { get => target; set => target = value; }
    public GameObject Target_Gimmick { get => target_Gimmick; set => target_Gimmick = value; }
    public int CanUse { get => canUse; set => canUse = value; }

    //code 101
    public void Sherrif_Do(Skills skills, Penaltys penalty, CommonCharacter player)
    {
        //��ǥ �÷��̾ ����
        skills.MeleeAttack(Target, CanUse);

        //���� �÷��̾ ���������� ����
        //�������̸� ���Ȱ� ����
        if (Target.jobCode < 200)
        {
            penalty.Dead(player);
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
    public void DeadDuck_Set(CommonCharacter player)
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
