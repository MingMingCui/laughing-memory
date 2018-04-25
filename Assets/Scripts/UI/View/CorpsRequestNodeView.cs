using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpsRequestNodeView : MonoBehaviour {

    public Image requesthead_img = null;
    public Text request_name_txt = null;
    public Text request_level_txt = null;
    public Text request_power_txt = null;
    public GameObject request_wait = null;
    public Button request_refuse_btn = null;
    public Button request_allow_btn = null;

    [HideInInspector]
    public uint UID = 0;

    public void SetInfo(uint roldId, int headid, string name, int level, int power, bool isAdmin)
    {
        //TODO:headid
        this.request_name_txt.text = string.Format("{0}", name);
        this.request_level_txt.text = string.Format("{0}级", level);
        this.request_power_txt.text = string.Format("战斗力:{0}", power);
        this.request_wait.SetActive(!isAdmin);
        this.request_refuse_btn.gameObject.SetActive(isAdmin);
        this.request_allow_btn.gameObject.SetActive(isAdmin);
        if (isAdmin)
        {
            EventListener.Get(request_refuse_btn.gameObject).OnClick = e => {
            };
            EventListener.Get(request_refuse_btn.gameObject).OnClick = e => {
            };
        }
    }
}
