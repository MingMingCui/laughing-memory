using JsonData;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 商店NPC半身像
/// </summary>
public enum ShopNPC
{
    ordinaryImg = 20102,
    passBarrierImg = 20103,
    competitiveImg = 20100,
    guildImg = 20101,
    westernImg = 20104
}

/// <summary>
/// 商店名称
/// </summary>
public enum ShopTitleName
{
    ordinaryName = 20107,
    passbarrierName = 20108,
    competitiveName = 20105,
    guidName = 20106,
    westernName = 20109
}

public class ShopUIView: ShopUIViewBase
{
    private int curGoodsId;//当前点击的货物id
    private float timer = 0;//聊天气泡的计时器
    private float curtime = 0;//货币不足提示的计时器
    //private const int SCROLLNUM = 2;
  //  private const int SCROLLSIZE = 305;
 
    CanvasGroup cg;//气泡聊天框的显示
    CanvasGroup curcg;//货币不足提示的显示
    public GoodsUIView goodsUIView;
    public List<GoodsUIView> goodsList = new List<GoodsUIView>();
    Color priceCol;
    RectTransform rect = null;
    GridLayoutGroup grid=null;
    RectTransform srrect = null;
    public void Init()
    {
        
        if (Role.Instance.Level > 20 && Role.Instance.Level <= 40)
        {
            shoppassbarrier_tog.interactable = true;
        }
        else if (Role.Instance.Level > 40 && Role.Instance.Level <= 60)
        {
            shoppassbarrier_tog.interactable = true;
            shopcompetitive_tog.interactable = false; //暂时
        }
        else if (Role.Instance.Level > 60)
        {
            shoppassbarrier_tog.interactable = true;
            shopcompetitive_tog.interactable = false;//暂时
            shopguild_tog.interactable = false;//暂时
        }
    }

    void Update()
    {
        WordsHidden();
        CurrencyTip();
    }

    /// <summary>
    /// 商店半身像和名称及气泡框的显示
    /// </summary>
    public void ShowNPC(int type)
    {
        if (type == (int)ShopType.ordinaryshop)
        {
            NPC_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopNPC.ordinaryImg);
            name_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopTitleName.ordinaryName);
        }
        else if (type == (int)ShopType.passbarriershop)
        {
            NPC_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopNPC.passBarrierImg);
            name_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopTitleName.passbarrierName);
        }
        else if (type == (int)ShopType.competitiveshop)
        {
            NPC_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopNPC.competitiveImg);
            name_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopTitleName.competitiveName);
        }
        else if (type == (int)ShopType.westernshop)
        {
            NPC_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopNPC.westernImg);
            name_img.sprite = ResourceMgr.Instance.LoadSprite((int)ShopTitleName.westernName);
        }
        ClickHead(type);
    }

    public void OrdinaryShopBtn()
    {
        if (ShopMgr.Instance.shoptype == ShopType.ordinaryshop) return;
        ShopMgr.Instance.shoptype = ShopType.ordinaryshop;
        ShowNPC((int)ShopMgr.Instance.shoptype);
        if (ShopMgr.Instance.goodsList.ContainsKey((int)ShopMgr.Instance.shoptype))
            UpdateUnitShop((int)ShopMgr.Instance.shoptype);
        else
            ShopMgr.Instance.OpenUnitShop((int)ShopMgr.Instance.shoptype,false);
    }

    /// <summary>
    /// 点击过关斩将商店
    /// </summary>
    public void PassBarrierShopBtn()
    {
        if (ShopMgr.Instance.shoptype == ShopType.passbarriershop) return;
        ShopMgr.Instance.shoptype = ShopType.passbarriershop;
        ShowNPC((int)ShopMgr.Instance.shoptype);
        if (ShopMgr.Instance.goodsList.ContainsKey((int)ShopMgr.Instance.shoptype))
            UpdateUnitShop((int)ShopMgr.Instance.shoptype);
        else
            ShopMgr.Instance.OpenUnitShop((int)ShopMgr.Instance.shoptype, false);
    }

    /// <summary>
    /// 点击竞技场商店
    /// </summary>
    public void CompetitiveShopBtn()
    {
        return;
        if (ShopMgr.Instance.shoptype == ShopType.competitiveshop) return;
        ShopMgr.Instance.shoptype = ShopType.competitiveshop;
        ShowNPC((int)ShopMgr.Instance.shoptype);
        if (ShopMgr.Instance.goodsList.ContainsKey((int)ShopMgr.Instance.shoptype))
            UpdateUnitShop((int)ShopMgr.Instance.shoptype);
        else
            ShopMgr.Instance.OpenUnitShop((int)ShopMgr.Instance.shoptype, false);
    }

    /// <summary>
    /// 点击公会商店
    /// </summary>
    public void GuildShopBtn()
    {
        return;
        if (ShopMgr.Instance.shoptype == ShopType.guildshop) return;
        ShopMgr.Instance.shoptype = ShopType.guildshop;
        ShowNPC((int)ShopMgr.Instance.shoptype);
        if (ShopMgr.Instance.goodsList.ContainsKey((int)ShopMgr.Instance.shoptype))
            UpdateUnitShop((int)ShopMgr.Instance.shoptype);
        else
            ShopMgr.Instance.OpenUnitShop((int)ShopMgr.Instance.shoptype, false);
    }


    /// <summary>
    /// 点击关闭货物弹框
    /// </summary>
    public void ClickGoodsClose()
    {
        Goods_obj.SetActive(false);
    }

    /// <summary>
    /// 确认购买
    /// </summary>
    public void ClickConfirmBuy()
    {
        Goods_obj.SetActive(false);
        for (int i = 0; i < goodsList.Count; i++)
        {
            if (curGoodsId == goodsList[i].goodsUIId)
            {
                if (goodsList[i].Currency == 1)
                {
                    if (Role.Instance.Cash < int.Parse(goodsList[i].price_txt.text))
                    {
                        CanvasView.Instance.AddNotice("铜钱不足！");
                        //currencytips_obj.SetActive(true);
                        //currencytip_txt.text = "铜钱不足！";
                        //curcg.alpha = 1;
                        //CurrencyTip();
                        return;
                    }
                }
                else if (goodsList[i].Currency == 2)
                {
                    if (Role.Instance.Gold < int.Parse(goodsList[i].price_txt.text))
                    {
                        CanvasView.Instance.AddNotice("金锭不足，无法购买！");
                        //currencytips_obj.SetActive(true);                       
                        //currencytip_txt.text = "金锭不足，无法购买！";
                        //curcg.alpha = 1;
                        //CurrencyTip();
                        return;
                    }

                }
                    goodsList[i].SoldOut_btn.gameObject.SetActive(true);
                    ShopMgr.Instance.goodsList[(int)ShopMgr.Instance.shoptype][i].num = 0;
                    if (goodsList[i].Currency == 1)
                        Role.Instance.Cash -= int.Parse(goodsList[i].price_txt.text);
                    else if (goodsList[i].Currency == 2)
                        Role.Instance.Gold -= int.Parse(goodsList[i].price_txt.text);
                    int num = 0;
                    if (goodsList[i].goodsnum_txt.text.Length == 0) num = 1;
                    else num = int.Parse(goodsList[i].goodsnum_txt.text);
                    Debug.Log(Role.Instance.Gold);
                
            }
        }
    }

    /// <summary>
    /// 点击半身像弹出聊天气泡框
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickHead(int type)
    {
        words_obj.SetActive(true);
        if (cg == null)
        {
            cg = words_img.GetComponent<CanvasGroup>();
        }
        cg.alpha = 1;
        timer = 0;
        if (type == (int)ShopType.ordinaryshop)
            words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1001, 1011)).desc;
        else if (type == (int)ShopType.passbarriershop)
            words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1011, 1021)).desc;
        else if (type == (int)ShopType.competitiveshop)
            words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1021, 1031)).desc;
        else if (type == (int)ShopType.guildshop)
            words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1031, 1041)).desc;
        else if (type == (int)ShopType.westernshop)
            words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1051,1061)).desc;
    }

    /// <summary>
    /// 聊天气泡框的隐藏
    /// </summary>
    public void WordsHidden()
    {
        if (words_obj.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                cg.alpha -= Time.deltaTime;
            }
            if (cg.alpha <= 0)
            {
                timer = 0;
                words_obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 货币提示框的隐藏
    /// </summary>
    public void CurrencyTip()
    {
        if (currencytips_obj.activeSelf)
        {
            curtime += Time.deltaTime;
            if (curtime > 1f)
            {
                curcg.alpha -= Time.deltaTime;
            }
            if (curcg.alpha <= 0)
            {
                curtime = 0;
                currencytips_obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 点击售罄货物时，半身像弹出聊天气泡框
    /// </summary>
    /// <param name="goods"></param>
    public void ClickSoldBtn(GoodsUIView goods)
    {
        timer = 0;
        words_obj.SetActive(true);
        cg.alpha = 1;
        words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1041, 1051)).desc;
        WordsHidden();
    }

    /// <summary>
    /// 点击货物，对货物弹框赋值
    /// </summary>
    /// <param name="goods"></param>
    public void ClickGoods(GoodsUIView goods)
    {
        curtime = 0;
        if (curcg == null) curcg = curtipbg_img.GetComponent<CanvasGroup>();
        GoodsUIView goodsUI = null;
        for (int i = 0; i < goodsList.Count; i++)
        {
            if (goods.goodsUIId == goodsList[i].goodsUIId)
            {
                goodsUI = goodsList[i];
                //if (goodsList[i].Currency == 1)
                //{
                //    if (Role.Instance.Cash < int.Parse(goodsList[i].price_txt.text))
                //    {
                //        currencytips_obj.SetActive(true);
                //        currencytip_txt.text = "铜钱不足！";
                //        curcg.alpha = 1;
                //        CurrencyTip();
                //        return;
                //    }
                //}
                //else if (goodsList[i].Currency == 2)
                //{
                //    if (Role.Instance.Gold < int.Parse(goodsList[i].price_txt.text))
                //    {
                //        currencytips_obj.SetActive(true);
                //        currencytip_txt.text = "金锭不足，无法购买！";
                //        curcg.alpha = 1;
                //        CurrencyTip();
                //        return;
                //    }
                //}
            }
        }
        curGoodsId = goodsUI.goodsUIId;
        goodslevel_img.sprite = goodsUI.GoodsLevel_img.sprite;
        goods_img.sprite = goodsUI.Goods_img.sprite;
        int s = ShopMgr.Instance.GetItemIDByUIID((int)ShopMgr.Instance.shoptype, goodsUI.goodsUIId);
        if (s == -1)
            return;
        else
            goodsnum_txt.text = ItemMgr.Instance.GetItemNum(s).ToString();
        int textcolor = JsonMgr.GetSingleton().GetItemConfigByID(ShopMgr.Instance.GetItemIDByUIID((int)ShopMgr.Instance.shoptype,goodsUI.goodsUIId)).rare;

        switch (textcolor)
        {
            case (int)RareType.RareWhite:
                goodsname_txt.text = "<color=#FFFFFF>" + goodsUI.GoodsName_txt.text+ "</color>";
                break;
            case (int)RareType.RareGreen:
                goodsname_txt.text = "<color=#00FF00>" + goodsUI.GoodsName_txt.text + "</color>";
                break;
            case (int)RareType.RareBlue:
                goodsname_txt.text = "<color=#00FFFF>" + goodsUI.GoodsName_txt.text + "</color>";
                break;
            case (int)RareType.RarePurple:
                goodsname_txt.text = "<color=#FF00FF>" + goodsUI.GoodsName_txt.text + "</color>";
                break;
            case (int)RareType.RareOrange:
                goodsname_txt.text = "<color=#FFFF00>" + goodsUI.GoodsName_txt.text + "</color>";
                break;
            default:
                goodsname_txt.text = "<color=#FF0000>" + goodsUI.GoodsName_txt.text + "</color>";
                break;
        }
        if (ItemMgr.Instance.GetItemNum(ShopMgr.Instance.GetItemIDByUIID((int)ShopMgr.Instance.shoptype, goodsUI.goodsUIId)) == 0)
        {
            goodsnum_txt.color = Color.red;
        }
        else goodsnum_txt.color = Color.yellow;
        buyprice_txt.text = goodsUI.price_txt.text;
        if (goodsUI.goodsnum_txt.text == "")
            buynum_txt.text = "1";
        else buynum_txt.text = goodsUI.goodsnum_txt.text;
        buynumicon_img.sprite = goodsUI.copper_img.sprite;
        int itemid = ShopMgr.Instance.GetItemID((int)ShopMgr.Instance.shoptype, goods.goodsUIId);
       
        goodsproperty_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemid).propertydes;
        goodsuse_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemid).usedes;
        Goods_obj.SetActive(true);
    }

    /// <summary>
    /// 显示货物
    /// </summary>
    /// <param name="itemList"></param>
    public void GoodsShow(int type)
    {
        for (int i = 0; i < ShopMgr.Instance.goodsList[type].Length; i++)
        {
            if (i == goodsList.Count)
            {
                GameObject shop = LoadGoodsInfo();
                GoodsUIView goodsUIView = shop.GetComponent<GoodsUIView>();
                goodsUIView.Init();
                goodsList.Add(goodsUIView);
                if (goodsList.Count == 1)
                    priceCol = goodsUIView.price_txt.color; 
            }
        }
    }

    /// <summary>
    /// 实例化货物
    /// </summary>
    /// <returns></returns>
    public GameObject LoadGoodsInfo()
    {
        GameObject _goods = GameObject.Instantiate(goodsUIView.gameObject);
        _goods.transform.SetParent(goodslist_obj.transform);
        _goods.transform.localScale = Vector3.one;
        _goods.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _goods;
    }

    /// <summary>
    /// 更新单个商店物品
    /// </summary>
    /// <param name="goods"></param>
    public void UpdateUnitShop(int shoptype)
    {
        if (ShopMgr.Instance.goodsList.Count <= 0) return;
        if (!ShopMgr.Instance.goodsList.ContainsKey(shoptype)) return;
        if (goodsList.Count < 0) return;
        if (grid == null)
        {
            grid = goodslist_obj.GetComponent<GridLayoutGroup>();
            rect = goodslist_obj.GetComponent<RectTransform>();
            srrect = goods_sr.GetComponent<RectTransform>();
        }
        int CurrencyId = 2000;
        ShopItem[] shop = ShopMgr.Instance.goodsList[shoptype];
        for (int i = 0; i < goodsList.Count; i++)
        {
            ItemConfig item = JsonMgr.GetSingleton().GetItemConfigByID(shop[i].item_id);
            switch (shop[i].num)
            {
                case 0:
                    goodsList[i].SoldOut_btn.gameObject.SetActive(true);
                    goodsList[i].GoodsName_txt.text = item.name;
                    break;
                case 1:
                    goodsList[i].GoodsName_txt.text = item.name;
                    goodsList[i].goodsnum_txt.text = "";
                    goodsList[i].SoldOut_btn.gameObject.SetActive(false);
                    break; 
                default:
                    goodsList[i].GoodsName_txt.text = item.name+"×"+shop[i].num.ToString();
                    goodsList[i].goodsnum_txt.text = shop[i].num.ToString();
                    goodsList[i].SoldOut_btn.gameObject.SetActive(false);
                    break;
            }
            goodsList[i].GoodsLevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[item.rare - 1]);
            goodsList[i].Goods_img.sprite = ResourceMgr.Instance.LoadSprite(item.icon);
            goodsList[i].Currency = ShopMgr.Instance.goodsList[shoptype][i].type;
            goodsList[i].goodsUIId = shop[i].item;
            //if (ShopMgr.Instance.goodsList[shoptype][i].saled == true)
            //    goodsList[i].SoldOut_btn.gameObject.SetActive(false);
            //else
            //    goodsList[i].SoldOut_btn.gameObject.SetActive(true);
            //refreshtime_txt.text = ShopMgr.Instance.shopRefreshTime[shoptype].ToString();
            if (goodsList[i].Currency == 1)
            {
                CurrencyId = 2000;
                if (Role.Instance.Cash < shop[i].price)
                    goodsList[i].price_txt.color = Color.red;
                else
                    goodsList[i].price_txt.color = priceCol;
            }
            else
            {
                CurrencyId = 2001;
                if (Role.Instance.Gold < shop[i].price)
                    goodsList[i].price_txt.color = Color.red;
                else
                    goodsList[i].price_txt.color = priceCol;
            }
               
            goodsList[i].copper_img.sprite = ResourceMgr.Instance.LoadSprite(CurrencyId);
            goodsList[i].price_txt.text = (shop[i].price*shop[i].num).ToString();

        }
        //滑动窗显示
        int num = (int)(srrect.sizeDelta.x / grid.cellSize.x);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = new Vector2(grid.cellSize.x*((goodsList.Count*0.5f)- num)+ grid.spacing.x, 0);
    }

    /// <summary>
    /// 页签显示
    /// </summary>
    /// <param name="isOpen"></param>
    public void ShowWestern(bool isOpen,int type)
    {
        if (isOpen)
        {
            shopordinary_tog.gameObject.SetActive(false);
            shoppassbarrier_tog.gameObject.SetActive(false);
            shopcompetitive_tog.gameObject.SetActive(false);
            shopguild_tog.gameObject.SetActive(false);
        }
        else
        {
            shopordinary_tog.gameObject.SetActive(true);
            shoppassbarrier_tog.gameObject.SetActive(true);
            shopcompetitive_tog.gameObject.SetActive(true);
            shopguild_tog.gameObject.SetActive(true);
            if (type == (int)ShopType.ordinaryshop)
            {
                shopordinary_tog.isOn = true;
            }
            else if (type == (int)ShopType.passbarriershop)
            {
                shoppassbarrier_tog.isOn = true;
            }
            else if (type == (int)ShopType.competitiveshop)
            {
                shopcompetitive_tog.isOn = true;
            }
            else if(type ==(int)ShopType.guildshop)
            {
                shopguild_tog.isOn = true;
            }
        }
    }

    /// <summary>
    ///商店货物界面页签显示
    /// </summary>
    public void ButtonSide()
    {
        switch (ShopMgr.Instance.shoptype)
        {
            case ShopType.none:
                return;
            case ShopType.ordinaryshop:
                ShowWestern(false, (int)ShopType.ordinaryshop);
                break;
            case ShopType.passbarriershop:
                ShowWestern(false,(int)ShopType.passbarriershop);
                break;
            case ShopType.competitiveshop:
                ShowWestern(false,(int)ShopType.competitiveshop);
                break;
            case ShopType.guildshop:
                ShowWestern(false,(int)ShopType.guildshop);
                break;
            case ShopType.westernshop:
                ShowWestern(true,(int)ShopType.westernshop);
                break;
        }
    }

    public void Close()
    {
        Goods_obj.SetActive(false);
    }
}
