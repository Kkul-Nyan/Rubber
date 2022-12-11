using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{
    //공격스킬
    public void MeleeAttack(CommonCharacter player, int canUse)
    {
        //근거리 공격
        //칼 활성화(칼에는 colider가 있음) -> 버튼 클릭시에만 활성화
        //
        //칼 colider에 닿은 개체는 기절
        //기절 후 10초? 안에 힐 없을 시 죽음(301(데드덕) 전직)
    }

    public void NeedleGun(CommonCharacter player, int canUse)
    {
        //중거리 공격
        //스킬 발동시 ray(컨트롤러)에 닿은 개체에 디버프 시전 -> 10초 후 기절
        //기절 후 10초? 안에 힐 없을 시 죽음(301(데드덕) 전직)
    }

    public void Magnum(CommonCharacter player, int canUse)
    {
        //장거리 공격
        //총알 활성화(총알에는 colider가 있음) -> 버튼 클릭시 앞으로 발사
        //개체 충돌시 destroy
    }

    public void PresentBomb(CommonCharacter player)
    {
        //타겟 러버덕에게 페널티 Bombed부여
        player.isBombed = true;
        player.penalty.Bombed(player);
    }

    //회복스킬
    public void Heal(CommonCharacter player, int canUse)
    {
        //죽은 오리 살림(턴1회) ->
        //중거리 당한 오리 소모없이 치료
    }

    //스테이터스 변화
    public void Energizer(ref float stamina)
    {
        //스태미너 총량 증가(1.5배정도)
        stamina *= 1.5f;
    }

    public void Adrenaline(ref float speed, int canUse)
    {
        if (canUse > 0)
        {
            //이동속도 1.5배
            speed *= 1.5f;
        }
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

    //카메라 변화
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

    public void Clocking(int canUse)
    {
        //일정시간(10초?)동안 오리의 알파값(투명도) 0 or 안보이는 레이어로 변환

    }


    //정보 조작or입수
    public void ImYourAlly()
    {
        //시작시 생오리(마피아)리스트에 올라감 -> 생오리들이 아군으로 착각
    }

    public void TellMeYourJob(CommonCharacter player, int canUse)
    {
        //투표중 선택한 오리의 직업정보 알수있음
        //코드별 직업명 세팅
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

        //표시될 직업명을 표시용 변수에 입력
        string showText = "ERROR!";
        if (jobName[player.jobCode] is not null)
        {
            showText = jobName[player.jobCode];
        }
        

    }

    public void ShowMeYourItem(CommonCharacter player)
    {
        //대상이 가지고 있는 아이템(목록)을 볼 수 있음
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

    public void ShutUp(CommonCharacter player)
    {
        //투표중에 한명을 채금시킬수 있음
    }

    public void Metamolphosis(CommonCharacter target)
    {
        //타겟 러버덕의 색상을 그대로 베껴옴
        Mesh mesh = target.meshFilter.mesh;
        //매 턴마다 한번 교체가능
    }
}
