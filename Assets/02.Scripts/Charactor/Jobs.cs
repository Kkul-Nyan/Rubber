using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jobs
{
    private CommonCharacter player;
    private CommonCharacter target;
    private GameObject target_Gimmick;
    private int canUse;
    private int canAttack;

    public CommonCharacter Player { get => player; set => player = value; }
    public CommonCharacter Target { get => target; set => target = value; }
    public GameObject Target_Gimmick { get => target_Gimmick; set => target_Gimmick = value; }
    public int CanUse { get => canUse; set => canUse = value; }
    public int CanAttack { get => canAttack; set => canAttack = value; }

    //code 101
    public void Sherrif_Do()
    {
        //��ǥ �÷��̾ ����
        canUse = player.skill.MeleeAttack(target, canUse);

        //���� �÷��̾ ���������� ����
        //�������̸� ���Ȱ� ����
        if (target.jobCode < 200)
        {
            player.penalty.Dead(player);
        }
    }

    //code 102
    public void Doctor_Set()
    {
        //������ ���̵�� ���� ȥ�� ���Ƽ(����? ���̵� �����? ���� ����?)
        player.penalty.IDontKnowWhoYouAre(true);
    }

    public void Doctor_Do()
    {
        //��ǥ �÷��̾��� ���� ����(����->����)
        //���� ���� ������ ��� �Ͻ�����(�����ϱ���)
        canUse = player.skill.Heal(target, canUse);
    }

    //code 103
    public void Shaman_Do()
    {
        //ž��, ��ü��ġȮ��
        canUse = player.skill.TopView(canUse);
    }

    //code 104
    public void GoodNose_Do()
    {
        //���� ��ü ��ó���� ���� ����� �̵����� Ȯ��
        player.skill.DogNose();
    }

    //code 105
    public void Betlayer_Set()
    {
        //���۽� ���Ǿ� ����Ʈ(���ǾƳ��� Ȯ�� ����)�� ����.
        player.skill.ImYourAlly(player);
    }

    //code 106
    public void Marathoner_Set()
    {
        //���¹̳� ���� 1.5��� ����.
        player.stamina *= 1.5f;
    }
    
    //code 107
    public void Detective_Do()
    {
        //�� ���� ȸ���߿� ���� �Ѹ��� ���������� �� �� ����
        if (player.isDiscuss && canUse != 0)
        {
            canUse = player.skill.TellMeYourJob(target, canUse);
            canUse--;
        }
    }

    //code 108
    public void Mechanic_Set()
    {
        //�������� ���� ���гʸ� �ϳ� ǥ���Ѵ�.
        player.penalty.IamEngineer();
    }

    public void Mechanic_Do()
    {
        //����� �ı��Ǿ������� ����, �ı����� �ʾ����� ��ȣ������
        player.skill.ImGenius(target_Gimmick, CanUse);
    }

    //code 109
    public void Wing_Set()
    {
        //���¹̳� �� 1/2��
        player.stamina *= 0.5f;
    }

    //code 110
    public void SpeedRacer_Do()
    {
        //1���� �̵��ӵ��� 1.5��(�ȴ¼ӵ� 1.5�� �ٴ¼ӵ�3��(1.5*2��))
        //1���Ӵ� 1ȸ ��밡����
        if (canUse != 0)
        {
            //�̵��ӵ� 1.5��
            player.speed *= 1.5f;
            canUse--;
        }
    }


    //code 201
    public void Blind_Set()
    {
        //�÷����� ���Ƽ(�������� �ش����� ����?)
        player.penalty.ICantSee(true);
    }

    public void Blind_Do()
    {
        //���� �� ����ȭ
        player.skill.Clocking(canUse);
        //���� �÷� ����ȭ
        player.penalty.ICantSee(false);
    }

    public void Blind_Attack()
    {
        //�������� ����(������)
        player.skill.MeleeAttack(player, -1);
    }


    //code 202
    public void Cowboy_Attack()
    {
        //���Ÿ� ���� ����
        //�ϴ� 1�� ����
        canAttack = player.skill.Magnum(target, canAttack);
        //���� �ѼҸ� ���� �鸲
        player.penalty.GunSound();
        //�ڵ�ȸ�� ����?
        //�ٰŸ� �÷��̾� ��ġ�ľ�?
    }

    //code 203
    public void NeedleSpitter_Attack()
    {
        //�߰Ÿ� ���ݰ���
        //�ϴ� 1�� ����
        canAttack = player.skill.NeedleGun(target, canAttack);
    }

    //code 204
    public void Spy_Do()
    {
        //������ ������ Ȯ��
        player.skill.ShowMeYourItem(target);
    }

    public void Spy_Attack()
    {
        //�������� ����(������)
        player.skill.MeleeAttack(target, -1);
    }


    //code 205
    public void Bully_Do()
    {
        //������ ä�ñ���
        if (player.isDiscuss)
        {
            canUse = player.skill.ShutUp(target, canUse);
        }
    }

    public void Bully_Attack()
    {
        //�������� ����(������)
        player.skill.MeleeAttack(target, -1);
    }

    //code 206
    public void Disguiser_Set()
    {
        //�ǾƱ��� ����
        player.penalty.IDontKnowWhoYouAre(true);
    }

    public void Disguiser_Do()
    {
        //������ ������
        canUse = player.skill.Metamolphosis(player, target, canUse);
    }

    public void Disguiser_Attack()
    {
        //�������� ����(������)
        player.skill.MeleeAttack(target, -1);
    }

    //code 207
    public void Bomber_Attack()
    {
        //��ź�� ���������� ��ġ
        player.skill.PresentBomb(target);
    }

    //code 301
    public void DeadDuck_Set()
    {
        //�ݶ��̴� ����(�� �� ĳ���Ͱ� ������ ����)
        player.GetComponent<Collider>().enabled = false;
        //ä�� ����(����� ä�� ����) ���� �� �� ä��(�� ���� ä�� ����) ���� ä��

        //���̾� ����("DeadDuck") -> (feat ������ ���� �ʿ�)
        player.gameObject.layer = LayerMask.NameToLayer("DeadDuck");
        //ī�޶� �������� ���̾�(�����) �߰� -> (feat ������ ���� �ʿ�)
        player.playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("DeadDuck");
        //isComa Ȱ��ȭ(��ų �� �޸���, Attack ����)
        player.isComa = true;
        //
    }
}
