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
    public Image hero_img;
    [HideInInspector]
    public GameObject nothave_obj;
    [HideInInspector]
    public int heroid;
    
    public void Init()
    {
        this.mTransform = this.transform;
        hero_img = transform.Find("herolevel/hero_img").GetComponent<Image>();
        nothave_obj = transform.Find("nothave_obj").gameObject;
    }

    public void SetHeroHaveInfo(Hero heros)
    {
        this.heroid =  heros.ID;
        hero_img.sprite = ResourceMgr.Instance.LoadSprite(JsonMgr.GetSingleton().GetHeroByID(heroid).headid);
    }
}
