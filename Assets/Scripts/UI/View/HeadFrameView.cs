using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadFrameView : MonoBehaviour
{
    [HideInInspector]
    public Image Frame;
    [HideInInspector]
    public Text Name;
    [HideInInspector]
    public GameObject isUse;
    [HideInInspector]
    public bool isOwn;
    [HideInInspector]
    public Button Frame_btn;
    [HideInInspector]
    public int id;
    public void Init()
    {
        Frame_btn = transform.Find("Frame").GetComponent<Button>();
        Frame = transform.Find("Frame").GetComponent<Image>();
        Name = transform.Find("Name").GetComponent<Text>();
        isUse = transform.Find("isUse").gameObject;
    }

    public void Endow(int id)
    {
        this.id = id;
        Frame.sprite = ResourceMgr.Instance.LoadSprite(id);
        isUse.SetActive(false);
    }


}
