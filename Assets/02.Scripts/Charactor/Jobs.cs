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
        //목표 플레이어를 죽임
        skills.MeleeAttack(Target, CanUse);

        //죽인 플레이어가 러버덕인지 판정
        //러버덕이면 보안관 죽음
        if (Target.jobCode < 200)
        {
            penalty.Dead(player);
        }
    }

    //code 102
    public void Doctor_Set(Penaltys penalty)
    {
        //상대방의 아이디와 색상 혼란 페널티(랜덤? 아이디 지우기? 색상 통일?)
        penalty.IDontKnowWhoYouAre(true);
    }

    public void Doctor_Do(Skills skills, Penaltys penalty)
    {
        //목표 플레이어의 상태 수정(죽음->보통)
        //상태 수정 성공시 기능 일시정지(다음턴까지)
        skills.Heal(Target, CanUse);
    }

    //code 103
    public void Shaman_Do(Skills skills, Penaltys penalty)
    {
        //탑뷰, 시체위치확인
        //skills.TopView(CanUse);
    }

    //code 104
    public void GoodNose_Do(Skills skills, Penaltys penalty)
    {
        //죽은 시체 근처에서 사용시 살덕마 이동방향 확인
        //skills.DogNose();
    }

    //code 105
    public void Betlayer_Set(Skills skills, Penaltys penalty)
    {
        //시작시 마피아 리스트(마피아끼리 확인 가능)에 포함.
        //skills.ImYourAlly();
    }

    //code 106
    public void Marathoner_Set(Skills skills, Penaltys penalty, ref float stamina)
    {
        //스태미나 양을 1.5배로 만듬.
        //stamina *= 1.5f;
    }

    public void Marathoner_Do(Skills skills, Penaltys penalty)
    {
        //달리기 시전중에 피아구분 안됨
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
        //그 턴의 회의중에 사용시 한명의 직업정보를 알 수 있음
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
        //러버덕의 몸에 스패너를 하나 표시한다.
        //penalty.IamEngineer();
    }

    public void Mechanic_Do(Skills skills, Penaltys penalty)
    {
        //기믹이 파괴되어있을시 복구, 파괴되지 않았을때 보호막생성
        //skills.ImGenius(Target_Gimmick, CanUse);
    }

    //code 109
    public void Wing_Set(Skills skills, Penaltys penalty, ref float stamina)
    {
        //스태미나 양 1/2배
        //stamina *= 0.5f;
    }

    public void Wing_Do(Skills skills, Penaltys penalty, ref float stamina)
    {
        //달리는중 장애물의 방해를 받지않음
        //skills.ICanFly(stamina);
    }

    //code 110
    public void SpeedRacer_Do(Skills skills, Penaltys penalty, ref float speed)
    {
        //1턴중 이동속도가 1.5배(걷는속도 1.5배 뛰는속도3배(1.5*2배))
        //1게임당 1회 사용가능함
        //skills.Adrenaline(ref speed, CanUse);
    }


    //code 201
    public void Blind_Set(Penaltys penalty)
    {
        //시력저하 페널티(색상대비의 극단적인 저하?)
        penalty.ICantSee(true);
    }

    public void Blind_Do(Skills skills, Penaltys penalty)
    {
        //사용시 몸 투명화
        skills.Clocking(CanUse);
        //사용시 시력 정상화
        penalty.ICantSee(false);
    }

    public void Blind_Attack(Skills skills, Penaltys penalty)
    {
        //근접공격 가능(무제한)
        skills.MeleeAttack(Target, -1);
    }


    //code 202
    public void Cowboy_Attack(Skills skills, Penaltys penalty)
    {
        //원거리 공격 가능
        //턴당 1번 가능
        skills.Magnum(Target, CanUse);
        //전맵 총소리 사운드 들림
        penalty.GunSound();
        //자동회의 소집?
        //근거리 플레이어 위치파악?
    }

    //code 203
    public void NeedleSpitter_Attack(Skills skills, Penaltys penalty)
    {
        //중거리 공격가능
        //턴당 1번 가능
        skills.NeedleGun(Target, CanUse);
    }

    //code 204
    public void Spy_Do(Skills skills, Penaltys penalty)
    {
        //러버덕 아이템 확인
        skills.ShowMeYourItem(Target);
    }

    public void Spy_Attack(Skills skills, Penaltys penalty)
    {
        //근접공격 가능(무제한)
        skills.MeleeAttack(Target, -1);
    }


    //code 205
    public void Bully_Do(Skills skills, Penaltys penalty)
    {
        //러버덕 채팅금지
        skills.ShutUp(Target);
    }

    public void Bully_Attack(Skills skills, Penaltys penalty)
    {
        //근접공격 가능(무제한)
        skills.MeleeAttack(Target, -1);
    }

    //code 206
    public void Disguiser_Set(Skills skills, Penaltys penalty)
    {
        //피아구분 못함
        penalty.IDontKnowWhoYouAre(true);
    }

    public void Disguiser_Do(Skills skills, Penaltys penalty)
    {
        //오리색 베끼기
        skills.Metamolphosis(Target);
    }

    public void Disguiser_Attack(Skills skills, Penaltys penalty)
    {
        //근접공격 가능(무제한)
        skills.MeleeAttack(Target, -1);
    }

    //code 207
    public void Bomber_Attack(Skills skills, Penaltys penalty)
    {
        //폭탄을 러버덕에게 설치
        skills.PresentBomb(Target);
    }

    //code 301
    public void DeadDuck_Set(CommonCharacter player)
    {
        //콜라이더 제거(벽 및 캐릭터간 물리력 제거)

        //채팅 서버(데드덕 채팅 서버) 가입 및 현 채팅(산 자의 채팅 서버) 서버 채금

        //레이어 변경(데드덕) -> (feat 정현기 도움 필요)

        //카메라 설정으로 레이어(데드덕) 추가 -> (feat 정현기 도움 필요)

        //isComa 활성화(스킬 및 달리기, Attack 금지)
        player.isComa = true;
        //
    }
}
