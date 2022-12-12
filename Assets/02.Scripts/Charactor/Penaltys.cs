using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penaltys
{
    public void Coma(CommonCharacter target)
    {
        //이동 및 행동 제약(캐릭터 쓰러짐)
        target.isComa = true;
        target.speed = 0;
    }

    public void Dead(CommonCharacter target)
    {
        //DeadDuck으로 잡체인지
        target.JobChange(301);
    }

    public void CantVote(CommonCharacter player)
    {
        //투표시간에 투표가 무조건 기권으로 자동투표
        player.cantVote = true;
    }

    public void IDontKnowWhoYouAre(bool onOff)
    {
        //다른 플레이어 구분 안됨(모든 플레이어색이 노란색?으로)

    }

    public void ICantSee(bool onOff)
    {
        //장님 디버프 -> 카메라 세팅
    }

    public void GunSound()
    {
        //큰소리(모든 맵 영역에서 들림, 즉시 회의 소집)

        //근거리에 있던 오리들에겐 대략적인 근원지 표시(맵)
    }

    public void IamEngineer()
    {
        //몸에 스페너 하나를 달고 다님
    }
}
