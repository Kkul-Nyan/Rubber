
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager: MonoBehaviour
{
    public GameObject Canvas;
    public async void LoginAnonymously()
    {
        await Authentication.Login();
        //ChatManager.LoginVivox();
        Debug.Log("Success Singin");
        

        
        Canvas.gameObject.SetActive(false);
        //SceneManager.LoadSceneAsync("00Test");
    }

}


