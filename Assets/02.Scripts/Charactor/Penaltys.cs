using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penaltys
{
    public void Coma(CommonCharacter player)
    {
        //�̵� �� �ൿ ����(ĳ���� ������)
        player.isComa = true;
        player.speed = 0;
        //���� ���Ѹ� ���� 10�� �� Dead
        float time = 0;

        while (!player.isHeal)
        {
            time += Time.deltaTime;
            if (time >= 10)
            {
                Dead(player);
                break;
            }
        }
    }

    public void Dead(CommonCharacter player)
    {
        //DeadDuck���� ��ü����
        player.JobChange(301);
    }

    public void CantVote(CommonCharacter player)
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

    public void Bombed(CommonCharacter player)
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
