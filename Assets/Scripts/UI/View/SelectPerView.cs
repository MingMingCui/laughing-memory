using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum RoleSort
{
    ManFirst = 20026,
    ManSecond,
    WomanFirst,
    WomanSecond
}

public class SelectPerView : SelectPerViewBase
{
    AlphaTween NumeNUll;
    AlphaTween Special;
    [HideInInspector]
    public List<GameObject> ManRoleList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> WomanRoleList = new List<GameObject>();
    [HideInInspector]
    public RoleSort role = RoleSort.ManFirst;
    [HideInInspector]
    public int isMan = 1;
    private Transform ManRole;
    private Transform WomanRole;
    private List<string> ShieldWordList; //屏蔽字库
    private Dictionary<int, Animator> RolePlayer = new Dictionary<int, Animator>();
    private bool scale = false;
    private bool isRoleMan = true;
    public void Init()
    {
        string ShieldWord = ResourceMgr.Instance.LoadData("ShieldWord");
        ShieldWordList = ShieldWord.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
        ShieldWordList = ShieldWordList.Where(s => !string.IsNullOrEmpty(s)).ToList(); //检查是否有空字符串

        role = RoleSort.ManFirst;
        GameObject Login;
        GameObject SelectPer;
        Login = GameObject.Find("Login").gameObject;
        SelectPer = GameObject.Find("SelectPer").gameObject;
        SetChildLayer(Login,9);
        SetChildLayer(SelectPer, 0);
        ManRole = SelectPer.transform.Find("ManRole").transform;
        WomanRole = SelectPer.transform.Find("WomanRole").transform;
        int idx = 20026;
        foreach (Transform item in ManRole)
        {
            ManRoleList.Add(item.gameObject);
            RolePlayer.Add(idx, item.GetComponent<Animator>());
            idx++;
        }
        foreach (Transform item in WomanRole)
        {
            RolePlayer.Add(idx, item.GetComponent<Animator>());
            WomanRoleList.Add(item.gameObject);
            idx++;
        }
        Import_input.text = JsonMgr.GetSingleton().RandomName(true);
    }
    /// <summary>
    /// 更改显示层级
    /// </summary>
    /// <param name="t"></param>
    /// <param name="layer"></param>
    private void SetChildLayer(GameObject t, int layer)
    {
        t.layer = layer;
        Transform partent = t.transform;
        for (int i = 0, count = partent.childCount; i < count; ++i)
        {
            GameObject child = partent.GetChild(i).gameObject;
            child.layer = layer;
            SetChildLayer(child, layer);
        }
    }

    /// <summary>
    /// 更换选择人物
    /// </summary>
    /// <param name="_isManRole"></param>
    public void Switch(bool _isManRole)
    {
           
        if (Man_tog.isOn || Man1_tog.isOn)
        {
            
            if (_isManRole)
            {
                MoveAndPlay();
            }
            else
            {
                if (!Man_tog.isOn)
                    Man_tog.isOn = true;
                else
                    Man1_tog.isOn = true;
            }
        }
        else
        {
           
            if (_isManRole)
            {
                MoveAndPlay();
            }
            else
            {
                if (false == Woman_tog)
                     Woman_tog.isOn = true;
                else
                     Woman1_tog.isOn = true;
            }
        }
    }

    /// <summary>
    /// 选择的角色位于最前方
    /// </summary>
    /// <param name="_role"></param>
    /// <param name="_iscc"></param>
    public void Switch(List<GameObject> _role, bool _iscc)
    {
        for (int i = 0; i < _role.Count; i++)
        {
           _role[i].GetComponent<RoleElectView>().Folding(!_iscc);
        }
    }
    /// <summary>
    /// 切换男女性角色
    /// </summary>
    /// <param name="isMen">男女？</param>
    public void CutRole(bool isMen,GameObject s)
    {
        if (isRoleMan != isMen)
        {
            Import_input.text = "";
            isRoleMan = isMen;
        }

        if (isMen)
        {
            if (isMan==2)
            {
                isMan = 1;
                ManRole.gameObject.SetActive(isMen);
                WomanRole.gameObject.SetActive(!isMen);
                if (Man_tog.isOn)
                    scale = true;
                else
                    scale = false;
            }
                if (Man_tog.isOn)
                {
                    if (role == RoleSort.ManFirst) return;
                    role = RoleSort.ManFirst;
                    Switch(ManRoleList, scale);
                    scale = false;
                    MoveAndPlay();
                }
                else if(Man1_tog.isOn)
                {
                    if (role == RoleSort.ManSecond) return;
                    role = RoleSort.ManSecond;
                    Switch(ManRoleList, scale);
                    scale = true;
                    MoveAndPlay();
                } 

        }
        else
        {
            if (isMan==1)
            {
                isMan = 2;
                ManRole.gameObject.SetActive(isMen);
                WomanRole.gameObject.SetActive(!isMen);
                if (Woman_tog.isOn)
                    scale = true;
                else
                    scale = false;
            }
                if (Woman_tog.isOn)
                {
                    if (role == RoleSort.WomanFirst) return;
                    role = RoleSort.WomanFirst;
                    Switch(WomanRoleList, scale);
                    scale = false;
                    MoveAndPlay();
                }

                else if (Woman1_tog.isOn)
                {
                    if (role == RoleSort.WomanSecond) return;
                    role = RoleSort.WomanSecond;
                    Switch(WomanRoleList, scale);
                    scale = true;
                    MoveAndPlay();
                }
        }
                
    }
    /// <summary>
    /// 点击人物播放动画
    /// </summary>
    public void MoveAndPlay()
    {
        switch (role)
        {
            case RoleSort.ManFirst:
                RolePlayer[(int)RoleSort.ManFirst].SetTrigger("ToggleAni");
                break;
            case RoleSort.ManSecond:
                RolePlayer[(int)RoleSort.ManSecond].SetTrigger("ToggleAni");
                break;
            case RoleSort.WomanFirst:
                RolePlayer[(int)RoleSort.WomanFirst].SetTrigger("ToggleAni");
                break;
            case RoleSort.WomanSecond:
                RolePlayer[(int)RoleSort.WomanSecond].SetTrigger("ToggleAni");
                break;
        }
    }

    /// <summary>
    /// 警示
    /// </summary>
    public void Caution(Image  _imag, bool Caution)
    {
        if (Caution)
        {
            if (NumeNUll == null)
                NumeNUll = _imag.GetComponent<AlphaTween>();
            NumeNUll.ResetToBeginning();
            NumeNUll.PlayForward();
        }
        else
        {
            if (Special == null)
                Special = _imag.GetComponent<AlphaTween>();
            Special.ResetToBeginning();
            Special.PlayForward();
        }
    }
/// <summary>
/// 判断是否包含屏蔽字
/// </summary>
/// <param name="_name"></param>
/// <returns></returns>
    public bool ExamineShieldWord(string _name)
    {
        bool isInclude = true;
        for (int idx = 0; idx < ShieldWordList.Count; idx++)
        {
            if (_name.IndexOf(ShieldWordList[idx]) != -1)
            {
                isInclude = false;
                return isInclude;
            }
        }
        return isInclude;
    }

    /// <summary>
    /// 获取随机名字
    /// </summary>
    public void GetName()
    {
        bool idman = false;
        if (Man_tog.isOn || Man1_tog.isOn)
            idman = true;
        else
            idman = false;
      Import_input.text = JsonMgr.GetSingleton().RandomName(idman);
    }

}
