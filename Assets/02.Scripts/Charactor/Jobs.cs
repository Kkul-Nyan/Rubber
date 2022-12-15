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
        //목표 플레이어를 죽임
        canUse = player.skill.MeleeAttack(target, canUse);

        //죽인 플레이어가 러버덕인지 판정
        //러버덕이면 보안관 죽음
        if (target.jobCode < 200)
        {
            player.penalty.Dead(player);
        }
    }

    //code 102
    public void Doctor_Set()
    {
        //상대방의 아이디와 색상 혼란 페널티(랜덤? 아이디 지우기? 색상 통일?)
        player.penalty.IDontKnowWhoYouAre(true);
    }

    public void Doctor_Do()
    {
        //목표 플레이어의 상태 수정(죽음->보통)
        //상태 수정 성공시 기능 일시정지(다음턴까지)
        canUse = player.skill.Heal(target, canUse);
    }

    //code 103
    public void Shaman_Do()
    {
        //탑뷰, 시체위치확인
        canUse = player.skill.TopView(canUse);
    }

    //code 104
    public void GoodNose_Do()
    {
        //죽은 시체 근처에서 사용시 살덕마 이동방향 확인
        player.skill.DogNose();
    }

    //code 105
    public void Betlayer_Set()
    {
        //시작시 마피아 리스트(마피아끼리 확인 가능)에 포함.
        player.skill.ImYourAlly(player);
    }

    //code 106
    public void Marathoner_Set()
    {
        //스태미나 양을 1.5배로 만듬.
        player.stamina *= 1.5f;
    }
    
    //code 107
    public void Detective_Do()
    {
        //그 턴의 회의중에 사용시 한명의 직업정보를 알 수 있음
        if (player.isDiscuss && canUse != 0)
        {
            canUse = player.skill.TellMeYourJob(target, canUse);
            canUse--;
        }
    }

    //code 108
    public void Mechanic_Set()
    {
        //러버덕의 몸에 스패너를 하나 표시한다.
        player.penalty.IamEngineer();
    }

    public void Mechanic_Do()
    {
        //기믹이 파괴되어있을시 복구, 파괴되지 않았을때 보호막생성
        player.skill.ImGenius(target_Gimmick, CanUse);
    }

    //code 109
    public void Wing_Set()
    {
        //스태미나 양 1/2배
        player.stamina *= 0.5f;
    }

    //code 110
    public void SpeedRacer_Do()
    {
        //1턴중 이동속도가 1.5배(걷는속도 1.5배 뛰는속도3배(1.5*2배))
        //1게임당 1회 사용가능함
        if (canUse != 0)
        {
            //이동속도 1.5배
            player.speed *= 1.5f;
            canUse--;
        }
    }


    //code 201
    public void Blind_Set()
    {
        //시력저하 페널티(색상대비의 극단적인 저하?)
        player.penalty.ICantSee(true);
    }

    public void Blind_Do()
    {
        //사용시 몸 투명화
        player.skill.Clocking(canUse);
        //사용시 시력 정상화
        player.penalty.ICantSee(false);
    }

    public void Blind_Attack()
    {
        //근접공격 가능(무제한)
        player.skill.MeleeAttack(player, -1);
    }


    //code 202
    public void Cowboy_Attack()
    {
        //원거리 공격 가능
        //턴당 1번 가능
        canAttack = player.skill.Magnum(target, canAttack);
        //전맵 총소리 사운드 들림
        player.penalty.GunSound();
        //자동회의 소집?
        //근거리 플레이어 위치파악?
    }

    //code 203
    public void NeedleSpitter_Attack()
    {
        //중거리 공격가능
        //턴당 1번 가능
        canAttack = player.skill.NeedleGun(target, canAttack);
    }

    //code 204
    public void Spy_Do()
    {
        //러버덕 아이템 확인
        player.skill.ShowMeYourItem(target);
    }

    public void Spy_Attack()
    {
        //근접공격 가능(무제한)
        player.skill.MeleeAttack(target, -1);
    }


    //code 205
    public void Bully_Do()
    {
        //러버덕 채팅금지
        if (player.isDiscuss)
        {
            canUse = player.skill.ShutUp(target, canUse);
        }
    }

    public void Bully_Attack()
    {
        //근접공격 가능(무제한)
        player.skill.MeleeAttack(target, -1);
    }

    //code 206
    public void Disguiser_Set()
    {
        //피아구분 못함
        player.penalty.IDontKnowWhoYouAre(true);
    }

    public void Disguiser_Do()
    {
        //오리색 베끼기
        canUse = player.skill.Metamolphosis(player, target, canUse);
    }

    public void Disguiser_Attack()
    {
        //근접공격 가능(무제한)
        player.skill.MeleeAttack(target, -1);
    }

    //code 207
    public void Bomber_Attack()
    {
        //폭탄을 러버덕에게 설치
        player.skill.PresentBomb(target);
    }

    //code 301
    public void DeadDuck_Set()
    {
        //콜라이더 제거(벽 및 캐릭터간 물리력 제거)
        player.GetComponent<Collider>().enabled = false;
        //채팅 서버(데드덕 채팅 서버) 가입 및 현 채팅(산 자의 채팅 서버) 서버 채금

        //레이어 변경("DeadDuck") -> (feat 정현기 도움 필요)
        player.gameObject.layer = LayerMask.NameToLayer("DeadDuck");
        //카메라 설정으로 레이어(데드덕) 추가 -> (feat 정현기 도움 필요)
        player.playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("DeadDuck");
        //isComa 활성화(스킬 및 달리기, Attack 금지)
        player.isComa = true;
        //
    }
}
