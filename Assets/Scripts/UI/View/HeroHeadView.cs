using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHeadView : MonoBehaviour
{
    public List<Sprite> Colors = null;

    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image Border_img;
    [HideInInspector]
    public Image Head_img;
    [HideInInspector]
    public Image Color_img;
    [HideInInspector]
    public GameObject ExpPro_obj;
    [HideInInspector]
    public Image ExpPro_img;
    [HideInInspector]
    public GameObject GetExp_obj;
    [HideInInspector]
    public Text Exp_txt;
    [HideInInspector]
    public Image LevelUp_img;
    [HideInInspector]
    public StarView stars_obj;
    [HideInInspector]
    public GameObject Level_obj;
    [HideInInspector]
    public Text Level_txt;
    [HideInInspector]
    public Image OnStub_img;

    [HideInInspector]
    public int HeroId = 0;
    [HideInInspector]
    public bool OnStub = false;

    public void Init ()
    {
        Transform mTransform = this.transform;
        Border_img = mTransform.GetComponent<Image>();
        Head_img = mTransform.Find("Head_img").GetComponent<Image>();
        Color_img = mTransform.Find("Color_img").GetComponent<Image>();
        ExpPro_obj = mTransform.Find("ExpPro_obj").gameObject;
        ExpPro_img = mTransform.Find("ExpPro_obj/ExpPro_img").GetComponent<Image>();
        GetExp_obj = mTransform.Find("GetExp_obj").gameObject;
        Exp_txt = mTransform.Find("GetExp_obj/Exp_txt").GetComponent<Text>();
        LevelUp_img = mTransform.Find("LevelUp_img").GetComponent<Image>();
        stars_obj = mTransform.Find("stars_obj").GetComponent<StarView>();
        Level_obj = mTransform.Find("Level_obj").gameObject;
        Level_txt = mTransform.Find("Level_obj/Level_txt").GetComponent<Text>();
        OnStub_img = mTransform.Find("OnStub_img").GetComponent<Image>();
    }


    public void SetHeroInfo(int headId, int rare, int star = 0, int level = 0, bool border = false)
    {
        HeroId = headId;    //武将id就是头像id
        Border_img.color = new Color(0,0,0,0);
        _setHeroInfo(headId, rare);
        if (star > 0)
            this.stars_obj.SetStar(star);
        if (level > 0)
        {
            this.Level_obj.SetActive(true);
            this.Level_txt.text = level.ToString();
        }
    }

    public void SetHeroInfo(int headId, int rare, int star, int level, int getExp, int curExp, int maxExp)
    {
        SetHeroInfo(headId, rare, star, level);
        _setExpInfo(getExp, curExp, maxExp);
        this.ExpPro_obj.SetActive(true);
        this.GetExp_obj.SetActive(true);
    }

    public void SetOnStub(bool onStub)
    {
        this.OnStub = onStub;
        this.OnStub_img.gameObject.SetActive(onStub);
    }

    private void _setHeroInfo(int headId, int rare)
    {
        if(headId > 0)
            this.Head_img.sprite = ResourceMgr.Instance.LoadSprite(headId);
        int headBorder = JsonMgr.GetSingleton().GetHeroRareByID(rare).HeadBorder;
        this.Color_img.sprite = ResourceMgr.Instance.LoadSprite(headBorder);
    }
    private void _setExpInfo(int getExp, int curExp, int maxExp)
    {
        this.Exp_txt.text = getExp.ToString();
        ExpPro_img.fillAmount = (float)curExp / maxExp;
    }
}
