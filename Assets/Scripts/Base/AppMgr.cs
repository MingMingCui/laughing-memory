using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMgr : Singleton<AppMgr> {

    public string ClientVer = "";
    public string ServerVer = "";

    public readonly int TargetFrameCount = 30;

    public void Init()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        Application.targetFrameRate = TargetFrameCount;

        //设置屏幕自动旋转， 并置支持的方向
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
}
