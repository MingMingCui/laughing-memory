using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainUI : MonoBehaviour
{
    void OnGUI()
    {

        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 110, 200, 500), "返回登录界面"))
        {
            SceneMgr.Instance.LoadScene("Login");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 300, Screen.height / 2 + 110, 200, 500), "开始战斗"))
        {
            SceneMgr.Instance.LoadScene("Game");
        }
    }
}