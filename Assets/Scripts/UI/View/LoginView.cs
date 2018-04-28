using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginView : LoginViewBase
{
    public GameObject Login = null;

    public void CreateLoginBg()
    {
        GameObject login = GameObject.Instantiate(Login);
        float aspect = (float)Screen.width / Screen.height;
        float designAspect = 1920f / 1080f;
        login.transform.Find("LoginBg").localScale *= aspect / designAspect;
        login.transform.Find("ui_caocao_prefab").localScale *= aspect / designAspect;
    }
}
