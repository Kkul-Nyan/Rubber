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
        //DeadDuck으로 잡체인지
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
            //러버덕진영
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

            //생체덕진영
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
            //생체덕진영
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
        //목표 플레이어를 죽임
        skills.MeleeAttack(Target, CanUse);

        //죽인 플레이어가 러버덕인지 판정
        //러버덕이면 보안관 죽음
        if (Target.jobCode < 200)
        {
            //penalty.Dead();
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
    public void DeadDuck_Set(RubberDuck player)
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

public class Skills
{
    public void MeleeAttack(RubberDuck player, int canUse)
    {
        //근거리 공격
        //칼 활성화(칼에는 colider가 있음) -> 버튼 클릭시에만 활성화
        //칼 colider에 닿은 개체는 기절
        //기절 후 10초? 안에 힐 없을 시 죽음(301(데드덕) 전직)
    }

    public void NeedleGun(RubberDuck player, int canUse)
    {
        //중거리 공격
        //스킬 발동시 ray(컨트롤러)에 닿은 개체에 디버프 시전 -> 10초 후 기절
        //기절 후 10초? 안에 힐 없을 시 죽음(301(데드덕) 전직)
    }

    public void Magnum(RubberDuck player, int canUse)
    {
        //장거리 공격
        //총알 활성화(총알에는 colider가 있음) -> 버튼 클릭시 앞으로 발사
        //개체 충돌시 destroy
    }

    public void Heal(RubberDuck player, int canUse)
    {
        //죽은 오리 살림(턴1회) ->
        //중거리 당한 오리 소모없이 치료
    }

    public void Run(float stamina)
    {
        //스태미나 소모하며 빠른이동(걷는 속도 2배)
    }

    public void TopView(int canUse)
    {
        //탑뷰시점에서 맵 확인 가능, 그 턴에 사망한 시체 위치 확인
    }

    public void DogNose()
    {
        //살덕자 이동방향 보임(위처3방식?)
        //살덕자가 러버덕을 죽이면 러버덕에서 살덕자 방향으로 화살표 생성(LookAt함수이용, 10초후 개체 Destroy)
        //이 메서드는 이 화살표를 볼 수 있게 만들어주는 메서드
    }

    public void ImYourAlly()
    {
        //시작시 생오리(마피아)리스트에 올라감 -> 생오리들이 아군으로 착각
    }

    public void Energizer(ref float stamina)
    {
        //스태미너 총량 증가(1.5배정도)
        stamina *= 1.5f;
    }

    public void TellMeYourJob(RubberDuck player, int canUse)
    {
        //투표중 선택한 오리의 직업정보 알수있음
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
                //파괴된 기믹 복구
                gimmick.destroyed = false;
            }
            else
            {
                //파괴보호막(그 턴에 한정)
                gimmick.cantDestroy = true;
            }
        }
        */
        //대기형 미션시간 50%단축(중요도 B)
    }

    public void ICanFly(float stamina)
    {
        if (stamina > 0)
        {
            //스태미나 소진시까지 콜라이더 무시
        }
        else
        {
            //벽콜라이더 속에서 스태미너 소진시 DeadDuck전직
        }
    }

    public void Adrenaline(ref float speed, int canUse)
    {
        if (canUse > 0)
        {
            //이동속도 1.5배
            speed *= 1.5f;
        }
    }

    public void Clocking(int canUse)
    {
        //일정시간(10초?)동안 오리의 알파값(투명도) 0 or 안보이는 레이어로 변환

    }

    public void ShowMeYourItem(RubberDuck player)
    {
        //대상이 가지고 있는 아이템(목록)을 볼 수 있음
    }

    public void ShutUp(RubberDuck player)
    {
        //투표중에 한명을 채금시킬수 있음
    }

    public void Metamolphosis(RubberDuck player)
    {
        //타겟 러버덕의 색상을 그대로 베껴옴
        //매 턴마다 한번 교체가능
    }

    public void PresentBomb(RubberDuck player)
    {
        //타겟 러버덕에게 페널티 Bombed부여
        player.isBombed = true;
        player.penalty.Bombed(player);
    }
}

public class Penaltys
{
    public void Coma(RubberDuck player)
    {
        //이동 및 행동 제약(캐릭터 쓰러짐)
        player.isComa = true;
        player.speed = 0;
        //살해 당한면 기절 10초 후 Dead
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
        //DeadDuck으로 잡체인지
        player.jobCode = 301;
        //DeadDuck Setting
        player.jobs.DeadDuck_Set(player);
    }

    public void CantVote(RubberDuck player)
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

    public void Bombed(RubberDuck player)
    {
        //폭탄 타이머가 다될때 이 디버프를 가진 플레이어는 사망
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
        //큰소리(모든 맵 영역에서 들림, 즉시 회의 소집)

        //근거리에 있던 오리들에겐 대략적인 근원지 표시(맵)
    }

    public void IamEngineer()
    {
        //몸에 스페너 하나를 달고 다님
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
            //러버덕진영
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
            //생체덕진영
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