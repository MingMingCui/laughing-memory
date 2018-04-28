using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabs : MonoBehaviour {

    public List<GameObject> TabList = new List<GameObject>();

    public List<RectTransform> TabObjList = new List<RectTransform>();

    [Tooltip("通过SetActive还是设置层级控制显隐")]
    public bool HideOrSetLayer = false;

    private List<bool> _tabState = new List<bool>();

	void Awake () {
        if (TabList.Count == 0 || TabList.Count != TabObjList.Count)
        {
            EDebug.LogErrorFormat("UITabs.Start, TabList and TabObjList not compaticable.");
            return;
        }
        foreach (GameObject tab in TabList)
        {
            EventListener.Get(tab).OnClick = onClickTab;
        }
        for (int idx = 0; idx < TabList.Count; ++idx)
            _tabState.Add(true);
	}

    /// <summary>
    /// 设置某个Tab的Enable状态
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="enable"></param>
    public void SetTabEnable(int idx, bool enable)
    {
        if (idx < 0 || idx >= TabList.Count)
        {
            EDebug.LogErrorFormat("UITabs.SetTabEnable failed, invalid idx {0}", idx);
            return;
        }
        _tabState[idx] = enable;
        TabList[idx].SetActive(enable);
    }

    /// <summary>
    /// 直接打开某个Tab
    /// </summary>
    /// <param name="idx"></param>
    public void OpenTab(int idx)
    {
        if (idx < 0 || idx >= TabList.Count)
        {
            EDebug.LogErrorFormat("UITabs.OpenTab failed, invalid idx {0}", idx);
            return;
        }
        if (!_tabState[idx])
        {
            EDebug.LogErrorFormat("UITabs.OpenTab failed, tab {0} is disable", idx);
            return;
        }
        GameObject tab = TabList[idx];
        Toggle togCom = tab.GetComponent<Toggle>();
        if(togCom != null)
            togCom.isOn = true;
        onClickTab(tab);
    }

    /// <summary>
    /// 点击某个Tab，显示对应Obj
    /// </summary>
    /// <param name="tab"></param>
    private void onClickTab(GameObject tab)
    {
        int tabIdx = TabList.IndexOf(tab);
        if (HideOrSetLayer)
        {
            for (int idx = 0; idx < TabObjList.Count; ++idx)
            {
                TabObjList[idx].gameObject.SetActive(idx == tabIdx);
            }
        }
        else
        {
            if (tabIdx >= 0)
            {
                TabObjList[tabIdx].SetAsLastSibling();
            }
        }
    }
}
