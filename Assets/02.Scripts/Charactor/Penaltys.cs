using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penaltys
{
    public void Coma(CommonCharacter target)
    {
        //�̵� �� �ൿ ����(ĳ���� ������)
        target.isComa = true;
        target.speed = 0;
    }

    public void Dead(CommonCharacter target)
    {
        //DeadDuck���� ��ü����
        target.JobChange(301);
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
