using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : UICtrlBase<ItemView>
{
    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.DefaultShow();
        _initEvent(true);
        this.mView.DefaultType();
        this.mView.itemclose_btn.onClick.AddListener(delegate () { this.CloseItem(); });
        this.mView.total_tog.onValueChanged.AddListener((bool value) => this.mView.ClickAll());
        this.mView.consumables_tog.onValueChanged.AddListener((bool value) => this.mView.ClickConsumables());
        this.mView.equip_tog.onValueChanged.AddListener((bool value) => this.mView.ClickEquip());
        this.mView.material_tog.onValueChanged.AddListener((bool value) => this.mView.ClickMaterial());
        this.mView.saleclose_btn.onClick.AddListener(delegate () { this.mView.ClickSaleColse(); });
        this.mView.other_tog.onValueChanged.AddListener((bool value) => this.mView.ClickOther());
        this.mView.collating_btn.onClick.AddListener(delegate () { this.mView.Collating(true); });
        this.mView.sale_btn.onClick.AddListener(delegate () { this.mView.ClickSale(); });
        this.mView.sub_btn.onClick.AddListener(delegate () { this.mView.ClickSubAndAdd(true); });
        this.mView.add_btn.onClick.AddListener(delegate () { this.mView.ClickSubAndAdd(false); });
        this.mView.max_btn.onClick.AddListener(delegate () { this.mView.ClickMax(); });
        this.mView.confirmsale_btn.onClick.AddListener(delegate () { this.mView.ClickConfirmsale(); });
    }
    public override bool OnClose()
    {
        base.OnClose();
        _initEvent(false);
        this.mView.itemclose_btn.onClick.RemoveAllListeners();
        this.mView.total_tog.onValueChanged.RemoveAllListeners();
        this.mView.consumables_tog.onValueChanged.RemoveAllListeners();
        this.mView.equip_tog.onValueChanged.RemoveAllListeners();
        this.mView.material_tog.onValueChanged.RemoveAllListeners();
        this.mView.other_tog.onValueChanged.RemoveAllListeners();
        this.mView.collating_btn.onClick.RemoveAllListeners();
        this.mView.saleclose_btn.onClick.RemoveAllListeners();
        this.mView.sale_btn.onClick.RemoveAllListeners();
        this.mView.sub_btn.onClick.RemoveAllListeners();
        this.mView.add_btn.onClick.RemoveAllListeners();
        this.mView.max_btn.onClick.RemoveAllListeners();
        this.mView.confirmsale_btn.onClick.RemoveAllListeners();
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void UpdateItemList(List<Item> allItem)
    {
        this.mView.UpdateItemView(allItem);
        _initAdditionEvent(true);
    }

    public void UpdateItemParts(List<Item> items)
    {
        this.mView.UpdateItemParts(items);
    }

    private void _initEvent(bool open)
    {
        if (open)
        {
            _initAdditionEvent(true);
            ZEventSystem.Register(EventConst.UpdateItemList, this, "UpdateItemList");
            ZEventSystem.Register(EventConst.UpdateItemParts, this, "UpdateItemParts");
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.UpdateItemList);
            ZEventSystem.DeRegister(EventConst.UpdateItemParts);
            _initAdditionEvent(false);
        }
    }

    public void _initAdditionEvent(bool open)
    {
        if (open)
        {
            if (this.mView.itemUIList != null)
            {
                foreach (var key in this.mView.itemUIList)
                {
                    if(key.isRegister== false)
                    {
                        key.itemlevelbg_btn.onClick.AddListener(delegate () { this.mView.SpawnItemPop(key); });
                        key.isRegister = true;
                    }
                   
                }
            }
        }
        else
        {
            if (this.mView.itemUIList != null)
            {
                foreach (var key in this.mView.itemUIList)
                {
                    key.isRegister = false;
                   key.itemlevelbg_btn.onClick.RemoveAllListeners();
                    
                }
            }
        }
    }  

    /// <summary>
    /// 关闭背包按钮
    /// </summary>
    /// <param name="isClick"></param>
    public void CloseItem()
    {
        UIFace.GetSingleton().Close(UIID.Bag);
    }
}