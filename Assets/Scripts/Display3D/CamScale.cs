﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CamScale : MonoBehaviour
{

    void Start()
    {
        int ManualWidth = 1920;
        int ManualHeight = 1080;
        int manualHeight;
        if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        else
            manualHeight = ManualHeight;
        Camera camera = GetComponent<Camera>();
        float scale = System.Convert.ToSingle(manualHeight / 1080);
        camera.fieldOfView *= scale;
    }
}