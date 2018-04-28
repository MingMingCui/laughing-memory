using System.Text;
using JsonData;
using UnityEngine;
using UnityEngine.UI;

public class StrengthenView : StrengthenViewBase
{
    public GameObject material;
    private float distance = 200;

    private int Tab;

    private EquipData equip;
    private Toggle[] locks;
    private int expend = 1025;

    public override void Awake()
    {
        base.Awake();
        locks = lock_trf.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < locks.Length; ++i)
        {
            Toggle t = locks[i];
            t.isOn = false;
            t.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    if (expend >= 1027)
                    {
                        t.isOn = false;
                        CanvasView.Instance.AddNotice("最多只能锁定两条属性");
                    }
                    expend++;
                }
                else
                    expend--;
                randomspend_txt.text = JsonMgr.GetSingleton().GetGlobalIntArrayByID((int)expend).ToString();
            });
        }
    }

    public void Open(EquipData equip)
    {
        Tab = 1;
        this.equip = equip;
        OnTipToggleClick();
        str_tog.isOn = true;
        SetStrengthenView();

        AddEvent();
    }
    private void AddEvent()
    {
        EventListener.Get(str_tog.gameObject).OnClick = e =>
        {
            if (Tab == 1)
                return;
            Tab = 1;
            OnTipToggleClick();
            SetStrengthenView();
        };
        EventListener.Get(adv_tog.gameObject).OnClick = e =>
        {
            if (Tab == 2)
                return;

            Tab = 2;
            OnTipToggleClick();
            SetAdvancedView();
        };
        EventListener.Get(ran_tog.gameObject).OnClick = e =>
        {
            if (equip.ItemData.rare < 3)
            {
                CanvasView.Instance.AddNotice("蓝色以上品质装备开启洗炼功能");
                switch(Tab)
                {
                    case 1:
                        str_tog.isOn = true;
                        break;
                    case 2:
                        adv_tog.isOn = true;
                        break;
                }
                return;
            }
            if (Tab == 3)
                return;
            Tab = 3;
            OnTipToggleClick();
            SetRandomView();
        };
        ZEventSystem.Register(EventConst.UPSTRENGTHENVIEW, this, "UpView");
    }

    public void UpView()
    {
        OnTipToggleClick();
        switch(Tab)
        {
            case 1:
                SetStrengthenView();
                break;
            case 2:
                SetAdvancedView();
                break;
            case 3:
                SetRandomView();
                break;
        }
    }

    private void OnTipToggleClick()
    {
        Strengthen_obj.SetActive(Tab == 1);
        Advanced_obj.SetActive(Tab == 2);
        Random_obj.SetActive(Tab == 3);
    }

    private void SetStrengthenView()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder nextSB = new StringBuilder();
        Pro[] p = equip.Attribute;
        for (int i = 0,length = p.Length; i < length; ++i)
        {
            sb.Append(AttrUtil.GetAttribute(p[i].attr));
            nextSB.Append(AttrUtil.GetAttribute(p[i].attr));
            sb.Append(": +");
            nextSB.Append(": +");
            sb.Append(AttrUtil.ShowText(p[i].attr, equip.JsonData.Attribute[i].num + p[i].num, equip.JsonData.Attribute[i].per + p[i].per));
            nextSB.Append(AttrUtil.ShowText(p[i].attr, equip.JsonData.Attribute[i].num * 2 + p[i].num, equip.JsonData.Attribute[i].per * 2 + p[i].per ));
            sb.Append("\n");
            nextSB.Append("\n");
        }
        attr_txt.supportRichText = true;
        string title = equip.ItemData.name + (equip.StrengthenLv == 0 ? "" : " +"  + equip.StrengthenLv) + "\n";
        title = title.AddColorLabel(ColorMgr.Colors[equip.ItemData.rare - 1]);
        attr_txt.text = title + sb;
        title = equip.ItemData.name + " +" + (equip.StrengthenLv + 1) + "\n";
        title = title.AddColorLabel(ColorMgr.Colors[equip.ItemData.rare - 1]);
        thenattr_txt.text = title + nextSB;
        sb = null;
        nextSB = null;
        
        equip_img.sprite = thenequip_img.sprite = ResourceMgr.Instance.LoadSprite(equip.ItemData.icon);
        thenborder_img.sprite = border_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[equip.ItemData.rare - 1]);
        lvfloor_cr.SetAlpha(equip.StrengthenLv);
        lv_txt.text = equip.StrengthenLv == 0 ? "" : "+" + equip.StrengthenLv;
        thenlvfloor_cr.SetAlpha(1);
        thenlv_txt.text = "+" + (equip.StrengthenLv + 1); 
        EventListener.Get(strengthen_btn.gameObject).OnClick = e =>
        {
            if(EquipMgr.GetSingleton().UpEquip(equip))
            {
                StrengthenOK_img.GetComponent<UISprite>().Play();
            }
            SetStrengthenView();
        };
        EventListener.Get(strengthenonekey_btn.gameObject).OnClick = e =>
        {

        };
        spend_txt.text = JsonMgr.GetSingleton().GetStrengthenSpendByID(equip.StrengthenLv).spend.ToString();
    }
    private void SetAdvancedView()
    {
        int[] targets = equip.JsonData.Advanced;
        int target = PlayerPrefs.GetInt("advancedtarget");

        //处理紫升橙
        if (targets.Length > 1)
        {
            bool has = false;
            for (int i = 0; i < targets.Length; ++i)
            {
                if (targets[i] == target)
                    has = true;
            }
            if(target !=0 && has)
            {
                advancedequip_img.raycastTarget = true;
                target_img.raycastTarget = false;
                EventListener.Get(advancedequip_img.gameObject).OnClick = e =>
                {
                    attr_obj.SetActive(false);
                    null_obj.SetActive(true);
                    for (int i = 0, length = parent_trf.childCount; i < length; ++i)
                    {
                        DestroyImmediate(parent_trf.GetChild(0).gameObject);
                    }
                    target = 0;
                    PlayerPrefs.SetInt("advancedtarget", 0);
                    UIFace.GetSingleton().Open(UIID.EuqipTipCtrl, targets);
                };
            }
            else
            {
                advancedequip_img.gameObject.SetActive(false);
                advancedborder_img.gameObject.SetActive(false);
                target_img.gameObject.SetActive(true);
                attr_obj.SetActive(false);
                null_obj.SetActive(true);
                target_img.raycastTarget = true;
                advancedequip_img.raycastTarget = false;

                EventListener.Get(target_img.gameObject).OnClick = e =>
                {
                    PlayerPrefs.SetInt("advancedtarget", 0);
                    UIFace.GetSingleton().Open(UIID.EuqipTipCtrl, targets);
                };
                return;
            }        
        }
        else if(targets.Length == 0)
        {
            advancedequip_img.gameObject.SetActive(false);
            advancedborder_img.gameObject.SetActive(false);
            target_img.gameObject.SetActive(true);
            attr_obj.SetActive(false);
            null_obj.SetActive(true);
            target_img.raycastTarget = true;
            advancedequip_img.raycastTarget = false;
            EventListener.Get(target_img.gameObject).OnClick = e =>
            {
                CanvasView.Instance.AddNotice("当前装备已进阶至最大等级");
            };
            return;
        }
        else
        {
            advancedequip_img.raycastTarget = false;
            target = targets[0];
        }

        advancedequip_img.gameObject.SetActive(true);
        advancedborder_img.gameObject.SetActive(true);
        target_img.gameObject.SetActive(false);
        attr_obj.SetActive(true);
        null_obj.SetActive(false);
        AdvancedSpend aspend = JsonMgr.GetSingleton().GetAdvancedSpendByID(target);
        SetSpendView(aspend);
        EventListener.Get(advanced_btn.gameObject).OnClick = e =>
        {
            if (equip.JsonData.Advanced.Length == 0)
            {
                CanvasView.Instance.AddNotice("当前装备已进阶至最大等级");
                return;
            }else if (equip.StrengthenLv < equip.JsonData.AdvancedCondition)
            {
                CanvasView.Instance.AddNotice(string.Format("进阶失败！需要强化等级达到{0},才能继续进阶。", equip.JsonData.AdvancedCondition));
                return;
            }
            else if (target == 0)
                return;
            for (int i = 0, length = parent_trf.childCount; i < length; ++i)
            {
                DestroyImmediate(parent_trf.GetChild(0).gameObject);
            }

            equip.EquipId = target;
            SetAdvancedView();
            HeroData hero = HeroMgr.GetSingleton().GetHeroData(equip.HeroId);
            if (hero == null)
                return;
            hero.ClearEquipAttr();
            ZEventSystem.Dispatch(EventConst.REFRESHRIGHT);
        };
        StringBuilder sb = new StringBuilder();
        Pro[] p = equip.Attribute;
        for (int i = 0, length = p.Length; i < length; ++i)
        {
            sb.Append(AttrUtil.GetAttribute(p[i].attr));
            sb.Append(": +");
            sb.Append(AttrUtil.ShowText(p[i].attr, equip.JsonData.Attribute[i].num + p[i].num , equip.JsonData.Attribute[i].per + p[i].per));
            sb.Append("\n");
        }
        name_txt.supportRichText = true;
        string color = ColorMgr.Colors[equip.ItemData.rare - 1];
        name_txt.text = string.Format("<color=#{0}>{1}{2}</color>", color, equip.JsonData.Name, equip.StrengthenLv > 0 ? "  +" + equip.StrengthenLv : "");
        baseattr_txt.text = string.Format("<color=#{0}>{1}</color>", color, sb);

        Equip q = JsonMgr.GetSingleton().GetEquipByID(target);
        StringBuilder append = new StringBuilder();
        p = q.Attribute;
        for (int i = 0, length = p.Length; i < length; ++i)
        {
            append.Append(AttrUtil.GetAttribute(p[i].attr));
            append.Append(": +");
            append.Append(AttrUtil.ShowText(p[i].attr, p[i].num + p[i].num * equip.StrengthenLv, p[i].per + p[i].per * equip.StrengthenLv));
            append.Append("\n");
        }
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(target);
        color = ColorMgr.Colors[ic.rare - 1];
        advanced_txt.supportRichText = true;
        advanced_txt.text = string.Format("<color=#{0}>{1}{2}</color>", color, q.Name, equip.StrengthenLv > 0 ? "  +" + equip.StrengthenLv : "");
        advancedattr_txt.text = string.Format("<color=#{0}>{1}</color>", color, append);   
        sb = null;
        append = null;
    }

    private void SetSpendView(AdvancedSpend aspend)
    {
        int share = aspend.material.Length - 1;
        float angle = 360f / share;

        for (int i = 0; i < aspend.material.Length - 1; ++i)
        {
            GameObject mGo = Instantiate(material, parent_trf);
            float radian = -(angle / 180) * Mathf.PI * i;
            mGo.transform.localPosition = new Vector3(Mathf.Sin(radian) * distance, Mathf.Cos(radian) * distance, 0);
            MateralItem mi = mGo.GetComponent<MateralItem>();
            mi.SetView(aspend.material[i]);
        }
        advancedspend_txt.text = aspend.material[share].num.ToString();
        advancedequip_img.sprite = ResourceMgr.Instance.LoadSprite(JsonMgr.GetSingleton().GetItemConfigByID(aspend.ID).icon);
        advancedborder_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[equip.ItemData.rare]);
    }

    private void SetRandomView()
    {
        randomequip_img.sprite = ResourceMgr.Instance.LoadSprite(equip.ItemData.icon);
        randomlvfloor_cr.SetAlpha(equip.StrengthenLv);
        randomlv_txt.text = equip.StrengthenLv > 0 ? " +" + equip.StrengthenLv: "";
        randomborder_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[equip.ItemData.rare - 1]);
        equipname_txt.text = equip.ItemData.name;

        StringBuilder append = new StringBuilder();
        for (int i = 0, length = equip.wishs.Length; i < length; ++i)
        {
            append.Append(AttrUtil.GetAttribute(equip.wishs[i].wish.attr));
            append.Append(": +");
            append.Append(AttrUtil.ShowText(equip.wishs[i].wish.attr, equip.wishs[i].wish.num, equip.wishs[i].wish.per));
            append.Append("\n");
            locks[i].isOn = equip.wishs[i].isLock;
        }
        oldattr_txt.text = append.ToString();

        StringBuilder temp = new StringBuilder();
        for (int i = 0, length = equip.tempWishs.Length; i < length; ++i)
        {
            temp.Append(AttrUtil.GetAttribute(equip.tempWishs[i].attr));
            temp.Append(": +");
            temp.Append(AttrUtil.ShowText(equip.tempWishs[i].attr, equip.tempWishs[i].num, equip.tempWishs[i].per));
            temp.Append("\n");
        }
        newattr_txt.text = temp.ToString();

        randomspend_txt.text = JsonMgr.GetSingleton().GetGlobalIntArrayByID(expend).value.ToString();
    }

    public void Close()
    {
        for (int i = 0,length = parent_trf.childCount; i < length; ++i)
        {
            DestroyImmediate(parent_trf.GetChild(0).gameObject);
        }
        ZEventSystem.DeRegister(EventConst.UPSTRENGTHENVIEW);
        ZEventSystem.Dispatch(EventConst.REFRESHLEFT);
    }
}
