using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterTog : MonoBehaviour
{
    [HideInInspector]
    public Toggle fighter_tog = null;
    [HideInInspector]
    public Text fightername_txt = null;
    [HideInInspector]
    public uint UID;
    [HideInInspector]
    public bool isEnemy;
    public void Init()
    {
        Transform mTransform = this.transform;
        fighter_tog = mTransform.GetComponent<Toggle>();
        fightername_txt = mTransform.Find("fightername_txt").GetComponent<Text>();
    }

    public void SetInfo(uint uid,string name)
    {
        this.UID = uid;
        fightername_txt.text = name;
    }
}
