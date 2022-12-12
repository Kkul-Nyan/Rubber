using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using VivoxUnity;

public class ChatManager : MonoBehaviour
{
    public static ILoginSession LoginSession;
    public static IChannelSession m_channelSession = null;
    
    #region vivox 로그인체크
    public void checklogin()
    {
        Debug.Log("Vivox LoginState :" + LoginSession.State);
    }
    #endregion
    
    #region 비복스 로그인
    /* 
    비복스 init을 해주었으면 로그인을 해주어야 한다. 로비이전에 먼저 로그인을 해주는 이유는 내 목소리를 듣고 미리 사운드를 체크 할수 있게 만들어줄려는 의도임
    Account는 우린 자동 로그인이기때문에 Authentication이 자동으로 만들어 놓은 아이디를 가져와서 사용
    토큰은 로그인섹션을 이용해서 이미 만들어진(자동으로 만들어서 가지고있음)토큰을 가져옴
     */
    public static void LoginVivox()
    {

        Account account = new Account(Authentication.PlayerId);
        LoginSession = VivoxService.Instance.Client.GetLoginSession(account);
        string token = LoginSession.GetLoginToken();
        LoginSession.BeginLogin(token, SubscriptionMode.Accept, null, null, null, result =>
        {
            try
            {
                LoginSession.EndLogin(result);
                Debug.Log("Vivox LoginState :" + LoginSession.State);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Vivox failed to login: " + ex.Message);
            }

            Debug.Log("Vivox isAuthenticated?" + VivoxService.Instance.IsAuthenticated);

        });

    
    }
    #endregion 

    #region  채널 생성및 참여
    /* 먼저 로그인 섹션을 불려와서 현재 로그인 인지 확인, 로그인이 안되어있으면 로그인이 안되어있다 로그 띄우고 종료,
        로그인이 되어있으면, 채널타입을 포지션(위치에 따라 사운드가 들림 3D sound적용됨)을 사용, 방을 만들때 사용한 방제+vivox를 채널이름으로 채널 생성 혹은 채널 참가
        토큰키는 한 컴퓨터당 1나라서 패럴싱크로도 여러개 사용 안됨 */
    public static void JoinChannel(string lobbyid)
    {
        if (LoginSession.State == LoginState.LoggedIn)
        {
            ChannelType channelType = ChannelType.Positional;
            Channel channel = new Channel(lobbyid +"vivox", channelType, null);

            m_channelSession = LoginSession.GetChannelSession(channel);
        
            string tokenKey = m_channelSession.GetConnectToken();

            m_channelSession.BeginConnect(true, false, true, tokenKey, result =>
            {
                try
                {
                    m_channelSession.EndConnect(result);
                    Debug.Log("Success" + channel.Name + " VivoxChannel");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Could not connect to channel: {e.Message}");
                    return;
                }
            });
        }
        else
        {
            Debug.LogError("Can't join a channel when not logged in.");
        }
    }
    #endregion

    #region 로그아웃 비복스(비복스 로그인섹션으로 로그아웃을 불려옴)
    public static void LogOut()
    {
        LoginSession.Logout();
    }
    #endregion

    #region 비복스 채널 나감
    
    public static void LeaveVivoxChannel(string lobbyid)
    {
        Channel channel = new Channel(lobbyid + "vivox");
        m_channelSession = LoginSession.GetChannelSession(channel);
        if(m_channelSession != null)
        {
            m_channelSession.Disconnect();
            LoginSession.DeleteChannelSession(channel);
            Debug.Log("disconnect channel");
        }
    }
    #endregion

}
