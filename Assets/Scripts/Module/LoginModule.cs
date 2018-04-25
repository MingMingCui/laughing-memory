using UnityEngine;
using Msg;
using Msg.LoginMsg;
using System;

public class LoginModule : ModuleBase
{
    public override void Register()
    {
        RegistEvent((int)ServerMsgId.DCMD_AUTH_SUCCEEDED, OnLogin);
        RegistEvent((int)ServerMsgId.ECMD_AUTH_FAILED, OnLoginFailed);
        RegistEvent((int)ServerMsgId.ECMD_REG_FAILED, OnRegFailed);
        RegistEvent((int)ServerMsgId.DCMD_RESP_REG_CHARTER, OnMain);
        RegistEvent((int)ServerMsgId.ECMD_REG_CHARTER, OnRegMain);
    }

    public void OnLogin(ServerMsgObj msg)
    {
        LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(msg.Msg);
        Debug.LogFormat("OnLogin", msg.Msg);
        if (loginResponse == null)
            return;
        Role.Instance.RoleName = loginResponse.nickname;
        Role.Instance.RoleId = uint.Parse(loginResponse.user_id);
        Role.Instance.HeadId = loginResponse.base_data.face_id;
        Role.Instance.Level = loginResponse.levels;
        Role.Instance.Cash = loginResponse.coins;
        Role.Instance.Gold = loginResponse.golds;
        Role.Instance.LockedGold = loginResponse.bind_golds;

        var stubdata = Role.Instance.GetStubData(StubType.PVE);
        stubdata.Clear();
        if (loginResponse.formation != null)
            Role.Instance.InitStubData(loginResponse.formation);
        OnMailData(msg); //测试邮箱
        ZEventSystem.Dispatch(EventConst.OnMsgLogin, msg);
    }

    public void OnLoginFailed(ServerMsgObj msg)
    {
        Debug.Log("OnLoginFailed" + msg.Msg);
        CanvasView.Instance.AddNotice("用户名或密码错误");
    }

    public void OnRegFailed(ServerMsgObj msg)
    {
        Debug.Log("OnRegFailed" + msg.Msg);
    }
    public void OnMain(ServerMsgObj msg)
    {
        ZEventSystem.Dispatch(EventConst.OnMsgOnMain, msg);
    }
    public void OnRegMain(ServerMsgObj msg)
    {
        if (msg.SubId == 0)
        {
            Debug.Log("未知错误");
        }else if (msg.SubId == 1)
        {
            Debug.Log("用户名重复");
        }
        else if (msg.SubId == 2)
        {
            Debug.Log("服务器异常");
        }
    }

    public void OnMailData(ServerMsgObj msg)
    {
        // MailData maildate = JsonUtility.FromJson<MailData>(msg.Msg);

        for (int i = 0; i < 5; i++)
        {
            MailDatas Mail = new MailDatas();
            Item item = new Item();
            System.Collections.Generic.List<Item> itemlist = new System.Collections.Generic.List<Item>();
            switch (i)
            {
                case 0:
                    item.itemId = 30001;
                    item.itemNum = 10 + i;
                    itemlist.Add(item);
                    break;
                case 1:
                    item.itemId = 30006;
                    item.itemNum = 10 + i;
                    itemlist.Add(item);
                    break;
                case 2:
                    item.itemId = 30011;
                    item.itemNum = 10 + i;
                    itemlist.Add(item);
                    break;
                case 3:
                    item.itemId = 30016;
                    item.itemNum = 10 + i;
                    itemlist.Add(item);
                    break;
                case 4:
                    item.itemId = 30021;
                    item.itemNum = 10 + i;
                    itemlist.Add(item);
                    break;
            }
            Mail.mail_id = 1000 + i;
            Mail.ico_id = 20110;
            Mail.addresser = "卫东" + i;
            Mail.content = "是人，都有一颗心，有血有肉。是心，难免会生情，有牵有挂。人与人一旦有了感情，就会关心，就会惦记；心与心一旦有了相吸，就会眷恋，就会难舍。其实，人心都是肉长的，每一份惦记，都关着情；每一份感动，都连着心；每一声问候，都藏着真；每一份守望，都是用情至深！人与人之间，全凭一颗坦诚的心；心与心之间，全凭一份挚热的情。人心是肉长，是真情，女人也会流泪，男人也会动容；人心是肉长，是真情，小孩也会感动，老人也会触动；人心是肉长，是真情，我们都要铭记，都要珍惜，都要感恩心中！" +
                "是人，都有一颗心，有血有肉。是心，难免会生情，有牵有挂。人与人一旦有了感情，就会关心，就会惦记；心与心一旦有了相吸，就会眷恋，就会难舍。其实，人心都是肉长的，每一份惦记，都关着情；每一份感动，都连着心；每一声问候，都藏着真；每一份守望，都是用情至深！人与人之间，全凭一颗坦诚的心；心与心之间，全凭一份挚热的情。人心是肉长，是真情，女人也会流泪，男人也会动容；人心是肉长，是真情，小孩也会感动，老人也会触动；人心是肉长，是真情，我们都要铭记，都要珍惜，都要感恩心中！";
            
            
            Mail.date = Util.Utc2DateTime(long.Parse(((int)ConvertDateTimeInt(DateTime.Now)+i).ToString()));
            Mail.mailname = "卫东置信" + i;
            Mail.isRead = 0;
            
            if (i % 2 == 0)
            {
               
            }
            switch (i)
            {
                case 0:
                    Mail.type = 1;
                    break;
                case 1:
                    Mail.type = 2;
                Mail.mailitem = itemlist;
                    break;
                case 2:
                    Mail.type = 3;
                    break;
                case 3:
                    Mail.type = 1;
                    break;
                case 4:
                    Mail.type = 2;
                Mail.mailitem = itemlist;
                    break;
            }
            bool idcon = false;
            if (PostBoxMgr.Instance.MailData.Count>0)
            {
                for (int idx = 0; idx < PostBoxMgr.Instance.MailData.Count; idx++)
                {
                    if (PostBoxMgr.Instance.MailData[idx].mail_id == Mail.mail_id)
                    {
                        idcon = true;
                    }
                }
            }
            if (false == idcon)
            {
                PostBoxMgr.Instance.MailData.Add(Mail);
            }
        }
        PostBoxMgr.Instance.isUpd = true;
    }

    /// <summary>
    /// 将c# DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>double</returns>
    public static double ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }

}
