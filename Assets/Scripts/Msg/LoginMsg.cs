using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Msg.LoginMsg
{
    /// <summary>
    /// 登录、注册消息
    /// </summary>
    [System.Serializable]
    public class LoginMsg
    {
        public int type;
        public string name;
        public string passwd;
    }

    [System.Serializable]
    public class CreateRoleMsg
    {
        
        public string nickname;
        public int sex;
        public int face_id;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string nickname;             //显示名字
        public string union_id;             //唯一id，先留着
        public string user_id;              //6位唯一id
        public int levels;                  //等级
        public int coins;                   //铜钱
        public int golds;                   //钻石
        public int bind_golds;              //绑定钻石
        public int group_id;                //帮会id
        public int is_first;                //暂时没用
        public CreateRoleMsg base_data;          //原始数据
        public StubMsg.StubMsg formation;       //布阵数据
        public ShopsData shopdata;           //商店数据
    }
   [System.Serializable]
    public class MailData
    {
        public int mail_id;                 //邮件id
        public int ico_id;                  //图标id
        public int type;                    //邮件类型
        public long date;                   //发件时间
        public string mailname;             //发件标题
        public string addresser;            //发件人
        public string content;              //邮件内容 
        public List<Item> mailitem = new List<Item>();         //邮件附件
    }

    [System.Serializable]
    public class ShopsData
    {
        public ShopItem[] shop_1;          //商店1
        public ShopItem[] shop_2;          //商店2
        public ShopItem[] shop_3;          //商店3
        public ShopItem[] shop_4;          //商店4
        public ShopItem[] shop_5;          //商店5

    }

    [System.Serializable]
    public class ShopItem
    {
        public int item;      //id;
        public int item_id; //道具id
        public int price;   //价格
        public int num;     //数量
        public int type;    //货币类型
    }
}
