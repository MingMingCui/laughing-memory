using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GMCommand
{
    modifylevel , //修改等级
    modifycopper, //修改铜钱
    clearclear, //修改关卡
    modifyPower, //修改体力
    modifyviplevel,//修改VIP等级
    modifyglevel,//修改武将等级
    modifygold,//修改金锭及绑金
    clearsection,//解锁章节
    modifydynamic,//修改精力
    equip,//一键装备
    modifyarena,//修改竞技场
    modifyhonor,//修改荣誉
    modifyskill,//修改技能等级
    modifyggzj,//修改过关斩将
    intensify,//一键强化
    modifyvigor,//增加活跃
    divination,//一键卜卦
    sophistication//一键洗练
}


public class GMCtrl: UICtrlBase<GMView> 
{
   
    public override void OnInit()
    {
        base.OnInit();
        this.mView.Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Init(true);
       
    }

    public void Init(bool open)
    {
        if (open)
        {

            //this.mView.RoleFullLevel_btn.onClick.AddListener(delegate () { this.mView.RoleFullLevel(); }); //主公满级
            //this.mView.AddCopper_btn.onClick.AddListener(delegate() { this.mView.AddCopper(); }); //给铜钱
            //this.mView.ClearStage_btn.onClick.AddListener(delegate() {this.ClearsClose();  }); //解锁关卡
            //this.mView.AddPower_btn.onClick.AddListener(delegate() { this.mView.AddPower(); });//增加体力
            //this.mView.ElevateVIP_btn.onClick.AddListener(delegate() { this.mView.ElevateVIP(); });//提升VIP等级
            //this.mView.GeneralFullLevel_btn.onClick.AddListener(delegate() { this.mView.GeneralFullLevel(); });//武将满级
            //this.mView.AddGold_btn.onClick.AddListener(delegate() { this.mView.AddGold(); });//增加金锭
            //this.mView.ClearLeve_btn.onClick.AddListener(delegate() { Simulate(); });//解锁章节
            //this.mView.AddVigor_btn.onClick.AddListener(delegate() { this.mView.AddVigor(); });//增加精力
            //this.mView.Outfit_btn.onClick.AddListener(delegate () { this.mView.Outfit(); });//一键装备
            //this.mView.Advance500_btn.onClick.AddListener(delegate() { this.mView.Advance500(); }); //竞技场前进500名
            //this.mView.AddHonor_btn.onClick.AddListener(delegate() { this.mView.AddHonor(); }); //增加荣誉
            //this.mView.SkillFullLevel_btn.onClick.AddListener(delegate() { this.mView.SkillFullLevel(); });//技能满级
            //this.mView.Advance3_btn.onClick.AddListener(delegate() { this.mView.Advance3(); });//过关斩将前进3名
            //this.mView.Intensify_btn.onClick.AddListener(delegate() { this.mView.Intensify(); }); //一键强化
            //this.mView.AddActive_btn.onClick.AddListener(delegate() { this.mView.AddActive(); });  //活跃度增加20
            //this.mView.Divination_btn.onClick.AddListener(delegate() { this.mView.Divination(); });//一键卜卦
            //this.mView.Sophistication_btn.onClick.AddListener(delegate(){ this.mView.Sophistication(); });//一键洗练
            this.mView.Close_btn.onClick.AddListener(delegate() { UIFace.GetSingleton().Close(UIID.GM); });
            this.mView.Send_btn.onClick.AddListener(delegate() { this.mView.GMorder(this.mView.GMOrder_input.text); });
          //  this.mView.Woman1_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(false, this.mView.Woman1_tog.gameObject));
        }
        else
        {
            //this.mView.RoleFullLevel_btn.onClick.RemoveAllListeners();
            //this.mView.AddCopper_btn.onClick.RemoveAllListeners();
            //this.mView.ClearStage_btn.onClick.RemoveAllListeners();
            //this.mView.AddPower_btn.onClick.RemoveAllListeners();
            //this.mView.ElevateVIP_btn.onClick.RemoveAllListeners();
            //this.mView.GeneralFullLevel_btn.onClick.RemoveAllListeners();
            //this.mView.AddGold_btn.onClick.RemoveAllListeners();
            //this.mView.ClearLeve_btn.onClick.RemoveAllListeners();
            //this.mView.AddVigor_btn.onClick.RemoveAllListeners();
            //this.mView.Outfit_btn.onClick.RemoveAllListeners();
            //this.mView.Advance500_btn.onClick.RemoveAllListeners();
            //this.mView.AddHonor_btn.onClick.RemoveAllListeners();
            //this.mView.SkillFullLevel_btn.onClick.RemoveAllListeners();
            //this.mView.Advance3_btn.onClick.RemoveAllListeners();
            //this.mView.Intensify_btn.onClick.RemoveAllListeners();
            //this.mView.AddVigor_btn.onClick.RemoveAllListeners();
            //this.mView.Divination_btn.onClick.RemoveAllListeners();
            //this.mView.Sophistication_btn.onClick.RemoveAllListeners();
            this.mView.Close_btn.onClick.RemoveAllListeners();
            // ZEventSystem.Instance.DeRegister(EventConst.OnMsgOnMain, this);
            // this.mView.Woman_tog.onValueChanged.RemoveAllListeners();
        }
    }

    public override bool OnClose()
    {
        base.OnClose();
        Init(false);
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    
}
