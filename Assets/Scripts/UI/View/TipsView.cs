using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TipsType
{
    SimpleTip = 1,
    ItemTip = 2,
    MonsterTip = 3,
    TreasureTip = 4,
}

public enum Alignment
{
    LT = 0,     //左上
    CT = 1,     //正上
    RT = 2,     //右上
    LM = 3,     //正左
    M = 4,      //正中
    RM = 5,     //正右
    LB = 6,     //左下
    CB = 7,     //正下
    RB = 8,     //右下
}

public class TipsView : TipsViewBase {

    public ItemUIView ItemView = null;
    public Text ItemNumTxt = null;

    [HideInInspector]
    public ItemUIView itemTipsView = null;

    private System.Text.StringBuilder _sb = new System.Text.StringBuilder();

    private List<ItemUIView> _items = new List<ItemUIView>();
    private List<Text> _itemNums = new List<Text>();

    private const float TexSingleWidth = 16;

    public override void Awake()
    {
        base.Awake();
        itemTipsView = this.ui_itemlevelbg_btn.GetComponent<ItemUIView>();
        itemTipsView.Init();
        CloseTip();
    }

    public void OpenTip(int tipsType, object data, Vector2 pos, int align, Vector2 offset, List<object> args = null)
    {
        switch (tipsType)
        {
            case (int)TipsType.SimpleTip:
                this.openSimpleTip((int)data, pos, align, offset, args != null ? args.ToArray() : new object[0]);
                break;
            case (int)TipsType.ItemTip:
                this.openItemTip((int)data, pos, align, offset);
                break;
            case (int)TipsType.MonsterTip:
                this.openMonsterTip((int)data, pos, align, offset, (args != null && (bool)args[0]));
                break;
            case (int)TipsType.TreasureTip:
                this.openTreasureTip(((List<Vector2Int>)data).ToArray(), pos, align, offset);
                break;
            default:
                break;
        }
    }

    private void openSimpleTip(int tipid, Vector2 pos, int align, Vector2 offset, object[] args)
    {
        JsonData.Tips tipsJson = JsonMgr.GetSingleton().GetTipsByID(tipid);
        if (tipsJson == null)
        {
            EDebug.LogErrorFormat("openSimpleTip, invalid tips id {0}", tipid);
            return;
        }
        Vector2 size = new Vector2(tipsJson.sizex, tipsJson.sizey);
        this.simpletips_rect.sizeDelta = size;
        this.simpletips_rect.anchoredPosition = calAlignment(pos, size, align, offset);
        this.simpletipst_txt.text = string.Format(tipsJson.content, args);
        this.simpletips_rect.gameObject.SetActive(true);
    }

    private void openItemTip(int itemId, Vector2 pos, int align, Vector2 offset)
    {
        JsonData.ItemConfig itemJson = JsonMgr.GetSingleton().GetItemConfigByID(itemId);
        if (itemJson == null)
        {
            EDebug.LogErrorFormat("openItemTip, invalid item id {0}", itemId);
            return;
        }
        itemTipsView.SetInfo(itemId, 0);
        string itemColor = ColorMgr.Colors[itemJson.rare - 1];
        this.ItemName_txt.text = itemJson.name.AddColorLabel(itemColor);
        this.ItemNum_txt.text = string.Format(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2005).desc,
            ItemMgr.Instance.GetItemNum(itemId).ToString());
        this.ItemPrice_txt.text = itemJson.price.ToString();
        if (itemJson.type == FuncType.EQUIP)
        {
            _sb.Length = 0;
            JsonData.Equip equipJson = JsonMgr.GetSingleton().GetEquipByID(itemId);
            if (equipJson != null)
            {
                for (int idx = 0; idx < equipJson.Attribute.Length; ++idx)
                {
                    Pro p = equipJson.Attribute[idx];
                    _sb.AppendLine(string.Format("  {0} +{1}", AttrUtil.GetAttribute(p.attr), AttrUtil.ShowText(p.attr, p.num, p.per)));
                }
                for (int idx = 0; idx < equipJson.Innate.Length; ++idx)
                {
                    Pro p = equipJson.Innate[idx];
                    _sb.AppendLine(string.Format("  {0} +{1}", AttrUtil.GetAttribute(p.attr), AttrUtil.ShowText(p.attr, p.num, p.per)));
                }
                _sb.Remove(_sb.Length - 1, 1);
                this.ItemIntroduce_txt.alignment = TextAnchor.MiddleLeft;
                this.ItemIntroduce_txt.text = _sb.ToString();
            }
            else
                EDebug.LogErrorFormat("openItemTip invalid equip id {0}", itemId);
        }
        else
        {
            this.ItemIntroduce_txt.alignment = TextAnchor.UpperLeft;
            this.ItemIntroduce_txt.text = itemJson.propertydes;
        }
        this.itemtips_rect.anchoredPosition = calAlignment(pos, this.itemtips_rect.sizeDelta, align, offset);
        this.itemtips_rect.gameObject.SetActive(true);
    }

    private void openMonsterTip(int monsterId, Vector2 pos, int align, Vector2 offset, bool isBoss)
    {
        Monster monsterJson = JsonMgr.GetSingleton().GetMonsterByID(monsterId);
        if (monsterJson == null)
        {
            EDebug.LogErrorFormat("openMonsterTip invalid monster id {0}", monsterId);
            return;
        }
        this.monsterhead_img.sprite = ResourceMgr.Instance.LoadSprite(monsterJson.headid);
        this.monsterlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[monsterJson.rare - 1]);
        HeroRare rareData = JsonMgr.GetSingleton().GetHeroRareByID(monsterJson.rare);
        this.monsterlevel_img.sprite = ResourceMgr.Instance.LoadSprite(rareData.HeadBorder);
        string monsterName = string.Format("{0}({1})", monsterJson.name, rareData.Name);
        this.monstername_txt.text = monsterName.AddColorLabel(rareData.Color);
        this.monsterlevel_txt.text = string.Format(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2004).desc, monsterJson.level);
        this.monsterisboss_obj.SetActive(isBoss);
        this.monsterintroduce_txt.text = monsterJson.desc;
        this.monstertips_rect.anchoredPosition = calAlignment(pos, this.monstertips_rect.sizeDelta, align, offset);
        this.monstertips_rect.gameObject.SetActive(true);
    }

    private void openTreasureTip(Vector2Int[] data, Vector2 pos, int align, Vector2 offset)
    {
        int maxTextWidth = 0;
        int needItem = data.Length - _items.Count;
        int needText = data.Length - _itemNums.Count;
        if (needItem > 0)
        {
            for (int idx = 0; idx < needItem; ++idx)
            {
                GameObject itemGo = GameObject.Instantiate(ItemView.gameObject);
                itemGo.transform.parent = this.treasuretips_rect;
                itemGo.transform.localScale = Vector3.one;
                ItemUIView view = itemGo.GetComponent<ItemUIView>();
                view.Init();
                _items.Add(view);
            }
        }
        if (needText > 0)
        {
            for (int idx = 0; idx < needText; ++idx)
            {
                GameObject textGo = GameObject.Instantiate(ItemNumTxt.gameObject);
                textGo.transform.parent = this.treasuretips_rect;
                textGo.transform.localScale = Vector3.one;
                Text tx = textGo.GetComponent<Text>();
                _itemNums.Add(tx);
            }
        }
        for (int idx = 0; idx < _items.Count; ++idx)
        {
            if (idx < data.Length)
            {
                _items[idx].SetInfo(data[idx].x, 0);
                _items[idx].gameObject.SetActive(true);
                RectTransform itemRect = _items[idx].GetComponent<RectTransform>();
                itemRect.sizeDelta = new Vector2(50, 50);
                itemRect.anchoredPosition = new Vector2(10, -idx * 61 - 11);
            }
            else
            {
                _items[idx].gameObject.SetActive(false);
            }
        }

        for (int idx = 0; idx < _itemNums.Count; ++idx)
        {
            if (idx < data.Length)
            {
                string itemnum = data[idx].y.ToString();
                _itemNums[idx].text = itemnum;
                if (itemnum.Length > maxTextWidth)
                    maxTextWidth = itemnum.Length;
                _itemNums[idx].gameObject.SetActive(true);
                RectTransform itemRect = _itemNums[idx].GetComponent<RectTransform>();
                itemRect.anchoredPosition = new Vector2(70, -idx * 61 - 11);
            }
            else
            {
                _items[idx].gameObject.SetActive(false);
            }
        }

        Vector2 size = new Vector2(80 + maxTextWidth * 16, 11 * (data.Length + 1) + 50 * data.Length);
        this.treasuretips_rect.sizeDelta = size;
        this.treasuretips_rect.anchoredPosition = calAlignment(pos, size, align, offset);
        this.treasuretips_rect.gameObject.SetActive(true);
    }

    private Vector2 calAlignment(Vector2 pos, Vector2 size, int align, Vector2 offset)
    {
        switch (align)
        {
            case (int)Alignment.LT:
                return new Vector2(pos.x + size.x / 2 + offset.x, pos.y - size.y / 2 - offset.y);
            case (int)Alignment.CT:
                return new Vector2(pos.x + offset.x, pos.y - size.y / 2 - offset.y);
            case (int)Alignment.RT:
                return new Vector2(pos.x - size.x / 2 - offset.x, pos.y - size.y / 2 - offset.y);
            case (int)Alignment.LM:
                return new Vector2(pos.x + size.x / 2 + offset.x, pos.y + offset.y);
            case (int)Alignment.M:
                return new Vector2(pos.x + offset.x, pos.y + offset.y);
            case (int)Alignment.RM:
                return new Vector2(pos.x - size.x / 2 - offset.x, pos.y + offset.y);
            case (int)Alignment.LB:
                return new Vector2(pos.x + size.x / 2 + offset.x, pos.y + size.y / 2 + offset.y);
            case (int)Alignment.CB:
                return new Vector2(pos.x + offset.x, pos.y + size.y / 2 + offset.y);
            case (int)Alignment.RB:
                return new Vector2(pos.x - size.x / 2 - offset.x, pos.y + size.y / 2 + offset.y);
            default:
                return Vector2.zero;
        }
    }

    public void CloseTip()
    {
        this.simpletips_rect.gameObject.SetActive(false);
        this.itemtips_rect.gameObject.SetActive(false);
        this.monstertips_rect.gameObject.SetActive(false);
        this.treasuretips_rect.gameObject.SetActive(false);
    }
}
