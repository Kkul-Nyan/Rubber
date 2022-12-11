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

    public void checklogin()
    {
        Debug.Log("Vivox LoginState :" + LoginSession.State);
    }

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
    public static void JoinChannel(LobbyData lobby)
    {
        if (LoginSession.State == LoginState.LoggedIn)
        {
            ChannelType channelType = ChannelType.Positional;
            Channel channel = new Channel(lobby.Name+"vivox", channelType, null);

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
}
