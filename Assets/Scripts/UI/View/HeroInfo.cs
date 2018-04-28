using UnityEngine;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject Unlock_obj;
    [HideInInspector]
    public GameObject Equip_obj;
    [HideInInspector]
    public Slider Exp_slider;
    [HideInInspector]
    public GameObject StarParent_obj;
    [HideInInspector]
    public Text Lv_txt;
    [HideInInspector]
    public GameObject HideStar_obj;
    [HideInInspector]
    public Image Head_img;
    [HideInInspector]
    public Slider lock_slider;
    [HideInInspector]
    public Text LockNum_txt;
    [HideInInspector]
    public Image lockHead_img;
    [HideInInspector]
    public Image Border_img;
    [HideInInspector]
    public Text HeroName_txt;
    [HideInInspector]
    public Image Worker_img;
    public virtual void Awake()
    {
        this.mTransform = this.transform;
        Unlock_obj = mTransform.Find("Unlock_obj").gameObject;
        Equip_obj = mTransform.Find("Unlock_obj/Equip_obj").gameObject;
        Exp_slider = mTransform.Find("Unlock_obj/Exp_slider").GetComponent<Slider>();
        StarParent_obj = mTransform.Find("Unlock_obj/StarParent_obj").gameObject;
        Lv_txt = mTransform.Find("Unlock_obj/Lv_txt").GetComponent<Text>();
        HideStar_obj = mTransform.Find("Unlock_obj/HideStar_obj").gameObject;
        Head_img = mTransform.Find("Unlock_obj/Head_img").GetComponent<Image>();
        lock_slider = mTransform.Find("lock_slider").GetComponent<Slider>();
        LockNum_txt = mTransform.Find("lock_slider/LockNum_txt").GetComponent<Text>();
        lockHead_img = mTransform.Find("lock_slider/lockHead_img").GetComponent<Image>();
        Border_img = mTransform.Find("Border_img").GetComponent<Image>();
        HeroName_txt = mTransform.Find("HeroName_txt").GetComponent<Text>();
        Worker_img = mTransform.Find("Worker_img").GetComponent<Image>();
    }


    [HideInInspector]
    public HeroData heroData;
    public Image[] starArray;

    public int Type
    {
        get { return heroData.JsonData.type; } 
    }

    public void SetView()
    {
        HeroName_txt.text = heroData.JsonData.name;
        Lv_txt.text = heroData.Level.ToString();
        Worker_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.JsonData.type);
        lockHead_img.sprite = Head_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.JsonData.headid);
        Border_img.sprite = ResourceMgr.Instance.LoadSprite(JsonMgr.GetSingleton().GetHeroRareByID(heroData.Rare).HeadBorder);
        if (!heroData.UnLock)
        {
            int needPiece = JsonMgr.GetSingleton().GetHeroStarByID(heroData.HeroId).matnum[heroData.Star - 1];
            LockNum_txt.text = heroData.Piece + "/" + needPiece;
            lock_slider.value = heroData.Piece * 1f / needPiece ;
        }
        else
        {
            Exp_slider.value = heroData.Exp * 1f / heroData.NeedHero ;
            for (int i = 0; i < heroData.Star; ++i)
            {
                starArray[i].transform.SetParent(StarParent_obj.transform, false);
            }
            HeroEquipView hev = Equip_obj.GetComponent<HeroEquipView>();
            hev.SetView(heroData);
        }
    } 
    public void SetData(HeroData data)
    {
        heroData = data;
        Unlock_obj.SetActive(data.UnLock);
        lock_slider.gameObject.SetActive(!data.UnLock);
    }

    public void Close()
    {
        Transform partent = StarParent_obj.transform;
        for (int i = partent.childCount - 1 ; i >= 0; --i)
            partent.GetChild(i).SetParent(HideStar_obj.transform, false);
    }
}
