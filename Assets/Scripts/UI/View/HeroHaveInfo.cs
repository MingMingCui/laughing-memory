using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JsonData;



public class HeroHaveInfo : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image herolevel_img;
    [HideInInspector]
    public Image hero_img;
    [HideInInspector]
    public GameObject nothave_obj;
    [HideInInspector]
    public int heroid;
    //[HideInInspector]
    //public int havetype;
    
    public void Init()
    {
        this.mTransform = this.transform;
        herolevel_img = transform.Find("herolevel_img").GetComponent<Image>();
        hero_img = transform.Find("herolevel_img/hero_img").GetComponent<Image>();
        nothave_obj = transform.Find("nothave_obj").gameObject;
    }

    public void SetHeroHaveInfo(Hero heros)
    {
        this.heroid =  heros.ID;
        herolevel_img.sprite = ResourceMgr.Instance.LoadSprite(JsonMgr.GetSingleton().GetHeroRareByID(heros.initstar).HeadBorder);
        hero_img.sprite = ResourceMgr.Instance.LoadSprite(JsonMgr.GetSingleton().GetHeroByID(heroid).headid);
    }
}
